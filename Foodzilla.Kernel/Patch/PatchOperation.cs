using System.Dynamic;
using System.Text.Json;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using Foodzilla.Kernel.Domain;
using Foodzilla.Kernel.Extension;

namespace Foodzilla.Kernel.Patch;

public sealed class PatchOperation<TEntity> where TEntity : Entity, IPatchValidator
{
    private static int total = 0;
    private const string Id = "Id";

    internal Guid Guid;
    internal Entity Entity;
    private bool _failed = false;
    private ExpandoObject _patchEntity;
    private PropertyInfo[] _entityProperties;
    private readonly IEnumerable<string> _ignoreFields;
    private readonly List<ExpandoObject> _patchEntities;
    private readonly Dictionary<Entity, bool> _entitiesStatusCollection;
    private readonly Dictionary<Entity, PropertyInfo[]> _entityPropertiesDictionary;
    private readonly Dictionary<Entity, List<ExpandoObject>> _patchEntitiesDictionary;
    private readonly Dictionary<Entity, Dictionary<object, object>> _navigationProperties;
    private readonly Dictionary<Entity, Dictionary<PropertyInfo, object>> _originalValuesCollection;

    public List<string> EntityIds { get; private set; }

    public List<PatchInvalidResult> InvalidResults { get; init; } = new();

    private PatchOperation(ExpandoObject patchEntity, string webPathRoot, string rrr)
    {
        _patchEntity = patchEntity;
        Initialize(webPathRoot);
    }

    private PatchOperation(ExpandoObject patchEntity, string webPathRoot)
    {
        total++;
        Guid = new Guid();

        _ignoreFields = new List<string>();
        _entityProperties = typeof(TEntity).GetProperties();
        _patchEntities = new List<ExpandoObject> { patchEntity };
        _entitiesStatusCollection = new Dictionary<Entity, bool>();
        _entityPropertiesDictionary = new Dictionary<Entity, PropertyInfo[]>();
        _patchEntitiesDictionary = new Dictionary<Entity, List<ExpandoObject>>();
        _navigationProperties = new Dictionary<Entity, Dictionary<object, object>>();
        _originalValuesCollection = new Dictionary<Entity, Dictionary<PropertyInfo, object>>();

        InitializeIds(webPathRoot);
    }

    private PatchOperation(List<ExpandoObject> patchEntities, string webPathRoot)
    {
        total++;

        _patchEntities = patchEntities ?? throw new NullReferenceException();

        _ignoreFields = new List<string>();
        _entityProperties = typeof(TEntity).GetProperties();
        _entitiesStatusCollection = new Dictionary<Entity, bool>();
        _entityPropertiesDictionary = new Dictionary<Entity, PropertyInfo[]>();
        _patchEntitiesDictionary = new Dictionary<Entity, List<ExpandoObject>>();
        _navigationProperties = new Dictionary<Entity, Dictionary<object, object>>();
        _originalValuesCollection = new Dictionary<Entity, Dictionary<PropertyInfo, object>>();
    }

    public static PatchOperation<TEntity> Create(ExpandoObject patchEntity, string webPathRoot = null)
    {
        return new PatchOperation<TEntity>(patchEntity, webPathRoot);
    }

    public static PatchOperation<TEntity> Create(List<ExpandoObject> patchEntities, string webPathRoot = null)
    {
        return new PatchOperation<TEntity>(patchEntities, webPathRoot);
    }

    public void ApplyOneToOneRelatively(List<TEntity> dbEntities)
    {
        foreach (var dbEntity in dbEntities)
        {
            ApplyOneToOneRelatively(dbEntity);
        }
    }

    public void ApplyOneToOneAbsolutely(List<TEntity> dbEntities)
    {
        foreach (var dbEntity in dbEntities)
        {
            ApplyOneToOneAbsolutely(dbEntity);
        }
    }

    public void ApplyOneToOneParentDominance(List<TEntity> dbEntities)
    {
        foreach (var dbEntity in dbEntities)
        {
            ApplyOneToOneParentDominance(dbEntity);
        }
    }

    /// <summary>
    /// This method uses single patchEntity to update single database entity
    /// Apply patch while patchEntity does not contain Id
    /// Id is sent separated in request body
    /// Only one instant is applied to a single database entity
    /// </summary>
    public bool ApplyOneToAll(TEntity dbEntity)
    {
        foreach ((string property, object value) in _patchEntity)
        {
            var commonProperty = _entityProperties.SingleOrDefault(p => p.Name.EqualsIgnoreCase(property));

            if (commonProperty != null)
            {
                if (_ignoreFields.Contains(commonProperty.Name.ToLower()))
                {
                    AddErrorResult(property, value?.ToString(), PatchError.PropertyIgnoredToUpdate);
                    continue;
                }

                try
                {
                    StoreDeepOriginalValues(dbEntity, commonProperty);

                    object castedValue = CastCorrectValue(commonProperty, value);

                    commonProperty.SetValue(dbEntity, castedValue);

                }
                catch (Exception exception)
                {
                    Failed();
                    AddErrorResult(commonProperty.Name, value?.ToString(), exception.Message);
                }
            }
            else
            {
                Failed();
                AddErrorResult(property, value?.ToString(), PatchError.PropertyMatchingFailed);
            }
        }

        if (OperationFailed() || !dbEntity.OnPatchCompleted())
        {
            RestoreOriginalValues(dbEntity);

            return false;
        }

        OperationReStart();

        return true;
    }

    /// <summary>
    /// This method uses single patchEntity to update single database entity
    /// Navigation properties are independent of their parent so if parent patch fails, the navigation will stills accept changes
    /// Apply patch while patchEntity contains id
    /// Id is sent inside each patchEntity. peer to peer patching
    /// Only one instant is applied to a single database entity
    /// </summary>
    public bool ApplyOneToOneRelatively(TEntity dbEntity)
    {
        var idValueString = SetPatchEntity(dbEntity);

        foreach ((string property, object value) in _patchEntity)
        {
            var commonProperty = _entityProperties.SingleOrDefault(p => p.Name.EqualsIgnoreCase(property));

            if (commonProperty != null)
            {
                if (_ignoreFields.Contains(commonProperty.Name.ToLower()))
                {
                    AddErrorResult(idValueString, property, value?.ToString(), PatchError.PropertyIgnoredToUpdate);
                    continue;
                }

                try
                {
                    StoreShallowOriginalValues(dbEntity, commonProperty);

                    if (StoreNavigationProperties(dbEntity, commonProperty, value)) continue;

                    object castedValue = CastCorrectValue(commonProperty, value);

                    commonProperty.SetValue(dbEntity, castedValue);

                }
                catch (Exception exception)
                {
                    AddErrorResult(idValueString, commonProperty.Name, value?.ToString(), exception.Message);

                    Failed(dbEntity);
                }
            }
            else
            {
                AddErrorResult(idValueString, property, value?.ToString(), PatchError.PropertyMatchingFailed);

                Failed(dbEntity);
            }
        }

        if (OperationFailed(dbEntity) || !dbEntity.OnPatchCompleted())
        {
            RestoreOriginalValues(dbEntity);

            PatchNavigationProperties(dbEntity, parentLoyalty: false);

            return false;
        }
        else
        {
            PatchNavigationProperties(dbEntity, parentLoyalty: true);
        }

        OperationReStart(dbEntity);

        return true;
    }

    /// <summary>
    /// This method uses single patchEntity to update single database entity
    /// Navigation properties are dependent of their parent so if parent patch fails, the navigation will not accept changes
    /// Apply patch while patchEntity contains id
    /// Id is sent inside each patchEntity. peer to peer patching
    /// Only one instant is applied to a single database entity
    /// </summary>
    public bool ApplyOneToOneAbsolutely(TEntity dbEntity)
    {
        var idValueString = SetPatchEntity(dbEntity);

        foreach ((string property, object value) in _patchEntity)
        {
            var commonProperty = _entityProperties.SingleOrDefault(p => p.Name.EqualsIgnoreCase(property));

            if (commonProperty != null)
            {
                if (_ignoreFields.Contains(commonProperty.Name.ToLower()))
                {
                    AddErrorResult(idValueString, property, value?.ToString(), PatchError.PropertyIgnoredToUpdate);
                    continue;
                }

                try
                {
                    if (NavigationDeepPatchOperation(dbEntity, commonProperty, value)) continue;

                    StoreDeepOriginalValues(dbEntity, commonProperty);

                    object castedValue = CastCorrectValue(commonProperty, value);

                    commonProperty.SetValue(dbEntity, castedValue);

                }
                catch (Exception exception)
                {
                    AddErrorResult(idValueString, commonProperty.Name, value?.ToString(), exception.Message);

                    Failed(dbEntity);
                }
            }
            else
            {
                AddErrorResult(idValueString, property, value?.ToString(), PatchError.PropertyMatchingFailed);

                Failed(dbEntity);
            }
        }

        if (OperationFailed(dbEntity) || !dbEntity.OnPatchCompleted())
        {
            RestoreOriginalValues(dbEntity);

            return false;
        }

        OperationReStart(dbEntity);

        return true;
    }

    public bool ApplyOneToOneParentDominance(TEntity dbEntity)
    {
        var idValueString = SetPatchEntity(dbEntity);

        foreach ((string property, object value) in _patchEntity)
        {
            var commonProperty = _entityProperties.SingleOrDefault(p => p.Name.EqualsIgnoreCase(property));

            if (commonProperty != null)
            {
                if (_ignoreFields.Contains(commonProperty.Name.ToLower()))
                {
                    AddErrorResult(idValueString, property, value?.ToString(), PatchError.PropertyIgnoredToUpdate);
                    continue;
                }

                try
                {
                    StoreShallowOriginalValues(dbEntity, commonProperty);

                    if (StoreNavigationProperties(dbEntity, commonProperty, value)) continue;

                    object castedValue = CastCorrectValue(commonProperty, value);

                    commonProperty.SetValue(dbEntity, castedValue);

                }
                catch (Exception exception)
                {
                    AddErrorResult(idValueString, commonProperty.Name, value?.ToString(), exception.Message);

                    Failed(dbEntity);
                }
            }
            else
            {
                AddErrorResult(idValueString, property, value?.ToString(), PatchError.PropertyMatchingFailed);

                Failed(dbEntity);
            }
        }

        if (OperationFailed(dbEntity) || !dbEntity.OnPatchCompleted())
        {
            RestoreOriginalValues(dbEntity);

            return false;
        }
        else
        {
            PatchNavigationProperties(dbEntity);
        }

        OperationReStart(dbEntity);

        return true;
    }

    public bool ApplyOneToOne(TEntity dbEntity, ExpandoObject patchEntity, PropertyInfo[] entityProperties)
    {
        var idProperty = entityProperties.First(p => p.Name.EqualsIgnoreCase(Id));

        var idValueString = idProperty.GetValue(dbEntity)?.ToString();

        foreach ((string property, object value) in patchEntity)
        {
            var commonProperty = entityProperties.SingleOrDefault(p => p.Name.EqualsIgnoreCase(property));

            if (commonProperty != null)
            {
                if (_ignoreFields.Contains(commonProperty.Name.ToLower()))
                {
                    AddErrorResult(idValueString, property, value?.ToString(), PatchError.PropertyIgnoredToUpdate);
                    continue;
                }

                try
                {
                    StoreDeepOriginalValues(dbEntity, commonProperty);

                    //if (InnerPatchOperation(commonProperty, value)) continue;

                    object castedValue = CastCorrectValue(commonProperty, value);

                    commonProperty.SetValue(dbEntity, castedValue);

                }
                catch (Exception exception)
                {
                    AddErrorResult(idValueString, commonProperty.Name, value?.ToString(), exception.Message);

                    Failed();
                }
            }
            else
            {
                AddErrorResult(idValueString, property, value?.ToString(), PatchError.PropertyMatchingFailed);

                Failed();
            }
        }

        if (OperationFailed() || !dbEntity.OnPatchCompleted())
        {
            RestoreOriginalValues(dbEntity);

            return false;
        }

        OperationReStart();

        return true;
    }

    private void ApplyShallowOneToOne(Entity dbEntity)
    {
        var idValueString = SetPatchEntity(dbEntity);

        foreach ((string property, object value) in _patchEntity)
        {
            var commonProperty = _entityProperties.SingleOrDefault(p => p.Name.EqualsIgnoreCase(property));

            if (commonProperty != null)
            {
                if (_ignoreFields.Contains(commonProperty.Name.ToLower()))
                {
                    AddErrorResult(idValueString, property, value?.ToString(), PatchError.PropertyIgnoredToUpdate);
                    continue;
                }

                try
                {
                    StoreShallowOriginalValues(dbEntity, commonProperty);

                    if (StoreNavigationProperties(dbEntity, commonProperty, value)) continue;

                    object castedValue = CastCorrectValue(commonProperty, value);

                    commonProperty.SetValue(dbEntity, castedValue);

                }
                catch (Exception exception)
                {
                    AddErrorResult(idValueString, commonProperty.Name, value?.ToString(), exception.Message);

                    Failed(dbEntity);
                }
            }
            else
            {
                AddErrorResult(idValueString, property, value?.ToString(), PatchError.PropertyMatchingFailed);

                Failed(dbEntity);
            }
        }

        if (dbEntity is IPatchValidator patchValidator)
        {
            if (OperationFailed(dbEntity) || !patchValidator.OnPatchCompleted())
            {
                RestoreOriginalValues(dbEntity);
            }
            else
            {
                PatchNavigationProperties(dbEntity);
            }

            OperationReStart(dbEntity);
        }
        else
        {
            RestoreOriginalValues(dbEntity);
        }

        OperationReStart(dbEntity);
    }

    private void ApplyShallowOneToOne(Entity dbEntity, bool parentLoyalty)
    {
        var idValueString = SetPatchEntity(dbEntity);

        foreach ((string property, object value) in _patchEntity)
        {
            var commonProperty = _entityProperties.SingleOrDefault(p => p.Name.EqualsIgnoreCase(property));

            if (commonProperty != null)
            {
                if (_ignoreFields.Contains(commonProperty.Name.ToLower()))
                {
                    AddErrorResult(idValueString, property, value?.ToString(), PatchError.PropertyIgnoredToUpdate);
                    continue;
                }

                try
                {
                    if (parentLoyalty)
                    {
                        StoreShallowOriginalValues(dbEntity, commonProperty);

                        if (StoreNavigationProperties(dbEntity, commonProperty, value)) continue;

                        object castedValue = CastCorrectValue(commonProperty, value);

                        commonProperty.SetValue(dbEntity, castedValue);
                    }
                    else
                    {
                        StoreNavigationProperties(dbEntity, commonProperty, value);
                    }
                }
                catch (Exception exception)
                {
                    AddErrorResult(idValueString, commonProperty.Name, value?.ToString(), exception.Message);

                    Failed(dbEntity);
                }
            }
            else
            {
                AddErrorResult(idValueString, property, value?.ToString(), PatchError.PropertyMatchingFailed);

                Failed(dbEntity);
            }
        }

        if (dbEntity is IPatchValidator patchValidator)
        {
            if (OperationFailed(dbEntity) || !patchValidator.OnPatchCompleted())
            {
                RestoreOriginalValues(dbEntity);

                PatchNavigationProperties(dbEntity, false);
            }
            else
            {
                PatchNavigationProperties(dbEntity, true);
            }

            OperationReStart(dbEntity);
        }
        else
        {
            RestoreOriginalValues(dbEntity);
        }

        OperationReStart(dbEntity);
    }

    private void ApplyDeepOneToOne(Entity dbEntity)
    {
        var idValueString = SetPatchEntity(dbEntity);

        foreach ((string property, object value) in _patchEntity)
        {
            var commonProperty = _entityProperties.SingleOrDefault(p => p.Name.EqualsIgnoreCase(property));

            if (commonProperty != null)
            {
                if (_ignoreFields.Contains(commonProperty.Name.ToLower()))
                {
                    AddErrorResult(idValueString, property, value?.ToString(), PatchError.PropertyIgnoredToUpdate);
                    continue;
                }

                try
                {
                    if (NavigationDeepPatchOperation(dbEntity, commonProperty, value)) continue;

                    StoreDeepOriginalValues(dbEntity, commonProperty);

                    object castedValue = CastCorrectValue(commonProperty, value);

                    commonProperty.SetValue(dbEntity, castedValue);

                }
                catch (Exception exception)
                {
                    AddErrorResult(idValueString, commonProperty.Name, value?.ToString(), exception.Message);

                    Failed(dbEntity);
                }
            }
            else
            {
                AddErrorResult(idValueString, property, value?.ToString(), PatchError.PropertyMatchingFailed);

                Failed(dbEntity);
            }
        }

        if (dbEntity is IPatchValidator patchValidator)
        {
            if (OperationFailed(dbEntity) || !patchValidator.OnPatchCompleted())
            {
                RestoreOriginalValues(dbEntity);
            }
        }
        else
        {
            RestoreOriginalValues(dbEntity);
        }

        OperationReStart(dbEntity);
    }

    /// <summary>
    /// This method uses single patchEntity to update set of database entities
    /// Apply patch while patchEntity does not contain Id
    /// Id is sent separated in request body
    /// Only one instant is applied to a set of database Entities
    /// </summary>
    public void ApplyWithoutIdentifier(List<TEntity> dbEntities)
    {
        var type = typeof(TEntity);
        var properties = type.GetProperties();

        for (var i = dbEntities.Count - 1; i >= 0; i--)
        {
            var dbEntity = dbEntities[i];

            foreach ((string property, object value) in _patchEntity)
            {
                var commonProperty = properties.SingleOrDefault(p => p.Name.EqualsIgnoreCase(property) && !_ignoreFields.Contains(p.Name.ToLower()));

                if (commonProperty != null)
                {
                    try
                    {
                        StoreDeepOriginalValues(dbEntity, commonProperty);

                        object castedValue = CastCorrectValue(commonProperty, value);

                        commonProperty.SetValue(dbEntity, castedValue);
                    }
                    catch (Exception)
                    {
                        RestoreOriginalValues(dbEntity);
                        dbEntities.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }

    private void Initialize(string contentRootPath)
    {
        if (_patchEntity == null)
        {
            throw new NullReferenceException();
        }

        _entityProperties = typeof(TEntity).GetProperties();
        //_originalValuesCollection = new Dictionary<Entity, Dictionary<PropertyInfo, object>>();
    }

    private void InitializeIds(string contentRootPath)
    {
        if (_patchEntities == null)
        {
            throw new NullReferenceException();
        }

        EntityIds = _patchEntities.Select(p => p.FirstOrDefault(q => q.Key.EqualsIgnoreCase(Id)).Value?.ToString()).ToList();
    }

    private string SetPatchEntity(Entity dbEntity)
    {
        Entity = dbEntity;

        if (!_entityPropertiesDictionary.TryGetValue(dbEntity, out var entityProperties))
        {
            entityProperties = typeof(TEntity).GetProperties();
        }

        var idProperty = entityProperties.First(p => p.Name.EqualsIgnoreCase(Id));

        var idValueString = idProperty.GetValue(dbEntity)!.ToString();

        if (_patchEntitiesDictionary.TryGetValue(dbEntity, out var patchEntities))
        {
            _patchEntity = patchEntities.Find(p =>
            {
                var value = p.FirstOrDefault(q => q.Key.EqualsIgnoreCase(Id)).Value;
                return value != null && value.ToString()!.Equals(idValueString);
            }) ?? new ExpandoObject();
        }
        else
        {
            _patchEntity = _patchEntities.Find(p =>
            {
                var value = p.FirstOrDefault(q => q.Key.EqualsIgnoreCase(Id)).Value;
                return value != null && value.ToString()!.Equals(idValueString);
            }) ?? new ExpandoObject();
        }

        _entityProperties = entityProperties;

        return idValueString;
    }

    //private bool InnerPatchOperation(PropertyInfo commonProperty, object value)
    //{
    //    if (commonProperty.InquireOneToOneNavigability(Entity, out var dbEntity))
    //    {
    //        if (value == null)
    //        {
    //            return true;
    //        }

    //        var patchEntity = (ExpandoObject)value;

    //        var patchDocument = Create(patchEntity);

    //        patchDocument.ApplyOneToOneRelatively((TEntity)dbEntity);

    //        return true;
    //    }

    //    if (commonProperty.InquireOneToManyNavigability(Entity, out var dbEntities))
    //    {
    //        if (value == null)
    //        {
    //            return true;
    //        }

    //        var patchEntities = (List<ExpandoObject>)value;

    //        var patchDocument = Create(patchEntities);

    //        foreach (var dbEntity1 in dbEntities)
    //        {
    //            patchDocument.ApplyOneToOneRelatively((TEntity)dbEntity1);
    //        }

    //        return true;
    //    }

    //    return false;
    //}

    private static object CastCorrectValue(PropertyInfo commonProperty, object value)
    {
        object castedValue;

        var nullability = commonProperty.InquireNullability(out var propertyType);

        if (propertyType.IsEnum)
        {
            CastEnumValue(propertyType, value, nullability, out castedValue);
        }
        else
        {
            if (propertyType.IsBoolean())
            {
                CastBooleanValue(propertyType, value, nullability, out castedValue);
            }
            else
            {
                CastStringOrStructValue(propertyType, value, nullability, out castedValue);
            }
        }

        return castedValue;
    }

    private static void CastEnumValue(Type enumType, object value, bool isNullable, out object castedValue)
    {
        if (ApprovedToAcceptNull(isNullable, value))
        {
            castedValue = null;
            return;
        }

        if (value != null)
        {
            JsonElement jsonElement = (JsonElement)value;
            JsonValueKind valueKind = jsonElement.ValueKind;

            if (valueKind == JsonValueKind.Number)
            {
                value = jsonElement.GetInt32();

                GetRelativeEnumOfInteger(enumType, value, out castedValue);

                return;
            }

            if (valueKind == JsonValueKind.String)
            {
                var valueString = jsonElement.GetString();

                if (int.TryParse(valueString, out var valueInt32))
                {
                    GetRelativeEnumOfInteger(enumType, valueInt32, out castedValue);
                }
                else
                {
                    castedValue = Enum.Parse(enumType, valueString, true);
                }
            }
            else
            {
                throw new Exception(PatchError.PropertyAcceptsOnlyStringOrStruct);
            }
        }
        else
        {
            throw new Exception(PatchError.PropertyNullOrEmpty);
        }
    }

    private static void CastBooleanValue(Type propertyType, object value, bool isNullable, out object castedValue)
    {
        if (ApprovedToAcceptNull(isNullable, value))
        {
            castedValue = null;
            return;
        }

        object booleanObject = null;

        if (value != null)
        {
            JsonElement element = (JsonElement)value;
            JsonValueKind valueKind = element.ValueKind;

            switch (valueKind)
            {
                case JsonValueKind.Number when element.GetInt32() == 1:
                    booleanObject = true;
                    break;

                case JsonValueKind.Number when element.GetInt32() == 0:
                    booleanObject = false;
                    break;

                case JsonValueKind.Number:
                    throw new ArgumentOutOfRangeException();

                case JsonValueKind.String when element.GetString() == "1":
                    booleanObject = Boolean.TrueString;
                    break;

                case JsonValueKind.String when element.GetString() == "0":
                    booleanObject = Boolean.FalseString;
                    break;

                case JsonValueKind.String:
                    booleanObject = element.GetString();
                    break;

                case JsonValueKind.True or JsonValueKind.False:
                    booleanObject = element.GetBoolean();

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        castedValue = Convert.ChangeType(booleanObject, propertyType);
    }

    private static void CastStringOrStructValue(Type propertyType, object value, bool isNullable, out object castedValue)
    {
        if (ApprovedToAcceptNull(isNullable, value))
        {
            castedValue = null;
            return;
        }

        string stringObject = null;

        if (value != null)
        {
            JsonElement element = (JsonElement)value;

            switch (element.ValueKind)
            {
                case JsonValueKind.Array:
                    var objects = GetArrayData(element);
                    var stringValue = string.Join(",", objects);
                    stringObject = stringValue.Insert(0, "[").Insert(stringValue.Length + 1, "]");
                    break;

                case JsonValueKind.String:
                    stringObject = element.GetString();
                    break;

                case JsonValueKind.Number:
                    stringObject = element.GetRawText();
                    break;

                case JsonValueKind.True:
                case JsonValueKind.False:
                    stringObject = element.GetBoolean().ToString();
                    break;

                case JsonValueKind.Null:
                    stringObject = null;
                    break;

                case JsonValueKind.Object:
                    break;

                case JsonValueKind.Undefined:
                    throw new Exception();

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        else
        {
            if (propertyType == typeof(string))
            {
                castedValue = null;

                return;
            }
        }

        castedValue = TypeDescriptor.GetConverter(propertyType).ConvertFromInvariantString(stringObject);
    }

    private static void GetRelativeEnumOfInteger(Type enumType, object value, out object castedValue)
    {
        if (Enum.IsDefined(enumType, value))
        {
            castedValue = Enum.ToObject(enumType, value);
        }
        else
        {
            throw new Exception(PatchError.PropertyValueOutOfRange);
        }
    }

    private static bool ApprovedToAcceptNull(bool isNullable, object value)
    {
        return isNullable && value == null;
    }

    private static IEnumerable<object> GetArrayData(JsonElement element)
    {
        List<object> objects = new();

        var enumerator = element.EnumerateArray();

        while (enumerator.MoveNext())
        {
            var current = enumerator.Current;

            if (current.TryGetGuid(out var guid))
            {
                objects.Add(guid);
                continue;
            }

            if (current.TryGetInt64(out var int64))
            {
                objects.Add(int64);
                continue;
            }

            if (current.TryGetDateTime(out var dateTime))
            {
                objects.Add(dateTime);
                continue;
            }

            if (current.TryGetDouble(out var doubleValue))
            {
                objects.Add(doubleValue);
                continue;
            }

            if (current.TryGetDecimal(out var decimalValue))
            {
                objects.Add(decimalValue);
            }
        }

        return objects;
    }

    private void RestoreOriginalValues(Entity dbEntity)
    {
        _originalValuesCollection.TryGetValue(dbEntity, out var originalValues);

        foreach (var property in originalValues.Keys)
        {
            var originalValue = originalValues[property];

            if (originalValue is IEnumerable<Entity> entities)
            {
                var instance = (IList)Activator.CreateInstance(property.PropertyType);

                foreach (var value in entities)
                {
                    instance.Add(value);
                }

                property.SetValue(dbEntity, instance);
            }
            else
            {
                property.SetValue(dbEntity, originalValue);

                originalValues.Remove(property);
            }
        }

        _originalValuesCollection.Remove(dbEntity);

        OperationReStart(dbEntity);
    }

    private void StoreDeepOriginalValues(Entity dbEntity, PropertyInfo commonProperty)
    {
        object originalValue = commonProperty.GetValue(dbEntity);

        if (_originalValuesCollection.ContainsKey(dbEntity))
        {
            _originalValuesCollection[dbEntity].Add(commonProperty, originalValue);
        }
        else
        {
            Dictionary<PropertyInfo, object> originalValues = new() { { commonProperty, originalValue } };

            _originalValuesCollection.Add(dbEntity, originalValues);
        }
    }

    private void StoreShallowOriginalValues(Entity dbEntity, PropertyInfo commonProperty)
    {
        //object originalValue = commonProperty.GetValue(dbEntity);

        //if (originalValue is Entity entity)
        //{
        //    originalValue = entity.Clone();
        //}

        //if (originalValue is IEnumerable<Entity> entities)
        //{
        //    originalValue = entities.Select(entityItem => entityItem.Clone()).ToList();
        //}

        object originalValue;

        if (commonProperty.InquireOneToOneNavigability(dbEntity, out var outEntity))
        {
            originalValue = outEntity.Clone();
        }

        else if (commonProperty.InquireOneToManyNavigability(dbEntity, out var outEntities))
        {
            originalValue = outEntities.Select(entityItem => entityItem.Clone()).ToList();
        }

        else
        {
            originalValue = commonProperty.GetValue(dbEntity);
        }

        if (_originalValuesCollection.ContainsKey(dbEntity))
        {
            _originalValuesCollection[dbEntity].Add(commonProperty, originalValue);
        }
        else
        {
            Dictionary<PropertyInfo, object> originalValues = new() { { commonProperty, originalValue } };

            _originalValuesCollection.Add(dbEntity, originalValues);
        }
    }

    private bool StoreNavigationProperties(Entity dbEntity, PropertyInfo commonProperty, object value)
    {
        if (commonProperty.InquireOneToOneNavigability(dbEntity, out var outEntity))
        {
            if (_navigationProperties.ContainsKey(dbEntity))
            {
                _navigationProperties[dbEntity].Add(outEntity, value);
            }
            else
            {
                _navigationProperties.Add(dbEntity, new Dictionary<object, object> { { outEntity, value } });
            }

            return true;
        }

        if (commonProperty.InquireOneToManyNavigability(dbEntity, out var outEntities))
        {
            if (_navigationProperties.ContainsKey(dbEntity))
            {
                _navigationProperties[dbEntity].Add(outEntities, value);
            }
            else
            {
                _navigationProperties.Add(dbEntity, new Dictionary<object, object> { { outEntities, value } });
            }

            return true;
        }

        return false;
    }

    private void PatchNavigationProperties(Entity dbEntity)
    {
        if (_navigationProperties.TryGetValue(dbEntity, out var navigationPropertyValues))
        {
            foreach (var (entity, value) in navigationPropertyValues)
            {
                if (entity is Entity outEntity)
                {
                    if (value != null)
                    {
                        _entityPropertiesDictionary.Add(outEntity, entity.GetRealType().GetProperties());

                        _patchEntitiesDictionary.Add(outEntity, new List<ExpandoObject> { (ExpandoObject)value });

                        ApplyShallowOneToOne(outEntity);

                        _entityProperties = dbEntity.GetRealType().GetProperties();
                    }
                }

                if (entity is IEnumerable<Entity> outEntities)
                {
                    var outEntitiesList = outEntities.ToList();

                    if (value != null && outEntitiesList.Count != 0)
                    {
                        foreach (var unitEntity in outEntitiesList)
                        {
                            _patchEntitiesDictionary.Add(unitEntity, (List<ExpandoObject>)value);

                            _entityPropertiesDictionary.Add(unitEntity, unitEntity.GetRealType().GetProperties());

                            ApplyShallowOneToOne(unitEntity);
                        }

                        _entityProperties = dbEntity.GetRealType().GetProperties();
                    }
                }
            }
        }
    }

    private void PatchNavigationProperties(Entity dbEntity, bool parentLoyalty)
    {
        if (_navigationProperties.TryGetValue(dbEntity, out var navigationPropertyValues))
        {
            foreach (var (entity, value) in navigationPropertyValues)
            {
                if (entity is Entity outEntity)
                {
                    if (value != null)
                    {
                        _entityPropertiesDictionary.Add(outEntity, entity.GetRealType().GetProperties());

                        _patchEntitiesDictionary.Add(outEntity, new List<ExpandoObject> { (ExpandoObject)value });

                        ApplyShallowOneToOne(outEntity, parentLoyalty);

                        _entityProperties = dbEntity.GetRealType().GetProperties();
                    }
                }

                if (entity is IEnumerable<Entity> outEntities)
                {
                    var outEntitiesList = outEntities.ToList();

                    if (value != null && outEntitiesList.ToList().Count != 0)
                    {
                        foreach (var unitEntity in outEntitiesList)
                        {
                            _patchEntitiesDictionary.Add(unitEntity, (List<ExpandoObject>)value);

                            _entityPropertiesDictionary.Add(unitEntity, unitEntity.GetRealType().GetProperties());

                            ApplyShallowOneToOne(unitEntity, parentLoyalty);
                        }

                        _entityProperties = dbEntity.GetRealType().GetProperties();
                    }
                }
            }
        }
    }

    private bool NavigationShallowPatchOperation(Entity dbEntity, PropertyInfo commonProperty, object value)
    {
        if (commonProperty.InquireOneToOneNavigability(dbEntity, out var outEntity))
        {
            if (value != null)
            {
                StoreShallowOriginalValues(dbEntity, commonProperty);

                _entityPropertiesDictionary.Add(outEntity, outEntity.GetRealType().GetProperties());

                _patchEntitiesDictionary.Add(outEntity, new List<ExpandoObject> { (ExpandoObject)value });

                ApplyShallowOneToOne(outEntity);

                _entityProperties = dbEntity.GetRealType().GetProperties();
            }

            return true;
        }

        if (commonProperty.InquireOneToManyNavigability(dbEntity, out var outEntities))
        {
            if (value != null && outEntities.Count != 0)
            {
                StoreShallowOriginalValues(dbEntity, commonProperty);

                foreach (var unitEntity in outEntities)
                {
                    _patchEntitiesDictionary.Add(unitEntity, (List<ExpandoObject>)value);

                    _entityPropertiesDictionary.Add(unitEntity, unitEntity.GetRealType().GetProperties());

                    ApplyShallowOneToOne(unitEntity);
                }

                _entityProperties = dbEntity.GetRealType().GetProperties();
            }

            return true;
        }

        return false;
    }

    private bool NavigationDeepPatchOperation(Entity dbEntity, PropertyInfo commonProperty, object value)
    {
        if (commonProperty.InquireOneToOneNavigability(dbEntity, out var outEntity))
        {
            if (value != null)
            {
                StoreDeepOriginalValues(dbEntity, commonProperty);

                _entityPropertiesDictionary.Add(outEntity, outEntity.GetRealType().GetProperties());

                _patchEntitiesDictionary.Add(outEntity, new List<ExpandoObject> { (ExpandoObject)value });

                ApplyDeepOneToOne(outEntity);

                _entityProperties = dbEntity.GetRealType().GetProperties();
            }

            return true;
        }

        if (commonProperty.InquireOneToManyNavigability(dbEntity, out var outEntities))
        {
            if (commonProperty.Name == "Juniors")
            {

            }

            if (value != null && outEntities.Count != 0)
            {
                StoreDeepOriginalValues(dbEntity, commonProperty);

                foreach (var unitEntity in outEntities)
                {
                    _patchEntitiesDictionary.Add(unitEntity, (List<ExpandoObject>)value);

                    _entityPropertiesDictionary.Add(unitEntity, unitEntity.GetRealType().GetProperties());

                    ApplyDeepOneToOne(unitEntity);
                }

                _entityProperties = dbEntity.GetRealType().GetProperties();
            }

            return true;
        }

        return false;
    }

    private void StoreOriginalValues(Entity dbEntity, PropertyInfo commonProperty, object originalValue)
    {
        if (_originalValuesCollection.ContainsKey(dbEntity))
        {
            _originalValuesCollection[dbEntity].Add(commonProperty, originalValue);
        }
        else
        {
            Dictionary<PropertyInfo, object> originalValues = new() { { commonProperty, originalValue } };

            _originalValuesCollection.Add(dbEntity, originalValues);
        }
    }

    private void AddErrorResult(string field, object value, string message)
    {
        var result = PatchInvalidResult.Create(field, value, message);
        InvalidResults.Add(result);
    }

    private void AddErrorResult(object entityId, string field, object value, string message)
    {
        var result = PatchInvalidResult.Create(entityId, field, value, message);
        InvalidResults.Add(result);
    }

    private void Failed()
    {
        _failed = true;
    }

    private bool OperationFailed()
    {
        return _failed;
    }

    private void OperationReStart()
    {
        _failed = false;
    }

    private void Failed(Entity dbEntity)
    {
        _entitiesStatusCollection.TryAdd(dbEntity, true);
    }

    private bool OperationFailed(Entity dbEntity)
    {
        return _entitiesStatusCollection.GetValueOrDefault(dbEntity);
    }

    private void OperationReStart(Entity dbEntity)
    {
        _entitiesStatusCollection[dbEntity] = false;
    }
}
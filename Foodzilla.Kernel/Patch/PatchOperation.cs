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
    private const string Id = "Id";

    internal Guid Guid;
    internal Entity Entity;
    internal object EntityId;
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

    public List<PatchInvalidResult> InvalidResults { get; init; } = [];

    private PatchOperation(ExpandoObject patchEntity)
    {
        Guid = Guid.Empty;

        _patchEntities = [patchEntity];

        _ignoreFields = [];
        _navigationProperties = [];
        _patchEntitiesDictionary = [];
        _originalValuesCollection = [];
        _entitiesStatusCollection = [];
        _entityPropertiesDictionary = [];

        _entityProperties = typeof(TEntity).GetProperties();

        InitializeIds();
    }

    private PatchOperation(List<ExpandoObject> patchEntities)
    {
        _patchEntities = patchEntities ?? throw new NullReferenceException();

        _ignoreFields = [];
        _navigationProperties = [];
        _patchEntitiesDictionary = [];
        _originalValuesCollection = [];
        _entitiesStatusCollection = [];
        _entityPropertiesDictionary = [];
        _entityProperties = typeof(TEntity).GetProperties();
    }

    public static PatchOperation<TEntity> Create(ExpandoObject patchEntity, string webPathRoot = null)
    {
        return new PatchOperation<TEntity>(patchEntity);
    }

    public static PatchOperation<TEntity> Create(List<ExpandoObject> patchEntities, string webPathRoot = null)
    {
        return new PatchOperation<TEntity>(patchEntities);
    }

    /// <summary>
    /// This method uses single patchEntity to update single database entity
    /// Navigation properties are independent of their parent so if parent patch fails, the navigation will stills accept changes
    /// Apply patch while patchEntity contains id
    /// Id is sent inside each patchEntity. peer to peer patching
    /// Only one instant is applied to a single database entity
    /// </summary>
    public void ApplyOneToOneRelatively(List<TEntity> dbEntities)
    {
        foreach (var dbEntity in dbEntities)
        {
            SetPatchEntity(dbEntity);

            foreach (var (property, value) in _patchEntity)
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
                        StoreShallowOriginalValues(commonProperty);

                        if (StoreNavigationProperties(commonProperty, value)) continue;

                        object castedValue = CastCorrectValue(commonProperty, value);

                        commonProperty.SetValue(Entity, castedValue);

                    }
                    catch (Exception exception)
                    {
                        AddErrorResult(commonProperty.Name, value?.ToString(), exception.Message);

                        Failed();
                    }
                }
                else
                {
                    AddErrorResult(property, value?.ToString(), PatchError.PropertyMatchingFailed);

                    Failed();
                }
            }

            if (OperationFailed() || !((TEntity)Entity).OnPatchCompleted())
            {
                RestoreOriginalValues();

                PatchShallowNavigationProperties(parentLoyalty: false);
            }
            else
            {
                PatchShallowNavigationProperties(parentLoyalty: true);

                OperationReStart();
            }
        }
    }

    /// <summary>
    /// This method uses single patchEntity to update single database entity
    /// Navigation properties are dependent of their parent so if parent patch fails, the navigation will not accept changes
    /// Apply patch while patchEntity contains id
    /// Id is sent inside each patchEntity. peer to peer patching
    /// Only one instant is applied to a single database entity
    /// </summary>
    public void ApplyOneToOneAbsolutely(List<TEntity> dbEntities)
    {
        foreach (var dbEntity in dbEntities)
        {
            SetPatchEntity(dbEntity);

            foreach (var (property, value) in _patchEntity)
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
                        StoreDeepOriginalValues(commonProperty);

                        if (StoreNavigationProperties(commonProperty, value)) continue;

                        object castedValue = CastCorrectValue(commonProperty, value);

                        commonProperty.SetValue(Entity, castedValue);

                    }
                    catch (Exception exception)
                    {
                        AddErrorResult(commonProperty.Name, value?.ToString(), exception.Message);

                        Failed();
                    }
                }
                else
                {
                    AddErrorResult(property, value?.ToString(), PatchError.PropertyMatchingFailed);

                    Failed();
                }
            }

            if (OperationFailed() || !((TEntity)Entity).OnPatchCompleted())
            {
                RestoreOriginalValues();

                PatchDeepNavigationProperties();
            }
            else
            {
                PatchDeepNavigationProperties();

                OperationReStart();
            }
        }
    }

    public void ApplyOneToOneParentDominance(List<TEntity> dbEntities)
    {
        foreach (var dbEntity in dbEntities)
        {
            SetPatchEntity(dbEntity);

            foreach (var (property, value) in _patchEntity)
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
                        StoreShallowOriginalValues(commonProperty);

                        if (StoreNavigationProperties(commonProperty, value)) continue;

                        object castedValue = CastCorrectValue(commonProperty, value);

                        commonProperty.SetValue(Entity, castedValue);

                    }
                    catch (Exception exception)
                    {
                        AddErrorResult(commonProperty.Name, value?.ToString(), exception.Message);

                        Failed();
                    }
                }
                else
                {
                    AddErrorResult(property, value?.ToString(), PatchError.PropertyMatchingFailed);

                    Failed();
                }
            }

            if (OperationFailed() || !((TEntity)Entity).OnPatchCompleted())
            {
                RestoreOriginalValues();
            }
            else
            {
                PatchShallowNavigationProperties();

                OperationReStart();
            }
        }
    }

    private void ApplyShallowOneToOne(Entity dbEntity)
    {
        SetPatchEntity(dbEntity);

        foreach (var (property, value) in _patchEntity)
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
                    StoreShallowOriginalValues(commonProperty);

                    if (StoreNavigationProperties(commonProperty, value)) continue;

                    object castedValue = CastCorrectValue(commonProperty, value);

                    commonProperty.SetValue(Entity, castedValue);

                }
                catch (Exception exception)
                {
                    AddErrorResult(commonProperty.Name, value?.ToString(), exception.Message);

                    Failed();
                }
            }
            else
            {
                AddErrorResult(property, value?.ToString(), PatchError.PropertyMatchingFailed);

                Failed();
            }
        }

        if (Entity is IPatchValidator patchValidator)
        {
            if (OperationFailed() || !patchValidator.OnPatchCompleted())
            {
                RestoreOriginalValues();
            }
            else
            {
                PatchShallowNavigationProperties();
            }
        }
        else
        {
            RestoreOriginalValues();
        }

        OperationReStart();
    }

    private void ApplyShallowOneToOne(Entity dbEntity, bool parentLoyalty)
    {
        SetPatchEntity(dbEntity);

        foreach (var (property, value) in _patchEntity)
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
                    if (parentLoyalty)
                    {
                        StoreShallowOriginalValues(commonProperty);

                        if (StoreNavigationProperties(commonProperty, value)) continue;

                        object castedValue = CastCorrectValue(commonProperty, value);

                        commonProperty.SetValue(Entity, castedValue);
                    }
                    else
                    {
                        StoreNavigationProperties(commonProperty, value);
                    }
                }
                catch (Exception exception)
                {
                    AddErrorResult(commonProperty.Name, value?.ToString(), exception.Message);

                    Failed();
                }
            }
            else
            {
                AddErrorResult(property, value?.ToString(), PatchError.PropertyMatchingFailed);

                Failed();
            }
        }

        if (Entity is IPatchValidator patchValidator)
        {
            if (OperationFailed() || !patchValidator.OnPatchCompleted())
            {
                RestoreOriginalValues();

                PatchShallowNavigationProperties(false);
            }
            else
            {
                PatchShallowNavigationProperties(true);
            }
        }
        else
        {
            RestoreOriginalValues();
        }

        OperationReStart();
    }

    private void ApplyDeepOneToOne(Entity dbEntity)
    {
        SetPatchEntity(dbEntity);

        foreach (var (property, value) in _patchEntity)
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
                    StoreDeepOriginalValues(commonProperty);

                    if (StoreNavigationProperties(commonProperty, value)) continue;

                    object castedValue = CastCorrectValue(commonProperty, value);

                    commonProperty.SetValue(Entity, castedValue);

                }
                catch (Exception exception)
                {
                    AddErrorResult(commonProperty.Name, value?.ToString(), exception.Message);

                    Failed();
                }
            }
            else
            {
                AddErrorResult(property, value?.ToString(), PatchError.PropertyMatchingFailed);

                Failed();
            }
        }

        if (Entity is IPatchValidator patchValidator)
        {
            if (OperationFailed() || !patchValidator.OnPatchCompleted())
            {
                RestoreOriginalValues();
            }

            PatchDeepNavigationProperties();
        }
        else
        {
            RestoreOriginalValues();
        }

        OperationReStart();
    }

    private void InitializeIds()
    {
        if (_patchEntities == null)
        {
            throw new NullReferenceException();
        }

        EntityIds = _patchEntities.Select(p => p.FirstOrDefault(q => q.Key.EqualsIgnoreCase(Id)).Value?.ToString()).ToList();
    }

    private void SetPatchEntity(Entity dbEntity)
    {
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

        Entity = dbEntity;
        EntityId = idValueString;
        _entityProperties = entityProperties;
    }

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
        List<object> objects = [];

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

    private void RestoreOriginalValues()
    {
        _originalValuesCollection.TryGetValue(Entity, out var originalValues);

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

                property.SetValue(Entity, instance);
            }
            else
            {
                property.SetValue(Entity, originalValue);

                originalValues.Remove(property);
            }
        }

        _originalValuesCollection.Remove(Entity);

        OperationReStart();
    }

    private void StoreDeepOriginalValues(PropertyInfo commonProperty)
    {
        object originalValue;

        if (commonProperty.InquireOneToOneNavigability(Entity, out var outEntity))
        {
            originalValue = outEntity;
        }

        else if (commonProperty.InquireOneToManyNavigability(Entity, out var outEntities))
        {
            originalValue = outEntities;
        }

        else
        {
            originalValue = commonProperty.GetValue(Entity);
        }

        if (_originalValuesCollection.TryGetValue(Entity, out var propertyValues))
        {
            propertyValues.Add(commonProperty, originalValue);
        }
        else
        {
            Dictionary<PropertyInfo, object> originalValues = new() { { commonProperty, originalValue } };

            _originalValuesCollection.Add(Entity, originalValues);
        }
    }

    private void StoreShallowOriginalValues(PropertyInfo commonProperty)
    {
        object originalValue;

        if (commonProperty.InquireOneToOneNavigability(Entity, out var outEntity))
        {
            originalValue = outEntity.Clone();
        }

        else if (commonProperty.InquireOneToManyNavigability(Entity, out var outEntities))
        {
            originalValue = outEntities.Select(entityItem => entityItem.Clone()).ToList();
        }

        else
        {
            originalValue = commonProperty.GetValue(Entity);
        }

        if (_originalValuesCollection.TryGetValue(Entity, out var propertyValues))
        {
            propertyValues.Add(commonProperty, originalValue);
        }
        else
        {
            Dictionary<PropertyInfo, object> originalValues = new() { { commonProperty, originalValue } };

            _originalValuesCollection.Add(Entity, originalValues);
        }
    }

    private bool StoreNavigationProperties(PropertyInfo commonProperty, object value)
    {
        if (commonProperty.InquireOneToOneNavigability(Entity, out var outEntity))
        {
            if (_navigationProperties.TryGetValue(Entity, out var propertyValues))
            {
                propertyValues.Add(outEntity, value);
            }
            else
            {
                _navigationProperties.Add(Entity, new Dictionary<object, object> { { outEntity, value } });
            }

            return true;
        }

        if (commonProperty.InquireOneToManyNavigability(Entity, out var outEntities))
        {
            if (_navigationProperties.TryGetValue(Entity, out var propertyValues))
            {
                propertyValues.Add(outEntities, value);
            }
            else
            {
                _navigationProperties.Add(Entity, new Dictionary<object, object> { { outEntities, value } });
            }

            return true;
        }

        return false;
    }

    private void PatchDeepNavigationProperties()
    {
        if (!_navigationProperties.TryGetValue(Entity, out var navigationProperties)) return;

        foreach (var (entity, value) in navigationProperties)
        {
            if (entity is Entity outEntity)
            {
                if (value != null)
                {
                    _entityProperties = Entity.GetRealType().GetProperties();

                    _entityPropertiesDictionary.Add(outEntity, entity.GetRealType().GetProperties());

                    _patchEntitiesDictionary.Add(outEntity, [(ExpandoObject)value]);

                    ApplyDeepOneToOne(outEntity);
                }
            }

            if (entity is IEnumerable<Entity> outEntities)
            {
                var outEntitiesList = outEntities.ToList();

                if (value == null || outEntitiesList.Count == 0) continue;

                _entityProperties = Entity.GetRealType().GetProperties();

                foreach (var unitEntity in outEntitiesList)
                {
                    _patchEntitiesDictionary.Add(unitEntity, (List<ExpandoObject>)value);

                    _entityPropertiesDictionary.Add(unitEntity, unitEntity.GetRealType().GetProperties());

                    ApplyDeepOneToOne(unitEntity);
                }
            }
        }
    }

    private void PatchShallowNavigationProperties()
    {
        if (!_navigationProperties.TryGetValue(Entity, out var navigationProperties)) return;

        foreach (var (entity, value) in navigationProperties)
        {
            if (entity is Entity outEntity)
            {
                if (value != null)
                {
                    _entityPropertiesDictionary.Add(outEntity, entity.GetRealType().GetProperties());

                    _patchEntitiesDictionary.Add(outEntity, [(ExpandoObject)value]);

                    ApplyShallowOneToOne(outEntity);

                    _entityProperties = Entity.GetRealType().GetProperties();
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

                    _entityProperties = Entity.GetRealType().GetProperties();
                }
            }
        }
    }

    private void PatchShallowNavigationProperties(bool parentLoyalty)
    {
        if (!_navigationProperties.TryGetValue(Entity, out var navigationProperties)) return;

        foreach (var (entity, value) in navigationProperties)
        {
            if (entity is Entity outEntity)
            {
                if (value != null)
                {
                    _entityProperties = Entity.GetRealType().GetProperties();

                    _entityPropertiesDictionary.Add(outEntity, entity.GetRealType().GetProperties());

                    _patchEntitiesDictionary.Add(outEntity, [(ExpandoObject)value]);

                    ApplyShallowOneToOne(outEntity, parentLoyalty);
                }
            }

            if (entity is IEnumerable<Entity> outEntities)
            {
                var outEntitiesList = outEntities.ToList();

                if (value == null || outEntitiesList.Count == 0) continue;

                _entityProperties = Entity.GetRealType().GetProperties();

                foreach (var unitEntity in outEntitiesList)
                {
                    _patchEntitiesDictionary.Add(unitEntity, (List<ExpandoObject>)value);

                    _entityPropertiesDictionary.Add(unitEntity, unitEntity.GetRealType().GetProperties());

                    ApplyShallowOneToOne(unitEntity, parentLoyalty);
                }
            }
        }
    }

    private void AddErrorResult(string field, object value, string message)
    {
        var result = PatchInvalidResult.Create(Id, field, value, message);
        InvalidResults.Add(result);
    }

    private void Failed()
    {
        _entitiesStatusCollection.TryAdd(Entity, true);
    }

    private bool OperationFailed()
    {
        return _entitiesStatusCollection.GetValueOrDefault(Entity);
    }

    private void OperationReStart()
    {
        _entitiesStatusCollection[Entity] = false;
    }
}
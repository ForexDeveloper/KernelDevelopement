using System.Dynamic;
using System.Text.Json;
using System.Reflection;
using System.ComponentModel;
using Foodzilla.Kernel.Domain;
using Foodzilla.Kernel.Extension;

namespace Foodzilla.Kernel.Patch;

public sealed class PatchDbContext<TEntity> where TEntity : Entity, IPatchValidator
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
    private readonly Dictionary<Entity, Dictionary<PropertyInfo, object>> _modifiedValuesCollection;

    public List<string> EntityIds { get; private set; }

    public List<PatchInvalidResult> InvalidResults { get; init; } = new();

    private PatchDbContext(ExpandoObject patchEntity, string webPathRoot, string rrr)
    {
        _patchEntity = patchEntity;
        Initialize(webPathRoot);
    }

    private PatchDbContext(ExpandoObject patchEntity, string webPathRoot)
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
        _modifiedValuesCollection = new Dictionary<Entity, Dictionary<PropertyInfo, object>>();

        InitializeIds(webPathRoot);
    }

    private PatchDbContext(List<ExpandoObject> patchEntities, string webPathRoot)
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
        _modifiedValuesCollection = new Dictionary<Entity, Dictionary<PropertyInfo, object>>();
    }

    public static PatchDbContext<TEntity> Create(List<ExpandoObject> patchEntities, string webPathRoot = null)
    {
        return new PatchDbContext<TEntity>(patchEntities, webPathRoot);
    }

    public static PatchDbContext<TEntity> Create(ExpandoObject patchEntity, string webPathRoot = null)
    {
        return new PatchDbContext<TEntity>(patchEntity, webPathRoot);
    }

    public bool ApplyOneToOneRelatively()
    {
        var dbEntity = Activator.CreateInstance<TEntity>();

        foreach (var patchEntity in _patchEntities)
        {
            foreach (var (property, value) in patchEntity)
            {
                var commonProperty = _entityProperties.SingleOrDefault(p => p.Name.EqualsIgnoreCase(property));

                if (commonProperty != null)
                {
                    if (_ignoreFields.Contains(commonProperty.Name.ToLower()))
                    {
                        AddErrorResult(null, property, value?.ToString(), PatchError.PropertyIgnoredToUpdate);
                        continue;
                    }

                    try
                    {
                        if (StoreNavigationProperties(dbEntity, commonProperty, value)) continue;

                        object castedValue = CastCorrectValue(commonProperty, value);

                        StoreModifiedReadyValues(dbEntity, commonProperty, castedValue);

                        commonProperty.SetValue(dbEntity, castedValue);

                    }
                    catch (Exception exception)
                    {
                        AddErrorResult(null, commonProperty.Name, value?.ToString(), exception.Message);

                        Failed(dbEntity);
                    }
                }
                else
                {
                    AddErrorResult(null, property, value?.ToString(), PatchError.PropertyMatchingFailed);

                    Failed(dbEntity);
                }
            }

            if (OperationFailed(dbEntity) || !dbEntity.OnPatchCompleted())
            {
                PatchNavigationProperties(dbEntity, parentLoyalty: false);
            }
            else
            {
                
                PatchNavigationProperties(dbEntity, parentLoyalty: true);
            }

            OperationReStart(dbEntity);
        }

        return true;
    }

    /// <summary>
    /// This method uses single patchEntity to update single database entity
    /// Navigation properties are dependent of their parent so if parent patch fails, the navigation will not accept changes
    /// Apply patch while patchEntity contains id
    /// Id is sent inside of each patchEntity. peer to peer patching
    /// Only one instant is apply to a single database entity
    /// </summary>
    public bool ApplyOneToOneAbsolutely()
    {
        return true;
    }

    public bool ApplyOneToOneParentDominance()
    {
        return true;
    }

    private void ApplyShallowOneToOne(Entity dbEntity, bool parentLoyalty)
    {
        //var idValueString = SetPatchEntity(dbEntity);

        foreach ((string property, object value) in _patchEntity)
        {
            var commonProperty = _entityProperties.SingleOrDefault(p => p.Name.EqualsIgnoreCase(property));

            if (commonProperty != null)
            {
                if (_ignoreFields.Contains(commonProperty.Name.ToLower()))
                {
                    AddErrorResult(null, property, value?.ToString(), PatchError.PropertyIgnoredToUpdate);
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
                    AddErrorResult(null, commonProperty.Name, value?.ToString(), exception.Message);

                    Failed(dbEntity);
                }
            }
            else
            {
                AddErrorResult(null, property, value?.ToString(), PatchError.PropertyMatchingFailed);

                Failed(dbEntity);
            }
        }

        if (dbEntity is IPatchValidator patchValidator)
        {
            if (OperationFailed(dbEntity) || !patchValidator.OnPatchCompleted())
            {
                PatchNavigationProperties(dbEntity, false);
            }
            else
            {
                //Attach to DbContext
                PatchNavigationProperties(dbEntity, true);
            }

            OperationReStart(dbEntity);
        }

        OperationReStart(dbEntity);
    }

    private void StoreModifiedReadyValues(Entity dbEntity, PropertyInfo commonProperty, object modifiedValue)
    {
        if (_modifiedValuesCollection.ContainsKey(dbEntity))
        {
            _modifiedValuesCollection[dbEntity].Add(commonProperty, modifiedValue);
        }
        else
        {
            Dictionary<PropertyInfo, object> originalValues = new() { { commonProperty, modifiedValue } };

            _modifiedValuesCollection.Add(dbEntity, originalValues);
        }
    }

    private void StoreShallowOriginalValues(Entity dbEntity, PropertyInfo commonProperty)
    {
        object originalValue = commonProperty.GetValue(dbEntity);

        if (originalValue is Entity entity)
        {
            originalValue = entity.Clone();
        }

        if (originalValue is IEnumerable<Entity> entities)
        {
            originalValue = entities.Select(entityItem => entityItem.Clone()).ToList();
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

    private void AttachToDbContext()
    {

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

            if (current.TryGetInt32(out var int32))
            {
                objects.Add(int32);
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

    private void Initialize(string contentRootPath)
    {
        if (_patchEntity == null)
        {
            throw new NullReferenceException();
        }

        _entityProperties = typeof(TEntity).GetProperties();
    }

    private void InitializeIds(string contentRootPath)
    {
        if (_patchEntities == null)
        {
            throw new NullReferenceException();
        }

        EntityIds = _patchEntities.Select(p => p.FirstOrDefault(q => q.Key.EqualsIgnoreCase(Id)).Value?.ToString()).ToList();
    }

    private void Failed(Entity dbEntity)
    {
        _entitiesStatusCollection.TryAdd(dbEntity, true);
    }

    private bool OperationFailed()
    {
        return _failed;
    }

    private void OperationReStart()
    {
        _failed = false;
    }

    private bool OperationFailed(Entity dbEntity)
    {
        return _entitiesStatusCollection.GetValueOrDefault(dbEntity);
    }

    private void OperationReStart(Entity dbEntity)
    {
        _entitiesStatusCollection[dbEntity] = false;
    }

    private void AAA()
    {
        //var propertyType = commonProperty.PropertyType.BaseType;

        //if (propertyType == typeof(ValueType))
        //{
        //    if (StoreNavigationProperties(dbEntity, commonProperty, value)) continue;

        //    object castedValue = CastCorrectValue(commonProperty, value);

        //    StoreModificationReadyValues(dbEntity, commonProperty, castedValue);
        //}
        //else
        //{
        //    while (propertyType != null)
        //    {
        //        propertyType = propertyType.BaseType;

        //        if (propertyType == typeof(Entity))
        //        {
        //            break;
        //        }

        //        if (propertyType is null)
        //        {
        //            throw new Exception(PatchError.PropertyIsNotDerivedFromEntity);
        //        }
        //    }
        //}
    }
}
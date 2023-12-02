using System.Dynamic;
using System.Text.Json;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices.ComTypes;
using Foodzilla.Kernel.Domain;
using Foodzilla.Kernel.Extension;
using Microsoft.EntityFrameworkCore;

namespace Foodzilla.Kernel.Patch;

public sealed class PatchDbContext<TContext, TEntity> where TContext : DbContext where TEntity : Entity, IPatchValidator
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

    public TContext DbContext { get; private set; }

    public List<string> EntityIds { get; private set; }

    public List<PatchInvalidResult> InvalidResults { get; init; } = new();

    private PatchDbContext(TContext dbContext, ExpandoObject patchEntity, string webPathRoot, string rrr)
    {
        DbContext = dbContext;
        _patchEntity = patchEntity;
        Initialize(webPathRoot);
    }

    private PatchDbContext(TContext dbContext, ExpandoObject patchEntity, string webPathRoot)
    {
        total++;
        Guid = new Guid();
        DbContext = dbContext;

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

    private PatchDbContext(TContext dbContext, List<ExpandoObject> patchEntities, string webPathRoot)
    {
        total++;
        DbContext = dbContext;

        _patchEntities = patchEntities ?? throw new NullReferenceException();

        _ignoreFields = new List<string>();
        _entityProperties = typeof(TEntity).GetProperties();
        _entitiesStatusCollection = new Dictionary<Entity, bool>();
        _entityPropertiesDictionary = new Dictionary<Entity, PropertyInfo[]>();
        _patchEntitiesDictionary = new Dictionary<Entity, List<ExpandoObject>>();
        _navigationProperties = new Dictionary<Entity, Dictionary<object, object>>();
        _originalValuesCollection = new Dictionary<Entity, Dictionary<PropertyInfo, object>>();
    }

    public bool ApplyOneToOneRelatively()
    {
        var dbEntity = new object() as Entity;

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
                        if (NavigationShallowPatchOperation(dbEntity, commonProperty, value)) continue;

                        //StoreShallowOriginalValues(dbEntity, commonProperty);

                        object castedValue = CastCorrectValue(commonProperty, value);

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

    private bool NavigationShallowPatchOperation(Entity dbEntity, PropertyInfo commonProperty, object value)
    {
        if (commonProperty.InquireOneToOneNavigability(dbEntity, out var outEntity))
        {
            if (value != null)
            {
                //StoreShallowOriginalValues(dbEntity, commonProperty);

                _entityPropertiesDictionary.Add(outEntity, outEntity.GetRealType().GetProperties());

                _patchEntitiesDictionary.Add(outEntity, new List<ExpandoObject> { (ExpandoObject)value });

                //ApplyShallowOneToOne(outEntity);

                _entityProperties = dbEntity.GetRealType().GetProperties();
            }

            return true;
        }

        if (commonProperty.InquireOneToManyNavigability(dbEntity, out var outEntities))
        {
            if (value != null && outEntities.Count != 0)
            {
                //StoreShallowOriginalValues(dbEntity, commonProperty);

                foreach (var unitEntity in outEntities)
                {
                    _patchEntitiesDictionary.Add(unitEntity, (List<ExpandoObject>)value);

                    _entityPropertiesDictionary.Add(unitEntity, unitEntity.GetRealType().GetProperties());

                    //ApplyShallowOneToOne(unitEntity);
                }

                _entityProperties = dbEntity.GetRealType().GetProperties();
            }

            return true;
        }

        return false;
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

}
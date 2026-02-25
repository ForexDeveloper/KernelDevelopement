using System.Dynamic;
using System.Text.Json;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using Foodzilla.Kernel.Domain;
using Foodzilla.Kernel.Extension;

namespace Foodzilla.Kernel.Patch;

public sealed class PatchDocument<TEntity> where TEntity : Entity, IPatchValidator
{
    private const string Id = "Id";

    internal Guid Guid;
    private bool _failed;
    internal TEntity Entity;
    internal object EntityId;
    private ExpandoObject _patchEntity;
    private readonly PropertyInfo[] _entityProperties;
    private readonly IEnumerable<string> _ignoreFields;
    private readonly List<ExpandoObject> _patchEntities;
    private readonly Dictionary<Entity, Dictionary<object, object>> _navigationProperties;
    private readonly Dictionary<Entity, Dictionary<PropertyInfo, object>> _originalValuesCollection;

    public List<string> EntityIds { get; private set; }

    public List<PatchInvalidResult> InvalidResults { get; init; } = [];

    private PatchDocument(ExpandoObject patchEntity)
    {
        Guid = Guid.Empty;

        _patchEntities = [patchEntity];
        _ignoreFields = [];
        _navigationProperties = [];
        _originalValuesCollection = [];

        _entityProperties = typeof(TEntity).GetProperties();

        InitializeIds();
    }

    private PatchDocument(List<ExpandoObject> patchEntities)
    {
        _patchEntities = patchEntities ?? throw new NullReferenceException();

        _ignoreFields = [];
        _navigationProperties = [];
        _originalValuesCollection = [];

        _entityProperties = typeof(TEntity).GetProperties();
    }

    public static PatchDocument<TEntity> Create(ExpandoObject patchEntity, string webPathRoot = null)
    {
        return new PatchDocument<TEntity>(patchEntity);
    }

    public static PatchDocument<TEntity> Create(List<ExpandoObject> patchEntities, string webPathRoot = null)
    {
        return new PatchDocument<TEntity>(patchEntities);
    }

    /// <summary>
    /// This method uses single patchEntity to update single database entity
    /// Navigation properties are independent of their parent so if parent patch fails, the navigation will still accept changes
    /// Apply patch while patchEntity contains id
    /// Id is sent inside each patchEntity. peer to peer patching
    /// Only one instant is applied to a single database entity
    /// </summary>
    public void ApplyOneToOneRelatively(List<TEntity> dbEntities, bool? parentAllegiance = null)
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
                        switch (parentAllegiance)
                        {
                            case null:
                            case true:

                                StoreOriginalValues(commonProperty);

                                if (StoreNavigationProperties(commonProperty, value)) continue;

                                object castedValue = CastCorrectValue(commonProperty, value);

                                commonProperty.SetValue(Entity, castedValue);

                                break;
                        }

                        StoreNavigationProperties(commonProperty, value);
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

            if (OperationFailed() || !Entity.OnPatchCompleted())
            {
                RestoreOriginalValues();

                PatchNavigationProperties(nameof(ApplyOneToOneRelatively), false);
            }
            else
            {
                PatchNavigationProperties(nameof(ApplyOneToOneRelatively), true);

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
                        StoreOriginalValues(commonProperty);

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

            if (OperationFailed() || !Entity.OnPatchCompleted())
            {
                RestoreOriginalValues();

                PatchNavigationProperties(nameof(ApplyOneToOneAbsolutely));
            }
            else
            {
                PatchNavigationProperties(nameof(ApplyOneToOneAbsolutely));

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
                        StoreOriginalValues(commonProperty);

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

            if (OperationFailed() || !Entity.OnPatchCompleted())
            {
                RestoreOriginalValues();
            }
            else
            {
                PatchNavigationProperties(nameof(ApplyOneToOneParentDominance));

                OperationReStart();
            }
        }
    }

    private void InitializeIds()
    {
        if (_patchEntities == null)
        {
            throw new NullReferenceException();
        }

        EntityIds = _patchEntities.Select(p => p.FirstOrDefault(q => q.Key.EqualsIgnoreCase(Id)).Value?.ToString()).ToList();
    }

    private void SetPatchEntity(TEntity dbEntity)
    {
        var idProperty = _entityProperties.First(p => p.Name.EqualsIgnoreCase(Id));

        var idValueString = idProperty.GetValue(dbEntity).ToString();

        _patchEntity = _patchEntities.Find(p =>
        {
            var value = p.FirstOrDefault(q => q.Key.EqualsIgnoreCase(Id)).Value;
            return value != null && value.ToString().Equals(idValueString);
        }) ?? new ExpandoObject();

        Entity = dbEntity;
        EntityId = idValueString;
    }

    private bool NavigationPatchOperation(PropertyInfo commonProperty, object value, string applyMethodName)
    {
        if (commonProperty.InquireOneToOneNavigability(Entity, out var outEntity))
        {
            CreatePatchDocument(outEntity, value, applyMethodName);

            return true;
        }

        if (commonProperty.InquireOneToManyNavigability(Entity, out var outEntities))
        {
            CreatePatchDocument(outEntities, value, applyMethodName);

            return true;
        }

        return false;
    }

    private void PatchNavigationProperties(string applyMethodName, bool? parentAllegiance = null)
    {
        if (!_navigationProperties.TryGetValue(Entity, out var navigationProperties)) return;

        foreach (var (outEntity, value) in navigationProperties)
        {
            if (outEntity is Entity entity)
            {
                CreatePatchDocument(entity, value, applyMethodName, parentAllegiance);
            }

            if (outEntity is IEnumerable<Entity> entities)
            {
                CreatePatchDocument(entities.ToList(), value, applyMethodName, parentAllegiance);
            }
        }
    }

    private static void CreatePatchDocument(Entity entity, object value, string applyMethodName, bool? parentAllegiance = null)
    {
        if (value == null)
        {
            return;
        }

        var patchEntity = (ExpandoObject)value;

        var proxyType = entity.GetRealType();

        var listProxyType = typeof(List<>).MakeGenericType(proxyType);

        var patchDocumentType = typeof(PatchDocument<>).MakeGenericType(proxyType!);

        var entities = ((IList)Activator.CreateInstance(listProxyType))!;

        entities.Add(entity);

        var patchDocument = Activator.CreateInstance(patchDocumentType, BindingFlags.Instance | BindingFlags.NonPublic,
            null, [patchEntity], null);

        MethodInfo methodInfo;

        if (parentAllegiance.HasValue)
        {
            methodInfo = patchDocumentType.GetMethod(applyMethodName, BindingFlags.Instance | BindingFlags.Public, null,
                [listProxyType, typeof(bool?)], null);

            methodInfo!.Invoke(patchDocument, [entities, parentAllegiance]);
        }
        else
        {
            methodInfo = patchDocumentType.GetMethod(applyMethodName, BindingFlags.Instance | BindingFlags.Public, null,
                [listProxyType], null);

            methodInfo!.Invoke(patchDocument, [entities]);
        }
    }

    private static void CreatePatchDocument(List<Entity> entities, object value, string applyMethodName, bool? parentAllegiance = null)
    {
        if (value == null || entities.Count == 0)
        {
            return;
        }

        var patchEntities = (List<ExpandoObject>)value;

        var proxyType = entities[0].GetRealType();

        var listProxyType = typeof(List<>).MakeGenericType(proxyType);

        var inputEntities = ((IList)Activator.CreateInstance(listProxyType))!;

        foreach (var entity in entities)
        {
            inputEntities.Add(entity);
        }

        var patchDocumentType = typeof(PatchDocument<>).MakeGenericType(proxyType!);

        var patchDocument = Activator.CreateInstance(patchDocumentType, BindingFlags.Instance | BindingFlags.NonPublic, null,
            [patchEntities], null);

        MethodInfo methodInfo;

        if (parentAllegiance.HasValue)
        {
            methodInfo = patchDocumentType.GetMethod(applyMethodName, BindingFlags.Instance | BindingFlags.Public, null,
                [listProxyType, typeof(bool?)], null);

            methodInfo!.Invoke(patchDocument, [inputEntities, parentAllegiance]);
        }
        else
        {
            methodInfo = patchDocumentType.GetMethod(applyMethodName, BindingFlags.Instance | BindingFlags.Public, null,
                [listProxyType], null);

            methodInfo!.Invoke(patchDocument, [inputEntities]);
        }
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

    private void StoreOriginalValues(PropertyInfo commonProperty)
    {
        object originalValue = commonProperty.GetValue(Entity);

        if (_originalValuesCollection.TryGetValue(Entity, out var value))
        {
            value.Add(commonProperty, originalValue);
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
            if (_navigationProperties.TryGetValue(Entity, out var property))
            {
                property.Add(outEntity, value);
            }
            else
            {
                _navigationProperties.Add(Entity, new Dictionary<object, object> { { outEntity, value } });
            }

            return true;
        }

        if (commonProperty.InquireOneToManyNavigability(Entity, out var outEntities))
        {
            if (_navigationProperties.TryGetValue(Entity, out var property))
            {
                property.Add(outEntities, value);
            }
            else
            {
                _navigationProperties.Add(Entity, new Dictionary<object, object> { { outEntities, value } });
            }

            return true;
        }

        return false;
    }

    private void AddErrorResult(string field, object value, string message)
    {
        var result = PatchInvalidResult.Create(EntityId, field, value, message);
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
}
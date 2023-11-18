using System.Collections;
using Foodzilla.Kernel.Domain;
using System.Reflection;
using System.Text.Json;

namespace Foodzilla.Kernel.Patch;

internal static class PatchExtension
{
    internal static bool IsList(this object obj)
    {
        if (obj == null) return false;
        return
            obj.GetType().IsGenericType &&
            obj.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
    }

    internal static Type GetRealType(this object obj)
    {
        Type type = obj.GetType();

        while (type.IsNested)
            type = type.BaseType;

        return type;
    }

    internal static bool IsBoolean(this Type propertyType)
    {
        return propertyType == typeof(bool);
    }

    internal static JsonElement JsonElement(this object @object)
    {
        return JsonSerializer.SerializeToElement(@object);
    }

    internal static bool InquireNullability(this PropertyInfo commonProperty, out Type propertyType)
    {
        propertyType = commonProperty.PropertyType;

        if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            propertyType = Nullable.GetUnderlyingType(propertyType);

            return true;
        }

        return false;
    }

    internal static bool InquireOneToOneNavigability<TEntity>(this PropertyInfo commonProperty, TEntity sourceEntity, out Entity outEntity)
    {
        if (commonProperty.GetValue(sourceEntity) is Entity dbEntity)
        {
            outEntity = dbEntity;
            return true;
        }
        else
        {
            outEntity = null;
            return false;
        }
    }

    internal static bool InquireOneToManyNavigability<TEntity>(this PropertyInfo commonProperty, TEntity sourceEntity, out List<Entity> outEntities)
    {
        if (commonProperty.GetValue(sourceEntity) is IEnumerable<Entity> dbEntities)
        {
            outEntities = dbEntities.ToList();
            return true;
        }
        else
        {
            outEntities = null;
            return false;
        }
    }
}
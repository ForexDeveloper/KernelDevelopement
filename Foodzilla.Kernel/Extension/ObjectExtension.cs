using System.Text.Json;

namespace Foodzilla.Kernel.Extension;

public static class ObjectExtension
{
    public static JsonElement JsonElement(this object @object)
    {
        return JsonSerializer.SerializeToElement(@object);
    }
}
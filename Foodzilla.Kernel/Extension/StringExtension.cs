namespace Foodzilla.Kernel.Extension;

public static class StringExtension
{
    public static bool EqualsIgnoreCase(this string originalObject, string comparingObject)
    {
        return originalObject.Equals(comparingObject, StringComparison.OrdinalIgnoreCase);
    }
}
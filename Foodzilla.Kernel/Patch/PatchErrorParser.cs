namespace Foodzilla.Kernel.Patch;

public static class PatchErrorParser
{
    public static string Parse(string errorMessage)
    {
        if (string.IsNullOrEmpty(errorMessage))
        {
            return null;
        }

        if (errorMessage.StartsWith("Requested value"))
        {
            return PatchError.PropertyValueOutOfRange;
        }

        if (errorMessage.Contains("recognized as a valid Boolean"))
        {
            return PatchError.PropertyValueOutOfRange;
        }

        if (errorMessage.Contains("cannot convert from (null)."))
        {
            return PatchError.PropertyNullOrEmpty;
        }
        
        if (errorMessage.Contains("is not a valid value for Int32"))
        {
            return PatchError.PropertyUnableToCastInt32;
        }

        if (errorMessage.Contains("is not a valid value for Int64"))
        {
            return PatchError.PropertyUnableToCastInt64;
        }
        
        if (errorMessage.Contains("is not a valid value for Decimal"))
        {
            return PatchError.PropertyUnableToCastDecimal;
        }

        if (errorMessage.Contains("is not a valid value for DateTimeOffset"))
        {
            return PatchError.PropertyUnableToCastDateTimeOffset;
        }

        if (errorMessage.Contains("is not a valid value for DateTime"))
        {
            return PatchError.PropertyUnableToCastDateTime;
        }

        if (errorMessage.Contains("is not a valid value for Double."))
        {
            return PatchError.PropertyUnableToCastDouble;
        }

        if (errorMessage.Contains("Guid should contain 32 digits with 4 dashes"))
        {
            return PatchError.PropertyUnableToCastGuid;
        }

        if (errorMessage.Contains("Specified argument was out of the range of valid values."))
        {
            return PatchError.PropertyValueOutOfRange;
        }
       
        if (errorMessage.Contains("Null object cannot be converted to a value type."))
        {
            return PatchError.PropertyNullOrEmpty;
        }

        return errorMessage;
    }
}
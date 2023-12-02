namespace Foodzilla.Kernel.Patch;

public static class PatchError
{
    public static string PropertyMatchingFailed = "فیلد مورد نظر یافت نشد";
    public static string PropertyIgnoredToUpdate = "فیلد مورد امکان ویرایش ندارد";
    public static string PropertyNullOrEmpty = "یا خالی باشد Null فیلد مورد نظر نمی تواند";
    public static string PropertyValueOutOfRange = "مقدار ارسال شده برای فیلد مورد نظر خارج از محدوده است";
    public static string PropertyUnableToCastGuid = "را ندارد Guid فرمت مقدار ارسال شده قابل تبدیل شدن به";
    public static string PropertyUnableToCastInt32 = "را ندارد In32 فرمت مقدار ارسال شده قابل تبدیل شدن به";
    public static string PropertyUnableToCastInt64 = "را ندارد In64 فرمت مقدار ارسال شده قابل تبدیل شدن به";
    public static string PropertyUnableToCastDouble = "را ندارد Double فرمت مقدار ارسال شده قابل تبدیل شدن به";
    public static string PropertyUnableToCastDecimal = "را ندارد Decimal فرمت مقدار ارسال شده قابل تبدیل شدن به";
    public static string PropertyUnableToCastDateTime = "را ندارد DateTime فرمت مقدار ارسال شده قابل تبدیل شدن به";
    public static string PropertyAcceptsOnlyStringOrStruct = "فیلد مورد نظر تنها امکان دریافت رشته یا عدد را دارد";
    public static string PropertyUnableToCastDateTimeOffset = "را ندارد DateTimeOffset فرمت مقدار ارسال شده قابل تبدیل شدن به";
    public static string PropertyIsNotDerivedFromEntity = "ارث بری ندارد Entity آبجکت مورد نظر از کلاس پایه";

    public static string PropertyUnableToCast(string fieldName)
    {
        return $"را ندارد {fieldName} فرمت مقدار ارسال شده قابل تبدیل شدن به";
    }
}
namespace Foodzilla.Kernel.Persistence.EF.Commons;

public static class ColumnType
{
    public const string Tinyint = "tinyint";

    public const string Date = "datetime";

    public const string DateTimeOffset = "datetimeoffset";

    public const string DateTime2WithMilliseconds = "datetime2(3)";

    public const string NvarcharMax = "nvarchar(4000)";

    public const string Nvarchar500 = "nvarchar(500)";

    public const string Nvarchar250 = "nvarchar(250)";

    public const string GeographyPoint = "geography";

    public static string Decimal(decimal start, decimal end) => $"decimal({start},{end})";
}
namespace Foodzilla.Kernel.Persistence.EF.Commons;

public static class Sequences<T>
{
    public static string Get() => $"{typeof(T).Name}Sequence";
}

public static class Sequences
{
    public const string OrderSequence = "OrderSequence";
    public const string ProductSequence = "ProductSequence";
}
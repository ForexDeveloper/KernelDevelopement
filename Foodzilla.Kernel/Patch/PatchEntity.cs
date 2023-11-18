using System.Dynamic;

namespace Foodzilla.Kernel.Patch;

public sealed class PatchEntity
{
    public string Path { get; init; } = string.Empty;

    public string Operation { get; init; } = "Replace";

    public ExpandoObject Value { get; init; } = new ExpandoObject();

    public PatchEntity(ExpandoObject value)
    {
        Value = value;
    }

    public PatchEntity()
    {

    }

    public ExpandoObject Clone()
    {
        return MemberwiseClone() as ExpandoObject;
    }
}
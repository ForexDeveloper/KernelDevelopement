namespace Foodzilla.Kernel.Patch;

public sealed record PatchInvalidResult
{
    public object EntityId { get; init; }

    public string Field { get; init; }

    public object Value { get; init; }

    public string Message { get; init; }

    private PatchInvalidResult(string field, object value, string message)
    {
        Field = field;
        Value = value;
        Message = message;
    }

    private PatchInvalidResult(object entityId, string field, object value, string message)
    {
        EntityId = entityId;
        Field = field;
        Value = value;
        Message = message;
    }

    public static PatchInvalidResult Create(string field, object value, string message)
    {
        return new PatchInvalidResult(field, value, message);
    }

    public static PatchInvalidResult Create(object entityId, string field, object value, string message)
    {
        return new PatchInvalidResult(entityId, field, value, PatchErrorParser.Parse(message));
    }
}
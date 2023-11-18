namespace Foodzilla.Kernel.Commons.Api;

public sealed class OFoodApiResponse<TResponse>
{
    public TResponse Response { get; init; }
}
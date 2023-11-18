using Foodzilla.Kernel.Patch;

namespace Foodzilla.Kernel.UnitTest.Patch;

internal static class PatchService
{
    public static async Task<PatchServiceResult> Execute()
    {
        await Task.CompletedTask;
        return PatchServiceResult.Create(null, null);
    }
}

internal sealed class PatchServiceResult
{
    public List<Customer> Customers { get; private set; }

    public PatchDocument<Customer> PagedDocument { get; private set; }

    private PatchServiceResult(List<Customer> customers, PatchDocument<Customer> pagedDocument)
    {
        Customers = customers;
        PagedDocument = pagedDocument;
    }

    public static PatchServiceResult Create(List<Customer> customers, PatchDocument<Customer> pagedDocument)
    {
        return new PatchServiceResult(customers, pagedDocument);
    }
}
using Xunit;
using Foodzilla.Kernel.Patch;
using Foodzilla.Kernel.UnitTest.Domain.ChiefOfficers;

namespace Foodzilla.Kernel.UnitTest.Patch;

public sealed class DbContextTests
{
    private const int TotalCount = 4;

    [Fact]
    public async Task Handle()
    {
        var executiveOfficers = PatchEngine.CreateChiefExecutiveOfficers(TotalCount);

        var patchExecutiveOfficers = PatchEngine.CreateValidPatchExecutiveOfficers(executiveOfficers);

        var patchDbContext = PatchDbContext<ChiefExecutiveOfficer>.Create(patchExecutiveOfficers);

        patchDbContext.ApplyOneToOneRelatively();

        await Task.CompletedTask;
    }
}
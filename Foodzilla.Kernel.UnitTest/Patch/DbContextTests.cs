using Xunit;
using Foodzilla.Kernel.Patch;
using Foodzilla.Kernel.UnitTest.Persistence;
using Foodzilla.Kernel.UnitTest.Domain.ChiefOfficers;
using Microsoft.EntityFrameworkCore;

namespace Foodzilla.Kernel.UnitTest.Patch;

public sealed class DbContextTests
{
    private const int TotalCount = 4;
    private readonly ApplicationDbContext _dbContext;

    [Fact]
    public async Task Handle()
    {
        var executiveOfficers = PatchEngine.CreateChiefExecutiveOfficers(TotalCount);

        var patchExecutiveOfficers = PatchEngine.CreateValidPatchExecutiveOfficers(executiveOfficers);

        var patchDbContext = PatchDbContext<DbContext, ChiefExecutiveOfficer>.Create(patchExecutiveOfficers);

        patchDbContext.ApplyOneToOneRelatively();

        await Task.CompletedTask;
    }
}
namespace Foodzilla.Kernel.UnitTest.Patch;

using Xunit;
using System.Dynamic;
using FluentAssertions;
using Domain.ChiefOfficers;
using Foodzilla.Kernel.Patch;
using FluentAssertions.Execution;

public sealed class AdvanceTheories
{
    private const int TotalCount = 1;
    private readonly int _validCount;
    private readonly int _leadCount;
    private readonly int _seniorCount;
    private readonly int _midlevelCount;
    private readonly int _juniorCount;
    private readonly int _fresherCount;

    public List<ExpandoObject> PatchOfficers { get; set; } = [];

    public readonly List<ChiefExecutiveOfficer> ChiefExecutiveOfficers;

    public AdvanceTheories()
    {
        _validCount = 1;

        _leadCount = _validCount * TotalCount;
        _seniorCount = _leadCount * TotalCount;
        _midlevelCount = _seniorCount * TotalCount;
        _juniorCount = _midlevelCount * TotalCount;
        _fresherCount = _juniorCount * TotalCount;

        ChiefExecutiveOfficers = PatchEngine.CreateChiefExecutiveOfficers(TotalCount);
    }

    #region PatchOperation

    [Fact]
    public async Task HandleSingleCoreAbsolutely_WhenAllPatchOfficersAreValid_ShouldPatchAll()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateValidPatchExecutiveOfficers(ChiefExecutiveOfficers);

        var patchOperation = PatchOperation<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchOperationAbsolutely(patchOperation);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefProductOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefTechnicalOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefMarketingOfficer!.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleSingleCoreRelatively_WhenAllPatchOfficersAreValid_ShouldPatchAll()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateValidPatchExecutiveOfficers(ChiefExecutiveOfficers);

        var patchOperation = PatchOperation<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchOperationRelatively(patchOperation);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefProductOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefTechnicalOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefMarketingOfficer!.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleSingleCoreParentDominance_WhenAllPatchOfficersAreValid_ShouldPatchAll()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateValidPatchExecutiveOfficers(ChiefExecutiveOfficers);

        var patchOperation = PatchOperation<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchOperationParentDominantly(patchOperation);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefProductOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefTechnicalOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefMarketingOfficer!.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleSingleCoreAbsolutely_WhenRootOfPatchOfficersAreInvalid_ShouldAvoidPatchForOnlyRoot()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateRootInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, _validCount);

        var patchOperation = PatchOperation<ChiefExecutiveOfficer>.Create(PatchOfficers);

        patchOperation.ApplyOneToOneAbsolutely(ChiefExecutiveOfficers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.Where(p => p.IsPatched()).Should().HaveCount(_validCount);

            ChiefExecutiveOfficers.Select(p => p.ChiefProductOfficer).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.Select(p => p.ChiefTechnicalOfficer).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.Select(p => p.ChiefMarketingOfficer).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleSingleCoreRelatively_WhenRootOfPatchOfficersAreInvalid_ShouldAvoidPatchForAllOfficers()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateRootInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, _validCount);

        var patchOperation = PatchOperation<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchOperationRelatively(patchOperation);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.Where(p => p.IsPatched()).Should().HaveCount(_validCount);

            ChiefExecutiveOfficers.Select(p => p.ChiefProductOfficer).Where(p => p!.IsPatched()).Should().HaveCount(_validCount);
            ChiefExecutiveOfficers.Select(p => p.ChiefTechnicalOfficer).Where(p => p!.IsPatched()).Should().HaveCount(_validCount);
            ChiefExecutiveOfficers.Select(p => p.ChiefMarketingOfficer).Where(p => p!.IsPatched()).Should().HaveCount(_validCount);


            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleSingleCoreParentDominance_WhenRootOfPatchOfficersAreInvalid_ShouldAvoidPatchPatchForAllOtherLevels()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateRootInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, _validCount);

        var patchOperation = PatchOperation<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchOperationParentDominantly(patchOperation);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.Where(p => p.IsPatched()).Should().HaveCount(_validCount);

            ChiefExecutiveOfficers.Select(p => p.ChiefProductOfficer).Where(p => p!.IsPatched()).Should().HaveCount(_validCount);
            ChiefExecutiveOfficers.Select(p => p.ChiefTechnicalOfficer).Where(p => p!.IsPatched()).Should().HaveCount(_validCount);
            ChiefExecutiveOfficers.Select(p => p.ChiefMarketingOfficer).Where(p => p!.IsPatched()).Should().HaveCount(_validCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads).Where(p => p.IsPatched()).Should().HaveCount(_leadCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads).Where(p => p.IsPatched()).Should().HaveCount(_leadCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads).Where(p => p.IsPatched()).Should().HaveCount(_leadCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads).Where(p => p.IsPatched()).Should().HaveCount(_leadCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads).Where(p => p.IsPatched()).Should().HaveCount(_leadCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).Where(p => p.IsPatched()).Should().HaveCount(_midlevelCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).Where(p => p.IsPatched()).Should().HaveCount(_midlevelCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).Where(p => p.IsPatched()).Should().HaveCount(_midlevelCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).Where(p => p.IsPatched()).Should().HaveCount(_juniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).Where(p => p.IsPatched()).Should().HaveCount(_juniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).Where(p => p.IsPatched()).Should().HaveCount(_juniorCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).Where(p => p.IsPatched()).Should().HaveCount(_fresherCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).Where(p => p.IsPatched()).Should().HaveCount(_fresherCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).Where(p => p.IsPatched()).Should().HaveCount(_fresherCount);
        }
    }

    [Fact]
    public async Task HandleSingleCoreAbsolutely_WhenCLevelsOfPatchOfficersAreInvalid_ShouldAvoidPatchForOnlyCLevels()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateCLevelInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, _validCount);

        var patchOperation = PatchOperation<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchOperationAbsolutely(patchOperation);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.Select(p => p.ChiefProductOfficer).Where(p => p!.IsPatched()).Should().HaveCount(_validCount);
            ChiefExecutiveOfficers.Select(p => p.ChiefTechnicalOfficer).Where(p => p!.IsPatched()).Should().HaveCount(_validCount);
            ChiefExecutiveOfficers.Select(p => p.ChiefMarketingOfficer).Where(p => p!.IsPatched()).Should().HaveCount(_validCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleSingleCoreRelatively_WhenCLevelsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAllTeamLeads()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateCLevelInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, _validCount);

        var patchOperation = PatchOperation<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchOperationRelatively(patchOperation);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.Select(p => p.ChiefProductOfficer).Where(p => p!.IsPatched()).Should().HaveCount(_validCount);
            ChiefExecutiveOfficers.Select(p => p.ChiefTechnicalOfficer).Where(p => p!.IsPatched()).Should().HaveCount(_validCount);
            ChiefExecutiveOfficers.Select(p => p.ChiefMarketingOfficer).Where(p => p!.IsPatched()).Should().HaveCount(_validCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads).Where(p => p.IsPatched()).Should().HaveCount(_leadCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads).Where(p => p.IsPatched()).Should().HaveCount(_leadCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads).Where(p => p.IsPatched()).Should().HaveCount(_leadCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads).Where(p => p.IsPatched()).Should().HaveCount(_leadCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads).Where(p => p.IsPatched()).Should().HaveCount(_leadCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleSingleCoreParentDominance_WhenCLevelsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAllOtherLevels()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateCLevelInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, _validCount);

        var patchOperation = PatchOperation<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchOperationParentDominantly(patchOperation);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.Select(p => p.ChiefProductOfficer).Where(p => p!.IsPatched()).Should().HaveCount(_validCount);
            ChiefExecutiveOfficers.Select(p => p.ChiefTechnicalOfficer).Where(p => p!.IsPatched()).Should().HaveCount(_validCount);
            ChiefExecutiveOfficers.Select(p => p.ChiefMarketingOfficer).Where(p => p!.IsPatched()).Should().HaveCount(_validCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads).Where(p => p.IsPatched()).Should().HaveCount(_leadCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads).Where(p => p.IsPatched()).Should().HaveCount(_leadCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads).Where(p => p.IsPatched()).Should().HaveCount(_leadCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads).Where(p => p.IsPatched()).Should().HaveCount(_leadCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads).Where(p => p.IsPatched()).Should().HaveCount(_leadCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).Where(p => p.IsPatched()).Should().HaveCount(_midlevelCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).Where(p => p.IsPatched()).Should().HaveCount(_midlevelCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).Where(p => p.IsPatched()).Should().HaveCount(_midlevelCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).Where(p => p.IsPatched()).Should().HaveCount(_juniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).Where(p => p.IsPatched()).Should().HaveCount(_juniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).Where(p => p.IsPatched()).Should().HaveCount(_juniorCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).Where(p => p.IsPatched()).Should().HaveCount(_fresherCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).Where(p => p.IsPatched()).Should().HaveCount(_fresherCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).Where(p => p.IsPatched()).Should().HaveCount(_fresherCount);
        }
    }

    [Fact]
    public async Task HandleSingleCoreAbsolutely_WhenLeadsOfPatchOfficersAreInvalid_ShouldAvoidPatchForOnlyLeads()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateLeadInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, _validCount);

        var patchOperation = PatchOperation<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchOperationAbsolutely(patchOperation);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.Select(p => p.ChiefProductOfficer).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.Select(p => p.ChiefTechnicalOfficer).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.Select(p => p.ChiefMarketingOfficer).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads).Where(p => p.IsPatched()).Should().HaveCount(_leadCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads).Where(p => p.IsPatched()).Should().HaveCount(_leadCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads).Where(p => p.IsPatched()).Should().HaveCount(_leadCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads).Where(p => p.IsPatched()).Should().HaveCount(_leadCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads).Where(p => p.IsPatched()).Should().HaveCount(_leadCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleSingleCoreRelatively_WhenLeadsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAllSeniors()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateLeadInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, _validCount);

        var patchOperation = PatchOperation<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchOperationRelatively(patchOperation);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.Select(p => p.ChiefProductOfficer).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.Select(p => p.ChiefTechnicalOfficer).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.Select(p => p.ChiefMarketingOfficer).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads).Where(p => p.IsPatched()).Should().HaveCount(_leadCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads).Where(p => p.IsPatched()).Should().HaveCount(_leadCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads).Where(p => p.IsPatched()).Should().HaveCount(_leadCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads).Where(p => p.IsPatched()).Should().HaveCount(_leadCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads).Where(p => p.IsPatched()).Should().HaveCount(_leadCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleSingleCoreParentDominance_WhenLeadsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAll_Seniors_Midlevels_Juniors_Freshers()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateLeadInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, _validCount);

        var patchOperation = PatchOperation<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchOperationParentDominantly(patchOperation);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.Select(p => p.ChiefProductOfficer).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.Select(p => p.ChiefTechnicalOfficer).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.Select(p => p.ChiefMarketingOfficer).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads).Where(p => p.IsPatched()).Should().HaveCount(_leadCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads).Where(p => p.IsPatched()).Should().HaveCount(_leadCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads).Where(p => p.IsPatched()).Should().HaveCount(_leadCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads).Where(p => p.IsPatched()).Should().HaveCount(_leadCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads).Where(p => p.IsPatched()).Should().HaveCount(_leadCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).Where(p => p.IsPatched()).Should().HaveCount(_midlevelCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).Where(p => p.IsPatched()).Should().HaveCount(_midlevelCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).Where(p => p.IsPatched()).Should().HaveCount(_midlevelCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).Where(p => p.IsPatched()).Should().HaveCount(_juniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).Where(p => p.IsPatched()).Should().HaveCount(_juniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).Where(p => p.IsPatched()).Should().HaveCount(_juniorCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).Where(p => p.IsPatched()).Should().HaveCount(_fresherCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).Where(p => p.IsPatched()).Should().HaveCount(_fresherCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).Where(p => p.IsPatched()).Should().HaveCount(_fresherCount);
        }
    }

    [Fact]
    public async Task HandleSingleCoreAbsolutely_WhenSeniorsOfPatchOfficersAreInvalid_ShouldAvoidPatchForOnlySeniors()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateSeniorInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, _validCount);

        var patchOperation = PatchOperation<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchOperationAbsolutely(patchOperation);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.All(p => p.ChiefProductOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefTechnicalOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefMarketingOfficer!.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleSingleCoreRelatively_WhenSeniorsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAllMidlevels()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateSeniorInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, _validCount);

        var patchOperation = PatchOperation<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchOperationRelatively(patchOperation);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.All(p => p.ChiefProductOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefTechnicalOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefMarketingOfficer!.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).Where(p => p.IsPatched()).Should().HaveCount(_midlevelCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).Where(p => p.IsPatched()).Should().HaveCount(_midlevelCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).Where(p => p.IsPatched()).Should().HaveCount(_midlevelCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleSingleCoreParentDominance_WhenSeniorsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAll_Midlevels_Juniors_Freshers()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateSeniorInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, _validCount);

        var patchOperation = PatchOperation<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchOperationParentDominantly(patchOperation);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.All(p => p.ChiefProductOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefTechnicalOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefMarketingOfficer!.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).Where(p => p.IsPatched()).Should().HaveCount(_seniorCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).Where(p => p.IsPatched()).Should().HaveCount(_midlevelCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).Where(p => p.IsPatched()).Should().HaveCount(_midlevelCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).Where(p => p.IsPatched()).Should().HaveCount(_midlevelCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).Where(p => p.IsPatched()).Should().HaveCount(_juniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).Where(p => p.IsPatched()).Should().HaveCount(_juniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).Where(p => p.IsPatched()).Should().HaveCount(_juniorCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).Where(p => p.IsPatched()).Should().HaveCount(_fresherCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).Where(p => p.IsPatched()).Should().HaveCount(_fresherCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).Where(p => p.IsPatched()).Should().HaveCount(_fresherCount);
        }
    }

    [Fact]
    public async Task HandleSingleCoreAbsolutely_WhenMidlevelsOfPatchOfficersAreInvalid_ShouldAvoidPatchForOnlyMidlevels()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateMidlevelInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, _validCount);

        var patchOperation = PatchOperation<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchOperationAbsolutely(patchOperation);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.Select(p => p.ChiefProductOfficer).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.Select(p => p.ChiefTechnicalOfficer).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.Select(p => p.ChiefMarketingOfficer).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).Where(p => p.IsPatched()).Should().HaveCount(_midlevelCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).Where(p => p.IsPatched()).Should().HaveCount(_midlevelCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).Where(p => p.IsPatched()).Should().HaveCount(_midlevelCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleSingleCoreRelatively_WhenMidlevelsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAllJuniors()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateMidlevelInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, _validCount);

        var patchOperation = PatchOperation<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchOperationRelatively(patchOperation);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.Select(p => p.ChiefProductOfficer).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.Select(p => p.ChiefTechnicalOfficer).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.Select(p => p.ChiefMarketingOfficer).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).Where(p => p.IsPatched()).Should().HaveCount(_midlevelCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).Where(p => p.IsPatched()).Should().HaveCount(_midlevelCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).Where(p => p.IsPatched()).Should().HaveCount(_midlevelCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).Where(p => p.IsPatched()).Should().HaveCount(_juniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).Where(p => p.IsPatched()).Should().HaveCount(_juniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).Where(p => p.IsPatched()).Should().HaveCount(_juniorCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleSingleCoreParentDominance_WhenMidlevelsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAll_Juniors_Freshers()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateMidlevelInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, _validCount);

        var patchOperation = PatchOperation<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchOperationParentDominantly(patchOperation);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.Select(p => p.ChiefProductOfficer).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.Select(p => p.ChiefTechnicalOfficer).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.Select(p => p.ChiefMarketingOfficer).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).Where(p => p.IsPatched()).Should().HaveCount(_midlevelCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).Where(p => p.IsPatched()).Should().HaveCount(_midlevelCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).Where(p => p.IsPatched()).Should().HaveCount(_midlevelCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).Where(p => p.IsPatched()).Should().HaveCount(_juniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).Where(p => p.IsPatched()).Should().HaveCount(_juniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).Where(p => p.IsPatched()).Should().HaveCount(_juniorCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).Where(p => p.IsPatched()).Should().HaveCount(_fresherCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).Where(p => p.IsPatched()).Should().HaveCount(_fresherCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).Where(p => p.IsPatched()).Should().HaveCount(_fresherCount);
        }
    }

    [Fact]
    public async Task HandleSingleCoreAbsolutely_WhenJuniorsOfPatchOfficersAreInvalid_ShouldAvoidPatchForOnlyJuniors()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateJuniorInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, _validCount);

        var patchOperation = PatchOperation<ChiefExecutiveOfficer>.Create(PatchOfficers);

        var juniorProduct = ChiefExecutiveOfficers.SelectMany(p =>
            p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q =>
                q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).FirstOrDefault();



        PatchOperationAbsolutely(patchOperation);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.All(p => p.ChiefProductOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefTechnicalOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefMarketingOfficer!.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).Where(p => p.IsPatched()).Should().HaveCount(_juniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).Where(p => p.IsPatched()).Should().HaveCount(_juniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).Where(p => p.IsPatched()).Should().HaveCount(_juniorCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleSingleCoreRelatively_WhenJuniorsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAllFreshers()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateJuniorInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, _validCount);

        var patchOperation = PatchOperation<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchOperationRelatively(patchOperation);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.All(p => p.ChiefProductOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefTechnicalOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefMarketingOfficer!.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).Where(p => p.IsPatched()).Should().HaveCount(_juniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).Where(p => p.IsPatched()).Should().HaveCount(_juniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).Where(p => p.IsPatched()).Should().HaveCount(_juniorCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).Where(p => p.IsPatched()).Should().HaveCount(_fresherCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).Where(p => p.IsPatched()).Should().HaveCount(_fresherCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).Where(p => p.IsPatched()).Should().HaveCount(_fresherCount);
        }
    }

    [Fact]
    public async Task HandleSingleCoreParentDominance_WhenJuniorsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAllFreshers()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateJuniorInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, _validCount);

        var patchOperation = PatchOperation<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchOperationParentDominantly(patchOperation);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.All(p => p.ChiefProductOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefTechnicalOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefMarketingOfficer!.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).Where(p => p.IsPatched()).Should().HaveCount(_juniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).Where(p => p.IsPatched()).Should().HaveCount(_juniorCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).Where(p => p.IsPatched()).Should().HaveCount(_juniorCount);

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).Where(p => p.IsPatched()).Should().HaveCount(_fresherCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).Where(p => p.IsPatched()).Should().HaveCount(_fresherCount);
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).Where(p => p.IsPatched()).Should().HaveCount(_fresherCount);
        }
    }

    #endregion


    #region PatchDocument

    [Fact]
    public async Task HandleAbsolutely_WhenAllPatchOfficersAreValid_ShouldPatchAll()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateValidPatchExecutiveOfficers(ChiefExecutiveOfficers);

        var patchDocument = PatchDocument<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchDocumentAbsolutely(patchDocument);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefProductOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefTechnicalOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefMarketingOfficer!.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleRelatively_WhenAllPatchOfficersAreValid_ShouldPatchAll()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateValidPatchExecutiveOfficers(ChiefExecutiveOfficers);

        var patchDocument = PatchDocument<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchDocumentRelatively(patchDocument);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefProductOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefTechnicalOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefMarketingOfficer!.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleParentDominance_WhenAllPatchOfficersAreValid_ShouldPatchAll()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateValidPatchExecutiveOfficers(ChiefExecutiveOfficers);

        var patchDocument = PatchDocument<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchDocumentRelatively(patchDocument);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefProductOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefTechnicalOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefMarketingOfficer!.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleAbsolutely_WhenRootOfPatchOfficersAreInvalid_ShouldAvoidPatchForOnlyRoot()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateRootInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, 0);

        var patchDocument = PatchDocument<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchDocumentAbsolutely(patchDocument);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.Select(p => p.ChiefProductOfficer).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.Select(p => p.ChiefTechnicalOfficer).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.Select(p => p.ChiefMarketingOfficer).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleRelatively_WhenRootOfPatchOfficersAreInvalid_ShouldAvoidPatchForAllOfficers()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateRootInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, 0);

        var patchDocument = PatchDocument<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchDocumentRelatively(patchDocument);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.Select(p => p.ChiefProductOfficer).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.Select(p => p.ChiefTechnicalOfficer).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.Select(p => p.ChiefMarketingOfficer).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleParentDominance_WhenRootOfPatchOfficersAreInvalid_ShouldAvoidPatchPatchForAllOtherLevels()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateRootInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, 0);

        var patchDocument = PatchDocument<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchDocumentParentDominantly(patchDocument);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.Select(p => p.ChiefProductOfficer).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.Select(p => p.ChiefTechnicalOfficer).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.Select(p => p.ChiefMarketingOfficer).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeFalse();
        }
    }

    [Fact]
    public async Task HandleAbsolutely_WhenCLevelsOfPatchOfficersAreInvalid_ShouldAvoidPatchForOnlyCLevels()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateCLevelInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, 0);

        var patchDocument = PatchDocument<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchDocumentAbsolutely(patchDocument);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.All(p => p.ChiefProductOfficer!.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.All(p => p.ChiefTechnicalOfficer!.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.All(p => p.ChiefMarketingOfficer!.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleRelatively_WhenCLevelsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAllTeamLeads()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateCLevelInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, 0);

        var patchDocument = PatchDocument<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchDocumentRelatively(patchDocument);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.All(p => p.ChiefProductOfficer!.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.All(p => p.ChiefTechnicalOfficer!.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.All(p => p.ChiefMarketingOfficer!.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleParentDominance_WhenCLevelsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAllOtherLevels()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateCLevelInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, 0);

        var patchDocument = PatchDocument<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchDocumentParentDominantly(patchDocument);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.All(p => p.ChiefProductOfficer!.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.All(p => p.ChiefTechnicalOfficer!.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.All(p => p.ChiefMarketingOfficer!.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeFalse();
        }
    }

    [Fact]
    public async Task HandleAbsolutely_WhenLeadsOfPatchOfficersAreInvalid_ShouldAvoidPatchForOnlyLeads()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateLeadInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, 0);

        var patchDocument = PatchDocument<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchDocumentAbsolutely(patchDocument);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.Select(p => p.ChiefProductOfficer).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.Select(p => p.ChiefTechnicalOfficer).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.Select(p => p.ChiefMarketingOfficer).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleRelatively_WhenLeadsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAllSeniors()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateLeadInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, 0);

        var patchDocument = PatchDocument<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchDocumentRelatively(patchDocument);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.Select(p => p.ChiefProductOfficer).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.Select(p => p.ChiefTechnicalOfficer).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.Select(p => p.ChiefMarketingOfficer).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleParentDominance_WhenLeadsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAll_Seniors_Midlevels_Juniors_Freshers()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateLeadInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, 0);

        var patchDocument = PatchDocument<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchDocumentParentDominantly(patchDocument);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.Select(p => p.ChiefProductOfficer).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.Select(p => p.ChiefTechnicalOfficer).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.Select(p => p.ChiefMarketingOfficer).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeFalse();
        }
    }

    [Fact]
    public async Task HandleAbsolutely_WhenSeniorsOfPatchOfficersAreInvalid_ShouldAvoidPatchForOnlySeniors()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateSeniorInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, 0);

        var patchDocument = PatchDocument<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchDocumentAbsolutely(patchDocument);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.All(p => p.ChiefProductOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefTechnicalOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefMarketingOfficer!.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleRelatively_WhenSeniorsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAllMidlevels()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateSeniorInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, 0);

        var patchDocument = PatchDocument<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchDocumentRelatively(patchDocument);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.All(p => p.ChiefProductOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefTechnicalOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefMarketingOfficer!.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleParentDominance_WhenSeniorsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAll_Midlevels_Juniors_Freshers()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateSeniorInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, 0);

        var patchDocument = PatchDocument<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchDocumentParentDominantly(patchDocument);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.All(p => p.ChiefProductOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefTechnicalOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefMarketingOfficer!.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeFalse();
        }
    }

    [Fact]
    public async Task HandleAbsolutely_WhenMidlevelsOfPatchOfficersAreInvalid_ShouldAvoidPatchForOnlyMidlevels()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateMidlevelInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, 0);

        var patchDocument = PatchDocument<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchDocumentAbsolutely(patchDocument);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.Select(p => p.ChiefProductOfficer).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.Select(p => p.ChiefTechnicalOfficer).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.Select(p => p.ChiefMarketingOfficer).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleRelatively_WhenMidlevelsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAllJuniors()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateMidlevelInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, 0);

        var patchDocument = PatchDocument<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchDocumentRelatively(patchDocument);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.Select(p => p.ChiefProductOfficer).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.Select(p => p.ChiefTechnicalOfficer).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.Select(p => p.ChiefMarketingOfficer).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleParentDominance_WhenMidlevelsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAll_Juniors_Freshers()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateMidlevelInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, 0);

        var patchDocument = PatchDocument<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchDocumentParentDominantly(patchDocument);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.Select(p => p.ChiefProductOfficer).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.Select(p => p.ChiefTechnicalOfficer).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.Select(p => p.ChiefMarketingOfficer).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeFalse();
        }
    }

    [Fact]
    public async Task HandleAbsolutely_WhenJuniorsOfPatchOfficersAreInvalid_ShouldAvoidPatchForOnlyJuniors()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateJuniorInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, 0);

        var patchDocument = PatchDocument<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchDocumentAbsolutely(patchDocument);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.All(p => p.ChiefProductOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefTechnicalOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefMarketingOfficer!.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleRelatively_WhenJuniorsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAllFreshers()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateJuniorInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, 0);

        var patchDocument = PatchDocument<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchDocumentRelatively(patchDocument);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.All(p => p.ChiefProductOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefTechnicalOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefMarketingOfficer!.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeFalse();
        }
    }

    [Fact]
    public async Task HandleParentDominance_WhenJuniorsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAllFreshers()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateJuniorInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers, 0);

        var patchDocument = PatchDocument<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchDocumentParentDominantly(patchDocument);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            ChiefExecutiveOfficers.All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.All(p => p.ChiefProductOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefTechnicalOfficer!.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.All(p => p.ChiefMarketingOfficer!.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ScrumMasterTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.QaTestingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors)).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels))).All(p => p.IsPatched()).Should().BeTrue();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeFalse();
        }
    }

    #endregion


    private void PatchDocumentAbsolutely(PatchDocument<ChiefExecutiveOfficer> patchDocument)
    {
        foreach (var chiefExecutiveOfficer in ChiefExecutiveOfficers)
        {
            patchDocument.ApplyOneToOneAbsolutely(chiefExecutiveOfficer);
        }
    }

    private void PatchDocumentRelatively(PatchDocument<ChiefExecutiveOfficer> patchDocument)
    {
        foreach (var chiefExecutiveOfficer in ChiefExecutiveOfficers)
        {
            patchDocument.ApplyOneToOneRelatively(chiefExecutiveOfficer);
        }
    }

    private void PatchDocumentParentDominantly(PatchDocument<ChiefExecutiveOfficer> patchDocument)
    {
        foreach (var chiefExecutiveOfficer in ChiefExecutiveOfficers)
        {
            patchDocument.ApplyOneToOneParentDominance(chiefExecutiveOfficer);
        }
    }

    private void PatchOperationAbsolutely(PatchOperation<ChiefExecutiveOfficer> patchOperation)
    {
        foreach (var chiefExecutiveOfficer in ChiefExecutiveOfficers)
        {
            patchOperation.ApplyOneToOneAbsolutely(chiefExecutiveOfficer);
        }
    }

    private void PatchOperationRelatively(PatchOperation<ChiefExecutiveOfficer> patchOperation)
    {
        foreach (var chiefExecutiveOfficer in ChiefExecutiveOfficers)
        {
            patchOperation.ApplyOneToOneRelatively(chiefExecutiveOfficer);
        }
    }

    private void PatchOperationParentDominantly(PatchOperation<ChiefExecutiveOfficer> patchOperation)
    {
        foreach (var chiefExecutiveOfficer in ChiefExecutiveOfficers)
        {
            patchOperation.ApplyOneToOneParentDominance(chiefExecutiveOfficer);
        }
    }
}
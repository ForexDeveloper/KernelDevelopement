namespace Foodzilla.Kernel.UnitTest.Patch;

using Xunit;
using System.Dynamic;
using FluentAssertions;
using Domain.ChiefOfficers;
using Foodzilla.Kernel.Patch;
using FluentAssertions.Execution;

public sealed class AdvanceTheories
{
    private const int TotalCount = 4;

    public List<ExpandoObject> PatchOfficers { get; set; }

    public readonly List<ChiefExecutiveOfficer> ChiefExecutiveOfficers;

    public AdvanceTheories()
    {
        ChiefExecutiveOfficers = PatchEngine.CreateChiefExecutiveOfficers(TotalCount);
        PatchOfficers = PatchEngine.CreateValidPatchExecutiveOfficers(ChiefExecutiveOfficers);
    }

    #region PatchOperation

    [Fact]
    public async Task HandleSingleCoreRelatively_WhenAllPatchOfficersAreValid_ShouldPatchAll()
    {
        const int totalCount = 10;

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

        var patchOperation = PatchOperation<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchOperationParentDominance(patchOperation);

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
    public async Task HandleSingleCoreRelatively_WhenRootOfPatchOfficersAreInvalid_ShouldAvoidPatchForAllOfficers()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateRootInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers);

        var patchOperation = PatchOperation<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchOperationRelatively(patchOperation);

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
    public async Task HandleSingleCoreParentDominance_WhenRootOfPatchOfficersAreInvalid_ShouldAvoidPatchPatchForAllOtherLevels()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateRootInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers);

        var patchOperation = PatchOperation<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchOperationParentDominance(patchOperation);

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
    public async Task HandleSingleCoreRelatively_WhenCLevelsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAllTeamLeads()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateCLevelInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers);

        var patchOperation = PatchOperation<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchOperationRelatively(patchOperation);

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
    public async Task HandleSingleCoreParentDominance_WhenCLevelsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAllOtherLevels()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateCLevelInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers);

        var patchOperation = PatchOperation<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchOperationParentDominance(patchOperation);

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
    public async Task HandleSingleCoreRelatively_WhenLeadsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAllSeniors()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateLeadInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers);

        var patchOperation = PatchOperation<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchOperationRelatively(patchOperation);

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
    public async Task HandleSingleCoreParentDominance_WhenLeadsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAll_Seniors_Midlevels_Juniors_Freshers()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateLeadInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers);

        var patchOperation = PatchOperation<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchOperationParentDominance(patchOperation);

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
    public async Task HandleSingleCoreRelatively_WhenSeniorsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAllMidlevels()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateSeniorInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers);

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
    public async Task HandleSingleCoreParentDominance_WhenSeniorsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAll_Midlevels_Juniors_Freshers()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateSeniorInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers);

        var patchOperation = PatchOperation<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchOperationParentDominance(patchOperation);

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
    public async Task HandleSingleCoreRelatively_WhenMidlevelsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAllJuniors()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateMidlevelInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers);

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
    public async Task HandleSingleCoreParentDominance_WhenMidlevelsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAll_Juniors_Freshers()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateMidlevelInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers);

        var patchOperation = PatchOperation<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchOperationParentDominance(patchOperation);

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
    public async Task HandleSingleCoreRelatively_WhenJuniorsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAllFreshers()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateJuniorInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers);

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

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors)))).All(p => p.IsPatched()).Should().BeFalse();

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer!.ProductTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer!.TechnicalTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer!.MarketingTeamLeads.SelectMany(q => q.Seniors.SelectMany(r => r.Midlevels.SelectMany(s => s.Juniors.SelectMany(t => t.Freshers))))).All(p => p.IsPatched()).Should().BeFalse();
        }
    }

    [Fact]
    public async Task HandleSingleCoreParentDominance_WhenJuniorsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAllFreshers()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateJuniorInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers);

        var patchOperation = PatchOperation<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchOperationParentDominance(patchOperation);

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


    #region PatchDocument

    [Fact]
    public async Task HandleRelatively_WhenAllPatchOfficersAreValid_ShouldPatchAll()
    {
        const int totalCount = 10;

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
    public async Task HandleRelatively_WhenRootOfPatchOfficersAreInvalid_ShouldAvoidPatchForAllOfficers()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateRootInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers);

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

        PatchOfficers = PatchEngine.CreateRootInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers);

        var patchDocument = PatchDocument<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchDocumentParentDominance(patchDocument);

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
    public async Task HandleRelatively_WhenCLevelsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAllTeamLeads()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateCLevelInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers);

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

        PatchOfficers = PatchEngine.CreateCLevelInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers);

        var patchDocument = PatchDocument<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchDocumentParentDominance(patchDocument);

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
    public async Task HandleRelatively_WhenLeadsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAllSeniors()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateLeadInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers);

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

        PatchOfficers = PatchEngine.CreateLeadInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers);

        var patchDocument = PatchDocument<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchDocumentParentDominance(patchDocument);

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
    public async Task HandleRelatively_WhenSeniorsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAllMidlevels()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateSeniorInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers);

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

        PatchOfficers = PatchEngine.CreateSeniorInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers);

        var patchDocument = PatchDocument<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchDocumentParentDominance(patchDocument);

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
    public async Task HandleRelatively_WhenMidlevelsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAllJuniors()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateMidlevelInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers);

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

        PatchOfficers = PatchEngine.CreateMidlevelInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers);

        var patchDocument = PatchDocument<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchDocumentParentDominance(patchDocument);

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
    public async Task HandleRelatively_WhenJuniorsOfPatchOfficersAreInvalid_ShouldAvoidPatchForAllFreshers()
    {
        const int totalCount = 10;

        PatchOfficers = PatchEngine.CreateJuniorInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers);

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

        PatchOfficers = PatchEngine.CreateJuniorInvalidPatchExecutiveOfficers(ChiefExecutiveOfficers);

        var patchDocument = PatchDocument<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchDocumentParentDominance(patchDocument);

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

    private void PatchDocumentRelatively(PatchDocument<ChiefExecutiveOfficer> patchDocument)
    {
        foreach (var chiefExecutiveOfficer in ChiefExecutiveOfficers)
        {
            patchDocument.ApplyOneToOneRelatively(chiefExecutiveOfficer);
        }
    }

    private void PatchDocumentParentDominance(PatchDocument<ChiefExecutiveOfficer> patchDocument)
    {
        foreach (var chiefExecutiveOfficer in ChiefExecutiveOfficers)
        {
            patchDocument.ApplyOneToOneParentDominance(chiefExecutiveOfficer);
        }
    }

    private void PatchOperationRelatively(PatchOperation<ChiefExecutiveOfficer> patchOperation)
    {
        foreach (var chiefExecutiveOfficer in ChiefExecutiveOfficers)
        {
            patchOperation.ApplyOneToOneRelatively(chiefExecutiveOfficer);
        }
    }

    private void PatchOperationParentDominance(PatchOperation<ChiefExecutiveOfficer> patchOperation)
    {
        foreach (var chiefExecutiveOfficer in ChiefExecutiveOfficers)
        {
            patchOperation.ApplyOneToOneParentDominance(chiefExecutiveOfficer);
        }
    }
}
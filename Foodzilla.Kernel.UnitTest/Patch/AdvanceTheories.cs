namespace Foodzilla.Kernel.UnitTest.Patch;

using Xunit;
using System.Dynamic;
using Newtonsoft.Json;
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
        PatchOfficers = PatchEngine.CreatePatchExecutiveOfficers(ChiefExecutiveOfficers);
    }

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
    public async Task HandleSingleCoreRelatively_WhenAllPatchOfficersAreValid_ShouldPatchAll()
    {
        const int totalCount = 10;

        var patchDocument = PatchOperation<ChiefExecutiveOfficer>.Create(PatchOfficers);

        PatchOperationRelatively(patchDocument);

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
    public async Task HandleRelatively_WhenRootOfPatchOfficersAreInvalid_ShouldNotPatchAllEntities()
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

            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer.ProductTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefProductOfficer.ScrumMasterTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer.TechnicalTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefTechnicalOfficer.QaTestingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();
            ChiefExecutiveOfficers.SelectMany(p => p.ChiefMarketingOfficer.MarketingTeamLeads.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();

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

    private void PatchDocumentRelatively(PatchDocument<ChiefExecutiveOfficer> patchDocument)
    {
        foreach (var chiefExecutiveOfficer in ChiefExecutiveOfficers)
        {
            patchDocument.ApplyOneToOneRelatively(chiefExecutiveOfficer);
        }
    }

    private void PatchOperationRelatively(PatchOperation<ChiefExecutiveOfficer> patchOperation)
    {
        foreach (var chiefExecutiveOfficer in ChiefExecutiveOfficers)
        {
            patchOperation.ApplyOneToOneRelatively(chiefExecutiveOfficer);
        }
    }
}
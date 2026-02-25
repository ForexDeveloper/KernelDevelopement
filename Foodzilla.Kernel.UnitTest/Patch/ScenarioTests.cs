using Foodzilla.Kernel.Extension;

namespace Foodzilla.Kernel.UnitTest.Patch;

using Castle.Core.Resource;
using Extension;
using FluentAssertions;
using FluentAssertions.Execution;
using Foodzilla.Kernel.Patch;
using Newtonsoft.Json;
using System.Dynamic;
using Xunit;

public sealed class ScenarioTests
{
    private const int TotalCount = 5;

    public readonly List<Customer> Customers = PatchBuilder.CreateComplexEntities(TotalCount);

    #region PatchDocument

    [Fact]
    public async Task HandleApplyOneToOneRelatively_WhenAllPatchEntitiesAreValid_ShouldPatchAll()
    {
        const int totalCount = 10;

        var patchEntities = CreateCompletePatchEntities();

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        patchDocument.ApplyRelatively(Customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            Customers.All(p => p.IsPatched()).Should().BeTrue();

            Customers.Select(p => p.NavigationOrder).All(p => p.IsPatched()).Should().BeTrue();
            Customers.Select(p => p.NavigationCustomer).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationListOrder.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationListCustomer.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            Customers.Select(p => p.NavigationCustomer.NavigationOrder).All(p => p.IsPatched()).Should().BeTrue();
            Customers.Select(p => p.NavigationCustomer.NavigationCustomer).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListOrder).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListCustomer).All(p => p.IsPatched()).Should().BeTrue();

            Customers.SelectMany(p => p.NavigationOrder.NavigationListOrder).All(p => p.IsPatched()).Should().BeTrue();

            Customers.SelectMany(p => p.NavigationListCustomer.SelectMany(q => q.NavigationListOrder)).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleApplyOneToOneRelatively_WhenRootOfPatchEntitiesAreInvalid_ShouldNotPatchAllEntities()
    {
        const int totalCount = 10;

        var patchEntities = CreateRootInvalidPatchEntities();

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        patchDocument.ApplyRelatively(Customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            Customers.All(p => p.IsPatched()).Should().BeFalse();

            Customers.Select(p => p.NavigationOrder).All(p => p.IsPatched()).Should().BeFalse();
            Customers.Select(p => p.NavigationCustomer).All(p => p.IsPatched()).Should().BeFalse();
            Customers.SelectMany(p => p.NavigationListOrder.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();
            Customers.SelectMany(p => p.NavigationListCustomer.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();

            Customers.Select(p => p.NavigationCustomer.NavigationOrder).All(p => p.IsPatched()).Should().BeTrue();
            Customers.Select(p => p.NavigationCustomer.NavigationCustomer).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListOrder).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListCustomer).All(p => p.IsPatched()).Should().BeTrue();

            Customers.SelectMany(p => p.NavigationOrder.NavigationListOrder).All(p => p.IsPatched()).Should().BeTrue();

            Customers.SelectMany(p => p.NavigationListCustomer.SelectMany(q => q.NavigationListOrder)).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleApplyOneToOneRelatively_WhenMiddleLayerOfPatchEntitiesAreInvalid_ShouldNotPatchAllEntities()
    {
        const int totalCount = 10;

        var patchEntities = CreateMiddleInvalidPatchEntities();

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        patchDocument.ApplyRelatively(Customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            Customers.All(p => p.IsPatched()).Should().BeTrue();

            Customers.Select(p => p.NavigationCustomer).All(p => p.IsPatched()).Should().BeFalse();
            Customers.Select(p => p.NavigationCustomer.NavigationOrder).All(p => p.IsPatched()).Should().BeFalse();
            Customers.Select(p => p.NavigationCustomer.NavigationCustomer).All(p => p.IsPatched()).Should().BeFalse();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListOrder).All(p => p.IsPatched()).Should().BeFalse();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListCustomer).All(p => p.IsPatched()).Should().BeFalse();

            Customers.Select(p => p.NavigationOrder).All(p => p.IsPatched()).Should().BeFalse();
            Customers.SelectMany(p => p.NavigationOrder.NavigationListOrder).All(p => p.IsPatched()).Should().BeFalse();

            Customers.SelectMany(p => p.NavigationListOrder.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationListCustomer.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationListCustomer.SelectMany(q => q.NavigationListOrder)).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleApplyOneToOneParentDominance_WhenAllPatchEntitiesAreValid_ShouldPatchAll()
    {
        const int totalCount = 10;

        var patchEntities = CreateCompletePatchEntities();

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        patchDocument.ApplyParentDominantly(Customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            Customers.All(p => p.IsPatched()).Should().BeTrue();

            Customers.Select(p => p.NavigationOrder).All(p => p.IsPatched()).Should().BeTrue();
            Customers.Select(p => p.NavigationCustomer).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationListOrder.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationListCustomer.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            Customers.Select(p => p.NavigationCustomer.NavigationOrder).All(p => p.IsPatched()).Should().BeTrue();
            Customers.Select(p => p.NavigationCustomer.NavigationCustomer).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListOrder).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListCustomer).All(p => p.IsPatched()).Should().BeTrue();

            Customers.SelectMany(p => p.NavigationOrder.NavigationListOrder).All(p => p.IsPatched()).Should().BeTrue();

            Customers.SelectMany(p => p.NavigationListCustomer.SelectMany(q => q.NavigationListOrder)).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleApplyOneToOneParentDominance_WhenRootOfPatchEntitiesAreInvalid_ShouldNotPatchAny()
    {
        const int totalCount = 10;

        var patchEntities = CreateRootInvalidPatchEntities();

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        patchDocument.ApplyParentDominantly(Customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            Customers.All(p => p.IsPatched()).Should().BeFalse();

            Customers.Select(p => p.NavigationOrder).All(p => p.IsPatched()).Should().BeFalse();
            Customers.Select(p => p.NavigationCustomer).All(p => p.IsPatched()).Should().BeFalse();
            Customers.SelectMany(p => p.NavigationListOrder.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();
            Customers.SelectMany(p => p.NavigationListCustomer.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();

            Customers.Select(p => p.NavigationCustomer.NavigationOrder).All(p => p.IsPatched()).Should().BeFalse();
            Customers.Select(p => p.NavigationCustomer.NavigationCustomer).All(p => p.IsPatched()).Should().BeFalse();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListOrder).All(p => p.IsPatched()).Should().BeFalse();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListCustomer).All(p => p.IsPatched()).Should().BeFalse();

            Customers.SelectMany(p => p.NavigationOrder.NavigationListOrder).All(p => p.IsPatched()).Should().BeFalse();

            Customers.SelectMany(p => p.NavigationListCustomer.SelectMany(q => q.NavigationListOrder)).All(p => p.IsPatched()).Should().BeFalse();

            Customers.SelectMany(p => p.NavigationListCustomer.SelectMany(q => q.NavigationListOrder)).All(p => p.IsPatched()).Should().BeFalse();
        }
    }

    [Fact]
    public async Task HandleApplyOneToOneParentDominance_WhenMiddleOfPatchEntitiesAreInvalid_ShouldNotPatchAll()
    {
        const int totalCount = 10;

        var patchEntities = CreateMiddleInvalidPatchEntities();

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        patchDocument.ApplyParentDominantly(Customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            Customers.All(p => p.IsPatched()).Should().BeTrue();

            Customers.Select(p => p.NavigationCustomer).All(p => p.IsPatched()).Should().BeFalse();
            Customers.Select(p => p.NavigationCustomer.NavigationOrder).All(p => p.IsPatched()).Should().BeFalse();
            Customers.Select(p => p.NavigationCustomer.NavigationCustomer).All(p => p.IsPatched()).Should().BeFalse();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListOrder).All(p => p.IsPatched()).Should().BeFalse();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListCustomer).All(p => p.IsPatched()).Should().BeFalse();

            Customers.Select(p => p.NavigationOrder).All(p => p.IsPatched()).Should().BeFalse();
            Customers.SelectMany(p => p.NavigationOrder.NavigationListOrder).All(p => p.IsPatched()).Should().BeFalse();

            Customers.SelectMany(p => p.NavigationListOrder.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationListCustomer.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationListCustomer.SelectMany(q => q.NavigationListOrder)).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleApplyOneToOneAbsolutely_WhenAllPatchEntitiesAreValid_ShouldPatchAll()
    {
        const int totalCount = 10;

        var patchEntities = CreateCompletePatchEntities();

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        patchDocument.ApplyAbsolutely(Customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            Customers.All(p => p.IsPatched()).Should().BeTrue();

            Customers.Select(p => p.NavigationOrder).All(p => p.IsPatched()).Should().BeTrue();
            Customers.Select(p => p.NavigationCustomer).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationListOrder.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationListCustomer.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            Customers.Select(p => p.NavigationCustomer.NavigationOrder).All(p => p.IsPatched()).Should().BeTrue();
            Customers.Select(p => p.NavigationCustomer.NavigationCustomer).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListOrder).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListCustomer).All(p => p.IsPatched()).Should().BeTrue();

            Customers.SelectMany(p => p.NavigationOrder.NavigationListOrder).All(p => p.IsPatched()).Should().BeTrue();

            Customers.SelectMany(p => p.NavigationListCustomer.SelectMany(q => q.NavigationListOrder)).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleApplyOneToOneAbsolutely_WhenRootOfPatchEntitiesAreInvalid_ShouldOnlyIgnorePatchForRoot()
    {
        const int totalCount = 10;

        var patchEntities = CreateRootInvalidPatchEntities();

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        patchDocument.ApplyAbsolutely(Customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            Customers.All(p => p.IsPatched()).Should().BeFalse();

            Customers.Select(p => p.NavigationOrder).All(p => p.IsPatched()).Should().BeTrue();
            Customers.Select(p => p.NavigationCustomer).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationListOrder.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationListCustomer.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            Customers.Select(p => p.NavigationCustomer.NavigationOrder).All(p => p.IsPatched()).Should().BeTrue();
            Customers.Select(p => p.NavigationCustomer.NavigationCustomer).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListOrder).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListCustomer).All(p => p.IsPatched()).Should().BeTrue();

            Customers.SelectMany(p => p.NavigationOrder.NavigationListOrder).All(p => p.IsPatched()).Should().BeTrue();

            Customers.SelectMany(p => p.NavigationListCustomer.SelectMany(q => q.NavigationListOrder)).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleApplyOneToOneAbsolutely_WhenMiddleLayerOfPatchEntitiesAreInvalid_ShouldOnlyIgnorePatchForMiddle()
    {
        const int totalCount = 10;

        var patchEntities = CreateMiddleInvalidPatchEntities();

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        patchDocument.ApplyAbsolutely(Customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            Customers.All(p => p.IsPatched()).Should().BeTrue();

            Customers.Select(p => p.NavigationOrder).All(p => p.IsPatched()).Should().BeFalse();
            Customers.Select(p => p.NavigationCustomer).All(p => p.IsPatched()).Should().BeFalse();

            Customers.Select(p => p.NavigationCustomer.NavigationOrder).All(p => p.IsPatched()).Should().BeTrue();
            Customers.Select(p => p.NavigationCustomer.NavigationCustomer).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListOrder).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListCustomer).All(p => p.IsPatched()).Should().BeTrue();

            Customers.SelectMany(p => p.NavigationOrder.NavigationListOrder).All(p => p.IsPatched()).Should().BeTrue();

            Customers.SelectMany(p => p.NavigationListOrder.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationListCustomer.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationListCustomer.SelectMany(q => q.NavigationListOrder)).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    #endregion


    #region PatchOperation

    [Fact]
    public async Task HandleSingleCoreApplyOneToOneRelatively_WhenAllPatchEntitiesAreValid_ShouldPatchAll()
    {
        const int totalCount = 10;

        var patchEntities = CreateCompletePatchEntities();

        var patchOperation = PatchOperation<Customer>.Create(patchEntities);

        patchOperation.ApplyRelatively(Customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            Customers.All(p => p.IsPatched()).Should().BeTrue();

            Customers.Select(p => p.NavigationOrder).All(p => p.IsPatched()).Should().BeTrue();
            Customers.Select(p => p.NavigationCustomer).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationListOrder.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationListCustomer.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            Customers.Select(p => p.NavigationCustomer.NavigationOrder).All(p => p.IsPatched()).Should().BeTrue();
            Customers.Select(p => p.NavigationCustomer.NavigationCustomer).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListOrder).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListCustomer).All(p => p.IsPatched()).Should().BeTrue();

            Customers.SelectMany(p => p.NavigationOrder.NavigationListOrder).All(p => p.IsPatched()).Should().BeTrue();

            Customers.SelectMany(p => p.NavigationListCustomer.SelectMany(q => q.NavigationListOrder)).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleSingleCoreApplyOneToOneRelatively_WhenRootOfPatchEntitiesAreInvalid_ShouldNotPatchAllEntities()
    {
        const int totalCount = 10;

        var patchEntities = CreateRootInvalidPatchEntities();

        var patchOperation = PatchOperation<Customer>.Create(patchEntities);

        patchOperation.ApplyRelatively(Customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            Customers.All(p => p.IsPatched()).Should().BeFalse();

            Customers.Select(p => p.NavigationOrder).All(p => p.IsPatched()).Should().BeFalse();
            Customers.Select(p => p.NavigationCustomer).All(p => p.IsPatched()).Should().BeFalse();
            Customers.SelectMany(p => p.NavigationListOrder.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();
            Customers.SelectMany(p => p.NavigationListCustomer.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();

            Customers.Select(p => p.NavigationCustomer.NavigationOrder).All(p => p.IsPatched()).Should().BeTrue();
            Customers.Select(p => p.NavigationCustomer.NavigationCustomer).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListOrder).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListCustomer).All(p => p.IsPatched()).Should().BeTrue();

            Customers.SelectMany(p => p.NavigationOrder.NavigationListOrder).All(p => p.IsPatched()).Should().BeTrue();

            Customers.SelectMany(p => p.NavigationListCustomer.SelectMany(q => q.NavigationListOrder)).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleSingleCoreApplyOneToOneRelatively_WhenMiddleLayerOfPatchEntitiesAreInvalid_ShouldNotPatchAllEntities()
    {
        const int totalCount = 10;

        var patchEntities = CreateMiddleInvalidPatchEntities();

        var patchOperation = PatchOperation<Customer>.Create(patchEntities);

        patchOperation.ApplyRelatively(Customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            Customers.All(p => p.IsPatched()).Should().BeTrue();

            Customers.Select(p => p.NavigationCustomer).All(p => p.IsPatched()).Should().BeFalse();
            Customers.Select(p => p.NavigationCustomer.NavigationOrder).All(p => p.IsPatched()).Should().BeFalse();
            Customers.Select(p => p.NavigationCustomer.NavigationCustomer).All(p => p.IsPatched()).Should().BeFalse();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListOrder).All(p => p.IsPatched()).Should().BeFalse();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListCustomer).All(p => p.IsPatched()).Should().BeFalse();

            Customers.Select(p => p.NavigationOrder).All(p => p.IsPatched()).Should().BeFalse();
            Customers.SelectMany(p => p.NavigationOrder.NavigationListOrder).All(p => p.IsPatched()).Should().BeFalse();

            Customers.SelectMany(p => p.NavigationListOrder.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationListCustomer.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationListCustomer.SelectMany(q => q.NavigationListOrder)).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleSingleCoreApplyOneToOneParentDominance_WhenAllPatchEntitiesAreValid_ShouldPatchAll()
    {
        const int totalCount = 10;

        var patchEntities = CreateCompletePatchEntities();

        var patchOperation = PatchOperation<Customer>.Create(patchEntities);

        patchOperation.ApplyParentDominantly(Customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            Customers.All(p => p.IsPatched()).Should().BeTrue();

            Customers.Select(p => p.NavigationOrder).All(p => p.IsPatched()).Should().BeTrue();
            Customers.Select(p => p.NavigationCustomer).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationListOrder.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationListCustomer.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            Customers.Select(p => p.NavigationCustomer.NavigationOrder).All(p => p.IsPatched()).Should().BeTrue();
            Customers.Select(p => p.NavigationCustomer.NavigationCustomer).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListOrder).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListCustomer).All(p => p.IsPatched()).Should().BeTrue();

            Customers.SelectMany(p => p.NavigationOrder.NavigationListOrder).All(p => p.IsPatched()).Should().BeTrue();

            Customers.SelectMany(p => p.NavigationListCustomer.SelectMany(q => q.NavigationListOrder)).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleSingleCoreApplyOneToOneParentDominance_WhenRootOfPatchEntitiesAreInvalid_ShouldNotPatchAny()
    {
        const int totalCount = 10;

        var patchEntities = CreateRootInvalidPatchEntities();

        var patchOperation = PatchOperation<Customer>.Create(patchEntities);

        patchOperation.ApplyParentDominantly(Customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            Customers.All(p => p.IsPatched()).Should().BeFalse();

            Customers.Select(p => p.NavigationOrder).All(p => p.IsPatched()).Should().BeFalse();
            Customers.Select(p => p.NavigationCustomer).All(p => p.IsPatched()).Should().BeFalse();
            Customers.SelectMany(p => p.NavigationListOrder.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();
            Customers.SelectMany(p => p.NavigationListCustomer.Select(q => q)).All(p => p.IsPatched()).Should().BeFalse();

            Customers.Select(p => p.NavigationCustomer.NavigationOrder).All(p => p.IsPatched()).Should().BeFalse();
            Customers.Select(p => p.NavigationCustomer.NavigationCustomer).All(p => p.IsPatched()).Should().BeFalse();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListOrder).All(p => p.IsPatched()).Should().BeFalse();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListCustomer).All(p => p.IsPatched()).Should().BeFalse();

            Customers.SelectMany(p => p.NavigationOrder.NavigationListOrder).All(p => p.IsPatched()).Should().BeFalse();

            Customers.SelectMany(p => p.NavigationListCustomer.SelectMany(q => q.NavigationListOrder)).All(p => p.IsPatched()).Should().BeFalse();

            Customers.SelectMany(p => p.NavigationListCustomer.SelectMany(q => q.NavigationListOrder)).All(p => p.IsPatched()).Should().BeFalse();
        }
    }

    [Fact]
    public async Task HandleSingleCoreApplyOneToOneParentDominance_WhenMiddleOfPatchEntitiesAreInvalid_ShouldNotPatchAll()
    {
        const int totalCount = 10;

        var patchEntities = CreateMiddleInvalidPatchEntities();

        var patchOperation = PatchOperation<Customer>.Create(patchEntities);

        patchOperation.ApplyParentDominantly(Customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            Customers.All(p => p.IsPatched()).Should().BeTrue();

            Customers.Select(p => p.NavigationCustomer).All(p => p.IsPatched()).Should().BeFalse();
            Customers.Select(p => p.NavigationCustomer.NavigationOrder).All(p => p.IsPatched()).Should().BeFalse();
            Customers.Select(p => p.NavigationCustomer.NavigationCustomer).All(p => p.IsPatched()).Should().BeFalse();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListOrder).All(p => p.IsPatched()).Should().BeFalse();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListCustomer).All(p => p.IsPatched()).Should().BeFalse();

            Customers.Select(p => p.NavigationOrder).All(p => p.IsPatched()).Should().BeFalse();
            Customers.SelectMany(p => p.NavigationOrder.NavigationListOrder).All(p => p.IsPatched()).Should().BeFalse();

            Customers.SelectMany(p => p.NavigationListOrder.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationListCustomer.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationListCustomer.SelectMany(q => q.NavigationListOrder)).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleSingleCoreApplyOneToOneAbsolutely_WhenAllPatchEntitiesAreValid_ShouldPatchAll()
    {
        const int totalCount = 10;

        var patchEntities = CreateCompletePatchEntities();

        var patchOperation = PatchOperation<Customer>.Create(patchEntities);

        patchOperation.ApplyAbsolutely(Customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            Customers.All(p => p.IsPatched()).Should().BeTrue();

            Customers.Select(p => p.NavigationOrder).All(p => p.IsPatched()).Should().BeTrue();
            Customers.Select(p => p.NavigationCustomer).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationListOrder.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationListCustomer.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            Customers.Select(p => p.NavigationCustomer.NavigationOrder).All(p => p.IsPatched()).Should().BeTrue();
            Customers.Select(p => p.NavigationCustomer.NavigationCustomer).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListOrder).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListCustomer).All(p => p.IsPatched()).Should().BeTrue();

            Customers.SelectMany(p => p.NavigationOrder.NavigationListOrder).All(p => p.IsPatched()).Should().BeTrue();

            Customers.SelectMany(p => p.NavigationListCustomer.SelectMany(q => q.NavigationListOrder)).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleSingleCoreApplyOneToOneAbsolutely_WhenRootOfPatchEntitiesAreInvalid_ShouldOnlyIgnorePatchForRoot()
    {
        const int totalCount = 10;

        var patchEntities = CreateRootInvalidPatchEntities();

        var patchOperation = PatchOperation<Customer>.Create(patchEntities);

        patchOperation.ApplyAbsolutely(Customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            Customers.All(p => p.IsPatched()).Should().BeFalse();

            Customers.Select(p => p.NavigationOrder).All(p => p.IsPatched()).Should().BeTrue();
            Customers.Select(p => p.NavigationCustomer).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationListOrder.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationListCustomer.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();

            Customers.Select(p => p.NavigationCustomer.NavigationOrder).All(p => p.IsPatched()).Should().BeTrue();
            Customers.Select(p => p.NavigationCustomer.NavigationCustomer).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListOrder).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListCustomer).All(p => p.IsPatched()).Should().BeTrue();

            Customers.SelectMany(p => p.NavigationOrder.NavigationListOrder).All(p => p.IsPatched()).Should().BeTrue();

            Customers.SelectMany(p => p.NavigationListCustomer.SelectMany(q => q.NavigationListOrder)).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleSingleCoreApplyOneToOneAbsolutely_WhenMiddleLayerOfPatchEntitiesAreInvalid_ShouldOnlyIgnorePatchForMiddle()
    {
        const int totalCount = 10;

        var patchEntities = CreateMiddleInvalidPatchEntities();

        var patchOperation = PatchOperation<Customer>.Create(patchEntities);

        patchOperation.ApplyAbsolutely(Customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            Customers.All(p => p.IsPatched()).Should().BeTrue();

            Customers.Select(p => p.NavigationOrder).All(p => p.IsPatched()).Should().BeFalse();
            Customers.Select(p => p.NavigationCustomer).All(p => p.IsPatched()).Should().BeFalse();

            Customers.Select(p => p.NavigationCustomer.NavigationOrder).All(p => p.IsPatched()).Should().BeTrue();
            Customers.Select(p => p.NavigationCustomer.NavigationCustomer).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListOrder).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationCustomer.NavigationListCustomer).All(p => p.IsPatched()).Should().BeTrue();

            Customers.SelectMany(p => p.NavigationOrder.NavigationListOrder).All(p => p.IsPatched()).Should().BeTrue();

            Customers.SelectMany(p => p.NavigationListOrder.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationListCustomer.Select(q => q)).All(p => p.IsPatched()).Should().BeTrue();
            Customers.SelectMany(p => p.NavigationListCustomer.SelectMany(q => q.NavigationListOrder)).All(p => p.IsPatched()).Should().BeTrue();
        }
    }

    #endregion

    private List<ExpandoObject> CreateCompletePatchEntities()
    {
        var patchEntities = CreateValidPatchEntities();

        return patchEntities;
    }

    private List<ExpandoObject> CreateRootInvalidPatchEntities()
    {
        var patchEntities = new List<ExpandoObject>();

        foreach (dynamic customer in Customers)
        {
            dynamic navigationListOrder = new List<ExpandoObject>();
            dynamic navigationListCustomer = new List<ExpandoObject>();

            dynamic navigationListOrder1 = new List<ExpandoObject>();
            dynamic navigationListCustomer1 = new List<ExpandoObject>();

            dynamic navigationListOrder2 = new List<ExpandoObject>();

            var patchEntity = CreatePatchEntity(customer.Id);
            var navigationOrder = CreatePatchEntity(customer.Id);
            var navigationCustomer = CreatePatchEntity(customer.Id);

            patchEntity.NavigationOrder = navigationOrder;
            patchEntity.NavigationCustomer = navigationCustomer;

            foreach (var navigateItem in customer.NavigationListCustomer)
            {
                var navigationOrderItem = CreatePatchEntity(navigateItem.Id);
                var navigationCustomerItem = CreatePatchEntity(navigateItem.Id);

                navigationListOrder.Add(navigationOrderItem);
                navigationListCustomer.Add(navigationCustomerItem);

                patchEntity.NavigationListOrder = navigationListOrder;
                patchEntity.NavigationListCustomer = navigationListCustomer;

                var navigationOrder1 = CreatePatchEntity(navigateItem.Id);
                var navigationCustomer1 = CreatePatchEntity(navigateItem.Id);

                patchEntity.NavigationCustomer.NavigationOrder = navigationOrder1;
                patchEntity.NavigationCustomer.NavigationCustomer = navigationCustomer1;

                var navigationOrderItem1 = CreatePatchEntity(navigateItem.Id);
                var navigationCustomerItem1 = CreatePatchEntity(navigateItem.Id);

                navigationListOrder1.Add(navigationOrderItem1);
                navigationListCustomer1.Add(navigationCustomerItem1);

                patchEntity.NavigationCustomer.NavigationListOrder = navigationListOrder1;
                patchEntity.NavigationCustomer.NavigationListCustomer = navigationListCustomer1;

                var navigationOrderItem2 = CreatePatchEntity(navigateItem.Id);
                navigationListOrder2.Add(navigationOrderItem2);
                patchEntity.NavigationOrder.NavigationListOrder = navigationListOrder2;

                dynamic navigationListOrder3 = new List<ExpandoObject>();

                foreach (var item in customer.NavigationListCustomer[0].NavigationListOrder)
                {
                    var navigationOrderItem3 = CreatePatchEntity(item.Id);
                    navigationCustomerItem.NavigationListOrder = navigationListOrder3;
                    navigationListOrder3.Add(navigationOrderItem3);
                }
            }

            patchEntity.Long = (-3000.2343).JsonElement();
            patchEntity.NullableLong = 30.4565.JsonElement();
            patchEntity.Boolean = null!;
            patchEntity.NullableBoolean = 12.JsonElement();
            patchEntity.Enum = "XXXXXXXX".JsonElement();
            patchEntity.NullableEnum = "XXXXXXXX".JsonElement();

            patchEntities.Add(patchEntity);
        }

        return patchEntities;
    }

    private List<ExpandoObject> CreateMiddleInvalidPatchEntities()
    {
        var patchEntities = new List<ExpandoObject>();

        foreach (dynamic customer in Customers)
        {
            dynamic navigationListOrder = new List<ExpandoObject>();
            dynamic navigationListCustomer = new List<ExpandoObject>();

            dynamic navigationListOrder1 = new List<ExpandoObject>();
            dynamic navigationListCustomer1 = new List<ExpandoObject>();

            dynamic navigationListOrder2 = new List<ExpandoObject>();

            var patchEntity = CreatePatchEntity(customer.Id);
            var navigationOrder = CreatePatchEntity(customer.Id);
            var navigationCustomer = CreatePatchEntity(customer.Id);

            patchEntity.NavigationOrder = navigationOrder;
            patchEntity.NavigationCustomer = navigationCustomer;

            foreach (var navigateItem in customer.NavigationListCustomer)
            {
                var navigationOrderItem = CreatePatchEntity(navigateItem.Id);
                var navigationCustomerItem = CreatePatchEntity(navigateItem.Id);

                navigationListOrder.Add(navigationOrderItem);
                navigationListCustomer.Add(navigationCustomerItem);

                patchEntity.NavigationListOrder = navigationListOrder;
                patchEntity.NavigationListCustomer = navigationListCustomer;

                var navigationOrder1 = CreatePatchEntity(navigateItem.Id);
                var navigationCustomer1 = CreatePatchEntity(navigateItem.Id);

                patchEntity.NavigationCustomer.NavigationOrder = navigationOrder1;
                patchEntity.NavigationCustomer.NavigationCustomer = navigationCustomer1;

                var navigationOrderItem1 = CreatePatchEntity(navigateItem.Id);
                var navigationCustomerItem1 = CreatePatchEntity(navigateItem.Id);

                navigationListOrder1.Add(navigationOrderItem1);
                navigationListCustomer1.Add(navigationCustomerItem1);

                patchEntity.NavigationCustomer.NavigationListOrder = navigationListOrder1;
                patchEntity.NavigationCustomer.NavigationListCustomer = navigationListCustomer1;

                var navigationOrderItem2 = CreatePatchEntity(navigateItem.Id);
                navigationListOrder2.Add(navigationOrderItem2);
                patchEntity.NavigationOrder.NavigationListOrder = navigationListOrder2;

                dynamic navigationListOrder3 = new List<ExpandoObject>();

                foreach (var item in customer.NavigationListCustomer[0].NavigationListOrder)
                {
                    var navigationOrderItem3 = CreatePatchEntity(item.Id);
                    navigationCustomerItem.NavigationListOrder = navigationListOrder3;
                    navigationListOrder3.Add(navigationOrderItem3);
                }
            }

            patchEntity.NavigationCustomer.Long = (-3000.2343).JsonElement();
            patchEntity.NavigationCustomer.NullableLong = 30.4565.JsonElement();
            patchEntity.NavigationCustomer.Boolean = null!;
            patchEntity.NavigationCustomer.NullableBoolean = 12.JsonElement();
            patchEntity.NavigationCustomer.Enum = "XXXXXXXX".JsonElement();
            patchEntity.NavigationCustomer.NullableEnum = "XXXXXXXX".JsonElement();

            patchEntity.NavigationOrder.Long = (-3000.2343).JsonElement();
            patchEntity.NavigationOrder.NullableLong = 30.4565.JsonElement();
            patchEntity.NavigationOrder.Boolean = null!;
            patchEntity.NavigationOrder.NullableBoolean = 12.JsonElement();
            patchEntity.NavigationOrder.Enum = "XXXXXXXX".JsonElement();
            patchEntity.NavigationOrder.NullableEnum = "XXXXXXXX".JsonElement();

            patchEntities.Add(patchEntity);
        }

        return patchEntities;
    }

    private List<ExpandoObject> CreateValidPatchEntities()
    {
        var patchEntities = new List<ExpandoObject>();

        foreach (dynamic customer in Customers)
        {
            dynamic navigationListOrder = new List<ExpandoObject>();
            dynamic navigationListCustomer = new List<ExpandoObject>();

            dynamic navigationListOrder1 = new List<ExpandoObject>();
            dynamic navigationListCustomer1 = new List<ExpandoObject>();

            dynamic navigationListOrder2 = new List<ExpandoObject>();

            var patchEntity = CreatePatchEntity(customer.Id);
            var navigationOrder = CreatePatchEntity(customer.Id);
            var navigationCustomer = CreatePatchEntity(customer.Id);

            patchEntity.NavigationOrder = navigationOrder;
            patchEntity.NavigationCustomer = navigationCustomer;

            foreach (var navigateItem in customer.NavigationListCustomer)
            {
                var navigationOrderItem = CreatePatchEntity(navigateItem.Id);
                var navigationCustomerItem = CreatePatchEntity(navigateItem.Id);

                navigationListOrder.Add(navigationOrderItem);
                navigationListCustomer.Add(navigationCustomerItem);

                patchEntity.NavigationListOrder = navigationListOrder;
                patchEntity.NavigationListCustomer = navigationListCustomer;

                var navigationOrder1 = CreatePatchEntity(navigateItem.Id);
                var navigationCustomer1 = CreatePatchEntity(navigateItem.Id);

                patchEntity.NavigationCustomer.NavigationOrder = navigationOrder1;
                patchEntity.NavigationCustomer.NavigationCustomer = navigationCustomer1;

                var navigationOrderItem1 = CreatePatchEntity(navigateItem.Id);
                var navigationCustomerItem1 = CreatePatchEntity(navigateItem.Id);

                navigationListOrder1.Add(navigationOrderItem1);
                navigationListCustomer1.Add(navigationCustomerItem1);

                patchEntity.NavigationCustomer.NavigationListOrder = navigationListOrder1;
                patchEntity.NavigationCustomer.NavigationListCustomer = navigationListCustomer1;

                var navigationOrderItem2 = CreatePatchEntity(navigateItem.Id);
                navigationListOrder2.Add(navigationOrderItem2);
                patchEntity.NavigationOrder.NavigationListOrder = navigationListOrder2;

                dynamic navigationListOrder3 = new List<ExpandoObject>();

                foreach (var item in customer.NavigationListCustomer[0].NavigationListOrder)
                {
                    var navigationOrderItem3 = CreatePatchEntity(item.Id);
                    navigationCustomerItem.NavigationListOrder = navigationListOrder3;
                    navigationListOrder3.Add(navigationOrderItem3);
                }
            }

            patchEntities.Add(patchEntity);
        }

        return patchEntities;
    }

    private static ExpandoObject CreatePatchEntity(int id)
    {
        var now = DateTimeOffset.Now;

        dynamic patchEntity = new ExpandoObject();

        patchEntity.Id = id.JsonElement();
        patchEntity.Name = "Patched !".JsonElement();
        patchEntity.Family = "Patched !".JsonElement();
        patchEntity.NationalCode = "4120583732".JsonElement();
        patchEntity.Long = 999999.JsonElement();
        patchEntity.NullableLong = 999999.JsonElement();
        patchEntity.Int = 999999.JsonElement();
        patchEntity.NullableInt = 999999.JsonElement();
        patchEntity.Decimal = 999999.123.JsonElement();
        patchEntity.NullableDecimal = 999999.123.JsonElement();
        patchEntity.Boolean = true.JsonElement();
        patchEntity.NullableBoolean = null!;
        patchEntity.Guid = Guid.NewGuid().JsonElement();
        patchEntity.NullableGuid = null!;
        patchEntity.DateTime = now.JsonElement();
        patchEntity.NullableDateTime = null!;
        patchEntity.Enum = 1.JsonElement();
        patchEntity.NullableEnum = "SILVER".JsonElement();

        return patchEntity;
    }
}
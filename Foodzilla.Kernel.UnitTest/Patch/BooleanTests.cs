namespace Foodzilla.Kernel.UnitTest.Patch;

using Xunit;
using FluentAssertions;
using Foodzilla.Kernel.Patch;
using FluentAssertions.Execution;

public sealed class BooleanTests
{
    #region Boolean

    [Fact]
    public async Task Handle_WhenSendStringTo_Boolean_ShouldCastBoolean()
    {
        const int validCountTrue = 100;
        const int validCountFalse = 200;

        List<(int, Dictionary<string, object>)> propertyValues =
        [
            (validCountTrue / 2, new Dictionary<string, object> { { nameof(Customer.Boolean), "1" } }),
            (validCountFalse / 2, new Dictionary<string, object> { { nameof(Customer.Boolean), "0" } }),
            (validCountTrue / 2, new Dictionary<string, object> { { nameof(Customer.Boolean), "TrUe" } }),
            (validCountFalse / 2, new Dictionary<string, object> { { nameof(Customer.Boolean), "FaLsE" } })
        ];

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCountTrue + validCountFalse);

        patchDocument.ApplyOneToOneRelatively(customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            patchDocument.InvalidResults.Should().BeNullOrEmpty();
            customers.Count(p => p.Boolean is true).Should().Be(validCountTrue);
            customers.Count(p => p.Boolean is false).Should().Be(validCountFalse);
        }
    }

    [Fact]
    public async Task Handle_WhenSendInvalidStringTo_Boolean_ShouldNotCastBoolean()
    {
        const int validCount = 100;
        const int invalidCount = 80;

        List<(int, Dictionary<string, object>)> propertyValues =
        [
            (validCount, new Dictionary<string, object> { { nameof(Customer.Boolean), "true" } }),
            (invalidCount / 2, new Dictionary<string, object> { { nameof(Customer.Boolean), "111" } }),
            (invalidCount / 2, new Dictionary<string, object> { { nameof(Customer.Boolean), "XXXXXX" } })
        ];

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCount + invalidCount);

        patchDocument.ApplyOneToOneRelatively(customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            customers.Where(p => p.Boolean is true).Should().HaveCount(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
        }
    }

    [Fact]
    public async Task Handle_WhenSendIntegerTo_Boolean_ShouldCastBoolean()
    {
        const int validCountTrue = 100;
        const int validCountFalse = 80;

        List<(int, Dictionary<string, object>)> propertyValues =
        [
            (validCountTrue, new Dictionary<string, object> { { nameof(Customer.Boolean), 1 } }),
            (validCountFalse, new Dictionary<string, object> { { nameof(Customer.Boolean), 0 } })
        ];

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCountTrue + validCountFalse);

        patchDocument.ApplyOneToOneRelatively(customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            patchDocument.InvalidResults.Should().BeNullOrEmpty();
            customers.Count(p => p.Boolean is true).Should().Be(validCountTrue);
            customers.Count(p => p.Boolean is false).Should().Be(validCountFalse);
        }
    }

    [Fact]
    public async Task Handle_WhenSendOutOfRangeIntegerTo_Boolean_ShouldNotCastBoolean()
    {
        const int validCount = 100;
        const int invalidCount = 80;

        List<(int, Dictionary<string, object>)> propertyValues =
        [
            (validCount, new Dictionary<string, object> { { nameof(Customer.Boolean), 1 } }),
            (invalidCount, new Dictionary<string, object> { { nameof(Customer.Boolean), 12 } })
        ];

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCount + invalidCount);

        patchDocument.ApplyOneToOneRelatively(customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            customers.Count(p => p.Boolean is true).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
        }
    }

    [Fact]
    public async Task Handle_WhenSendNullTo_Boolean_ShouldCastNotBoolean()
    {
        const int validCount = 100;
        const int invalidCount = 80;

        List<(int, Dictionary<string, object>)> propertyValues =
        [
            (validCount, new Dictionary<string, object> { { nameof(Customer.Boolean), true } }),
            (invalidCount, new Dictionary<string, object> { { nameof(Customer.Boolean), null! } })
        ];

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCount + invalidCount);

        patchDocument.ApplyOneToOneRelatively(customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            customers.Count(p => p.Boolean is true).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
        }
    }

    #endregion

    #region NullableBoolean

    [Fact]
    public async Task Handle_WhenSendStringTo_NullableBoolean_ShouldCastBoolean()
    {
        const int validCountTrue = 100;
        const int validCountFalse = 200;

        List<ValueTuple<int, Dictionary<string, object>>> propertyValues =
        [
            (validCountTrue / 2, new Dictionary<string, object> { { nameof(Customer.NullableBoolean), "1" } }),
            (validCountFalse / 2, new Dictionary<string, object> { { nameof(Customer.NullableBoolean), "0" } }),
            (validCountTrue / 2, new Dictionary<string, object> { { nameof(Customer.NullableBoolean), "TrUe" } }),
            (validCountFalse / 2, new Dictionary<string, object> { { nameof(Customer.NullableBoolean), "FaLsE" } })
        ];

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCountTrue + validCountFalse);

        patchDocument.ApplyOneToOneRelatively(customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            patchDocument.InvalidResults.Should().BeNullOrEmpty();
            customers.Count(p => p.NullableBoolean is true).Should().Be(validCountTrue);
            customers.Count(p => p.NullableBoolean is false).Should().Be(validCountFalse);
        }
    }

    [Fact]
    public async Task Handle_WhenSendInvalidStringTo_NullableBoolean_ShouldNotCastBoolean()
    {
        const int validCount = 100;
        const int invalidCount = 80;

        List<ValueTuple<int, Dictionary<string, object>>> propertyValues =
        [
            (validCount, new Dictionary<string, object> { { nameof(Customer.NullableBoolean), "true" } }),
            (invalidCount / 2, new Dictionary<string, object> { { nameof(Customer.NullableBoolean), "111" } }),
            (invalidCount / 2, new Dictionary<string, object> { { nameof(Customer.NullableBoolean), "XXXXXX" } })
        ];

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCount + invalidCount);

        patchDocument.ApplyOneToOneRelatively(customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            customers.Where(p => p.NullableBoolean is true).Should().HaveCount(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
        }
    }

    [Fact]
    public async Task Handle_WhenSendIntegerTo_NullableBoolean_ShouldCastBoolean()
    {
        const int validCountTrue = 100;
        const int validCountFalse = 80;

        List<ValueTuple<int, Dictionary<string, object>>> propertyValues =
        [
            (validCountTrue, new Dictionary<string, object> { { nameof(Customer.NullableBoolean), 1 } }),
            (validCountFalse, new Dictionary<string, object> { { nameof(Customer.NullableBoolean), 0 } })
        ];

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCountTrue + validCountFalse);

        patchDocument.ApplyOneToOneRelatively(customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            patchDocument.InvalidResults.Should().BeNullOrEmpty();
            customers.Count(p => p.NullableBoolean is true).Should().Be(validCountTrue);
            customers.Count(p => p.NullableBoolean is false).Should().Be(validCountFalse);
        }
    }

    [Fact]
    public async Task Handle_WhenSendOutOfRangeIntegerTo_NullableBoolean_ShouldNotCastBoolean()
    {
        const int validCount = 100;
        const int invalidCount = 80;

        List<ValueTuple<int, Dictionary<string, object>>> propertyValues =
        [
            (validCount, new Dictionary<string, object> { { nameof(Customer.NullableBoolean), 1 } }),
            (invalidCount, new Dictionary<string, object> { { nameof(Customer.NullableBoolean), 12 } })
        ];

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCount + invalidCount);

        patchDocument.ApplyOneToOneRelatively(customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            customers.Count(p => p.NullableBoolean is true).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
        }
    }

    [Fact]
    public async Task Handle_WhenSendNullTo_NullableBoolean_ShouldCastBoolean()
    {
        const int validCountNull = 80;
        const int validCountTrue = 100;
        const int validCountFalse = 100;

        List<ValueTuple<int, Dictionary<string, object>>> propertyValues =
        [
            (validCountNull, new Dictionary<string, object> { { nameof(Customer.NullableBoolean), null! } }),
            (validCountTrue, new Dictionary<string, object> { { nameof(Customer.NullableBoolean), true } }),
            (validCountFalse, new Dictionary<string, object> { { nameof(Customer.NullableBoolean), false } })
        ];

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCountNull + validCountFalse + validCountTrue);

        patchDocument.ApplyOneToOneRelatively(customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            patchDocument.InvalidResults.Should().BeNullOrEmpty();
            customers.Count(p => p.NullableBoolean is null).Should().Be(validCountNull);
            customers.Count(p => p.NullableBoolean is true).Should().Be(validCountTrue);
            customers.Count(p => p.NullableBoolean is false).Should().Be(validCountFalse);
        }
    }

    #endregion
}
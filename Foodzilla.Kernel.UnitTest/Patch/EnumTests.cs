namespace Foodzilla.Kernel.UnitTest.Patch;

using Xunit;
using FluentAssertions;
using Foodzilla.Kernel.Patch;
using FluentAssertions.Execution;

public sealed class EnumTests
{
    #region Enum

    [Fact]
    public async Task Handle_WhenSendStringTo_Enum_ShouldCastEnum()
    {
        const int validCountGold = 100;
        const int validCountSilver = 200;
        const int validCountNormal = 100;
        const int validCountDiamond = 200;

        List<(int, Dictionary<string, object>)> propertyValues =
        [
            (validCountGold / 2, new Dictionary<string, object> { { nameof(Customer.Enum), "2" } }),
            (validCountGold / 2, new Dictionary<string, object> { { nameof(Customer.Enum), "GOLD" } }),
            (validCountSilver / 2, new Dictionary<string, object> { { nameof(Customer.Enum), "1" } }),
            (validCountSilver / 2, new Dictionary<string, object> { { nameof(Customer.Enum), "SiLvEr" } }),
            (validCountNormal / 2, new Dictionary<string, object> { { nameof(Customer.Enum), "0" } }),
            (validCountNormal / 2, new Dictionary<string, object> { { nameof(Customer.Enum), "NorMaL" } }),
            (validCountDiamond / 2, new Dictionary<string, object> { { nameof(Customer.Enum), "3" } }),
            (validCountDiamond / 2, new Dictionary<string, object> { { nameof(Customer.Enum), "DIAMOND" } })
        ];

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCountNormal + validCountSilver + validCountGold + validCountDiamond);

        patchDocument.ApplyRelatively(customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            patchDocument.InvalidResults.Should().BeNullOrEmpty();
            customers.Count(p => p.Enum is RankingEnum.Gold).Should().Be(validCountGold);
            customers.Count(p => p.Enum is RankingEnum.Silver).Should().Be(validCountSilver);
            customers.Count(p => p.Enum is RankingEnum.Normal).Should().Be(validCountNormal);
            customers.Count(p => p.Enum is RankingEnum.Diamond).Should().Be(validCountDiamond);
        }
    }

    [Fact]
    public async Task Handle_WhenSendInvalidStringTo_Enum_ShouldNotCastEnum()
    {
        const int validCount = 100;
        const int invalidCount = 80;

        List<(int, Dictionary<string, object>)> propertyValues =
        [
            (validCount, new Dictionary<string, object> { { nameof(Customer.Enum), "SILVER" } }),
            (invalidCount, new Dictionary<string, object> { { nameof(Customer.Enum), "XXXXXX" } })
        ];

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCount + invalidCount);

        patchDocument.ApplyRelatively(customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            customers.Count(p => p.Enum is RankingEnum.Silver).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
        }
    }

    [Fact]
    public async Task Handle_WhenSendIntegerTo_Enum_ShouldCastEnum()
    {
        const int validCountGold = 100;
        const int validCountSilver = 200;
        const int validCountNormal = 100;
        const int validCountDiamond = 200;

        List<(int, Dictionary<string, object>)> propertyValues =
        [
            (validCountGold, new Dictionary<string, object> { { nameof(Customer.Enum), 2 } }),
            (validCountSilver, new Dictionary<string, object> { { nameof(Customer.Enum), 1 } }),
            (validCountNormal, new Dictionary<string, object> { { nameof(Customer.Enum), 0 } }),
            (validCountDiamond, new Dictionary<string, object> { { nameof(Customer.Enum), 3 } })
        ];

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCountNormal + validCountSilver + validCountGold + validCountDiamond);

        patchDocument.ApplyRelatively(customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            patchDocument.InvalidResults.Should().BeNullOrEmpty();
            customers.Count(p => p.Enum is RankingEnum.Gold).Should().Be(validCountGold);
            customers.Count(p => p.Enum is RankingEnum.Silver).Should().Be(validCountSilver);
            customers.Count(p => p.Enum is RankingEnum.Normal).Should().Be(validCountNormal);
            customers.Count(p => p.Enum is RankingEnum.Diamond).Should().Be(validCountDiamond);
        }
    }

    [Fact]
    public async Task Handle_WhenSendOutOfRangeIntegerTo_Enum_ShouldNotCastEnum()
    {
        const int validCount = 100;
        const int invalidCount = 80;

        List<(int, Dictionary<string, object>)> propertyValues =
        [
            (validCount, new Dictionary<string, object> { { nameof(Customer.Enum), 1 } }),
            (invalidCount, new Dictionary<string, object> { { nameof(Customer.Enum), 12 } })
        ];

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCount + invalidCount);

        patchDocument.ApplyRelatively(customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            customers.Count(p => p.Enum is RankingEnum.Silver).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
        }
    }

    [Fact]
    public async Task Handle_WhenSendNullTo_Enum_ShouldNotCastEnum()
    {
        const int validCount = 1;
        const int invalidCount = 5;

        List<(int, Dictionary<string, object>)> propertyValues =
        [
            (invalidCount, new Dictionary<string, object> { { nameof(Customer.Enum), null! } }),
            (validCount, new Dictionary<string, object> { { nameof(Customer.Enum), RankingEnum.Diamond } })
        ];

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCount + invalidCount);

        patchDocument.ApplyRelatively(customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            customers.Count(p => p.Enum is RankingEnum.Diamond).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
        }
    }

    #endregion

    #region NullableEnum

    [Fact]
    public async Task Handle_WhenSendStringTo_NullableEnum_ShouldCastEnum()
    {
        const int validCountGold = 100;
        const int validCountSilver = 200;
        const int validCountNormal = 100;
        const int validCountDiamond = 200;

        List<(int, Dictionary<string, object>)> propertyValues =
        [
            (validCountGold / 2, new Dictionary<string, object> { { nameof(Customer.NullableEnum), "2" } }),
            (validCountGold / 2, new Dictionary<string, object> { { nameof(Customer.NullableEnum), "GOLD" } }),
            (validCountSilver / 2, new Dictionary<string, object> { { nameof(Customer.NullableEnum), "1" } }),
            (validCountSilver / 2, new Dictionary<string, object> { { nameof(Customer.NullableEnum), "SiLvEr" } }),
            (validCountNormal / 2, new Dictionary<string, object> { { nameof(Customer.NullableEnum), "0" } }),
            (validCountNormal / 2, new Dictionary<string, object> { { nameof(Customer.NullableEnum), "NorMaL" } }),
            (validCountDiamond / 2, new Dictionary<string, object> { { nameof(Customer.NullableEnum), "3" } }),
            (validCountDiamond / 2, new Dictionary<string, object> { { nameof(Customer.NullableEnum), "DIAMOND" } })
        ];

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCountNormal + validCountSilver + validCountGold + validCountDiamond);

        patchDocument.ApplyRelatively(customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            patchDocument.InvalidResults.Should().BeNullOrEmpty();
            customers.Count(p => p.NullableEnum is RankingEnum.Gold).Should().Be(validCountGold);
            customers.Count(p => p.NullableEnum is RankingEnum.Silver).Should().Be(validCountSilver);
            customers.Count(p => p.NullableEnum is RankingEnum.Normal).Should().Be(validCountNormal);
            customers.Count(p => p.NullableEnum is RankingEnum.Diamond).Should().Be(validCountDiamond);
        }
    }

    [Fact]
    public async Task Handle_WhenSendInvalidStringTo_NullableEnum_ShouldNotCastEnum()
    {
        const int validCount = 100;
        const int invalidCount = 80;

        List<(int, Dictionary<string, object>)> propertyValues =
        [
            (validCount, new Dictionary<string, object> { { nameof(Customer.NullableEnum), "GOLD" } }),
            (invalidCount, new Dictionary<string, object> { { nameof(Customer.NullableEnum), "XXXXXX" } })
        ];

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCount + invalidCount);

        patchDocument.ApplyRelatively(customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            customers.Count(p => p.NullableEnum is RankingEnum.Gold).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
        }
    }

    [Fact]
    public async Task Handle_WhenSendIntegerTo_NullableEnum_ShouldCastEnum()
    {
        const int validCountGold = 100;
        const int validCountSilver = 200;
        const int validCountNormal = 100;
        const int validCountDiamond = 200;

        List<(int, Dictionary<string, object>)> propertyValues =
        [
            (validCountGold, new Dictionary<string, object> { { nameof(Customer.NullableEnum), 2 } }),
            (validCountSilver, new Dictionary<string, object> { { nameof(Customer.NullableEnum), 1 } }),
            (validCountNormal, new Dictionary<string, object> { { nameof(Customer.NullableEnum), 0 } }),
            (validCountDiamond, new Dictionary<string, object> { { nameof(Customer.NullableEnum), 3 } })
        ];

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCountNormal + validCountSilver + validCountGold + validCountDiamond);

        patchDocument.ApplyRelatively(customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            patchDocument.InvalidResults.Should().BeNullOrEmpty();
            customers.Count(p => p.NullableEnum is RankingEnum.Gold).Should().Be(validCountGold);
            customers.Count(p => p.NullableEnum is RankingEnum.Silver).Should().Be(validCountSilver);
            customers.Count(p => p.NullableEnum is RankingEnum.Normal).Should().Be(validCountNormal);
            customers.Count(p => p.NullableEnum is RankingEnum.Diamond).Should().Be(validCountDiamond);
        }
    }

    [Fact]
    public async Task Handle_WhenSendOutOfRangeIntegerTo_NullableEnum_ShouldNotCastEnum()
    {
        const int validCount = 100;
        const int invalidCount = 80;

        List<(int, Dictionary<string, object>)> propertyValues =
        [
            (validCount, new Dictionary<string, object> { { nameof(Customer.NullableEnum), 1 } }),
            (invalidCount, new Dictionary<string, object> { { nameof(Customer.NullableEnum), 12 } })
        ];

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCount + invalidCount);

        patchDocument.ApplyRelatively(customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            customers.Count(p => p.NullableEnum is RankingEnum.Silver).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
        }
    }

    [Fact]
    public async Task Handle_WhenSendNullTo_NullableEnum_ShouldCastEnum()
    {
        const int validCountNull = 80;
        const int validCountGold = 100;
        const int validCountSilver = 200;
        const int validCountNormal = 100;
        const int validCountDiamond = 200;

        List<(int, Dictionary<string, object>)> propertyValues =
        [
            (validCountNull, new Dictionary<string, object> { { nameof(Customer.NullableEnum), null! } }),
            (validCountGold, new Dictionary<string, object> { { nameof(Customer.NullableEnum), RankingEnum.Gold } }),
            (validCountNormal, new Dictionary<string, object> { { nameof(Customer.NullableEnum), RankingEnum.Normal } }),
            (validCountSilver, new Dictionary<string, object> { { nameof(Customer.NullableEnum), RankingEnum.Silver } }),
            (validCountDiamond, new Dictionary<string, object> { { nameof(Customer.NullableEnum), RankingEnum.Diamond } })
        ];

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCountNull + validCountNormal + validCountSilver + validCountGold + validCountDiamond);

        patchDocument.ApplyRelatively(customers);

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            patchDocument.InvalidResults.Should().BeNullOrEmpty();
            customers.Count(p => p.NullableEnum is null).Should().Be(validCountNull);
            customers.Count(p => p.NullableEnum is RankingEnum.Gold).Should().Be(validCountGold);
            customers.Count(p => p.NullableEnum is RankingEnum.Silver).Should().Be(validCountSilver);
            customers.Count(p => p.NullableEnum is RankingEnum.Normal).Should().Be(validCountNormal);
            customers.Count(p => p.NullableEnum is RankingEnum.Diamond).Should().Be(validCountDiamond);
        }
    }

    #endregion
}
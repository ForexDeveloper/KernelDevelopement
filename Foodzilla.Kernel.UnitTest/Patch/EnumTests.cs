namespace Foodzilla.Kernel.UnitTest.Patch;

using Xunit;
using System.Dynamic;
using FluentAssertions;
using System.Text.Json;
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

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCountGold / 2, new Dictionary<string, object> { { nameof(Customer.Enum), "2" } }));
        propertyValues.Add((validCountGold / 2, new Dictionary<string, object> { { nameof(Customer.Enum), "GOLD" } }));
        propertyValues.Add((validCountSilver / 2, new Dictionary<string, object> { { nameof(Customer.Enum), "1" } }));
        propertyValues.Add((validCountSilver / 2, new Dictionary<string, object> { { nameof(Customer.Enum), "SiLvEr" } }));
        propertyValues.Add((validCountNormal / 2, new Dictionary<string, object> { { nameof(Customer.Enum), "0" } }));
        propertyValues.Add((validCountNormal / 2, new Dictionary<string, object> { { nameof(Customer.Enum), "NorMaL" } }));
        propertyValues.Add((validCountDiamond / 2, new Dictionary<string, object> { { nameof(Customer.Enum), "3" } }));
        propertyValues.Add((validCountDiamond / 2, new Dictionary<string, object> { { nameof(Customer.Enum), "DIAMOND" } }));

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCountNormal + validCountSilver + validCountGold + validCountDiamond);

        for (var i = customers.Count - 1; i >= 0; i--)
        {
            var customer = customers[i];

            if (!patchDocument.ApplyOneToOneRelatively(customer))
            {
                customers.RemoveAt(i);
            }
        }

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            patchDocument.InvalidResults.Should().BeNullOrEmpty();
            customers.Count(p => p.Enum is RankingEnum.Gold).Should().Be(validCountGold);
            customers.Count(p => p.Enum is RankingEnum.Silver).Should().Be(validCountSilver);
            customers.Count(p => p.Enum is RankingEnum.Normal).Should().Be(validCountNormal);
            customers.Count(p => p.Enum is RankingEnum.Diamond).Should().Be(validCountDiamond);
            customers.Count.Should().Be(validCountNormal + validCountSilver + validCountGold + validCountDiamond);
        }
    }

    [Fact]
    public async Task Handle_WhenSendInvalidStringTo_Enum_ShouldNotCastEnum()
    {
        const int validCount = 100;
        const int invalidCount = 80;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.Enum), "GOLD" } }));
        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.Enum), "XXXXXX" } }));

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCount + invalidCount);

        for (var i = customers.Count - 1; i >= 0; i--)
        {
            var customer = customers[i];

            if (!patchDocument.ApplyOneToOneRelatively(customer))
            {
                customers.RemoveAt(i);
            }
        }

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            customers.Count.Should().Be(validCount);
            customers.Where(p => p.Enum is RankingEnum.Gold).Should().HaveCount(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.Enum.Should().HaveFlag(RankingEnum.Gold);
            }
        }
    }

    [Fact]
    public async Task Handle_WhenSendIntegerTo_Enum_ShouldCastEnum()
    {
        const int validCountGold = 100;
        const int validCountSilver = 200;
        const int validCountNormal = 100;
        const int validCountDiamond = 200;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCountGold, new Dictionary<string, object> { { nameof(Customer.Enum), 2 } }));
        propertyValues.Add((validCountSilver, new Dictionary<string, object> { { nameof(Customer.Enum), 1 } }));
        propertyValues.Add((validCountNormal, new Dictionary<string, object> { { nameof(Customer.Enum), 0 } }));
        propertyValues.Add((validCountDiamond, new Dictionary<string, object> { { nameof(Customer.Enum), 3 } }));

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCountNormal + validCountSilver + validCountGold + validCountDiamond);

        for (var i = customers.Count - 1; i >= 0; i--)
        {
            var customer = customers[i];

            if (!patchDocument.ApplyOneToOneRelatively(customer))
            {
                customers.RemoveAt(i);
            }
        }

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            patchDocument.InvalidResults.Should().BeNullOrEmpty();
            customers.Count(p => p.Enum is RankingEnum.Gold).Should().Be(validCountGold);
            customers.Count(p => p.Enum is RankingEnum.Silver).Should().Be(validCountSilver);
            customers.Count(p => p.Enum is RankingEnum.Normal).Should().Be(validCountNormal);
            customers.Count(p => p.Enum is RankingEnum.Diamond).Should().Be(validCountDiamond);
            customers.Count.Should().Be(validCountNormal + validCountSilver + validCountGold + validCountDiamond);
        }
    }

    [Fact]
    public async Task Handle_WhenSendOutOfRangeIntegerTo_Enum_ShouldNotCastEnum()
    {
        const int validCount = 100;
        const int invalidCount = 80;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.Enum), 1 } }));
        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.Enum), 12 } }));

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCount + invalidCount);

        for (var i = customers.Count - 1; i >= 0; i--)
        {
            var customer = customers[i];

            if (!patchDocument.ApplyOneToOneRelatively(customer))
            {
                customers.RemoveAt(i);
            }
        }

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            customers.Count.Should().Be(validCount);
            customers.Count(p => p.Enum is RankingEnum.Silver).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.Enum.Should().HaveFlag(RankingEnum.Silver);
            }
        }
    }

    [Fact]
    public async Task Handle_WhenSendNullTo_Enum_ShouldNotCastEnum()
    {
        const int validCount = 1;
        const int invalidCount = 5;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.Enum), null! } }));
        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.Enum), RankingEnum.Diamond } }));

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCount + invalidCount);

        for (var i = customers.Count - 1; i >= 0; i--)
        {
            var customer = customers[i];

            if (!patchDocument.ApplyOneToOneRelatively(customer))
            {
                customers.RemoveAt(i);
            }
        }

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            customers.Count.Should().Be(validCount);
            customers.Count(p => p.Enum is RankingEnum.Diamond).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.Enum.Should().HaveFlag(RankingEnum.Diamond);
            }
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

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCountGold / 2, new Dictionary<string, object> { { nameof(Customer.NullableEnum), "2" } }));
        propertyValues.Add((validCountGold / 2, new Dictionary<string, object> { { nameof(Customer.NullableEnum), "GOLD" } }));
        propertyValues.Add((validCountSilver / 2, new Dictionary<string, object> { { nameof(Customer.NullableEnum), "1" } }));
        propertyValues.Add((validCountSilver / 2, new Dictionary<string, object> { { nameof(Customer.NullableEnum), "SiLvEr" } }));
        propertyValues.Add((validCountNormal / 2, new Dictionary<string, object> { { nameof(Customer.NullableEnum), "0" } }));
        propertyValues.Add((validCountNormal / 2, new Dictionary<string, object> { { nameof(Customer.NullableEnum), "NorMaL" } }));
        propertyValues.Add((validCountDiamond / 2, new Dictionary<string, object> { { nameof(Customer.NullableEnum), "3" } }));
        propertyValues.Add((validCountDiamond / 2, new Dictionary<string, object> { { nameof(Customer.NullableEnum), "DIAMOND" } }));

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCountNormal + validCountSilver + validCountGold + validCountDiamond);

        for (var i = customers.Count - 1; i >= 0; i--)
        {
            var customer = customers[i];

            if (!patchDocument.ApplyOneToOneRelatively(customer))
            {
                customers.RemoveAt(i);
            }
        }

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            patchDocument.InvalidResults.Should().BeNullOrEmpty();
            customers.Count(p => p.NullableEnum is RankingEnum.Gold).Should().Be(validCountGold);
            customers.Count(p => p.NullableEnum is RankingEnum.Silver).Should().Be(validCountSilver);
            customers.Count(p => p.NullableEnum is RankingEnum.Normal).Should().Be(validCountNormal);
            customers.Count(p => p.NullableEnum is RankingEnum.Diamond).Should().Be(validCountDiamond);
            customers.Count.Should().Be(validCountNormal + validCountSilver + validCountGold + validCountDiamond);
        }
    }

    [Fact]
    public async Task Handle_WhenSendInvalidStringTo_NullableEnum_ShouldNotCastEnum()
    {
        const int validCount = 100;
        const int invalidCount = 80;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.NullableEnum), "GOLD" } }));
        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.NullableEnum), "XXXXXX" } }));

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCount + invalidCount);

        for (var i = customers.Count - 1; i >= 0; i--)
        {
            var customer = customers[i];

            if (!patchDocument.ApplyOneToOneRelatively(customer))
            {
                customers.RemoveAt(i);
            }
        }

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            customers.Count.Should().Be(validCount);
            customers.Where(p => p.NullableEnum is RankingEnum.Gold).Should().HaveCount(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.NullableEnum.Should().HaveFlag(RankingEnum.Gold);
            }
        }
    }

    [Fact]
    public async Task Handle_WhenSendIntegerTo_NullableEnum_ShouldCastEnum()
    {
        const int validCountGold = 100;
        const int validCountSilver = 200;
        const int validCountNormal = 100;
        const int validCountDiamond = 200;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCountGold, new Dictionary<string, object> { { nameof(Customer.NullableEnum), 2 } }));
        propertyValues.Add((validCountSilver, new Dictionary<string, object> { { nameof(Customer.NullableEnum), 1 } }));
        propertyValues.Add((validCountNormal, new Dictionary<string, object> { { nameof(Customer.NullableEnum), 0 } }));
        propertyValues.Add((validCountDiamond, new Dictionary<string, object> { { nameof(Customer.NullableEnum), 3 } }));

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCountNormal + validCountSilver + validCountGold + validCountDiamond);

        for (var i = customers.Count - 1; i >= 0; i--)
        {
            var customer = customers[i];

            if (!patchDocument.ApplyOneToOneRelatively(customer))
            {
                customers.RemoveAt(i);
            }
        }

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            patchDocument.InvalidResults.Should().BeNullOrEmpty();
            customers.Count(p => p.NullableEnum is RankingEnum.Gold).Should().Be(validCountGold);
            customers.Count(p => p.NullableEnum is RankingEnum.Silver).Should().Be(validCountSilver);
            customers.Count(p => p.NullableEnum is RankingEnum.Normal).Should().Be(validCountNormal);
            customers.Count(p => p.NullableEnum is RankingEnum.Diamond).Should().Be(validCountDiamond);
            customers.Count.Should().Be(validCountNormal + validCountSilver + validCountGold + validCountDiamond);
        }
    }

    [Fact]
    public async Task Handle_WhenSendOutOfRangeIntegerTo_NullableEnum_ShouldNotCastEnum()
    {
        const int validCount = 100;
        const int invalidCount = 80;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.NullableEnum), 1 } }));
        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.NullableEnum), 12 } }));

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCount + invalidCount);

        for (var i = customers.Count - 1; i >= 0; i--)
        {
            var customer = customers[i];

            if (!patchDocument.ApplyOneToOneRelatively(customer))
            {
                customers.RemoveAt(i);
            }
        }

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            customers.Count.Should().Be(validCount);
            customers.Count(p => p.NullableEnum is RankingEnum.Silver).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.NullableEnum.Should().HaveFlag(RankingEnum.Silver);
            }
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

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCountNull, new Dictionary<string, object> { { nameof(Customer.NullableEnum), null! } }));
        propertyValues.Add((validCountGold, new Dictionary<string, object> { { nameof(Customer.NullableEnum), RankingEnum.Gold } }));
        propertyValues.Add((validCountNormal, new Dictionary<string, object> { { nameof(Customer.NullableEnum), RankingEnum.Normal } }));
        propertyValues.Add((validCountSilver, new Dictionary<string, object> { { nameof(Customer.NullableEnum), RankingEnum.Silver } }));
        propertyValues.Add((validCountDiamond, new Dictionary<string, object> { { nameof(Customer.NullableEnum), RankingEnum.Diamond } }));

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCountNull + validCountNormal + validCountSilver + validCountGold + validCountDiamond);

        for (var i = customers.Count - 1; i >= 0; i--)
        {
            var customer = customers[i];

            if (!patchDocument.ApplyOneToOneRelatively(customer))
            {
                customers.RemoveAt(i);
            }
        }

        await Task.CompletedTask;

        using (new AssertionScope())
        {
            patchDocument.InvalidResults.Should().BeNullOrEmpty();
            customers.Count.Should().Be(validCountNull + validCountNormal + validCountSilver + validCountGold + validCountDiamond);
            customers.Count(p => p.NullableEnum is null).Should().Be(validCountNull);
            customers.Count(p => p.NullableEnum is RankingEnum.Gold).Should().Be(validCountGold);
            customers.Count(p => p.NullableEnum is RankingEnum.Silver).Should().Be(validCountSilver);
            customers.Count(p => p.NullableEnum is RankingEnum.Normal).Should().Be(validCountNormal);
            customers.Count(p => p.NullableEnum is RankingEnum.Diamond).Should().Be(validCountDiamond);
        }
    }

    #endregion
}
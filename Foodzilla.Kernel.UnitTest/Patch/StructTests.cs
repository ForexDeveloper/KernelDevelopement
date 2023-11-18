namespace Foodzilla.Kernel.UnitTest.Patch;

using Moq;
using Xunit;
using System.Dynamic;
using FluentAssertions;
using System.Text.Json;
using FluentAssertions.Execution;
using Foodzilla.Kernel.Patch;

public sealed class StructTests
{
    #region Long

    [Fact]
    public async Task Handle_WhenSendStringTo_Long_ShouldCastLong()
    {
        const int validCount = 100;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount / 2, new Dictionary<string, object> { { nameof(Customer.Long), "11111" } }));
        propertyValues.Add((validCount / 2, new Dictionary<string, object> { { nameof(Customer.Long), "99999" } }));

        var customers = PatchBuilder.CreateValidEntities(validCount);

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

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
            customers.Count.Should().Be(validCount);
            customers.Count(p => p.Long is 99999).Should().Be(validCount / 2);
            customers.Count(p => p.Long is 11111).Should().Be(validCount / 2);
        }
    }

    [Fact]
    public async Task Handle_WhenSendInvalidStringTo_Long_ShouldNotCastLong()
    {
        const int validCount = 100;
        const int invalidCount = 220;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.Long), 11111 } }));
        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.Long), "XXXX" } }));

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
            customers.Count(p => p.Long is 11111).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.Long.Should().Be(11111);
            }
        }
    }

    [Fact]
    public async Task Handle_WhenSendDecimalTo_Long_ShouldNotCastLong()
    {
        const int validCount = 100;
        const int invalidCount = 350;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.Long), 11111 } }));
        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.Long), 123.123 } }));

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
            customers.Count(p => p.Long is 11111).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.Long.Should().Be(11111);
            }
        }
    }

    [Fact]
    public async Task Handle_WhenSendBooleanTo_Long_ShouldNotCastLong()
    {
        const int validCount = 100;
        const int invalidCount = 350;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.Long), 11111 } }));
        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.Long), true } }));

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
            customers.Count(p => p.Long is 11111).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.Long.Should().Be(11111);
            }
        }
    }

    [Fact]
    public async Task Handle_WhenSendNullTo_Long_ShouldNotCastLong()
    {
        const int validCount = 100;
        const int invalidCount = 350;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.Long), 11111 } }));
        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.Long), null! } }));

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
            customers.Count(p => p.Long is 11111).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.Long.Should().Be(11111);
            }
        }
    }

    #endregion

    #region NullabelLong

    [Fact]
    public async Task Handle_WhenSendStringTo_NullableLong_ShouldCastLong()
    {
        const int validCount = 100;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount / 2, new Dictionary<string, object> { { nameof(Customer.NullableLong), "11111" } }));
        propertyValues.Add((validCount / 2, new Dictionary<string, object> { { nameof(Customer.NullableLong), "99999" } }));

        var customers = PatchBuilder.CreateValidEntities(validCount);

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

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
            customers.Count.Should().Be(validCount);
            customers.Count(p => p.NullableLong is 99999).Should().Be(validCount / 2);
            customers.Count(p => p.NullableLong is 11111).Should().Be(validCount / 2);
        }
    }

    [Fact]
    public async Task Handle_WhenSendInvalidStringTo_NullableLong_ShouldNotCastLong()
    {
        const int validCount = 100;
        const int invalidCount = 220;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.NullableLong), 11111 } }));
        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.NullableLong), "XXXX" } }));

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
            customers.Count(p => p.NullableLong is 11111).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.NullableLong.Should().Be(11111);
            }
        }
    }

    [Fact]
    public async Task Handle_WhenSendDecimalTo_NullableLong_ShouldNotCastLong()
    {
        const int validCount = 100;
        const int invalidCount = 350;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.NullableLong), 11111 } }));
        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.NullableLong), 123.123 } }));

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
            customers.Count(p => p.NullableLong is 11111).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.NullableLong.Should().Be(11111);
            }
        }
    }

    [Fact]
    public async Task Handle_WhenSendBooleanTo_NullableLong_ShouldNotCastLong()
    {
        const int validCount = 100;
        const int invalidCount = 350;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.NullableLong), 11111 } }));
        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.NullableLong), true } }));

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
            customers.Count(p => p.NullableLong is 11111).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.NullableLong.Should().Be(11111);
            }
        }
    }

    [Fact]
    public async Task Handle_WhenSendNullTo_NullableLong_ShouldCastLong()
    {
        const int validCount = 100;
        const int validCountNull = 350;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.NullableLong), 11111 } }));
        propertyValues.Add((validCountNull, new Dictionary<string, object> { { nameof(Customer.NullableLong), null! } }));

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCount + validCountNull);

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
            customers.Count.Should().Be(validCount + validCountNull);
            customers.Count(p => p.NullableLong is 11111).Should().Be(validCount);
            customers.Count(p => p.NullableLong is null).Should().Be(validCountNull);
        }
    }

    #endregion

    #region Int

    [Fact]
    public async Task Handle_WhenSendStringTo_Int_ShouldCastInt()
    {
        const int validCount = 100;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount / 2, new Dictionary<string, object> { { nameof(Customer.Int), "11111" } }));
        propertyValues.Add((validCount / 2, new Dictionary<string, object> { { nameof(Customer.Int), "99999" } }));

        var customers = PatchBuilder.CreateValidEntities(validCount);

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

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
            customers.Count.Should().Be(validCount);
            customers.Count(p => p.Int is 99999).Should().Be(validCount / 2);
            customers.Count(p => p.Int is 11111).Should().Be(validCount / 2);
        }
    }

    [Fact]
    public async Task Handle_WhenSendInvalidStringTo_Int_ShouldNotCastInt()
    {
        const int validCount = 100;
        const int invalidCount = 220;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.Int), 11111 } }));
        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.Int), "XXXX" } }));

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
            customers.Count(p => p.Int is 11111).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.Int.Should().Be(11111);
            }
        }
    }

    [Fact]
    public async Task Handle_WhenSendDecimalTo_Int_ShouldNotCastInt()
    {
        const int validCount = 100;
        const int invalidCount = 350;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.Int), 11111 } }));
        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.Int), 123.123 } }));

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
            customers.Count(p => p.Int is 11111).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.Int.Should().Be(11111);
            }
        }
    }

    [Fact]
    public async Task Handle_WhenSendBooleanTo_Int_ShouldNotCastInt()
    {
        const int validCount = 100;
        const int invalidCount = 350;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.Int), 11111 } }));
        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.Int), true } }));

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
            customers.Count(p => p.Int is 11111).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.Int.Should().Be(11111);
            }
        }
    }

    [Fact]
    public async Task Handle_WhenSendNullTo_Int_ShouldNotCastInt()
    {
        const int validCount = 100;
        const int invalidCount = 350;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.Int), 11111 } }));
        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.Int), null! } }));

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
            customers.Count(p => p.Int is 11111).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.Int.Should().Be(11111);
            }
        }
    }

    #endregion

    #region NullableInt

    [Fact]
    public async Task Handle_WhenSendString_To_NullableInt_ShouldCastInt()
    {
        const int validCount = 100;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount / 2, new Dictionary<string, object> { { nameof(Customer.NullableInt), "11111" } }));
        propertyValues.Add((validCount / 2, new Dictionary<string, object> { { nameof(Customer.NullableInt), "99999" } }));

        var customers = PatchBuilder.CreateValidEntities(validCount);

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

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
            customers.Count.Should().Be(validCount);
            customers.Count(p => p.NullableInt is 99999).Should().Be(validCount / 2);
            customers.Count(p => p.NullableInt is 11111).Should().Be(validCount / 2);
        }
    }

    [Fact]
    public async Task Handle_WhenSendInvalidStringTo_NullableInt_ShouldNotCastInt()
    {
        const int validCount = 100;
        const int invalidCount = 220;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.NullableInt), 11111 } }));
        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.NullableInt), "XXXX" } }));

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
            customers.Count(p => p.NullableInt is 11111).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.NullableInt.Should().Be(11111);
            }
        }
    }

    [Fact]
    public async Task Handle_WhenSendDecimalTo_NullableInt_ShouldNotCastInt()
    {
        const int validCount = 100;
        const int invalidCount = 350;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.NullableInt), 11111 } }));
        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.NullableInt), 123.123 } }));

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
            customers.Count(p => p.NullableInt is 11111).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.NullableInt.Should().Be(11111);
            }
        }
    }

    [Fact]
    public async Task Handle_WhenSendBooleanTo_NullableInt_ShouldNotCastInt()
    {
        const int validCount = 100;
        const int invalidCount = 350;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.NullableInt), 11111 } }));
        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.NullableInt), true } }));

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
            customers.Count(p => p.NullableInt is 11111).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.NullableInt.Should().Be(11111);
            }
        }
    }

    [Fact]
    public async Task Handle_WhenSendNullTo_NullableInt_ShouldCastInt()
    {
        const int validCount = 100;
        const int validCountNull = 350;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.NullableInt), 11111 } }));
        propertyValues.Add((validCountNull, new Dictionary<string, object> { { nameof(Customer.NullableInt), null! } }));

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCount + validCountNull);

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
            customers.Count.Should().Be(validCount + validCountNull);
            customers.Count(p => p.NullableInt is 11111).Should().Be(validCount);
            customers.Count(p => p.NullableInt is null).Should().Be(validCountNull);
        }
    }

    #endregion

    #region Guid

    [Fact]
    public async Task Handle_WhenSendStringTo_Guid_ShouldCastGuid()
    {
        const int validCount = 100;

        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount / 2, new Dictionary<string, object> { { nameof(Customer.Guid), guid1 } }));
        propertyValues.Add((validCount / 2, new Dictionary<string, object> { { nameof(Customer.Guid), guid2 } }));

        var customers = PatchBuilder.CreateValidEntities(validCount);

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

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
            customers.Count.Should().Be(validCount);
            customers.Count(p => p.Guid == guid1).Should().Be(validCount / 2);
            customers.Count(p => p.Guid == guid2).Should().Be(validCount / 2);
        }
    }

    [Fact]
    public async Task Handle_WhenSendInvalidStringTo_Guid_ShouldNotCastGuid()
    {
        const int validCount = 100;
        const int invalidCount = 220;

        var guid = Guid.NewGuid();

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.Guid), guid } }));
        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.Guid), "XXXX" } }));

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
            customers.Count(p => p.Guid == guid).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.Guid.Should().Be(guid);
            }
        }
    }

    [Fact]
    public async Task Handle_WhenSendDecimalTo_Guid_ShouldNotCastGuid()
    {
        const int validCount = 100;
        const int invalidCount = 350;

        var guid = Guid.NewGuid();

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.Guid), guid } }));
        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.Guid), 123.123 } }));

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
            customers.Count(p => p.Guid == guid).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.Guid.Should().Be(guid);
            }
        }
    }

    [Fact]
    public async Task Handle_WhenSendBooleanTo_Guid_ShouldNotCastGuid()
    {
        const int validCount = 100;
        const int invalidCount = 350;

        var guid = Guid.NewGuid();

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.Guid), guid } }));
        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.Guid), true } }));

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
            customers.Count(p => p.Guid == guid).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.Guid.Should().Be(guid);
            }
        }
    }

    [Fact]
    public async Task Handle_WhenSendNullTo_Guid_ShouldNotCastGuid()
    {
        const int validCount = 100;
        const int invalidCount = 350;

        var guid = Guid.NewGuid();

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.Guid), guid } }));
        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.Guid), null! } }));

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
            customers.Count(p => p.Guid == guid).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.Guid.Should().Be(guid);
            }
        }
    }

    #endregion

    #region NullableGuid

    [Fact]
    public async Task Handle_WhenSendStringTo_NullableGuid_ShouldCastGuid()
    {
        const int validCount = 100;

        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount / 2, new Dictionary<string, object> { { nameof(Customer.NullableGuid), guid1 } }));
        propertyValues.Add((validCount / 2, new Dictionary<string, object> { { nameof(Customer.NullableGuid), guid2 } }));

        var customers = PatchBuilder.CreateValidEntities(validCount);

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

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
            customers.Count.Should().Be(validCount);
            customers.Count(p => p.NullableGuid == guid1).Should().Be(validCount / 2);
            customers.Count(p => p.NullableGuid == guid2).Should().Be(validCount / 2);
        }
    }

    [Fact]
    public async Task Handle_WhenSendInvalidStringTo_NullableGuid_ShouldNotCastGuid()
    {
        const int validCount = 100;
        const int invalidCount = 220;

        var guid = Guid.NewGuid();

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.NullableGuid), guid } }));
        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.NullableGuid), "XXXX" } }));

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
            customers.Count(p => p.NullableGuid == guid).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.NullableGuid.Should().Be(guid);
            }
        }
    }

    [Fact]
    public async Task Handle_WhenSendDecimalTo_NullableGuid_ShouldNotCastGuid()
    {
        const int validCount = 100;
        const int invalidCount = 350;

        var guid = Guid.NewGuid();

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.NullableGuid), guid } }));
        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.NullableGuid), 123.123 } }));

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
            customers.Count(p => p.NullableGuid == guid).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.NullableGuid.Should().Be(guid);
            }
        }
    }

    [Fact]
    public async Task Handle_WhenSendBooleanTo_NullableGuid_ShouldNotCastGuid()
    {
        const int validCount = 100;
        const int invalidCount = 350;

        var guid = Guid.NewGuid();

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.NullableGuid), guid } }));
        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.NullableGuid), true } }));

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
            customers.Count(p => p.NullableGuid == guid).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.NullableGuid.Should().Be(guid);
            }
        }
    }

    [Fact]
    public async Task Handle_WhenSendNullTo_NullableGuid_ShouldCastGuid()
    {
        const int validCount = 100;
        const int validCountNull = 220;

        var guid = Guid.NewGuid();

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.NullableGuid), guid } }));
        propertyValues.Add((validCountNull, new Dictionary<string, object> { { nameof(Customer.NullableGuid), null! } }));

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCount + validCountNull);

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
            customers.Count.Should().Be(validCount + validCountNull);
            customers.Count(p => p.NullableGuid == guid).Should().Be(validCount);
            customers.Count(p => p.NullableGuid is null).Should().Be(validCountNull);
        }
    }

    #endregion
}
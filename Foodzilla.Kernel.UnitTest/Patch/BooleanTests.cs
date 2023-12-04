namespace Foodzilla.Kernel.UnitTest.Patch;

using Xunit;
using System.Dynamic;
using FluentAssertions;
using System.Text.Json;
using Foodzilla.Kernel.Patch;
using FluentAssertions.Execution;
using System.Reflection;

public sealed class BooleanTests
{
    #region Boolean

    [Fact]
    public async Task Handle_WhenSendStringTo_Boolean_ShouldCastBoolean()
    {
        const int validCountTrue = 100;
        const int validCountFalse = 200;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCountTrue / 2, new Dictionary<string, object> { { nameof(Customer.Boolean), "1" } }));
        propertyValues.Add((validCountFalse / 2, new Dictionary<string, object> { { nameof(Customer.Boolean), "0" } }));
        propertyValues.Add((validCountTrue / 2, new Dictionary<string, object> { { nameof(Customer.Boolean), "TrUe" } }));
        propertyValues.Add((validCountFalse / 2, new Dictionary<string, object> { { nameof(Customer.Boolean), "FaLsE" } }));

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCountTrue + validCountFalse);

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
            customers.Count.Should().Be(validCountTrue + validCountFalse);
            customers.Count(p => p.Boolean is true).Should().Be(validCountTrue);
            customers.Count(p => p.Boolean is false).Should().Be(validCountFalse);
        }
    }

    [Fact]
    public async Task Handle_WhenSendInvalidStringTo_Boolean_ShouldNotCastBoolean()
    {
        const int validCount = 100;
        const int invalidCount = 80;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.Boolean), "true" } }));
        propertyValues.Add((invalidCount / 2, new Dictionary<string, object> { { nameof(Customer.Boolean), "111" } }));
        propertyValues.Add((invalidCount / 2, new Dictionary<string, object> { { nameof(Customer.Boolean), "XXXXXX" } }));

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
            customers.Where(p => p.Boolean is true).Should().HaveCount(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.Boolean.Should().BeTrue();
            }
        }
    }

    [Fact]
    public async Task Handle_WhenSendIntegerTo_Boolean_ShouldCastBoolean()
    {
        const int validCountTrue = 100;
        const int validCountFalse = 80;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCountTrue, new Dictionary<string, object> { { nameof(Customer.Boolean), 1 } }));
        propertyValues.Add((validCountFalse, new Dictionary<string, object> { { nameof(Customer.Boolean), 0 } }));

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCountTrue + validCountFalse);

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
            customers.Count.Should().Be(validCountTrue + validCountFalse);
            customers.Count(p => p.Boolean is true).Should().Be(validCountTrue);
            customers.Count(p => p.Boolean is false).Should().Be(validCountFalse);
        }
    }

    [Fact]
    public async Task Handle_WhenSendOutOfRangeIntegerTo_Boolean_ShouldNotCastBoolean()
    {
        const int validCount = 100;
        const int invalidCount = 80;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.Boolean), 0 } }));
        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.Boolean), 12 } }));

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
            customers.Count(p => p.Boolean is false).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.NullableBoolean.Should().BeFalse();
            }
        }
    }

    [Fact]
    public async Task Handle_WhenSendNullTo_Boolean_ShouldCastNotBoolean()
    {
        const int validCount = 100;
        const int invalidCount = 80;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.Boolean), true } }));
        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.Boolean), null! } }));

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
            customers.Count(p => p.Boolean is true).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.Boolean.Should().BeTrue();
            }
        }
    }

    #endregion

    #region NullableBoolean

    [Fact]
    public async Task Handle_WhenSendStringTo_NullableBoolean_ShouldCastBoolean()
    {
        const int validCountTrue = 100;
        const int validCountFalse = 200;

        List<ValueTuple<int, Dictionary<string, object>>> propertyValues = new();

        propertyValues.Add((validCountTrue / 2, new Dictionary<string, object> { { nameof(Customer.NullableBoolean), "1" } }));
        propertyValues.Add((validCountFalse / 2, new Dictionary<string, object> { { nameof(Customer.NullableBoolean), "0" } }));
        propertyValues.Add((validCountTrue / 2, new Dictionary<string, object> { { nameof(Customer.NullableBoolean), "TrUe" } }));
        propertyValues.Add((validCountFalse / 2, new Dictionary<string, object> { { nameof(Customer.NullableBoolean), "FaLsE" } }));

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCountTrue + validCountFalse);

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
            customers.Count.Should().Be(validCountTrue + validCountFalse);
            customers.Count(p => p.NullableBoolean is true).Should().Be(validCountTrue);
            customers.Count(p => p.NullableBoolean is false).Should().Be(validCountFalse);
        }
    }

    [Fact]
    public async Task Handle_WhenSendInvalidStringTo_NullableBoolean_ShouldNotCastBoolean()
    {
        const int validCount = 100;
        const int invalidCount = 80;

        List<ValueTuple<int, Dictionary<string, object>>> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.NullableBoolean), "true" } }));
        propertyValues.Add((invalidCount / 2, new Dictionary<string, object> { { nameof(Customer.NullableBoolean), "111" } }));
        propertyValues.Add((invalidCount / 2, new Dictionary<string, object> { { nameof(Customer.NullableBoolean), "XXXXXX" } }));

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
            customers.Where(p => p.NullableBoolean is true).Should().HaveCount(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.NullableBoolean.Should().BeTrue();
            }
        }
    }

    [Fact]
    public async Task Handle_WhenSendIntegerTo_NullableBoolean_ShouldCastBoolean()
    {
        const int validCountTrue = 100;
        const int validCountFalse = 80;

        List<ValueTuple<int, Dictionary<string, object>>> propertyValues = new();

        propertyValues.Add((validCountTrue, new Dictionary<string, object> { { nameof(Customer.NullableBoolean), 1 } }));
        propertyValues.Add((validCountFalse, new Dictionary<string, object> { { nameof(Customer.NullableBoolean), 0 } }));

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCountTrue + validCountFalse);

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
            customers.Count.Should().Be(validCountTrue + validCountFalse);
            customers.Count(p => p.NullableBoolean is true).Should().Be(validCountTrue);
            customers.Count(p => p.NullableBoolean is false).Should().Be(validCountFalse);
        }
    }

    [Fact]
    public async Task Handle_WhenSendOutOfRangeIntegerTo_NullableBoolean_ShouldNotCastBoolean()
    {
        const int validCount = 100;
        const int invalidCount = 80;

        List<ValueTuple<int, Dictionary<string, object>>> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.NullableBoolean), 0 } }));
        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.NullableBoolean), 12 } }));

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
            customers.Count(p => p.NullableBoolean is false).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.NullableBoolean.Should().BeFalse();
            }
        }
    }

    [Fact]
    public async Task Handle_WhenSendNullTo_NullableBoolean_ShouldCastBoolean()
    {
        const int validCountNull = 80;
        const int validCountTrue = 100;
        const int validCountFalse = 100;

        List<ValueTuple<int, Dictionary<string, object>>> propertyValues = new();

        propertyValues.Add((validCountNull, new Dictionary<string, object> { { nameof(Customer.NullableBoolean), null! } }));
        propertyValues.Add((validCountTrue, new Dictionary<string, object> { { nameof(Customer.NullableBoolean), true } }));
        propertyValues.Add((validCountFalse, new Dictionary<string, object> { { nameof(Customer.NullableBoolean), false } }));

        var patchEntities = PatchBuilder.CreatePatchEntities(propertyValues);

        var patchDocument = PatchDocument<Customer>.Create(patchEntities);

        var customers = PatchBuilder.CreateValidEntities(validCountNull + validCountFalse + validCountTrue);

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
            customers.Count(p => p.NullableBoolean is null).Should().Be(validCountNull);
            customers.Count(p => p.NullableBoolean is true).Should().Be(validCountTrue);
            customers.Count(p => p.NullableBoolean is false).Should().Be(validCountFalse);
            customers.Count.Should().Be(validCountNull + validCountFalse + validCountTrue);
        }
    }

    #endregion
}
public static class ObjectExtension
{
    public static object ToExpando(this object value)
    {
        IDictionary<string, object> expando = (IDictionary<string, object>)new ExpandoObject();
        foreach (PropertyInfo property in value.GetType().GetTypeInfo().GetProperties())
            expando.Add(property.Name, property.GetValue(value, (object[])null));
        return (object)(expando as ExpandoObject);
    }
}
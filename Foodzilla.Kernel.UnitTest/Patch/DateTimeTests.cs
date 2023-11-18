namespace Foodzilla.Kernel.UnitTest.Patch;

using Xunit;
using System.Dynamic;
using FluentAssertions;
using System.Text.Json;
using Foodzilla.Kernel.Patch;
using FluentAssertions.Execution;

public sealed class DateTimeTests
{
    #region DateTimeOffset

    [Fact]
    public async Task Handle_WhenSendStringTo_DateTimOffset_ShouldCastDateTimeOffset()
    {
        const int validCount = 100;

        var dateTime1 = DateTimeOffset.Now;
        var dateTime2 = DateTimeOffset.Now.AddDays(12);

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount / 2, new Dictionary<string, object> { { nameof(Customer.DateTime), dateTime1 } }));
        propertyValues.Add((validCount / 2, new Dictionary<string, object> { { nameof(Customer.DateTime), dateTime2 } }));

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
            customers.Count(p => p.DateTime == dateTime1).Should().Be(validCount / 2);
            customers.Count(p => p.DateTime == dateTime2).Should().Be(validCount / 2);
        }
    }

    [Fact]
    public async Task Handle_WhenSendInvalidStringTo_DateTimeOffset_ShouldNotCastDateTimeOffset()
    {
        const int validCount = 100;
        const int invalidCount = 220;

        var dateTime = DateTimeOffset.Now;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.DateTime), dateTime } }));
        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.DateTime), "XXXX" } }));

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
            customers.Count(p => p.DateTime == dateTime).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.DateTime.Should().Be(dateTime);
            }
        }
    }

    [Fact]
    public async Task Handle_WhenSendDecimalOrLongTo_DateTimeOffset_ShouldNotCastDateTimeOffset()
    {
        const int validCount = 100;
        const int invalidCount = 220;

        var dateTime = DateTimeOffset.Now;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.DateTime), dateTime } }));
        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.DateTime), 12312.1231 } }));

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
            customers.Count(p => p.DateTime == dateTime).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.DateTime.Should().Be(dateTime);
            }
        }
    }

    [Fact]
    public async Task Handle_WhenSendBooleanTo_DateTimeOffset_ShouldNotCastDateTimeOffset()
    {
        const int validCount = 100;
        const int invalidCount = 220;

        var dateTime = DateTimeOffset.Now;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.DateTime), true } }));
        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.DateTime), dateTime } }));

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
            customers.Count(p => p.DateTime == dateTime).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.DateTime.Should().Be(dateTime);
            }
        }
    }

    [Fact]
    public async Task Handle_WhenSendNullTo_DateTimeOffset_ShouldNotCastDateTimeOffset()
    {
        const int validCount = 100;
        const int invalidCount = 220;

        var dateTime = DateTimeOffset.Now;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.DateTime), null! } }));
        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.DateTime), dateTime } }));

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
            customers.Count(p => p.DateTime == dateTime).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.DateTime.Should().Be(dateTime);
            }
        }
    }

    #endregion

    #region NullableDateTimeOffset

    [Fact]
    public async Task Handle_WhenSendStringTo_NullableDateTimOffset_ShouldCastDateTimeOffset()
    {
        const int validCount = 100;

        var dateTime1 = DateTimeOffset.Now;
        var dateTime2 = DateTimeOffset.Now.AddDays(12);

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount / 2, new Dictionary<string, object> { { nameof(Customer.NullableDateTime), dateTime1 } }));
        propertyValues.Add((validCount / 2, new Dictionary<string, object> { { nameof(Customer.NullableDateTime), dateTime2 } }));

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
            customers.Count(p => p.NullableDateTime == dateTime1).Should().Be(validCount / 2);
            customers.Count(p => p.NullableDateTime == dateTime2).Should().Be(validCount / 2);
        }
    }

    [Fact]
    public async Task Handle_WhenSendInvalidStringTo_NullableDateTimeOffset_ShouldNotCastDateTimeOffset()
    {
        const int validCount = 100;
        const int invalidCount = 220;

        var dateTime = DateTimeOffset.Now;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.NullableDateTime), dateTime } }));
        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.NullableDateTime), "XXXX" } }));

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
            customers.Count(p => p.NullableDateTime == dateTime).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.NullableDateTime.Should().Be(dateTime);
            }
        }
    }

    [Fact]
    public async Task Handle_WhenSendDecimalOrLongTo_NullableDateTimeOffset_ShouldNotCastDateTimeOffset()
    {
        const int validCount = 100;
        const int invalidCount = 220;

        var dateTime = DateTimeOffset.Now;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.NullableDateTime), dateTime } }));
        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.NullableDateTime), 1233.123 } }));

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
            customers.Count(p => p.NullableDateTime == dateTime).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.NullableDateTime.Should().Be(dateTime);
            }
        }
    }

    [Fact]
    public async Task Handle_WhenSendBooleanTo_NullableDateTimeOffset_ShouldNotCastDateTimeOffset()
    {
        const int validCount = 100;
        const int invalidCount = 220;

        var dateTime = DateTimeOffset.Now;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((invalidCount, new Dictionary<string, object> { { nameof(Customer.NullableDateTime), true } }));
        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.NullableDateTime), dateTime } }));

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
            customers.Count(p => p.NullableDateTime == dateTime).Should().Be(validCount);
            patchDocument.InvalidResults.Should().NotBeNullOrEmpty().And.HaveCount(invalidCount);
            foreach (var customer in customers)
            {
                customer.NullableDateTime.Should().Be(dateTime);
            }
        }
    }

    [Fact]
    public async Task Handle_WhenSendNullTo_NullableDateTimeOffset_ShouldCastDateTimeOffset()
    {
        const int validCount = 100;
        const int validCountNull = 220;

        var dateTime = DateTimeOffset.Now;

        List<(int, Dictionary<string, object>)> propertyValues = new();

        propertyValues.Add((validCount, new Dictionary<string, object> { { nameof(Customer.NullableDateTime), dateTime } }));
        propertyValues.Add((validCountNull, new Dictionary<string, object> { { nameof(Customer.NullableDateTime), null! } }));

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
            customers.Count(p => p.NullableDateTime == dateTime).Should().Be(validCount);
            customers.Count(p => p.NullableDateTime is null).Should().Be(validCountNull);
        }
    }

    #endregion
}
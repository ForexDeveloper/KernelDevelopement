using Bogus;
using System.Dynamic;
using System.Text.Json;
using Foodzilla.Kernel.Patch;
using Foodzilla.Kernel.Domain;

namespace Foodzilla.Kernel.UnitTest.Patch;

public sealed class PatchBuilder
{
    private readonly Faker _faker;

    public PatchBuilder(Faker faker)
    {
        _faker = faker;
    }

    public static PatchDocument<Customer> Create(ExpandoObject patchEntity)
    {
        return PatchDocument<Customer>.Create(new List<ExpandoObject> { patchEntity });
    }

    public static List<Customer> CreateValidEntities()
    {
        var now = DateTimeOffset.Now;

        var customer1 = Customer.Create(1, "MohammadReza", "JavidPoor", "4120583732", "String", null, RankingEnum.Gold, null, true, null, 10, null, 40, null, 321312.12321M, null, 400, null, Guid.NewGuid(), null, now, null);
        var customer2 = Customer.Create(2, "Mehran", "Hashemi", "3120583632", "String", null, RankingEnum.Silver, RankingEnum.Diamond, true, null, 10, null, 40, null, 321312.12321M, null, 400, null, Guid.NewGuid(), null, now, null);
        var customer3 = Customer.Create(3, "Mehran", "Hashemi", "3120583632", "String", null, RankingEnum.Diamond, RankingEnum.Silver, true, false, 10, null, 40, null, 321312.12321M, null, 400, null, Guid.NewGuid(), null, now, null);
        var customer4 = Customer.Create(4, "Mehdi", "Ebrahimi", "0120583632", "String", null, RankingEnum.Gold, RankingEnum.Normal, true, false, 10, 20, 40, null, 321312.12321M, null, 400, null, Guid.NewGuid(), null, now, null);
        var customer5 = Customer.Create(5, "Ghazaleh", "Eshghi", "9120583632", "String", null, RankingEnum.Gold, RankingEnum.Normal, true, false, 10, 20, 40, 50, 321312.12321M, null, 400, null, Guid.NewGuid(), null, now, null);
        var customer6 = Customer.Create(6, "Amir", "Heydari", "3120583632", "String", null, RankingEnum.Gold, RankingEnum.Normal, true, false, 10, 20, 40, 50, 321312.12321M, null, 400, 56, Guid.NewGuid(), null, now, null);
        var customer7 = Customer.Create(7, "Amin", "Salami", "3120583632", "String", null, RankingEnum.Gold, RankingEnum.Normal, true, false, 10, 20, 40, 50, 321312.12321M, null, 400, 56, Guid.NewGuid(), null, now, now.AddDays(120));

        var customer8 = Customer.Create(8, "MohammadReza", "JavidPoor", "4120583732", "String", "NullableString", RankingEnum.Gold, null, true, null, 10, null, 40, null, 321312.12321M, null, 400, null, Guid.NewGuid(), null, now, null);
        var customer9 = Customer.Create(9, "Mehran", "Hashemi", "3120583632", "String", "NullableString", RankingEnum.Silver, RankingEnum.Diamond, true, null, 10, null, 40, null, 321312.12321M, null, 400, null, Guid.NewGuid(), null, now, null);
        var customer10 = Customer.Create(10, "Mehran", "Hashemi", "3120583632", "String", "NullableString", RankingEnum.Diamond, RankingEnum.Silver, true, false, 10, null, 40, null, 321312.12321M, null, 400, null, Guid.NewGuid(), null, now, null);
        var customer11 = Customer.Create(11, "Mehdi", "Ebrahimi", "0120583632", "String", "NullableString", RankingEnum.Gold, RankingEnum.Normal, true, false, 10, 20, 40, null, 321312.12321M, null, 400, null, Guid.NewGuid(), null, now, null);
        var customer12 = Customer.Create(12, "Queen", "Eshghi", "9120583632", "String", "NullableString", RankingEnum.Gold, RankingEnum.Normal, true, false, 10, 20, 40, 50, 321312.12321M, null, 400, null, Guid.NewGuid(), null, now, null);
        var customer13 = Customer.Create(13, "Amir", "Heydari", "3120583632", "String", "NullableString", RankingEnum.Gold, RankingEnum.Normal, true, false, 10, 20, 40, 50, 321312.12321M, null, 400, 56, Guid.NewGuid(), null, now, null);
        var customer14 = Customer.Create(14, "Amin", "Salami", "3120583632", "String", "NullableString", RankingEnum.Gold, RankingEnum.Normal, true, false, 10, 20, 40, 50, 321312.12321M, null, 400, 56, Guid.NewGuid(), null, now, now.AddDays(120));

        var customers = new List<Customer>
        {
            customer1,
            customer2,
            customer3,
            customer3,
            customer4,
            customer5,
            customer6,
            customer7,
            customer8,
            customer9,
            customer10,
            customer10,
            customer11,
            customer12,
            customer13,
            customer14
        };

        return customers;
    }

    public static List<Customer> CreateValidEntities(int instanceCount)
    {
        var customers = new List<Customer>();

        for (var i = 1; i <= instanceCount; i++)
        {
            var customer = CreateCustomer(i);

            customers.Add(customer);
        }

        return customers;
    }

    public static List<Customer> CreateComplexEntities(int instanceCount)
    {
        var customers = new List<Customer>();

        for (var i = 1; i <= instanceCount; i++)
        {
            var customer = CreateCustomer(i);
            var navigationOrder = CreateOrder(i);
            var navigationCustomer = CreateCustomer(i);

            customer.SetNavigation(navigationOrder);
            customer.SetNavigation(navigationCustomer);

            for (var j = 1; j <= 5; j++)
            {
                var orderItem = CreateOrder(j);
                var customerItem = CreateCustomer(j);

                customer.AddUnitToNavigationList(orderItem);
                customer.AddUnitToNavigationList(customerItem);

                customer.NavigationCustomer.SetNavigation((Order)orderItem.Clone());
                customer.NavigationCustomer.SetNavigation((Customer)customerItem.Clone());
                customer.NavigationCustomer.AddUnitToNavigationList((Order)orderItem.Clone());
                customer.NavigationCustomer.AddUnitToNavigationList((Customer)customerItem.Clone());

                customer.NavigationOrder.AddUnitToNavigationList((Order)orderItem.Clone());

                for (var k = 1; k <= 5; k++)
                {
                    var orderItem2 = CreateOrder(k);
                    customerItem.AddUnitToNavigationList(orderItem2);
                }
            }

            customers.Add(customer);
        }

        return customers;
    }

    public static List<ExpandoObject> CreatePatchEntities(int instanceCount)
    {
        var now = DateTimeOffset.Now;
        var patchEntities = new List<ExpandoObject>();

        for (var i = 1; i <= instanceCount; i++)
        {
            dynamic patchEntity = new ExpandoObject();

            patchEntity.Id = JsonSerializer.SerializeToElement(i);
            patchEntity.Name = JsonSerializer.SerializeToElement($"Name_{i}");
            patchEntity.Family = JsonSerializer.SerializeToElement($"Family_{i}");
            patchEntity.NationalCode = JsonSerializer.SerializeToElement($"NationalCode_{i}");
            patchEntity.String = JsonSerializer.SerializeToElement($"String_{i}");
            patchEntity.NullableString = JsonSerializer.SerializeToElement($"NullableString_{i}");

            patchEntity.CustomerRank = JsonSerializer.SerializeToElement(2);
            patchEntity.CustomerRankNullable = JsonSerializer.SerializeToElement(3);

            patchEntity.Boolean = JsonSerializer.SerializeToElement(true);
            patchEntity.NullableBoolean = JsonSerializer.SerializeToElement(false);

            patchEntity.Int = JsonSerializer.SerializeToElement(i);
            patchEntity.NullableInt = JsonSerializer.SerializeToElement(i);

            patchEntity.Long = JsonSerializer.SerializeToElement(i);
            patchEntity.NullableLong = JsonSerializer.SerializeToElement(i);

            patchEntity.Double = JsonSerializer.SerializeToElement(5 * i / 2);
            patchEntity.NullableDouble = JsonSerializer.SerializeToElement(5 * i / 2);

            patchEntity.Decimal = JsonSerializer.SerializeToElement(7 * i / 2);
            patchEntity.NullableDecimal = JsonSerializer.SerializeToElement(7 * i / 2);

            patchEntity.Guid = JsonSerializer.SerializeToElement(Guid.NewGuid());
            patchEntity.NullableGuid = JsonSerializer.SerializeToElement(Guid.NewGuid());

            patchEntity.DateTime = JsonSerializer.SerializeToElement(now);
            patchEntity.NullableDateTime = JsonSerializer.SerializeToElement(now);

            patchEntities.Add(patchEntity);
        }

        return patchEntities;
    }
    
    public static List<ExpandoObject> CreatePatchEntities(List<Customer> customers)
    {
        var now = DateTimeOffset.Now;
        var patchEntities = new List<ExpandoObject>();

        for (var i = 0; i < customers.Count; i++)
        {
            dynamic patchEntity = new ExpandoObject();

            patchEntity.Id = JsonSerializer.SerializeToElement(customers[i].Id);
            patchEntity.Name = JsonSerializer.SerializeToElement($"Name_{i}");
            patchEntity.Family = JsonSerializer.SerializeToElement($"Family_{i}");
            patchEntity.NationalCode = JsonSerializer.SerializeToElement($"NationalCode_{i}");
            patchEntity.String = JsonSerializer.SerializeToElement($"String_{i}");
            patchEntity.NullableString = JsonSerializer.SerializeToElement($"NullableString_{i}");

            patchEntity.CustomerRank = JsonSerializer.SerializeToElement(2);
            patchEntity.CustomerRankNullable = JsonSerializer.SerializeToElement(3);

            patchEntity.Boolean = JsonSerializer.SerializeToElement(true);
            patchEntity.NullableBoolean = JsonSerializer.SerializeToElement(false);

            patchEntity.Int = JsonSerializer.SerializeToElement(i);
            patchEntity.NullableInt = JsonSerializer.SerializeToElement(i);

            patchEntity.Long = JsonSerializer.SerializeToElement(i);
            patchEntity.NullableLong = JsonSerializer.SerializeToElement(i);

            patchEntity.Double = JsonSerializer.SerializeToElement(5 * i / 2);
            patchEntity.NullableDouble = JsonSerializer.SerializeToElement(5 * i / 2);

            patchEntity.Decimal = JsonSerializer.SerializeToElement(7 * i / 2);
            patchEntity.NullableDecimal = JsonSerializer.SerializeToElement(7 * i / 2);

            patchEntity.Guid = JsonSerializer.SerializeToElement(Guid.NewGuid());
            patchEntity.NullableGuid = JsonSerializer.SerializeToElement(Guid.NewGuid());

            patchEntity.DateTime = JsonSerializer.SerializeToElement(now);
            patchEntity.NullableDateTime = JsonSerializer.SerializeToElement(now);

            patchEntities.Add(patchEntity);
        }

        return patchEntities;
    }

    public static List<ExpandoObject> CreatePatchEntities(List<(int Count, Dictionary<string, object> Properties)> @params)
    {
        var i = 1;
        var patchEntities = new List<ExpandoObject>();

        foreach (var param in @params)
        {
            for (var j = 0; j < param.Count; j++)
            {
                dynamic patchEntity = new ExpandoObject();
                patchEntity.Id = JsonSerializer.SerializeToElement(i);

                foreach (var (property, value) in param.Properties)
                {
                    object valueElement;

                    if (value is not null)
                    {
                        valueElement = JsonSerializer.SerializeToElement(value);
                    }
                    else
                    {
                        valueElement = null!;
                    }

                    switch (property)
                    {
                        case nameof(Customer.Name):
                            patchEntity.Name = valueElement;
                            break;

                        case nameof(Customer.Family):
                            patchEntity.Family = valueElement;
                            break;

                        case nameof(Customer.NationalCode):
                            patchEntity.NationalCode = valueElement;
                            break;

                        case nameof(Customer.String):
                            patchEntity.String = valueElement;
                            break;

                        case nameof(Customer.Enum):
                            patchEntity.Enum = valueElement;
                            break;

                        case nameof(Customer.NullableEnum):
                            patchEntity.NullableEnum = valueElement;
                            break;

                        case nameof(Customer.Boolean):
                            patchEntity.Boolean = valueElement;
                            break;

                        case nameof(Customer.NullableBoolean):
                            patchEntity.NullableBoolean = valueElement;
                            break;

                        case nameof(Customer.Int):
                            patchEntity.Int = valueElement;
                            break;

                        case nameof(Customer.NullableInt):
                            patchEntity.NullableInt = valueElement;
                            break;

                        case nameof(Customer.Long):
                            patchEntity.Long = valueElement;
                            break;

                        case nameof(Customer.NullableLong):
                            patchEntity.NullableLong = valueElement;
                            break;

                        case nameof(Customer.Double):
                            patchEntity.Double = valueElement;
                            break;

                        case nameof(Customer.NullableDouble):
                            patchEntity.NullableDouble = valueElement;
                            break;

                        case nameof(Customer.Decimal):
                            patchEntity.Decimal = valueElement;
                            break;

                        case nameof(Customer.NullableDecimal):
                            patchEntity.NullableDecimal = valueElement;
                            break;

                        case nameof(Customer.Guid):
                            patchEntity.Guid = valueElement;
                            break;

                        case nameof(Customer.NullableGuid):
                            patchEntity.NullableGuid = valueElement;
                            break;

                        case nameof(Customer.DateTime):
                            patchEntity.DateTime = valueElement;
                            break;

                        case nameof(Customer.NullableDateTime):
                            patchEntity.NullableDateTime = valueElement;
                            break;
                    }
                }

                patchEntities.Add(patchEntity);

                i++;
            }
        }

        return patchEntities;
    }

    private static Customer CreateCustomer(int id)
    {
        var faker = new Faker();
        var now = DateTimeOffset.Now;
        var @string = faker.Random.ToString();
        var name = "Customer";
        var family = "Customer";
        var nullableString = "Customer";
        var nationalCode = "2373850214";

        return Customer.Create(id,
            name!,
            family!,
            nationalCode,
            @string!,
            nullableString,
            RankingEnum.Gold,
            RankingEnum.Normal,
            true,
            false,
            10,
            20,
            40,
            50,
            321312.12321M,
            123.312M,
            400,
            56,
            Guid.NewGuid(),
            Guid.NewGuid(),
            now,
            now.AddDays(12));
    }

    private static Order CreateOrder(int id)
    {
        var faker = new Faker();
        var now = DateTimeOffset.Now;
        var @string = faker.Random.ToString();
        var name = "Order";
        var family = "Order";
        var nullableString = "Order";
        var nationalCode = "2373850214";

        return Order.Create(id,
            name!,
            family!,
            nationalCode,
            @string!,
            nullableString,
            RankingEnum.Gold,
            RankingEnum.Normal,
            true,
            false,
            10,
            20,
            40,
            50,
            321312.12321M,
            123.312M,
            400,
            56,
            Guid.NewGuid(),
            Guid.NewGuid(),
            now,
            now.AddDays(12));
    }
}

public sealed class Customer : Entity, IPatchValidator
{
    public int Id { get; private set; }

    public string Name { get; private set; } = null!;

    public string Family { get; private set; } = null!;

    public string NationalCode { get; private set; } = null!;

    public string String { get; private set; }

    public string? NullableString { get; private set; }

    public RankingEnum Enum { get; private set; }

    public RankingEnum? NullableEnum { get; private set; }

    public bool Boolean { get; private set; }

    public bool? NullableBoolean { get; private set; }

    public int Int { get; private set; }

    public int? NullableInt { get; private set; }

    public long Long { get; private set; }

    public long? NullableLong { get; private set; }

    public double Double { get; private set; }

    public double? NullableDouble { get; private set; }

    public decimal Decimal { get; private set; }

    public decimal? NullableDecimal { get; private set; }

    public Guid Guid { get; private set; }

    public Guid? NullableGuid { get; private set; }

    public DateTimeOffset DateTime { get; private set; }

    public DateTimeOffset? NullableDateTime { get; private set; }

    public Order NavigationOrder { get; private set; }

    public Customer NavigationCustomer { get; private set; }

    public List<Order> NavigationListOrder { get; private set; }

    public List<Customer> NavigationListCustomer { get; private set; }

    private Customer(int id, string name, string family, string nationalCode, string @string, string? nullableString, RankingEnum customerRank, RankingEnum? customerRankNullable, bool boolean, bool? nullableBoolean, int @int, int? nullableInt, double @double, double? nullableDouble, decimal @decimal, decimal? nullableDecimal, long @long, long? nullableLong, Guid guid, Guid? nullableGuid, DateTimeOffset dateTime, DateTimeOffset? nullableDateTime)
    {
        Id = id;
        Name = name;
        Family = family;
        NationalCode = nationalCode;
        String = @string;
        NullableString = nullableString;
        Enum = customerRank;
        NullableEnum = customerRankNullable;
        Boolean = boolean;
        NullableBoolean = nullableBoolean;
        Int = @int;
        NullableInt = nullableInt;
        Double = @double;
        NullableDouble = nullableDouble;
        Decimal = @decimal;
        NullableDecimal = nullableDecimal;
        Long = @long;
        NullableLong = nullableLong;
        Guid = guid;
        NullableGuid = nullableGuid;
        DateTime = dateTime;
        NullableDateTime = nullableDateTime;
        NavigationListCustomer = new List<Customer>();
        NavigationListOrder = new List<Order>();
    }

    public static Customer Create(int id, string name, string family, string nationalCode, string @string, string? nullableString, RankingEnum customerRank, RankingEnum? customerRankNullable, bool boolean, bool? nullableBoolean, int @int, int? nullableInt, double @double, double? nullableDouble, decimal @decimal, decimal? nullableDecimal, long @long, long? nullableLong, Guid guid, Guid? nullableGuid, DateTimeOffset dateTime, DateTimeOffset? nullableDateTime)
    {
        return new Customer(id, name, family, nationalCode, @string, nullableString, customerRank, customerRankNullable, boolean, nullableBoolean, @int, nullableInt, @double, nullableDouble, @decimal, nullableDecimal, @long, nullableLong, guid, nullableGuid, dateTime, nullableDateTime);
    }

    public void SetNavigation(Customer navigationCustomer)
    {
        NavigationCustomer = navigationCustomer;
    }

    public void AddUnitToNavigationList(Customer customer)
    {
        NavigationListCustomer.Add(customer);
    }

    public void SetNavigation(Order navigationOrder)
    {
        NavigationOrder = navigationOrder;
    }

    public void AddUnitToNavigationList(Order order)
    {
        NavigationListOrder.Add(order);
    }

    public bool IsPatched()
    {
        return Name == "Patched !" &&
               Family == "Patched !" &&
               NationalCode == "4120583732" &&
               Long == 999999 &&
               Int == 999999 &&
               NullableInt == 999999 &&
               Decimal == 999999.123M &&
               NullableDecimal == 999999.123M &&
               Enum == RankingEnum.Silver &&
               NullableEnum == RankingEnum.Silver;
    }

    public bool OnPatchCompleted()
    {
        return true;
    }
}

public sealed class Order : Entity, IPatchValidator
{
    public int Id { get; private set; }

    public string Name { get; private set; } = null!;

    public string Family { get; private set; } = null!;

    public string NationalCode { get; private set; } = null!;

    public string String { get; private set; }

    public string? NullableString { get; private set; }

    public RankingEnum Enum { get; private set; }

    public RankingEnum? NullableEnum { get; private set; }

    public bool Boolean { get; private set; }

    public bool? NullableBoolean { get; private set; }

    public int Int { get; private set; }

    public int? NullableInt { get; private set; }

    public long Long { get; private set; }

    public long? NullableLong { get; private set; }

    public double Double { get; private set; }

    public double? NullableDouble { get; private set; }

    public decimal Decimal { get; private set; }

    public decimal? NullableDecimal { get; private set; }

    public Guid Guid { get; private set; }

    public Guid? NullableGuid { get; private set; }

    public DateTimeOffset DateTime { get; private set; }

    public DateTimeOffset? NullableDateTime { get; private set; }

    public List<Order> NavigationListOrder { get; private set; }

    private Order(int id, string name, string family, string nationalCode, string @string, string? nullableString, RankingEnum customerRank, RankingEnum? customerRankNullable, bool boolean, bool? nullableBoolean, int @int, int? nullableInt, double @double, double? nullableDouble, decimal @decimal, decimal? nullableDecimal, long @long, long? nullableLong, Guid guid, Guid? nullableGuid, DateTimeOffset dateTime, DateTimeOffset? nullableDateTime)
    {
        Id = id;
        Name = name;
        Family = family;
        NationalCode = nationalCode;
        String = @string;
        NullableString = nullableString;
        Enum = customerRank;
        NullableEnum = customerRankNullable;
        Boolean = boolean;
        NullableBoolean = nullableBoolean;
        Int = @int;
        NullableInt = nullableInt;
        Double = @double;
        NullableDouble = nullableDouble;
        Decimal = @decimal;
        NullableDecimal = nullableDecimal;
        Long = @long;
        NullableLong = nullableLong;
        Guid = guid;
        NullableGuid = nullableGuid;
        DateTime = dateTime;
        NullableDateTime = nullableDateTime;
        NavigationListOrder = new List<Order>();
    }

    public static Order Create(int id, string name, string family, string nationalCode, string @string, string? nullableString, RankingEnum customerRank, RankingEnum? customerRankNullable, bool boolean, bool? nullableBoolean, int @int, int? nullableInt, double @double, double? nullableDouble, decimal @decimal, decimal? nullableDecimal, long @long, long? nullableLong, Guid guid, Guid? nullableGuid, DateTimeOffset dateTime, DateTimeOffset? nullableDateTime)
    {
        return new Order(id, name, family, nationalCode, @string, nullableString, customerRank, customerRankNullable, boolean, nullableBoolean, @int, nullableInt, @double, nullableDouble, @decimal, nullableDecimal, @long, nullableLong, guid, nullableGuid, dateTime, nullableDateTime);
    }

    public void AddUnitToNavigationList(Order order)
    {
        NavigationListOrder.Add(order);
    }

    public bool IsPatched()
    {
        return Name == "Patched !" &&
               Family == "Patched !" &&
               NationalCode == "4120583732" &&
               Long == 999999 &&
               Int == 999999 &&
               NullableInt == 999999 &&
               Decimal == 999999.123M &&
               NullableDecimal == 999999.123M &&
               Enum == RankingEnum.Silver &&
               NullableEnum == RankingEnum.Silver;
    }

    public bool OnPatchCompleted()
    {
        return true;
    }
}

public sealed class OrderItem
{

}

public sealed class OrderItemHistory
{

}
public sealed class OrderItemHistoryOption
{

}

public sealed class CustomerIdentity
{

}

public sealed class ShoppingCart
{

}

public sealed class ShoppingCartItem
{

}

public sealed class ShoppingCartItemHistory
{

}

public sealed class ChiefExecutiveOfficer
{

}

public sealed class ChiefTechnicalOfficer
{

}

public sealed class TechnicalTeamLead
{

}

public sealed class SeniorDeveloper
{

}

public sealed class MidLevelDeveloper
{

}

public sealed class JuniorDeveloper
{

}

public sealed class FresherDeveloper
{

}

public sealed class ChiefProductOfficer
{

}

public sealed class ProductTeamLead
{

}

public sealed class SeniorProductManager
{

}

public sealed class ChiefMarketingOfficer
{

}

public sealed class MarketingTeamLead
{

}

public sealed class SeniorMarketing
{

}

public enum RankingEnum
{
    Normal = 0,
    Silver = 1,
    Gold = 2,
    Diamond = 3,
}
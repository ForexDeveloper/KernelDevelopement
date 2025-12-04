using Foodzilla.Kernel.Domain;

namespace Foodzilla.Kernel.UnitTest.Domain;

public abstract class Identity<TKey> : Entity<TKey> where TKey : struct
{
    public string Name { get; init; }

    public string LastName { get; init; }

    public string NationalCode { get; init; }

    public string PersonalCode { get; init; }

    public string Address { get; init; }

    public int Age { get; init; }

    public int? DaysOfVacation { get; init; }

    public decimal? Height { get; init; }

    public decimal? Weight { get; init; }

    public bool IsFired { get; init; }

    public Guid UniqueIdentifier { get; init; }

    public EyeColor? EyeColor { get; init; }

    public Graduation Graduation { get; init; }

    public Experience Experience { get; init; }

    public DateTimeOffset? ModifiedDate { get; init; }

    public DateTimeOffset BirthDate { get; init; }

    public DateTimeOffset ContractDateEnd { get; init; }

    public DateTimeOffset ContractDateStart { get; init; }

    public virtual string[] RestrictedProperties { get; } = [nameof(NationalCode), nameof(PersonalCode)];

    protected Identity()
    {

    }

    protected Identity(string name, string lastName, string nationalCode, string personalCode, string address, int age,
        int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor,
        Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate,
        DateTimeOffset contractDateEnd, DateTimeOffset contractDateStart)
    {
        Age = age;
        Name = name;
        Height = height;
        Weight = weight;
        IsFired = isFired;
        Address = address;
        EyeColor = eyeColor;
        LastName = lastName;
        BirthDate = birthDate;
        Graduation = graduation;
        Experience = experience;
        NationalCode = nationalCode;
        PersonalCode = personalCode;
        ModifiedDate = modifiedDate;
        DaysOfVacation = daysOfVacation;
        ContractDateEnd = contractDateEnd;
        UniqueIdentifier = uniqueIdentifier;
        ContractDateStart = contractDateStart;
        CreatedAt = DateTimeOffset.Now.AddYears(-1);
    }

    public virtual bool IsPatched()
    {
        return Name == "Patched !" &&
               LastName == "Patched !" &&
               NationalCode == "99999999" &&
               PersonalCode == "Patched !" &&
               Address == "Patched !" &&
               Height == 188 &&
               Weight == 85 &&
               Experience == Experience.Elementary &&
               Graduation == Graduation.Diploma;
    }
}

public enum Graduation
{
    Diploma = 0,
    Bachelor = 1,
    Master = 2,
    Associate = 3,
    Phd = 4,
}

public enum EyeColor
{
    Black = 0,
    Brown = 1,
    Green = 2,
    Blue = 3,
    Gold = 4,
}

public enum Experience
{
    Elementary = 0,
    Intermediate = 1,
    Advance = 2
}
using Foodzilla.Kernel.Domain;

namespace Foodzilla.Kernel.UnitTest.Domain;

public abstract class Identity<TKey> : Entity<TKey> where TKey : struct
{
    public string Name { get; private set; }

    public string LastName { get; private set; }

    public string NationalCode { get; private set; }

    public string PersonalCode { get; private set; }

    public string Address { get; private set; }

    public int Age { get; private set; }

    public int? DaysOfVacation { get; private set; }

    public decimal? Height { get; private set; }

    public decimal? Weight { get; private set; }

    public bool IsFired { get; private set; }

    public Guid UniqueIdentifier { get; private set; }

    public EyeColor? EyeColor { get; private set; }

    public Graduation Graduation { get; private set; }

    public Experience Experience { get; private set; }

    public DateTimeOffset? ModifiedDate { get; private set; }

    public DateTimeOffset BirthDate { get; private set; }

    public DateTimeOffset ContraDateEnd { get; private set; }

    public DateTimeOffset ContraDateStart { get; private set; }

    public virtual string[] RestrictedProperties { get; } = { nameof(NationalCode), nameof(PersonalCode) };

    protected Identity(string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart)
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
        ContraDateEnd = contraDateEnd;
        DaysOfVacation = daysOfVacation;
        ContraDateStart = contraDateStart;
        UniqueIdentifier = uniqueIdentifier;
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
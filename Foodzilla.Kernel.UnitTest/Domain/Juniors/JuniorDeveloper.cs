using Foodzilla.Kernel.Patch;
using Foodzilla.Kernel.UnitTest.Domain.Freshers;
using Foodzilla.Kernel.UnitTest.Domain.MidLevels;

namespace Foodzilla.Kernel.UnitTest.Domain.Juniors;

public sealed class JuniorDeveloper : Identity<long>, IPatchValidator
{
    public long MidlevelDeveloperId { get; private set; }

    public MidlevelDeveloper MidlevelDeveloper { get; private set; }

    public IList<FresherDeveloper> Freshers => new List<FresherDeveloper>();

    private JuniorDeveloper(long id, string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, long midlevelDeveloperId)
        : base(name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart)
    {
        SetIdentity(id);
        MidlevelDeveloperId = midlevelDeveloperId;
    }

    public static JuniorDeveloper Create(long id, string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, long midlevelDeveloperId)
    {
        return new JuniorDeveloper(id, name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart, midlevelDeveloperId);
    }

    public void AddFresherTechnical(FresherDeveloper fresher)
    {
        Freshers.Add(fresher);
    }

    public bool OnPatchCompleted()
    {
        return true;
    }
}
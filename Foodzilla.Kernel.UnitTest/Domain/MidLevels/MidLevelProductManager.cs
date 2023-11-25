using Foodzilla.Kernel.Patch;
using Foodzilla.Kernel.UnitTest.Domain.Juniors;
using Foodzilla.Kernel.UnitTest.Domain.Seniors;

namespace Foodzilla.Kernel.UnitTest.Domain.MidLevels;

public sealed class MidlevelProductManager : Identity<long>, IPatchValidator
{
    public long SeniorProductManagerId { get; private set; }

    public SeniorProductManager? SeniorProductManager { get; init; }

    public List<JuniorProductManager> Juniors { get; init; } = new List<JuniorProductManager>();

    private MidlevelProductManager(long id, string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, long seniorProductManagerId)
        : base(name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart)
    {
        SetIdentity(id);
        SeniorProductManagerId = seniorProductManagerId;
    }

    public static MidlevelProductManager Create(long id, string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, long seniorProductManagerId)
    {
        return new MidlevelProductManager(id, name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart, seniorProductManagerId);
    }

    public void AddJuniorProductManager(JuniorProductManager junior)
    {
        Juniors.Add(junior);
    }

    public bool OnPatchCompleted()
    {
        return true;
    }
}
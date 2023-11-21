using Foodzilla.Kernel.Patch;
using Foodzilla.Kernel.UnitTest.Domain.Juniors;

namespace Foodzilla.Kernel.UnitTest.Domain.Freshers;

public sealed class FresherProductManager : Identity<long>, IPatchValidator
{
    public long JuniorProductManagerId { get; private set; }

    public JuniorProductManager JuniorProductManager { get; private set; }

    private FresherProductManager(string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, long juniorProductManagerId)
        : base(name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart)
    {
        JuniorProductManagerId = juniorProductManagerId;
    }

    public static FresherProductManager Create(string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, long juniorProductManagerId)
    {
        return new FresherProductManager(name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart, juniorProductManagerId);
    }

    public bool OnPatchCompleted()
    {
        return true;
    }
}
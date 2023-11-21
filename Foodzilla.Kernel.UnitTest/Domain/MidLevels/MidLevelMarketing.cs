using Foodzilla.Kernel.Patch;
using Foodzilla.Kernel.UnitTest.Domain.Juniors;

namespace Foodzilla.Kernel.UnitTest.Domain.MidLevels;

public sealed class MidlevelMarketing : Identity<long>, IPatchValidator
{
    public IReadOnlyCollection<JuniorMarketing>? Juniors { get; private set; }

    private MidlevelMarketing(string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart)
        : base(name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart)
    {
    }

    public static MidlevelMarketing Create(string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart)
    {
        return new MidlevelMarketing(name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart);
    }

    public bool OnPatchCompleted()
    {
        return true;
    }
}
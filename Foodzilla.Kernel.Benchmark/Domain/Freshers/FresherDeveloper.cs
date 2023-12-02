using Foodzilla.Kernel.Benchmark.Domain.Juniors;
using Foodzilla.Kernel.Patch;

namespace Foodzilla.Kernel.Benchmark.Domain.Freshers;

public sealed class FresherDeveloper : Identity<long>, IPatchValidator
{
    public long JuniorDeveloperId { get; private set; }

    public JuniorDeveloper? JuniorDeveloper { get; init; }

    private FresherDeveloper(long id, string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, long juniorDeveloperId)
        : base(name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart)
    {
        SetIdentity(id);
        JuniorDeveloperId = juniorDeveloperId;
    }

    public static FresherDeveloper Create(long id, string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, long juniorDeveloperId)
    {
        return new FresherDeveloper(id, name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart, juniorDeveloperId);
    }

    public bool OnPatchCompleted()
    {
        return true;
    }
}
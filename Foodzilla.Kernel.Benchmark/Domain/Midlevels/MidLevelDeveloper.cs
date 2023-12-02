using Foodzilla.Kernel.Benchmark.Domain.Juniors;
using Foodzilla.Kernel.Benchmark.Domain.Seniors;
using Foodzilla.Kernel.Patch;

namespace Foodzilla.Kernel.Benchmark.Domain.Midlevels;

public sealed class MidlevelDeveloper : Identity<long>, IPatchValidator
{
    public long SeniorDeveloperId { get; private set; }

    public SeniorDeveloper? SeniorDeveloper { get; init; }

    public List<JuniorDeveloper> Juniors { get; init; } = new List<JuniorDeveloper>();

    private MidlevelDeveloper(long id, string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, long seniorDeveloperId)
        : base(name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart)
    {
        SetIdentity(id);
        SeniorDeveloperId = seniorDeveloperId;
    }

    public static MidlevelDeveloper Create(long id, string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, long seniorDeveloperId)
    {
        return new MidlevelDeveloper(id, name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart, seniorDeveloperId);
    }

    public void AddJuniorTechnical(JuniorDeveloper junior)
    {
        Juniors.Add(junior);
    }

    public bool OnPatchCompleted()
    {
        return true;
    }
}
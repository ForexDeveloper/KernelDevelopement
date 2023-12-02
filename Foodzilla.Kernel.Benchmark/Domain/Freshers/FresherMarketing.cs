using Foodzilla.Kernel.Benchmark.Domain.Juniors;
using Foodzilla.Kernel.Patch;

namespace Foodzilla.Kernel.Benchmark.Domain.Freshers;

public sealed class FresherMarketing : Identity<long>, IPatchValidator
{
    public long JuniorMarketingId { get; private set; }

    public JuniorMarketing? JuniorMarketing { get; init; }

    private FresherMarketing(long id, string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, long juniorMarketingId)
        : base(name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart)
    {
        SetIdentity(id);
        JuniorMarketingId = juniorMarketingId;
    }

    public static FresherMarketing Create(long id, string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, long juniorMarketingId)
    {
        return new FresherMarketing(id, name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart, juniorMarketingId);
    }

    public bool OnPatchCompleted()
    {
        return true;
    }
}
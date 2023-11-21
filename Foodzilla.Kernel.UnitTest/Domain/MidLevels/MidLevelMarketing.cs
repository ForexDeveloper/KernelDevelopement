using Foodzilla.Kernel.Patch;
using Foodzilla.Kernel.UnitTest.Domain.Juniors;
using Foodzilla.Kernel.UnitTest.Domain.Seniors;

namespace Foodzilla.Kernel.UnitTest.Domain.MidLevels;

public sealed class MidlevelMarketing : Identity<long>, IPatchValidator
{
    public long SeniorMarketingId { get; private set; }

    public SeniorMarketing SeniorMarketing { get; private set; }

    public IList<JuniorMarketing> Juniors => new List<JuniorMarketing>();

    private MidlevelMarketing(string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, long seniorMarketingId)
        : base(name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart)
    {
        SeniorMarketingId = seniorMarketingId;
    }

    public static MidlevelMarketing Create(string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, long seniorMarketingId)
    {
        return new MidlevelMarketing(name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart, seniorMarketingId);
    }

    public void AddJuniorMarketing(JuniorMarketing junior)
    {
        Juniors.Add(junior);
    }

    public bool OnPatchCompleted()
    {
        return true;
    }
}
using Foodzilla.Kernel.Patch;
using Foodzilla.Kernel.UnitTest.Domain.Freshers;
using Foodzilla.Kernel.UnitTest.Domain.MidLevels;

namespace Foodzilla.Kernel.UnitTest.Domain.Juniors;

public sealed class JuniorMarketing : Identity<long>, IPatchValidator
{
    public long MidlevelMarketingId { get; private set; }

    public MidlevelMarketing? MidlevelMarketing { get; init; }

    public List<FresherMarketing> Freshers { get; init; } = [];

    private JuniorMarketing(long id, string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, long midlevelMarketingId)
        : base(name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart)
    {
        SetIdentity(id);
        MidlevelMarketingId = midlevelMarketingId;
    }

    public JuniorMarketing()
    {
        
    }

    public static JuniorMarketing Create(long id, string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, long midlevelMarketingId)
    {
        return new JuniorMarketing(id, name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart, midlevelMarketingId);
    }

    public void AddFresherMarketing(FresherMarketing fresher)
    {
        Freshers.Add(fresher);
    }

    public bool OnPatchCompleted()
    {
        return true;
    }
}
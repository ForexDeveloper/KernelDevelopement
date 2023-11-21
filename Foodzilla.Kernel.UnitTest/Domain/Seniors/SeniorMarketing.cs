using Foodzilla.Kernel.Patch;
using Foodzilla.Kernel.UnitTest.Domain.MidLevels;
using Foodzilla.Kernel.UnitTest.Domain.TeamLeads;

namespace Foodzilla.Kernel.UnitTest.Domain.Seniors;

public sealed class SeniorMarketing : Identity<long>, IPatchValidator
{
    public int MarketingTeamLeadId { get; private set; }

    public MarketingTeamLead MarketingTeamLead { get; private set; }

    public IList<MidlevelMarketing> Midlevels => new List<MidlevelMarketing>();

    private SeniorMarketing(string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, int marketingTeamLeadId)
        : base(name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart)
    {
        MarketingTeamLeadId = marketingTeamLeadId;
    }
    
    public static SeniorMarketing Create(string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, int marketingTeamLeadId)
    {
        return new SeniorMarketing(name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart, marketingTeamLeadId);
    }

    public void AddMidlevelMarketing(MidlevelMarketing midlevel)
    {
        Midlevels.Add(midlevel);
    }

    public bool OnPatchCompleted()
    {
        return true;
    }
}
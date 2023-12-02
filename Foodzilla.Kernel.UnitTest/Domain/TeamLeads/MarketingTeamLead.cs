using Foodzilla.Kernel.Patch;
using Foodzilla.Kernel.UnitTest.Domain.Seniors;
using Foodzilla.Kernel.UnitTest.Domain.ChiefOfficers;

namespace Foodzilla.Kernel.UnitTest.Domain.TeamLeads;

public sealed class MarketingTeamLead : Identity<int>, IPatchValidator
{
    public int ChiefMarketingOfficerId { get; private set; }

    public ChiefMarketingOfficer? ChiefMarketingOfficer { get; init; }

    public List<SeniorMarketing> Seniors { get; init; } = new List<SeniorMarketing>();

    private MarketingTeamLead(int id, string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, int chiefMarketingOfficerId)
        : base(name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart)
    {
        SetIdentity(id);
        ChiefMarketingOfficerId = chiefMarketingOfficerId;
    }

    public MarketingTeamLead()
    {
        
    }

    public static MarketingTeamLead Create(int id, string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, int chiefMarketingOfficerId)
    {
        return new MarketingTeamLead(id, name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart, chiefMarketingOfficerId);
    }

    public void AddSeniorMarketing(SeniorMarketing senior)
    {
        Seniors.Add(senior);
    }

    public bool OnPatchCompleted()
    {
        return true;
    }
}
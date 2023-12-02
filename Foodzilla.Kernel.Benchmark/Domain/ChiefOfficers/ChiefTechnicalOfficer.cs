using Foodzilla.Kernel.Benchmark.Domain.TeamLeads;
using Foodzilla.Kernel.Patch;

namespace Foodzilla.Kernel.Benchmark.Domain.ChiefOfficers;

public sealed class ChiefTechnicalOfficer : ChiefUnitIdentity, IPatchValidator
{
    public int ChiefExecutiveOfficerId { get; init; }

    public ChiefExecutiveOfficer? ChiefExecutiveOfficer { get; init; }

    public List<TechnicalTeamLead> TechnicalTeamLeads { get; init; } = new List<TechnicalTeamLead>();

    public List<QaTestingTeamLead> QaTestingTeamLeads { get; init; } = new List<QaTestingTeamLead>();

    private ChiefTechnicalOfficer(int id, string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, string assignedOrganization)
        : base(name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart, assignedOrganization)
    {
        SetIdentity(id);
    }

    public static ChiefTechnicalOfficer Create(int id, string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, string assignedOrganization)
    {
        return new ChiefTechnicalOfficer(id, name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart, assignedOrganization);
    }

    public void AddTechnicalLead(TechnicalTeamLead technicalTeamLead)
    {
        TechnicalTeamLeads.Add(technicalTeamLead);
    }

    public void AddLeadQaTesting(QaTestingTeamLead qaTestingTeamLead)
    {
        QaTestingTeamLeads.Add(qaTestingTeamLead);
    }

    public bool OnPatchCompleted()
    {
        return true;
    }
}
using Foodzilla.Kernel.Patch;
using Foodzilla.Kernel.UnitTest.Domain.Seniors;
using Foodzilla.Kernel.UnitTest.Domain.ChiefOfficers;

namespace Foodzilla.Kernel.UnitTest.Domain.TeamLeads;

public sealed class TechnicalTeamLead : Identity<int>, IPatchValidator
{
    public int ChiefTechnicalOfficerId { get; private set; }

    public ChiefTechnicalOfficer ChiefTechnicalOfficer { get; private set; }

    public IList<SeniorDeveloper> Seniors => new List<SeniorDeveloper>();

    private TechnicalTeamLead(string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, int chiefTechnicalOfficerId)
        : base(name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart)
    {
        ChiefTechnicalOfficerId = chiefTechnicalOfficerId;
    }

    public static TechnicalTeamLead Create(string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, int chiefTechnicalOfficerId)
    {
        return new TechnicalTeamLead(name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart, chiefTechnicalOfficerId);
    }

    public void AddSeniorTechnical(SeniorDeveloper senior)
    {
        Seniors.Add(senior);
    }

    public bool OnPatchCompleted()
    {
        return true;
    }
}
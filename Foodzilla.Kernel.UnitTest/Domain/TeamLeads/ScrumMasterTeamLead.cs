using Foodzilla.Kernel.Patch;
using Foodzilla.Kernel.UnitTest.Domain.Seniors;
using Foodzilla.Kernel.UnitTest.Domain.ChiefOfficers;

namespace Foodzilla.Kernel.UnitTest.Domain.TeamLeads;

public sealed class ScrumMasterTeamLead : Identity<int>, IPatchValidator
{
    public int ChiefProductOfficerId { get; private set; }

    public ChiefProductOfficer ChiefProductOfficer { get; private set; }

    public IList<SeniorScrumMaster> Seniors => new List<SeniorScrumMaster>();

    private ScrumMasterTeamLead(string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, int chiefProductOfficerId)
        : base(name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart)
    {
        ChiefProductOfficerId = chiefProductOfficerId;
    }

    public static ScrumMasterTeamLead Create(string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, int chiefProductOfficerId)
    {
        return new ScrumMasterTeamLead(name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart, chiefProductOfficerId);
    }

    public void AddSeniorScrumMaster(SeniorScrumMaster senior)
    {
        Seniors.Add(senior);
    }

    public bool OnPatchCompleted()
    {
        return true;
    }
}
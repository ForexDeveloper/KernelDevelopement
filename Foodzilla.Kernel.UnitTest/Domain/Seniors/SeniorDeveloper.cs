using Foodzilla.Kernel.Patch;
using Foodzilla.Kernel.UnitTest.Domain.MidLevels;
using Foodzilla.Kernel.UnitTest.Domain.TeamLeads;

namespace Foodzilla.Kernel.UnitTest.Domain.Seniors;

public sealed class SeniorDeveloper : Identity<long>, IPatchValidator
{
    public int TechnicalTeamLeadId { get; private set; }

    public TechnicalTeamLead TechnicalTeamLead {get; private set; }

    public IList<MidlevelDeveloper> Midlevels => new List<MidlevelDeveloper>();

    private SeniorDeveloper(string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, int technicalTeamLeadId)
        : base(name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart)
    {
        TechnicalTeamLeadId = technicalTeamLeadId;
    }

    public static SeniorDeveloper Create(string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, int technicalTeamLeadId)
    {
        return new SeniorDeveloper(name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart, technicalTeamLeadId);
    }

    public void AddMidlevelTechnical(MidlevelDeveloper midlevel)
    {
        Midlevels.Add(midlevel);
    }

    public bool OnPatchCompleted()
    {
        return true;
    }
}
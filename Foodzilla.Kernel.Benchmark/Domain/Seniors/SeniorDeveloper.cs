using Foodzilla.Kernel.Benchmark.Domain.Midlevels;
using Foodzilla.Kernel.Benchmark.Domain.TeamLeads;
using Foodzilla.Kernel.Patch;

namespace Foodzilla.Kernel.Benchmark.Domain.Seniors;

public sealed class SeniorDeveloper : Identity<long>, IPatchValidator
{
    public int TechnicalTeamLeadId { get; private set; }

    public TechnicalTeamLead? TechnicalTeamLead { get; init; }

    public List<MidlevelDeveloper> Midlevels { get; init; } = new List<MidlevelDeveloper>();

    private SeniorDeveloper(long id, string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, int technicalTeamLeadId)
        : base(name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart)
    {
        SetIdentity(id);
        TechnicalTeamLeadId = technicalTeamLeadId;
    }

    public static SeniorDeveloper Create(long id, string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, int technicalTeamLeadId)
    {
        return new SeniorDeveloper(id, name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart, technicalTeamLeadId);
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
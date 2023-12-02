using Foodzilla.Kernel.Benchmark.Domain.ChiefOfficers;
using Foodzilla.Kernel.Benchmark.Domain.Seniors;
using Foodzilla.Kernel.Patch;

namespace Foodzilla.Kernel.Benchmark.Domain.TeamLeads;

public sealed class QaTestingTeamLead : Identity<int>, IPatchValidator
{
    public int ChiefTechnicalOfficerId { get; private set; }

    public ChiefTechnicalOfficer? ChiefTechnicalOfficer { get; init; }

    public List<SeniorQaTesting> Seniors { get; init; } = new List<SeniorQaTesting>();

    private QaTestingTeamLead(int id, string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, int chiefTechnicalOfficerId)
        : base(name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart)
    {
        SetIdentity(id);
        ChiefTechnicalOfficerId = chiefTechnicalOfficerId;
    }

    public static QaTestingTeamLead Create(int id, string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, int chiefTechnicalOfficerId)
    {
        return new QaTestingTeamLead(id, name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart, chiefTechnicalOfficerId);
    }

    public void AddSeniorQaTesting(SeniorQaTesting senior)
    {
        Seniors.Add(senior);
    }

    public bool OnPatchCompleted()
    {
        return true;
    }
}
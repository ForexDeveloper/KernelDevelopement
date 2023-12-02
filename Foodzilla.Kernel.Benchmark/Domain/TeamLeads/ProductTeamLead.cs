using Foodzilla.Kernel.Benchmark.Domain.ChiefOfficers;
using Foodzilla.Kernel.Benchmark.Domain.Seniors;
using Foodzilla.Kernel.Patch;

namespace Foodzilla.Kernel.Benchmark.Domain.TeamLeads;

public sealed class ProductTeamLead : Identity<int>, IPatchValidator
{
    public int ChiefProductOfficerId { get; private set;}

    public ChiefProductOfficer? ChiefProductOfficer { get; init; }

    public List<SeniorProductManager> Seniors { get; init; } = new List<SeniorProductManager>();

    private ProductTeamLead(int id, string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, int chiefProductOfficerId)
        : base(name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart)
    {
        SetIdentity(id);
        ChiefProductOfficerId = chiefProductOfficerId;
    }

    public static ProductTeamLead Create(int id, string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, int chiefProductOfficerId)
    {
        return new ProductTeamLead(id, name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart, chiefProductOfficerId);
    }

    public void AddSeniorProductManger(SeniorProductManager senior)
    {
        Seniors.Add(senior);
    }

    public bool OnPatchCompleted()
    {
        return true;
    }
}
namespace Foodzilla.Kernel.UnitTest.Domain.ChiefOfficers;

public abstract class ChiefUnitIdentity : Identity<int>
{
    public string AssignedOrganization { get; set; }

    protected ChiefUnitIdentity(string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contractDateEnd, DateTimeOffset contractDateStart, string assignedOrganization)
        : base(name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart)
    {
        AssignedOrganization = assignedOrganization;
    }

    public override bool IsPatched()
    {
        return AssignedOrganization == "SnappTrip" && base.IsPatched();
    }
}
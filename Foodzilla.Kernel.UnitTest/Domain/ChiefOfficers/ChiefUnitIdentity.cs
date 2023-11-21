﻿namespace Foodzilla.Kernel.UnitTest.Domain.ChiefOfficers;

public abstract class ChiefUnitIdentity : Identity<int>
{
    public string AssignedOrganization { get; private set; }

    protected ChiefUnitIdentity(string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, string assignedOrganization)
        : base(name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart)
    {
        AssignedOrganization = assignedOrganization;
    }
}
﻿using Foodzilla.Kernel.Patch;

namespace Foodzilla.Kernel.UnitTest.Domain.ChiefOfficers;

public sealed class ChiefTechnicalOfficer : ChiefUnitIdentity, IPatchValidator
{
    public int ChiefTechnicalOfficerId { get; private set; }

    public ChiefProductOfficer? ChiefProductOfficer { get; private set; }

    public int ChiefMarketingOfficerId { get; private set; }

    public ChiefExecutiveOfficer? ChiefExecutiveOfficer { get; private set; }

    public int ChiefProductOfficerId { get; private set; }

    public ChiefMarketingOfficer? ChiefMarketingOfficer { get; private set; }

    private ChiefTechnicalOfficer(string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, string assignedOrganization)
        : base(name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart, assignedOrganization)
    {
    }

    public static ChiefTechnicalOfficer Create(string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, string assignedOrganization)
    {
        return new ChiefTechnicalOfficer(name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart, assignedOrganization);
    }

    public bool OnPatchCompleted()
    {
        return true;
    }
}
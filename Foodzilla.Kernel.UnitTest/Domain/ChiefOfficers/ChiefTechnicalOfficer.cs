﻿using Foodzilla.Kernel.Patch;
using Foodzilla.Kernel.UnitTest.Domain.TeamLeads;

namespace Foodzilla.Kernel.UnitTest.Domain.ChiefOfficers;

public sealed class ChiefTechnicalOfficer : ChiefUnitIdentity, IPatchValidator
{
    public int ChiefExecutiveOfficerId { get; private set; }

    public ChiefExecutiveOfficer? ChiefExecutiveOfficer { get; private set; }

    public IList<TechnicalTeamLead> TechnicalTeamLeads => new List<TechnicalTeamLead>();

    public IList<QaTestingTeamLead> QaTestingTeamLeads => new List<QaTestingTeamLead>();

    private ChiefTechnicalOfficer(string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, string assignedOrganization)
        : base(name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart, assignedOrganization)
    {
    }

    public static ChiefTechnicalOfficer Create(string name, string lastName, string nationalCode, string personalCode, string address, int age, int? daysOfVacation, decimal? height, decimal? weight, bool isFired, Guid uniqueIdentifier, EyeColor? eyeColor, Graduation graduation, Experience experience, DateTimeOffset? modifiedDate, DateTimeOffset birthDate, DateTimeOffset contraDateEnd, DateTimeOffset contraDateStart, string assignedOrganization)
    {
        return new ChiefTechnicalOfficer(name, lastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, isFired, uniqueIdentifier, eyeColor, graduation, experience, modifiedDate, birthDate, contraDateEnd, contraDateStart, assignedOrganization);
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
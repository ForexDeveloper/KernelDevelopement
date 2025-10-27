using System.Dynamic;
using Foodzilla.Kernel.Extension;
using Foodzilla.Kernel.UnitTest.Domain;
using Foodzilla.Kernel.UnitTest.Domain.Juniors;
using Foodzilla.Kernel.UnitTest.Domain.Seniors;
using Foodzilla.Kernel.UnitTest.Domain.Freshers;
using Foodzilla.Kernel.UnitTest.Domain.MidLevels;
using Foodzilla.Kernel.UnitTest.Domain.TeamLeads;
using Foodzilla.Kernel.UnitTest.Domain.ChiefOfficers;

namespace Foodzilla.Kernel.UnitTest;

public static class PatchEngine
{
    private static readonly DateTimeOffset ModifiedDate = DateTimeOffset.Now;

    private static readonly DateTimeOffset BirthDate = DateTimeOffset.Now.AddYears(-25);
    private static readonly DateTimeOffset BirthDate10 = BirthDate.AddYears(-10);
    private static readonly DateTimeOffset BirthDate20 = BirthDate.AddYears(-20);
    private static readonly DateTimeOffset BirthDate30 = BirthDate.AddYears(-30);
    private static readonly DateTimeOffset BirthDate40 = BirthDate.AddYears(-40);
    private static readonly DateTimeOffset BirthDate50 = BirthDate.AddYears(-50);

    private static readonly DateTimeOffset ContractDateEnd1 = DateTimeOffset.Now.AddYears(1);
    private static readonly DateTimeOffset ContractDateEnd2 = ContractDateEnd1.AddYears(2);
    private static readonly DateTimeOffset ContractDateEnd3 = ContractDateEnd1.AddYears(3);
    private static readonly DateTimeOffset ContractDateEnd4 = ContractDateEnd1.AddYears(4);
    private static readonly DateTimeOffset ContractDateEnd5 = ContractDateEnd1.AddYears(5);
    private static readonly DateTimeOffset ContractDateEnd6 = ContractDateEnd1.AddYears(6);

    private static readonly DateTimeOffset ContractDateStart1 = DateTimeOffset.Now.AddYears(-1);
    private static readonly DateTimeOffset ContractDateStart2 = ContractDateEnd1.AddYears(-2);
    private static readonly DateTimeOffset ContractDateStart3 = ContractDateEnd1.AddYears(-3);
    private static readonly DateTimeOffset ContractDateStart4 = ContractDateEnd1.AddYears(-4);
    private static readonly DateTimeOffset ContractDateStart5 = ContractDateEnd1.AddYears(-5);
    private static readonly DateTimeOffset ContractDateStart6 = ContractDateEnd1.AddYears(-6);

    private static string[] Organizations => ["Google", "Microsoft", "Apple", "SpaceX", "Amazon", "Facebook"];

    private static string[] OfficerNames => ["Christopher", "David", "John", "Steve", "Leonardo", "Rafael"];

    private static string[] OfficerLastNames => ["Stones", "Hamilton", "Rabin", "Anderson", "Kane", "Smith"];

    private static string[] OtherLevelNames => ["Micheal", "Tom", "Victor", "Williams", "Sam", "Harry"];

    private static string[] OtherLevelLastNames => ["Jordan", "Kennedy", "Graham", "Jenkins", "Garret", "Bradley"];

    private static string[] NationalCodes => ["4120583732", "9182736455", "1324354657", "0897867564", "1425364758", "0896857463"];

    private static string[] PersonalCodes => ["#MdEjdSjE$kEfKxD#", "#MdRkXkEk&jDjExS#", "#MdsSdkEk&jDjExS#", "#PoLefDkEk&jDjExS#", "#OldElXkEk&jDjExS#", "#QrEtyUeIkEk&jDjEx#"];

    private static string[] Addresses => ["#MdEjdSjE$kEfKxD#", "#MdRkXkEk&jDjExS#", "#MdsSdkEk&jDjExS#", "#PoLefDkEk&jDjExS#", "#OldElXkEk&jDjExS#", "#QrEtyUeIkEk&jDjEx#"];

    private static int[] Ages => [25, 30, 35, 45, 55, 65];

    private static bool[] FireStatuses => [true, false, true, false, true, false];

    private static decimal?[] Weights => [80, null, 78.5M, null, 85, null];

    private static decimal?[] Heights => [187.56M, null, 178.5M, null, 196, null];

    private static int?[] DaysOfVacation => [2, null, 3, null, 4, null];

    private static EyeColor?[] EyeColors => [EyeColor.Black, null, EyeColor.Blue, null, EyeColor.Green, null];

    private static Graduation[] Graduations => [Graduation.Diploma, Graduation.Bachelor, Graduation.Associate, Graduation.Master, Graduation.Phd, Graduation.Bachelor];

    private static Experience[] Experiences => [Experience.Elementary, Experience.Intermediate, Experience.Advance, Experience.Elementary, Experience.Intermediate, Experience.Advance];

    private static DateTimeOffset[] BirthDates => [BirthDate, BirthDate10, BirthDate20, BirthDate30, BirthDate40, BirthDate50];

    private static DateTimeOffset?[] ModifiedDates => [ModifiedDate, ModifiedDate.AddDays(1), null, ModifiedDate.AddDays(-10), null, null];

    private static DateTimeOffset[] ContractDatesStart => [ContractDateStart1, ContractDateStart2, ContractDateStart3, ContractDateStart4, ContractDateStart5, ContractDateStart6];

    private static DateTimeOffset[] ContractDatesEnd => [ContractDateEnd1, ContractDateEnd2, ContractDateEnd3, ContractDateEnd4, ContractDateEnd5, ContractDateEnd6];

    private static Guid[] UniqueIdentifiers => [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()];

    public static List<ChiefExecutiveOfficer> CreateChiefExecutiveOfficers(int count)
    {
        var random = new Random();
        var chiefExecutiveOfficers = new List<ChiefExecutiveOfficer>();

        for (var officerId = 1; officerId <= count; officerId++)
        {
            int index = random.Next(0, 6);

            var age = Ages[index];
            var height = Heights[index];
            var weight = Weights[index];
            var address = Addresses[index];
            var eyeColor = EyeColors[index];
            var birthDate = BirthDates[index];
            var graduation = Graduations[index];
            var experience = Experiences[index];
            var officerName = OfficerNames[index];
            var modifiedDate = ModifiedDates[index];
            var organization = Organizations[index];
            var nationalCode = NationalCodes[index];
            var personalCode = PersonalCodes[index];
            var daysOfVacation = DaysOfVacation[index];
            var otherLevelName = OtherLevelNames[index];
            var officerLastName = OfficerLastNames[index];
            var contractDateEnd = ContractDatesEnd[index];
            var uniqueIdentifier = UniqueIdentifiers[index];
            var contractDateStart = ContractDatesStart[index];
            var otherLevelLastName = OtherLevelLastNames[index];

            var chiefExecutiveOfficer = ChiefExecutiveOfficer.Create(officerId, officerName, officerLastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, organization);

            var chiefTechnicalOfficer = ChiefTechnicalOfficer.Create(officerId, officerName, officerLastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, organization);

            var chiefProductOfficer = ChiefProductOfficer.Create(officerId, officerName, officerLastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, organization);

            var chiefMarketingOfficer = ChiefMarketingOfficer.Create(officerId, officerName, officerLastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, organization);

            chiefExecutiveOfficers.Add(chiefExecutiveOfficer);
            chiefExecutiveOfficer.AddChiefProductOfficer(chiefProductOfficer);
            chiefExecutiveOfficer.AddChiefMarketingOfficer(chiefMarketingOfficer);
            chiefExecutiveOfficer.AddChiefTechnicalOfficer(chiefTechnicalOfficer);

            for (int teamLeadId = 1; teamLeadId <= count; teamLeadId++)
            {
                var technicalTeamLead = TechnicalTeamLead.Create(teamLeadId, otherLevelName, otherLevelLastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                    graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, chiefTechnicalOfficer.Id);

                var qATestingTeamLead = QaTestingTeamLead.Create(teamLeadId, otherLevelName, otherLevelLastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                    graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, chiefTechnicalOfficer.Id);

                chiefTechnicalOfficer.AddTechnicalLead(technicalTeamLead);
                chiefTechnicalOfficer.AddLeadQaTesting(qATestingTeamLead);

                var productTeamLead = ProductTeamLead.Create(teamLeadId, otherLevelName, otherLevelLastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                    graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, chiefProductOfficer.Id);

                var scrumMasterTeamLead = ScrumMasterTeamLead.Create(teamLeadId, otherLevelName, otherLevelLastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                    graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, chiefProductOfficer.Id);

                chiefProductOfficer.AddLeadProductManager(productTeamLead);
                chiefProductOfficer.AddLeadScrumMaster(scrumMasterTeamLead);

                var marketingTeamLead = MarketingTeamLead.Create(teamLeadId, otherLevelName, otherLevelLastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                    graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, chiefMarketingOfficer.Id);

                chiefMarketingOfficer.AddLeadMarketing(marketingTeamLead);

                for (int seniorId = 1; seniorId <= count; seniorId++)
                {
                    var seniorDeveloper = SeniorDeveloper.Create(seniorId, otherLevelName, otherLevelLastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                        graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, technicalTeamLead.Id);
                    technicalTeamLead.AddSeniorTechnical(seniorDeveloper);

                    var seniorQaTesting = SeniorQaTesting.Create(seniorId, otherLevelName, otherLevelLastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                        graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, qATestingTeamLead.Id);
                    qATestingTeamLead.AddSeniorQaTesting(seniorQaTesting);

                    var seniorProductManager = SeniorProductManager.Create(seniorId, otherLevelName, otherLevelLastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                        graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, productTeamLead.Id);
                    productTeamLead.AddSeniorProductManger(seniorProductManager);

                    var seniorScrumMaster = SeniorScrumMaster.Create(seniorId, otherLevelName, otherLevelLastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                        graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, scrumMasterTeamLead.Id);
                    scrumMasterTeamLead.AddSeniorScrumMaster(seniorScrumMaster);

                    var seniorMarketing = SeniorMarketing.Create(seniorId, otherLevelName, otherLevelLastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                        graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, marketingTeamLead.Id);
                    marketingTeamLead.AddSeniorMarketing(seniorMarketing);

                    for (int midlevelId = 1; midlevelId <= count; midlevelId++)
                    {
                        var midlevelDeveloper = MidlevelDeveloper.Create(midlevelId, otherLevelName, otherLevelLastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                            graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, seniorDeveloper.Id);
                        seniorDeveloper.AddMidlevelTechnical(midlevelDeveloper);

                        var midlevelProductManager = MidlevelProductManager.Create(midlevelId, otherLevelName, otherLevelLastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                            graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, seniorProductManager.Id);
                        seniorProductManager.AddMidlevelProductManager(midlevelProductManager);

                        var midlevelMarketing = MidlevelMarketing.Create(midlevelId, otherLevelName, otherLevelLastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                            graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, seniorMarketing.Id);
                        seniorMarketing.AddMidlevelMarketing(midlevelMarketing);

                        for (int juniorId = 1; juniorId <= count; juniorId++)
                        {
                            var juniorDeveloper = JuniorDeveloper.Create(juniorId, otherLevelName, otherLevelLastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                                graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, midlevelDeveloper.Id);
                            midlevelDeveloper.AddJuniorTechnical(juniorDeveloper);

                            var juniorProductManager = JuniorProductManager.Create(juniorId, otherLevelName, otherLevelLastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                                graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, midlevelProductManager.Id);
                            midlevelProductManager.AddJuniorProductManager(juniorProductManager);

                            var juniorMarketing = JuniorMarketing.Create(juniorId, otherLevelName, otherLevelLastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                                graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, midlevelMarketing.Id);
                            midlevelMarketing.AddJuniorMarketing(juniorMarketing);

                            for (int fresherId = 1; fresherId <= count; fresherId++)
                            {
                                var fresherDeveloper = FresherDeveloper.Create(fresherId, otherLevelName, otherLevelLastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                                    graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, juniorDeveloper.Id);
                                juniorDeveloper.AddFresherTechnical(fresherDeveloper);

                                var fresherProductManager = FresherProductManager.Create(fresherId, otherLevelName, otherLevelLastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                                    graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, juniorProductManager.Id);
                                juniorProductManager.AddFresherProductManager(fresherProductManager);

                                var fresherMarketing = FresherMarketing.Create(fresherId, otherLevelName, otherLevelLastName, nationalCode, personalCode, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                                    graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, juniorMarketing.Id);
                                juniorMarketing.AddFresherMarketing(fresherMarketing);
                            }
                        }
                    }
                }
            }
        }

        return chiefExecutiveOfficers;
    }

    public static List<ExpandoObject> CreateValidPatchExecutiveOfficers(List<ChiefExecutiveOfficer> chiefExecutiveOfficers)
    {
        var random = new Random();
        var executiveOfficers = new List<ExpandoObject>();

        foreach (var chiefExecutiveOfficer in chiefExecutiveOfficers)
        {
            ChiefProductOfficer? chiefProductOfficer = chiefExecutiveOfficer.ChiefProductOfficer;
            ChiefTechnicalOfficer? chiefTechnicalOfficer = chiefExecutiveOfficer.ChiefTechnicalOfficer;
            ChiefMarketingOfficer? chiefMarketingOfficer = chiefExecutiveOfficer.ChiefMarketingOfficer;

            int index = random.Next(0, 6);

            var propertyValues = new Dictionary<string, object?>();

            var age = Ages[index];
            var height = Heights[index];
            var weight = Weights[index];
            var address = Addresses[index];
            var eyeColor = EyeColors[index];
            var birthDate = BirthDates[index];
            var isFired = FireStatuses[index];
            var graduation = Graduations[index];
            var experience = Experiences[index];
            var officerName = OfficerNames[index];
            var modifiedDate = ModifiedDates[index];
            var organization = Organizations[index];
            var nationalCode = NationalCodes[index];
            var personalCode = PersonalCodes[index];
            var daysOfVacation = DaysOfVacation[index];
            var officerLastName = OfficerLastNames[index];
            var contractDateEnd = ContractDatesEnd[index];
            var uniqueIdentifier = UniqueIdentifiers[index];
            var contractDateStart = ContractDatesStart[index];

            propertyValues.Add(nameof(ChiefUnitIdentity.Age), 25.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Height), 188.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Weight), 85.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.IsFired), true.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Address), "Patched !".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Name), "Patched !".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.EyeColor), eyeColor?.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.BirthDate), birthDate.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Experience), Experience.Elementary.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Graduation), Graduation.Diploma.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.LastName), "Patched !".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.NationalCode), "99999999".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.PersonalCode), "Patched !".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.ModifiedDate), modifiedDate?.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.DaysOfVacation), daysOfVacation?.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.ContractDateEnd), contractDateEnd.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.UniqueIdentifier), uniqueIdentifier.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.AssignedOrganization), "SnappTrip".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.ContractDateStart), contractDateStart.JsonElement());

            var productOfficer = new ExpandoObject() as IDictionary<string, object>;
            var executiveOfficer = new ExpandoObject() as IDictionary<string, object>;
            var technicalOfficer = new ExpandoObject() as IDictionary<string, object>;
            var marketingOfficer = new ExpandoObject() as IDictionary<string, object>;


            #region ChiefExecutiveOfficer

            executiveOfficer.Add("Id", chiefExecutiveOfficer.Id.JsonElement());

            foreach (var property in typeof(ChiefExecutiveOfficer).GetProperties())
            {
                if (propertyValues.TryGetValue(property.Name, out var value))
                {
                    executiveOfficer.Add(property.Name, value);
                    continue;
                }

                if (property.Name is nameof(ChiefExecutiveOfficer.ChiefTechnicalOfficer))
                {
                    executiveOfficer.Add(property.Name, technicalOfficer);
                    continue;
                }

                if (property.Name is nameof(ChiefExecutiveOfficer.ChiefProductOfficer))
                {
                    executiveOfficer.Add(property.Name, productOfficer);
                    continue;
                }

                if (property.Name is nameof(ChiefExecutiveOfficer.ChiefMarketingOfficer))
                {
                    executiveOfficer.Add(property.Name, marketingOfficer);
                }
            }

            #endregion


            #region ChiefTechnicalOfficer

            var technicalTeamLeads = new List<ExpandoObject>();
            var qATestingTeamLeads = new List<ExpandoObject>();

            technicalOfficer.Add("Id", chiefTechnicalOfficer.Id.JsonElement());

            foreach (var property in typeof(ChiefTechnicalOfficer).GetProperties())
            {
                if (propertyValues.TryGetValue(property.Name, out var value))
                {
                    technicalOfficer.Add(property.Name, value);
                    continue;
                }

                if (property.Name is nameof(ChiefTechnicalOfficer.TechnicalTeamLeads))
                {
                    technicalOfficer.Add(property.Name, technicalTeamLeads);
                    continue;
                }

                if (property.Name is nameof(ChiefTechnicalOfficer.QaTestingTeamLeads))
                {
                    technicalOfficer.Add(property.Name, qATestingTeamLeads);
                }
            }

            foreach (var technicalTeamLead in chiefTechnicalOfficer!.TechnicalTeamLeads)
            {
                var technicalSeniors = new List<ExpandoObject>();

                var technicalLead = new ExpandoObject() as IDictionary<string, object>;

                technicalLead.Add("Id", technicalTeamLead.Id.JsonElement());

                foreach (var property in typeof(TechnicalTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        technicalLead.Add(property.Name, value);
                        continue;
                    }

                    if (property.Name is nameof(TechnicalTeamLead.Seniors))
                    {
                        technicalLead.Add(property.Name, technicalSeniors);
                    }
                }

                technicalTeamLeads.Add((dynamic)technicalLead);

                foreach (var senior in technicalTeamLead.Seniors)
                {
                    var technicalMidlevels = new List<ExpandoObject>();

                    var technicalSenior = new ExpandoObject() as IDictionary<string, object>;

                    technicalSenior.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorDeveloper).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            technicalSenior.Add(property.Name, value);
                            continue;
                        }

                        if (property.Name is nameof(SeniorDeveloper.Midlevels))
                        {
                            technicalSenior.Add(property.Name, technicalMidlevels);
                        }
                    }

                    technicalSeniors.Add((dynamic)technicalSenior);

                    foreach (var midlevel in senior.Midlevels)
                    {
                        var technicalJuniors = new List<ExpandoObject>();

                        var technicalMidlevel = new ExpandoObject() as IDictionary<string, object>;

                        technicalMidlevel.Add("Id", midlevel.Id.JsonElement());

                        foreach (var property in typeof(MidlevelDeveloper).GetProperties())
                        {
                            if (propertyValues.TryGetValue(property.Name, out var value))
                            {
                                technicalMidlevel.Add(property.Name, value);
                                continue;
                            }

                            if (property.Name is nameof(MidlevelDeveloper.Juniors))
                            {
                                technicalMidlevel.Add(property.Name, technicalJuniors);
                            }
                        }

                        technicalMidlevels.Add((dynamic)technicalMidlevel);

                        foreach (var junior in midlevel.Juniors)
                        {
                            var technicalFreshers = new List<ExpandoObject>();

                            var technicalJunior = new ExpandoObject() as IDictionary<string, object>;

                            technicalJunior.Add("Id", junior.Id.JsonElement());

                            foreach (var property in typeof(JuniorDeveloper).GetProperties())
                            {
                                if (propertyValues.TryGetValue(property.Name, out var value))
                                {
                                    technicalJunior.Add(property.Name, value);
                                    continue;
                                }

                                if (property.Name is nameof(JuniorDeveloper.Freshers))
                                {
                                    technicalJunior.Add(property.Name, technicalFreshers);
                                }
                            }

                            technicalJuniors.Add((dynamic)technicalJunior);

                            foreach (var fresher in junior.Freshers)
                            {
                                var technicalFresher = new ExpandoObject() as IDictionary<string, object>;

                                technicalFresher.Add("Id", fresher.Id.JsonElement());

                                foreach (var property in typeof(FresherDeveloper).GetProperties())
                                {
                                    if (propertyValues.TryGetValue(property.Name, out var value))
                                    {
                                        technicalFresher.Add(property.Name, value);
                                    }
                                }

                                technicalFreshers.Add((dynamic)technicalFresher);
                            }
                        }
                    }
                }
            }

            foreach (var qaTestingTeamLead in chiefTechnicalOfficer.QaTestingTeamLeads)
            {
                var qATestingSeniors = new List<ExpandoObject>();

                var qATestingLead = new ExpandoObject() as IDictionary<string, object>;

                qATestingLead.Add("Id", qaTestingTeamLead.Id.JsonElement());

                foreach (var property in typeof(QaTestingTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        qATestingLead.Add(property.Name, value);
                        continue;
                    }

                    if (property.Name is nameof(QaTestingTeamLead.Seniors))
                    {
                        qATestingLead.Add(property.Name, qATestingSeniors);
                    }
                }

                qATestingTeamLeads.Add((dynamic)qATestingLead);

                foreach (var senior in qaTestingTeamLead.Seniors)
                {
                    var qaTestingSenior = new ExpandoObject() as IDictionary<string, object>;

                    qaTestingSenior.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorDeveloper).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            qaTestingSenior.Add(property.Name, value);
                            continue;
                        }
                    }

                    qATestingSeniors.Add((dynamic)qaTestingSenior);
                }
            }

            #endregion


            #region ChiefProductOfficer

            productOfficer.Add("Id", chiefProductOfficer.Id.JsonElement());

            foreach (var property in typeof(ChiefProductOfficer).GetProperties())
            {
                if (propertyValues.TryGetValue(property.Name, out var value))
                {
                    productOfficer.Add(property.Name, value);
                }
            }

            var productTeamLeads = new List<ExpandoObject>();
            ((dynamic)productOfficer).ProductTeamLeads = productTeamLeads;

            var scrumMasterTeamLeads = new List<ExpandoObject>();
            ((dynamic)productOfficer).ScrumMasterTeamLeads = scrumMasterTeamLeads;

            foreach (var productTeamLead in chiefProductOfficer!.ProductTeamLeads)
            {
                var productMangerLead = new ExpandoObject() as IDictionary<string, object>;

                productMangerLead.Add("Id", productTeamLead.Id.JsonElement());

                foreach (var property in typeof(ProductTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        productMangerLead.Add(property.Name, value);
                    }
                }

                productTeamLeads.Add((dynamic)productMangerLead);

                var seniorProducts = new List<ExpandoObject>();
                ((dynamic)productMangerLead).Seniors = seniorProducts;

                foreach (var senior in productTeamLead.Seniors)
                {
                    var seniorProduct = new ExpandoObject() as IDictionary<string, object>;

                    seniorProduct.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorProductManager).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            seniorProduct.Add(property.Name, value);
                        }
                    }

                    seniorProducts.Add((dynamic)seniorProduct);

                    var midlevelProducts = new List<ExpandoObject>();
                    ((dynamic)seniorProduct).Midlevels = midlevelProducts;

                    foreach (var midlevel in senior.Midlevels)
                    {
                        var midlevelProduct = new ExpandoObject() as IDictionary<string, object>;

                        midlevelProduct.Add("Id", midlevel.Id.JsonElement());

                        foreach (var property in typeof(MidlevelProductManager).GetProperties())
                        {
                            if (propertyValues.TryGetValue(property.Name, out var value))
                            {
                                midlevelProduct.Add(property.Name, value);
                            }
                        }

                        midlevelProducts.Add((dynamic)midlevelProduct);

                        var juniorProducts = new List<ExpandoObject>();
                        ((dynamic)midlevelProduct).Juniors = juniorProducts;

                        foreach (var junior in midlevel.Juniors)
                        {
                            var juniorProduct = new ExpandoObject() as IDictionary<string, object>;

                            juniorProduct.Add("Id", junior.Id.JsonElement());

                            foreach (var property in typeof(JuniorProductManager).GetProperties())
                            {
                                if (propertyValues.TryGetValue(property.Name, out var value))
                                {
                                    juniorProduct.Add(property.Name, value);
                                }
                            }

                            juniorProducts.Add((dynamic)juniorProduct);

                            var fresherProducts = new List<ExpandoObject>();
                            ((dynamic)juniorProduct).Freshers = fresherProducts;

                            foreach (var fresher in junior.Freshers)
                            {
                                var fresherProduct = new ExpandoObject() as IDictionary<string, object>;

                                fresherProduct.Add("Id", fresher.Id.JsonElement());

                                foreach (var property in typeof(FresherProductManager).GetProperties())
                                {
                                    if (propertyValues.TryGetValue(property.Name, out var value))
                                    {
                                        fresherProduct.Add(property.Name, value);
                                    }
                                }

                                fresherProducts.Add((dynamic)fresherProduct);
                            }
                        }
                    }
                }
            }

            foreach (var scrumTeamLead in chiefProductOfficer.ScrumMasterTeamLeads)
            {
                var scrumMasterTeamLead = new ExpandoObject() as IDictionary<string, object>;

                scrumMasterTeamLead.Add("Id", scrumTeamLead.Id.JsonElement());

                foreach (var property in typeof(ScrumMasterTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        scrumMasterTeamLead.Add(property.Name, value);
                    }
                }

                scrumMasterTeamLeads.Add((dynamic)scrumMasterTeamLead);

                var seniorScrumMasters = new List<ExpandoObject>();
                ((dynamic)scrumMasterTeamLead).Seniors = seniorScrumMasters;

                foreach (var senior in scrumTeamLead.Seniors)
                {
                    var seniorScrum = new ExpandoObject() as IDictionary<string, object>;

                    seniorScrum.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorScrumMaster).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            seniorScrum.Add(property.Name, value);
                        }
                    }

                    seniorScrumMasters.Add((dynamic)seniorScrum);
                }
            }

            #endregion


            #region ChiefMarketingOfficer

            var marketingTeamLeads = new List<ExpandoObject>();

            marketingOfficer.Add("Id", chiefMarketingOfficer.Id.JsonElement());

            foreach (var property in typeof(ChiefMarketingOfficer).GetProperties())
            {
                if (propertyValues.TryGetValue(property.Name, out var value))
                {
                    marketingOfficer.Add(property.Name, value);
                    continue;
                }

                if (property.Name is nameof(ChiefMarketingOfficer.MarketingTeamLeads))
                {
                    marketingOfficer.Add(property.Name, marketingTeamLeads);
                }
            }

            foreach (var marketingTeamLead in chiefMarketingOfficer!.MarketingTeamLeads)
            {
                var marketingSeniors = new List<ExpandoObject>();

                var marketingLead = new ExpandoObject() as IDictionary<string, object>;

                marketingLead.Add("Id", marketingTeamLead.Id.JsonElement());

                foreach (var property in typeof(MarketingTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        marketingLead.Add(property.Name, value);
                        continue;
                    }

                    if (property.Name is nameof(MarketingTeamLead.Seniors))
                    {
                        marketingLead.Add(property.Name, marketingSeniors);
                    }
                }

                marketingTeamLeads.Add((dynamic)marketingLead);

                foreach (var senior in marketingTeamLead.Seniors)
                {
                    var marketingMidlevels = new List<ExpandoObject>();

                    var marketingSenior = new ExpandoObject() as IDictionary<string, object>;

                    marketingSenior.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorMarketing).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            marketingSenior.Add(property.Name, value);
                            continue;
                        }

                        if (property.Name is nameof(SeniorMarketing.Midlevels))
                        {
                            marketingSenior.Add(property.Name, marketingMidlevels);
                        }
                    }

                    marketingSeniors.Add((dynamic)marketingSenior);

                    foreach (var midlevel in senior.Midlevels)
                    {
                        var marketingJuniors = new List<ExpandoObject>();

                        var marketingMidlevel = new ExpandoObject() as IDictionary<string, object>;

                        marketingMidlevel.Add("Id", midlevel.Id.JsonElement());

                        foreach (var property in typeof(MidlevelMarketing).GetProperties())
                        {
                            if (propertyValues.TryGetValue(property.Name, out var value))
                            {
                                marketingMidlevel.Add(property.Name, value);
                                continue;
                            }

                            if (property.Name is nameof(MidlevelMarketing.Juniors))
                            {
                                marketingMidlevel.Add(property.Name, marketingJuniors);
                            }
                        }

                        marketingMidlevels.Add((dynamic)marketingMidlevel);

                        foreach (var junior in midlevel.Juniors)
                        {
                            var marketingFreshers = new List<ExpandoObject>();

                            var marketingJunior = new ExpandoObject() as IDictionary<string, object>;

                            marketingJunior.Add("Id", junior.Id.JsonElement());

                            foreach (var property in typeof(JuniorMarketing).GetProperties())
                            {
                                if (propertyValues.TryGetValue(property.Name, out var value))
                                {
                                    marketingJunior.Add(property.Name, value);
                                    continue;
                                }

                                if (property.Name is nameof(JuniorMarketing.Freshers))
                                {
                                    marketingJunior.Add(property.Name, marketingFreshers);
                                }
                            }

                            marketingJuniors.Add((dynamic)marketingJunior);

                            foreach (var fresher in junior.Freshers)
                            {
                                var marketingFresher = new ExpandoObject() as IDictionary<string, object>;

                                marketingFresher.Add("Id", fresher.Id.JsonElement());

                                foreach (var property in typeof(FresherMarketing).GetProperties())
                                {
                                    if (propertyValues.TryGetValue(property.Name, out var value))
                                    {
                                        marketingFresher.Add(property.Name, value);
                                    }
                                }

                                marketingFreshers.Add((dynamic)marketingFresher);
                            }
                        }
                    }
                }
            }


            #endregion


            executiveOfficers.Add((dynamic)executiveOfficer);
        }

        return executiveOfficers;
    }

    public static List<ExpandoObject> CreateRootInvalidPatchExecutiveOfficers(List<ChiefExecutiveOfficer> chiefExecutiveOfficers, int validItems)
    {
        var random = new Random();

        var executiveOfficers = new List<ExpandoObject>();

        foreach (var chiefExecutiveOfficer in chiefExecutiveOfficers)
        {
            ChiefProductOfficer? chiefProductOfficer = chiefExecutiveOfficer.ChiefProductOfficer;
            ChiefTechnicalOfficer? chiefTechnicalOfficer = chiefExecutiveOfficer.ChiefTechnicalOfficer;
            ChiefMarketingOfficer? chiefMarketingOfficer = chiefExecutiveOfficer.ChiefMarketingOfficer;

            int index = random.Next(0, 6);

            var propertyValues = new Dictionary<string, object?>();

            var age = Ages[index];
            var height = Heights[index];
            var weight = Weights[index];
            var address = Addresses[index];
            var eyeColor = EyeColors[index];
            var birthDate = BirthDates[index];
            var isFired = FireStatuses[index];
            var graduation = Graduations[index];
            var experience = Experiences[index];
            var officerName = OfficerNames[index];
            var modifiedDate = ModifiedDates[index];
            var organization = Organizations[index];
            var nationalCode = NationalCodes[index];
            var personalCode = PersonalCodes[index];
            var daysOfVacation = DaysOfVacation[index];
            var officerLastName = OfficerLastNames[index];
            var contractDateEnd = ContractDatesEnd[index];
            var uniqueIdentifier = UniqueIdentifiers[index];
            var contractDateStart = ContractDatesStart[index];

            propertyValues.Add(nameof(ChiefUnitIdentity.Age), 25.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Height), 188.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Weight), 85.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.IsFired), true.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Address), "Patched !".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Name), "Patched !".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.EyeColor), eyeColor?.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.BirthDate), birthDate.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Experience), Experience.Elementary.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Graduation), Graduation.Diploma.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.LastName), "Patched !".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.NationalCode), "99999999".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.PersonalCode), "Patched !".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.ModifiedDate), modifiedDate?.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.DaysOfVacation), daysOfVacation?.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.ContractDateEnd), contractDateEnd.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.UniqueIdentifier), uniqueIdentifier.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.AssignedOrganization), "SnappTrip".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.ContractDateStart), contractDateStart.JsonElement());

            var executiveOfficer = new ExpandoObject() as IDictionary<string, object>;
            var technicalOfficer = new ExpandoObject() as IDictionary<string, object>;
            var marketingOfficer = new ExpandoObject() as IDictionary<string, object>;
            var productOfficer = new ExpandoObject() as IDictionary<string, object>;


            #region ChiefExecutiveOfficer

            executiveOfficer.Add("Id", chiefExecutiveOfficer.Id.JsonElement());

            foreach (var property in typeof(ChiefExecutiveOfficer).GetProperties())
            {
                if (propertyValues.TryGetValue(property.Name, out var value))
                {
                    executiveOfficer.Add(property.Name, value);
                    continue;
                }

                if (property.Name is nameof(ChiefExecutiveOfficer.ChiefTechnicalOfficer))
                {
                    executiveOfficer.Add(property.Name, technicalOfficer);
                    continue;
                }

                if (property.Name is nameof(ChiefExecutiveOfficer.ChiefProductOfficer))
                {
                    executiveOfficer.Add(property.Name, productOfficer);
                    continue;
                }

                if (property.Name is nameof(ChiefExecutiveOfficer.ChiefMarketingOfficer))
                {
                    executiveOfficer.Add(property.Name, marketingOfficer);
                }
            }

            #endregion


            #region ChiefTechnicalOfficer

            var technicalTeamLeads = new List<ExpandoObject>();
            var qATestingTeamLeads = new List<ExpandoObject>();

            technicalOfficer.Add("Id", chiefTechnicalOfficer.Id.JsonElement());

            foreach (var property in typeof(ChiefTechnicalOfficer).GetProperties())
            {
                if (propertyValues.TryGetValue(property.Name, out var value))
                {
                    technicalOfficer.Add(property.Name, value);
                    continue;
                }

                if (property.Name is nameof(ChiefTechnicalOfficer.TechnicalTeamLeads))
                {
                    technicalOfficer.Add(property.Name, technicalTeamLeads);
                    continue;
                }

                if (property.Name is nameof(ChiefTechnicalOfficer.QaTestingTeamLeads))
                {
                    technicalOfficer.Add(property.Name, qATestingTeamLeads);
                }
            }

            foreach (var technicalTeamLead in chiefTechnicalOfficer!.TechnicalTeamLeads)
            {
                var technicalSeniors = new List<ExpandoObject>();

                var technicalLead = new ExpandoObject() as IDictionary<string, object>;

                technicalLead.Add("Id", technicalTeamLead.Id.JsonElement());

                foreach (var property in typeof(TechnicalTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        technicalLead.Add(property.Name, value);
                        continue;
                    }

                    if (property.Name is nameof(TechnicalTeamLead.Seniors))
                    {
                        technicalLead.Add(property.Name, technicalSeniors);
                    }
                }

                technicalTeamLeads.Add((dynamic)technicalLead);

                foreach (var senior in technicalTeamLead.Seniors)
                {
                    var technicalMidlevels = new List<ExpandoObject>();

                    var technicalSenior = new ExpandoObject() as IDictionary<string, object>;

                    technicalSenior.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorDeveloper).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            technicalSenior.Add(property.Name, value);
                            continue;
                        }

                        if (property.Name is nameof(SeniorDeveloper.Midlevels))
                        {
                            technicalSenior.Add(property.Name, technicalMidlevels);
                        }
                    }

                    technicalSeniors.Add((dynamic)technicalSenior);

                    foreach (var midlevel in senior.Midlevels)
                    {
                        var technicalJuniors = new List<ExpandoObject>();

                        var technicalMidlevel = new ExpandoObject() as IDictionary<string, object>;

                        technicalMidlevel.Add("Id", midlevel.Id.JsonElement());

                        foreach (var property in typeof(MidlevelDeveloper).GetProperties())
                        {
                            if (propertyValues.TryGetValue(property.Name, out var value))
                            {
                                technicalMidlevel.Add(property.Name, value);
                                continue;
                            }

                            if (property.Name is nameof(MidlevelDeveloper.Juniors))
                            {
                                technicalMidlevel.Add(property.Name, technicalJuniors);
                            }
                        }

                        technicalMidlevels.Add((dynamic)technicalMidlevel);

                        foreach (var junior in midlevel.Juniors)
                        {
                            var technicalFreshers = new List<ExpandoObject>();

                            var technicalJunior = new ExpandoObject() as IDictionary<string, object>;

                            technicalJunior.Add("Id", junior.Id.JsonElement());

                            foreach (var property in typeof(JuniorDeveloper).GetProperties())
                            {
                                if (propertyValues.TryGetValue(property.Name, out var value))
                                {
                                    technicalJunior.Add(property.Name, value);
                                    continue;
                                }

                                if (property.Name is nameof(JuniorDeveloper.Freshers))
                                {
                                    technicalJunior.Add(property.Name, technicalFreshers);
                                }
                            }

                            technicalJuniors.Add((dynamic)technicalJunior);

                            foreach (var fresher in junior.Freshers)
                            {
                                var technicalFresher = new ExpandoObject() as IDictionary<string, object>;

                                technicalFresher.Add("Id", fresher.Id.JsonElement());

                                foreach (var property in typeof(FresherDeveloper).GetProperties())
                                {
                                    if (propertyValues.TryGetValue(property.Name, out var value))
                                    {
                                        technicalFresher.Add(property.Name, value);
                                    }
                                }

                                technicalFreshers.Add((dynamic)technicalFresher);
                            }
                        }
                    }
                }
            }

            foreach (var qaTestingTeamLead in chiefTechnicalOfficer.QaTestingTeamLeads)
            {
                var qATestingSeniors = new List<ExpandoObject>();

                var qATestingLead = new ExpandoObject() as IDictionary<string, object>;

                qATestingLead.Add("Id", qaTestingTeamLead.Id.JsonElement());

                foreach (var property in typeof(QaTestingTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        qATestingLead.Add(property.Name, value);
                        continue;
                    }

                    if (property.Name is nameof(QaTestingTeamLead.Seniors))
                    {
                        qATestingLead.Add(property.Name, qATestingSeniors);
                    }
                }

                qATestingTeamLeads.Add((dynamic)qATestingLead);

                foreach (var senior in qaTestingTeamLead.Seniors)
                {
                    var qaTestingSenior = new ExpandoObject() as IDictionary<string, object>;

                    qaTestingSenior.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorDeveloper).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            qaTestingSenior.Add(property.Name, value);
                            continue;
                        }
                    }

                    qATestingSeniors.Add((dynamic)qaTestingSenior);
                }
            }

            #endregion


            #region ChiefProductOfficer

            productOfficer.Add("Id", chiefProductOfficer.Id.JsonElement());

            foreach (var property in typeof(ChiefProductOfficer).GetProperties())
            {
                if (propertyValues.TryGetValue(property.Name, out var value))
                {
                    productOfficer.Add(property.Name, value);
                }
            }

            var productTeamLeads = new List<ExpandoObject>();
            ((dynamic)productOfficer).ProductTeamLeads = productTeamLeads;

            var scrumMasterTeamLeads = new List<ExpandoObject>();
            ((dynamic)productOfficer).ScrumMasterTeamLeads = scrumMasterTeamLeads;

            foreach (var productTeamLead in chiefProductOfficer!.ProductTeamLeads)
            {
                var productMangerLead = new ExpandoObject() as IDictionary<string, object>;

                productMangerLead.Add("Id", productTeamLead.Id.JsonElement());

                foreach (var property in typeof(ProductTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        productMangerLead.Add(property.Name, value);
                    }
                }

                productTeamLeads.Add((dynamic)productMangerLead);

                var seniorProducts = new List<ExpandoObject>();
                ((dynamic)productMangerLead).Seniors = seniorProducts;

                foreach (var senior in productTeamLead.Seniors)
                {
                    var seniorProduct = new ExpandoObject() as IDictionary<string, object>;

                    seniorProduct.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorProductManager).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            seniorProduct.Add(property.Name, value);
                        }
                    }

                    seniorProducts.Add((dynamic)seniorProduct);

                    var midlevelProducts = new List<ExpandoObject>();
                    ((dynamic)seniorProduct).Midlevels = midlevelProducts;

                    foreach (var midlevel in senior.Midlevels)
                    {
                        var midlevelProduct = new ExpandoObject() as IDictionary<string, object>;

                        midlevelProduct.Add("Id", midlevel.Id.JsonElement());

                        foreach (var property in typeof(MidlevelProductManager).GetProperties())
                        {
                            if (propertyValues.TryGetValue(property.Name, out var value))
                            {
                                midlevelProduct.Add(property.Name, value);
                            }
                        }

                        midlevelProducts.Add((dynamic)midlevelProduct);

                        var juniorProducts = new List<ExpandoObject>();
                        ((dynamic)midlevelProduct).Juniors = juniorProducts;

                        foreach (var junior in midlevel.Juniors)
                        {
                            var juniorProduct = new ExpandoObject() as IDictionary<string, object>;

                            juniorProduct.Add("Id", junior.Id.JsonElement());

                            foreach (var property in typeof(JuniorProductManager).GetProperties())
                            {
                                if (propertyValues.TryGetValue(property.Name, out var value))
                                {
                                    juniorProduct.Add(property.Name, value);
                                }
                            }

                            juniorProducts.Add((dynamic)juniorProduct);

                            var fresherProducts = new List<ExpandoObject>();
                            ((dynamic)juniorProduct).Freshers = fresherProducts;

                            foreach (var fresher in junior.Freshers)
                            {
                                var fresherProduct = new ExpandoObject() as IDictionary<string, object>;

                                fresherProduct.Add("Id", fresher.Id.JsonElement());

                                foreach (var property in typeof(FresherProductManager).GetProperties())
                                {
                                    if (propertyValues.TryGetValue(property.Name, out var value))
                                    {
                                        fresherProduct.Add(property.Name, value);
                                    }
                                }

                                fresherProducts.Add((dynamic)fresherProduct);
                            }
                        }
                    }
                }
            }

            foreach (var scrumTeamLead in chiefProductOfficer.ScrumMasterTeamLeads)
            {
                var scrumMasterTeamLead = new ExpandoObject() as IDictionary<string, object>;

                scrumMasterTeamLead.Add("Id", scrumTeamLead.Id.JsonElement());

                foreach (var property in typeof(ScrumMasterTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        scrumMasterTeamLead.Add(property.Name, value);
                    }
                }

                scrumMasterTeamLeads.Add((dynamic)scrumMasterTeamLead);

                var seniorScrumMasters = new List<ExpandoObject>();
                ((dynamic)scrumMasterTeamLead).Seniors = seniorScrumMasters;

                foreach (var senior in scrumTeamLead.Seniors)
                {
                    var seniorScrum = new ExpandoObject() as IDictionary<string, object>;

                    seniorScrum.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorScrumMaster).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            seniorScrum.Add(property.Name, value);
                        }
                    }

                    seniorScrumMasters.Add((dynamic)seniorScrum);
                }
            }

            #endregion


            #region ChiefMarketingOfficer

            var marketingTeamLeads = new List<ExpandoObject>();

            marketingOfficer.Add("Id", chiefMarketingOfficer.Id.JsonElement());

            foreach (var property in typeof(ChiefMarketingOfficer).GetProperties())
            {
                if (propertyValues.TryGetValue(property.Name, out var value))
                {
                    marketingOfficer.Add(property.Name, value);
                    continue;
                }

                if (property.Name is nameof(ChiefMarketingOfficer.MarketingTeamLeads))
                {
                    marketingOfficer.Add(property.Name, marketingTeamLeads);
                }
            }

            foreach (var marketingTeamLead in chiefMarketingOfficer!.MarketingTeamLeads)
            {
                var marketingSeniors = new List<ExpandoObject>();

                var marketingLead = new ExpandoObject() as IDictionary<string, object>;

                marketingLead.Add("Id", marketingTeamLead.Id.JsonElement());

                foreach (var property in typeof(MarketingTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        marketingLead.Add(property.Name, value);
                        continue;
                    }

                    if (property.Name is nameof(MarketingTeamLead.Seniors))
                    {
                        marketingLead.Add(property.Name, marketingSeniors);
                    }
                }

                marketingTeamLeads.Add((dynamic)marketingLead);

                foreach (var senior in marketingTeamLead.Seniors)
                {
                    var marketingMidlevels = new List<ExpandoObject>();

                    var marketingSenior = new ExpandoObject() as IDictionary<string, object>;

                    marketingSenior.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorMarketing).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            marketingSenior.Add(property.Name, value);
                            continue;
                        }

                        if (property.Name is nameof(SeniorMarketing.Midlevels))
                        {
                            marketingSenior.Add(property.Name, marketingMidlevels);
                        }
                    }

                    marketingSeniors.Add((dynamic)marketingSenior);

                    foreach (var midlevel in senior.Midlevels)
                    {
                        var marketingJuniors = new List<ExpandoObject>();

                        var marketingMidlevel = new ExpandoObject() as IDictionary<string, object>;

                        marketingMidlevel.Add("Id", midlevel.Id.JsonElement());

                        foreach (var property in typeof(MidlevelMarketing).GetProperties())
                        {
                            if (propertyValues.TryGetValue(property.Name, out var value))
                            {
                                marketingMidlevel.Add(property.Name, value);
                                continue;
                            }

                            if (property.Name is nameof(MidlevelMarketing.Juniors))
                            {
                                marketingMidlevel.Add(property.Name, marketingJuniors);
                            }
                        }

                        marketingMidlevels.Add((dynamic)marketingMidlevel);

                        foreach (var junior in midlevel.Juniors)
                        {
                            var marketingFreshers = new List<ExpandoObject>();

                            var marketingJunior = new ExpandoObject() as IDictionary<string, object>;

                            marketingJunior.Add("Id", junior.Id.JsonElement());

                            foreach (var property in typeof(JuniorMarketing).GetProperties())
                            {
                                if (propertyValues.TryGetValue(property.Name, out var value))
                                {
                                    marketingJunior.Add(property.Name, value);
                                    continue;
                                }

                                if (property.Name is nameof(JuniorMarketing.Freshers))
                                {
                                    marketingJunior.Add(property.Name, marketingFreshers);
                                }
                            }

                            marketingJuniors.Add((dynamic)marketingJunior);

                            foreach (var fresher in junior.Freshers)
                            {
                                var marketingFresher = new ExpandoObject() as IDictionary<string, object>;

                                marketingFresher.Add("Id", fresher.Id.JsonElement());

                                foreach (var property in typeof(FresherMarketing).GetProperties())
                                {
                                    if (propertyValues.TryGetValue(property.Name, out var value))
                                    {
                                        marketingFresher.Add(property.Name, value);
                                    }
                                }

                                marketingFreshers.Add((dynamic)marketingFresher);
                            }
                        }
                    }
                }
            }

            #endregion


            executiveOfficers.Add((dynamic)executiveOfficer);
        }

        PrepareForInvalidScenario(executiveOfficers, validItems);

        return executiveOfficers;
    }

    public static List<ExpandoObject> CreateCLevelInvalidPatchExecutiveOfficers(List<ChiefExecutiveOfficer> chiefExecutiveOfficers, int validItems)
    {
        var random = new Random();

        var executiveOfficers = new List<ExpandoObject>();

        foreach (var chiefExecutiveOfficer in chiefExecutiveOfficers)
        {
            ChiefProductOfficer? chiefProductOfficer = chiefExecutiveOfficer.ChiefProductOfficer;
            ChiefTechnicalOfficer? chiefTechnicalOfficer = chiefExecutiveOfficer.ChiefTechnicalOfficer;
            ChiefMarketingOfficer? chiefMarketingOfficer = chiefExecutiveOfficer.ChiefMarketingOfficer;

            int index = random.Next(0, 6);

            var propertyValues = new Dictionary<string, object?>();

            var age = Ages[index];
            var height = Heights[index];
            var weight = Weights[index];
            var address = Addresses[index];
            var eyeColor = EyeColors[index];
            var birthDate = BirthDates[index];
            var isFired = FireStatuses[index];
            var graduation = Graduations[index];
            var experience = Experiences[index];
            var officerName = OfficerNames[index];
            var modifiedDate = ModifiedDates[index];
            var organization = Organizations[index];
            var nationalCode = NationalCodes[index];
            var personalCode = PersonalCodes[index];
            var daysOfVacation = DaysOfVacation[index];
            var officerLastName = OfficerLastNames[index];
            var contractDateEnd = ContractDatesEnd[index];
            var uniqueIdentifier = UniqueIdentifiers[index];
            var contractDateStart = ContractDatesStart[index];

            propertyValues.Add(nameof(ChiefUnitIdentity.Age), 25.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Height), 188.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Weight), 85.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.IsFired), true.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Address), "Patched !".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Name), "Patched !".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.EyeColor), eyeColor?.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.BirthDate), birthDate.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Experience), Experience.Elementary.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Graduation), Graduation.Diploma.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.LastName), "Patched !".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.NationalCode), "99999999".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.PersonalCode), "Patched !".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.ModifiedDate), modifiedDate?.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.DaysOfVacation), daysOfVacation?.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.ContractDateEnd), contractDateEnd.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.UniqueIdentifier), uniqueIdentifier.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.AssignedOrganization), "SnappTrip".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.ContractDateStart), contractDateStart.JsonElement());

            var executiveOfficer = new ExpandoObject() as IDictionary<string, object>;
            var technicalOfficer = new ExpandoObject() as IDictionary<string, object>;
            var marketingOfficer = new ExpandoObject() as IDictionary<string, object>;
            var productOfficer = new ExpandoObject() as IDictionary<string, object>;


            #region ChiefExecutiveOfficer

            executiveOfficer.Add("Id", chiefExecutiveOfficer.Id.JsonElement());

            foreach (var property in typeof(ChiefExecutiveOfficer).GetProperties())
            {
                if (propertyValues.TryGetValue(property.Name, out var value))
                {
                    executiveOfficer.Add(property.Name, value);
                    continue;
                }

                if (property.Name is nameof(ChiefExecutiveOfficer.ChiefTechnicalOfficer))
                {
                    executiveOfficer.Add(property.Name, technicalOfficer);
                    continue;
                }

                if (property.Name is nameof(ChiefExecutiveOfficer.ChiefProductOfficer))
                {
                    executiveOfficer.Add(property.Name, productOfficer);
                    continue;
                }

                if (property.Name is nameof(ChiefExecutiveOfficer.ChiefMarketingOfficer))
                {
                    executiveOfficer.Add(property.Name, marketingOfficer);
                }
            }

            #endregion


            #region ChiefTechnicalOfficer

            var technicalTeamLeads = new List<ExpandoObject>();
            var qATestingTeamLeads = new List<ExpandoObject>();

            technicalOfficer.Add("Id", chiefTechnicalOfficer.Id.JsonElement());

            foreach (var property in typeof(ChiefTechnicalOfficer).GetProperties())
            {
                if (propertyValues.TryGetValue(property.Name, out var value))
                {
                    technicalOfficer.Add(property.Name, value);
                    continue;
                }

                if (property.Name is nameof(ChiefTechnicalOfficer.TechnicalTeamLeads))
                {
                    technicalOfficer.Add(property.Name, technicalTeamLeads);
                    continue;
                }

                if (property.Name is nameof(ChiefTechnicalOfficer.QaTestingTeamLeads))
                {
                    technicalOfficer.Add(property.Name, qATestingTeamLeads);
                }
            }

            foreach (var technicalTeamLead in chiefTechnicalOfficer!.TechnicalTeamLeads)
            {
                var technicalSeniors = new List<ExpandoObject>();

                var technicalLead = new ExpandoObject() as IDictionary<string, object>;

                technicalLead.Add("Id", technicalTeamLead.Id.JsonElement());

                foreach (var property in typeof(TechnicalTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        technicalLead.Add(property.Name, value);
                        continue;
                    }

                    if (property.Name is nameof(TechnicalTeamLead.Seniors))
                    {
                        technicalLead.Add(property.Name, technicalSeniors);
                    }
                }

                technicalTeamLeads.Add((dynamic)technicalLead);

                foreach (var senior in technicalTeamLead.Seniors)
                {
                    var technicalMidlevels = new List<ExpandoObject>();

                    var technicalSenior = new ExpandoObject() as IDictionary<string, object>;

                    technicalSenior.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorDeveloper).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            technicalSenior.Add(property.Name, value);
                            continue;
                        }

                        if (property.Name is nameof(SeniorDeveloper.Midlevels))
                        {
                            technicalSenior.Add(property.Name, technicalMidlevels);
                        }
                    }

                    technicalSeniors.Add((dynamic)technicalSenior);

                    foreach (var midlevel in senior.Midlevels)
                    {
                        var technicalJuniors = new List<ExpandoObject>();

                        var technicalMidlevel = new ExpandoObject() as IDictionary<string, object>;

                        technicalMidlevel.Add("Id", midlevel.Id.JsonElement());

                        foreach (var property in typeof(MidlevelDeveloper).GetProperties())
                        {
                            if (propertyValues.TryGetValue(property.Name, out var value))
                            {
                                technicalMidlevel.Add(property.Name, value);
                                continue;
                            }

                            if (property.Name is nameof(MidlevelDeveloper.Juniors))
                            {
                                technicalMidlevel.Add(property.Name, technicalJuniors);
                            }
                        }

                        technicalMidlevels.Add((dynamic)technicalMidlevel);

                        foreach (var junior in midlevel.Juniors)
                        {
                            var technicalFreshers = new List<ExpandoObject>();

                            var technicalJunior = new ExpandoObject() as IDictionary<string, object>;

                            technicalJunior.Add("Id", junior.Id.JsonElement());

                            foreach (var property in typeof(JuniorDeveloper).GetProperties())
                            {
                                if (propertyValues.TryGetValue(property.Name, out var value))
                                {
                                    technicalJunior.Add(property.Name, value);
                                    continue;
                                }

                                if (property.Name is nameof(JuniorDeveloper.Freshers))
                                {
                                    technicalJunior.Add(property.Name, technicalFreshers);
                                }
                            }

                            technicalJuniors.Add((dynamic)technicalJunior);

                            foreach (var fresher in junior.Freshers)
                            {
                                var technicalFresher = new ExpandoObject() as IDictionary<string, object>;

                                technicalFresher.Add("Id", fresher.Id.JsonElement());

                                foreach (var property in typeof(FresherDeveloper).GetProperties())
                                {
                                    if (propertyValues.TryGetValue(property.Name, out var value))
                                    {
                                        technicalFresher.Add(property.Name, value);
                                    }
                                }

                                technicalFreshers.Add((dynamic)technicalFresher);
                            }
                        }
                    }
                }
            }

            foreach (var qaTestingTeamLead in chiefTechnicalOfficer.QaTestingTeamLeads)
            {
                var qATestingSeniors = new List<ExpandoObject>();

                var qATestingLead = new ExpandoObject() as IDictionary<string, object>;

                qATestingLead.Add("Id", qaTestingTeamLead.Id.JsonElement());

                foreach (var property in typeof(QaTestingTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        qATestingLead.Add(property.Name, value);
                        continue;
                    }

                    if (property.Name is nameof(QaTestingTeamLead.Seniors))
                    {
                        qATestingLead.Add(property.Name, qATestingSeniors);
                    }
                }

                qATestingTeamLeads.Add((dynamic)qATestingLead);

                foreach (var senior in qaTestingTeamLead.Seniors)
                {
                    var qaTestingSenior = new ExpandoObject() as IDictionary<string, object>;

                    qaTestingSenior.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorDeveloper).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            qaTestingSenior.Add(property.Name, value);
                            continue;
                        }
                    }

                    qATestingSeniors.Add((dynamic)qaTestingSenior);
                }
            }

            #endregion


            #region ChiefProductOfficer

            productOfficer.Add("Id", chiefProductOfficer.Id.JsonElement());

            foreach (var property in typeof(ChiefProductOfficer).GetProperties())
            {
                if (propertyValues.TryGetValue(property.Name, out var value))
                {
                    productOfficer.Add(property.Name, value);
                }
            }

            var productTeamLeads = new List<ExpandoObject>();
            ((dynamic)productOfficer).ProductTeamLeads = productTeamLeads;

            var scrumMasterTeamLeads = new List<ExpandoObject>();
            ((dynamic)productOfficer).ScrumMasterTeamLeads = scrumMasterTeamLeads;

            foreach (var productTeamLead in chiefProductOfficer!.ProductTeamLeads)
            {
                var productMangerLead = new ExpandoObject() as IDictionary<string, object>;

                productMangerLead.Add("Id", productTeamLead.Id.JsonElement());

                foreach (var property in typeof(ProductTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        productMangerLead.Add(property.Name, value);
                    }
                }

                productTeamLeads.Add((dynamic)productMangerLead);

                var seniorProducts = new List<ExpandoObject>();
                ((dynamic)productMangerLead).Seniors = seniorProducts;

                foreach (var senior in productTeamLead.Seniors)
                {
                    var seniorProduct = new ExpandoObject() as IDictionary<string, object>;

                    seniorProduct.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorProductManager).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            seniorProduct.Add(property.Name, value);
                        }
                    }

                    seniorProducts.Add((dynamic)seniorProduct);

                    var midlevelProducts = new List<ExpandoObject>();
                    ((dynamic)seniorProduct).Midlevels = midlevelProducts;

                    foreach (var midlevel in senior.Midlevels)
                    {
                        var midlevelProduct = new ExpandoObject() as IDictionary<string, object>;

                        midlevelProduct.Add("Id", midlevel.Id.JsonElement());

                        foreach (var property in typeof(MidlevelProductManager).GetProperties())
                        {
                            if (propertyValues.TryGetValue(property.Name, out var value))
                            {
                                midlevelProduct.Add(property.Name, value);
                            }
                        }

                        midlevelProducts.Add((dynamic)midlevelProduct);

                        var juniorProducts = new List<ExpandoObject>();
                        ((dynamic)midlevelProduct).Juniors = juniorProducts;

                        foreach (var junior in midlevel.Juniors)
                        {
                            var juniorProduct = new ExpandoObject() as IDictionary<string, object>;

                            juniorProduct.Add("Id", junior.Id.JsonElement());

                            foreach (var property in typeof(JuniorProductManager).GetProperties())
                            {
                                if (propertyValues.TryGetValue(property.Name, out var value))
                                {
                                    juniorProduct.Add(property.Name, value);
                                }
                            }

                            juniorProducts.Add((dynamic)juniorProduct);

                            var fresherProducts = new List<ExpandoObject>();
                            ((dynamic)juniorProduct).Freshers = fresherProducts;

                            foreach (var fresher in junior.Freshers)
                            {
                                var fresherProduct = new ExpandoObject() as IDictionary<string, object>;

                                fresherProduct.Add("Id", fresher.Id.JsonElement());

                                foreach (var property in typeof(FresherProductManager).GetProperties())
                                {
                                    if (propertyValues.TryGetValue(property.Name, out var value))
                                    {
                                        fresherProduct.Add(property.Name, value);
                                    }
                                }

                                fresherProducts.Add((dynamic)fresherProduct);
                            }
                        }
                    }
                }
            }

            foreach (var scrumTeamLead in chiefProductOfficer.ScrumMasterTeamLeads)
            {
                var scrumMasterTeamLead = new ExpandoObject() as IDictionary<string, object>;

                scrumMasterTeamLead.Add("Id", scrumTeamLead.Id.JsonElement());

                foreach (var property in typeof(ScrumMasterTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        scrumMasterTeamLead.Add(property.Name, value);
                    }
                }

                scrumMasterTeamLeads.Add((dynamic)scrumMasterTeamLead);

                var seniorScrumMasters = new List<ExpandoObject>();
                ((dynamic)scrumMasterTeamLead).Seniors = seniorScrumMasters;

                foreach (var senior in scrumTeamLead.Seniors)
                {
                    var seniorScrum = new ExpandoObject() as IDictionary<string, object>;

                    seniorScrum.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorScrumMaster).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            seniorScrum.Add(property.Name, value);
                        }
                    }

                    seniorScrumMasters.Add((dynamic)seniorScrum);
                }
            }

            #endregion


            #region ChiefMarketingOfficer

            var marketingTeamLeads = new List<ExpandoObject>();

            marketingOfficer.Add("Id", chiefMarketingOfficer.Id.JsonElement());

            foreach (var property in typeof(ChiefMarketingOfficer).GetProperties())
            {
                if (propertyValues.TryGetValue(property.Name, out var value))
                {
                    marketingOfficer.Add(property.Name, value);
                    continue;
                }

                if (property.Name is nameof(ChiefMarketingOfficer.MarketingTeamLeads))
                {
                    marketingOfficer.Add(property.Name, marketingTeamLeads);
                }
            }

            foreach (var marketingTeamLead in chiefMarketingOfficer!.MarketingTeamLeads)
            {
                var marketingSeniors = new List<ExpandoObject>();

                var marketingLead = new ExpandoObject() as IDictionary<string, object>;

                marketingLead.Add("Id", marketingTeamLead.Id.JsonElement());

                foreach (var property in typeof(MarketingTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        marketingLead.Add(property.Name, value);
                        continue;
                    }

                    if (property.Name is nameof(MarketingTeamLead.Seniors))
                    {
                        marketingLead.Add(property.Name, marketingSeniors);
                    }
                }

                marketingTeamLeads.Add((dynamic)marketingLead);

                foreach (var senior in marketingTeamLead.Seniors)
                {
                    var marketingMidlevels = new List<ExpandoObject>();

                    var marketingSenior = new ExpandoObject() as IDictionary<string, object>;

                    marketingSenior.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorMarketing).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            marketingSenior.Add(property.Name, value);
                            continue;
                        }

                        if (property.Name is nameof(SeniorMarketing.Midlevels))
                        {
                            marketingSenior.Add(property.Name, marketingMidlevels);
                        }
                    }

                    marketingSeniors.Add((dynamic)marketingSenior);

                    foreach (var midlevel in senior.Midlevels)
                    {
                        var marketingJuniors = new List<ExpandoObject>();

                        var marketingMidlevel = new ExpandoObject() as IDictionary<string, object>;

                        marketingMidlevel.Add("Id", midlevel.Id.JsonElement());

                        foreach (var property in typeof(MidlevelMarketing).GetProperties())
                        {
                            if (propertyValues.TryGetValue(property.Name, out var value))
                            {
                                marketingMidlevel.Add(property.Name, value);
                                continue;
                            }

                            if (property.Name is nameof(MidlevelMarketing.Juniors))
                            {
                                marketingMidlevel.Add(property.Name, marketingJuniors);
                            }
                        }

                        marketingMidlevels.Add((dynamic)marketingMidlevel);

                        foreach (var junior in midlevel.Juniors)
                        {
                            var marketingFreshers = new List<ExpandoObject>();

                            var marketingJunior = new ExpandoObject() as IDictionary<string, object>;

                            marketingJunior.Add("Id", junior.Id.JsonElement());

                            foreach (var property in typeof(JuniorMarketing).GetProperties())
                            {
                                if (propertyValues.TryGetValue(property.Name, out var value))
                                {
                                    marketingJunior.Add(property.Name, value);
                                    continue;
                                }

                                if (property.Name is nameof(JuniorMarketing.Freshers))
                                {
                                    marketingJunior.Add(property.Name, marketingFreshers);
                                }
                            }

                            marketingJuniors.Add((dynamic)marketingJunior);

                            foreach (var fresher in junior.Freshers)
                            {
                                var marketingFresher = new ExpandoObject() as IDictionary<string, object>;

                                marketingFresher.Add("Id", fresher.Id.JsonElement());

                                foreach (var property in typeof(FresherMarketing).GetProperties())
                                {
                                    if (propertyValues.TryGetValue(property.Name, out var value))
                                    {
                                        marketingFresher.Add(property.Name, value);
                                    }
                                }

                                marketingFreshers.Add((dynamic)marketingFresher);
                            }
                        }
                    }
                }
            }


            #endregion


            executiveOfficers.Add((dynamic)executiveOfficer);
        }

        foreach (IDictionary<string, object> executiveOfficer in executiveOfficers.Skip(validItems))
        {
            var productOfficer = executiveOfficer[nameof(ChiefExecutiveOfficer.ChiefProductOfficer)] as IDictionary<string, object>;
            var technicalOfficer = executiveOfficer[nameof(ChiefExecutiveOfficer.ChiefTechnicalOfficer)] as IDictionary<string, object>;
            var marketingOfficer = executiveOfficer[nameof(ChiefExecutiveOfficer.ChiefMarketingOfficer)] as IDictionary<string, object>;

            PrepareForInvalidScenario(productOfficer);
            PrepareForInvalidScenario(technicalOfficer);
            PrepareForInvalidScenario(marketingOfficer);
        }

        return executiveOfficers;
    }

    public static List<ExpandoObject> CreateLeadInvalidPatchExecutiveOfficers(List<ChiefExecutiveOfficer> chiefExecutiveOfficers, int validItems)
    {
        var random = new Random();

        var executiveOfficers = new List<ExpandoObject>();

        foreach (var chiefExecutiveOfficer in chiefExecutiveOfficers)
        {
            ChiefProductOfficer? chiefProductOfficer = chiefExecutiveOfficer.ChiefProductOfficer;
            ChiefTechnicalOfficer? chiefTechnicalOfficer = chiefExecutiveOfficer.ChiefTechnicalOfficer;
            ChiefMarketingOfficer? chiefMarketingOfficer = chiefExecutiveOfficer.ChiefMarketingOfficer;

            int index = random.Next(0, 6);

            var propertyValues = new Dictionary<string, object?>();

            var age = Ages[index];
            var height = Heights[index];
            var weight = Weights[index];
            var address = Addresses[index];
            var eyeColor = EyeColors[index];
            var birthDate = BirthDates[index];
            var isFired = FireStatuses[index];
            var graduation = Graduations[index];
            var experience = Experiences[index];
            var officerName = OfficerNames[index];
            var modifiedDate = ModifiedDates[index];
            var organization = Organizations[index];
            var nationalCode = NationalCodes[index];
            var personalCode = PersonalCodes[index];
            var daysOfVacation = DaysOfVacation[index];
            var officerLastName = OfficerLastNames[index];
            var contractDateEnd = ContractDatesEnd[index];
            var uniqueIdentifier = UniqueIdentifiers[index];
            var contractDateStart = ContractDatesStart[index];

            propertyValues.Add(nameof(ChiefUnitIdentity.Age), 25.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Height), 188.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Weight), 85.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.IsFired), true.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Address), "Patched !".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Name), "Patched !".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.EyeColor), eyeColor?.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.BirthDate), birthDate.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Experience), Experience.Elementary.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Graduation), Graduation.Diploma.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.LastName), "Patched !".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.NationalCode), "99999999".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.PersonalCode), "Patched !".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.ModifiedDate), modifiedDate?.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.DaysOfVacation), daysOfVacation?.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.ContractDateEnd), contractDateEnd.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.UniqueIdentifier), uniqueIdentifier.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.AssignedOrganization), "SnappTrip".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.ContractDateStart), contractDateStart.JsonElement());

            var executiveOfficer = new ExpandoObject() as IDictionary<string, object>;
            var technicalOfficer = new ExpandoObject() as IDictionary<string, object>;
            var marketingOfficer = new ExpandoObject() as IDictionary<string, object>;
            var productOfficer = new ExpandoObject() as IDictionary<string, object>;


            #region ChiefExecutiveOfficer

            executiveOfficer.Add("Id", chiefExecutiveOfficer.Id.JsonElement());

            foreach (var property in typeof(ChiefExecutiveOfficer).GetProperties())
            {
                if (propertyValues.TryGetValue(property.Name, out var value))
                {
                    executiveOfficer.Add(property.Name, value);
                    continue;
                }

                if (property.Name is nameof(ChiefExecutiveOfficer.ChiefTechnicalOfficer))
                {
                    executiveOfficer.Add(property.Name, technicalOfficer);
                    continue;
                }

                if (property.Name is nameof(ChiefExecutiveOfficer.ChiefProductOfficer))
                {
                    executiveOfficer.Add(property.Name, productOfficer);
                    continue;
                }

                if (property.Name is nameof(ChiefExecutiveOfficer.ChiefMarketingOfficer))
                {
                    executiveOfficer.Add(property.Name, marketingOfficer);
                }
            }

            #endregion


            #region ChiefTechnicalOfficer

            var technicalTeamLeads = new List<ExpandoObject>();
            var qATestingTeamLeads = new List<ExpandoObject>();

            technicalOfficer.Add("Id", chiefTechnicalOfficer.Id.JsonElement());

            foreach (var property in typeof(ChiefTechnicalOfficer).GetProperties())
            {
                if (propertyValues.TryGetValue(property.Name, out var value))
                {
                    technicalOfficer.Add(property.Name, value);
                    continue;
                }

                if (property.Name is nameof(ChiefTechnicalOfficer.TechnicalTeamLeads))
                {
                    technicalOfficer.Add(property.Name, technicalTeamLeads);
                    continue;
                }

                if (property.Name is nameof(ChiefTechnicalOfficer.QaTestingTeamLeads))
                {
                    technicalOfficer.Add(property.Name, qATestingTeamLeads);
                }
            }

            foreach (var technicalTeamLead in chiefTechnicalOfficer!.TechnicalTeamLeads)
            {
                var technicalSeniors = new List<ExpandoObject>();

                var technicalLead = new ExpandoObject() as IDictionary<string, object>;

                technicalLead.Add("Id", technicalTeamLead.Id.JsonElement());

                foreach (var property in typeof(TechnicalTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        technicalLead.Add(property.Name, value);
                        continue;
                    }

                    if (property.Name is nameof(TechnicalTeamLead.Seniors))
                    {
                        technicalLead.Add(property.Name, technicalSeniors);
                    }
                }

                technicalTeamLeads.Add((dynamic)technicalLead);

                foreach (var senior in technicalTeamLead.Seniors)
                {
                    var technicalMidlevels = new List<ExpandoObject>();

                    var technicalSenior = new ExpandoObject() as IDictionary<string, object>;

                    technicalSenior.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorDeveloper).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            technicalSenior.Add(property.Name, value);
                            continue;
                        }

                        if (property.Name is nameof(SeniorDeveloper.Midlevels))
                        {
                            technicalSenior.Add(property.Name, technicalMidlevels);
                        }
                    }

                    technicalSeniors.Add((dynamic)technicalSenior);

                    foreach (var midlevel in senior.Midlevels)
                    {
                        var technicalJuniors = new List<ExpandoObject>();

                        var technicalMidlevel = new ExpandoObject() as IDictionary<string, object>;

                        technicalMidlevel.Add("Id", midlevel.Id.JsonElement());

                        foreach (var property in typeof(MidlevelDeveloper).GetProperties())
                        {
                            if (propertyValues.TryGetValue(property.Name, out var value))
                            {
                                technicalMidlevel.Add(property.Name, value);
                                continue;
                            }

                            if (property.Name is nameof(MidlevelDeveloper.Juniors))
                            {
                                technicalMidlevel.Add(property.Name, technicalJuniors);
                            }
                        }

                        technicalMidlevels.Add((dynamic)technicalMidlevel);

                        foreach (var junior in midlevel.Juniors)
                        {
                            var technicalFreshers = new List<ExpandoObject>();

                            var technicalJunior = new ExpandoObject() as IDictionary<string, object>;

                            technicalJunior.Add("Id", junior.Id.JsonElement());

                            foreach (var property in typeof(JuniorDeveloper).GetProperties())
                            {
                                if (propertyValues.TryGetValue(property.Name, out var value))
                                {
                                    technicalJunior.Add(property.Name, value);
                                    continue;
                                }

                                if (property.Name is nameof(JuniorDeveloper.Freshers))
                                {
                                    technicalJunior.Add(property.Name, technicalFreshers);
                                }
                            }

                            technicalJuniors.Add((dynamic)technicalJunior);

                            foreach (var fresher in junior.Freshers)
                            {
                                var technicalFresher = new ExpandoObject() as IDictionary<string, object>;

                                technicalFresher.Add("Id", fresher.Id.JsonElement());

                                foreach (var property in typeof(FresherDeveloper).GetProperties())
                                {
                                    if (propertyValues.TryGetValue(property.Name, out var value))
                                    {
                                        technicalFresher.Add(property.Name, value);
                                    }
                                }

                                technicalFreshers.Add((dynamic)technicalFresher);
                            }
                        }
                    }
                }
            }

            PrepareForInvalidScenario(technicalTeamLeads, validItems);

            PrepareForInvalidScenario(technicalTeamLeads, validItems);

            foreach (var qaTestingTeamLead in chiefTechnicalOfficer.QaTestingTeamLeads)
            {
                var qATestingSeniors = new List<ExpandoObject>();

                var qATestingLead = new ExpandoObject() as IDictionary<string, object>;

                qATestingLead.Add("Id", qaTestingTeamLead.Id.JsonElement());

                foreach (var property in typeof(QaTestingTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        qATestingLead.Add(property.Name, value);
                        continue;
                    }

                    if (property.Name is nameof(QaTestingTeamLead.Seniors))
                    {
                        qATestingLead.Add(property.Name, qATestingSeniors);
                    }
                }

                qATestingTeamLeads.Add((dynamic)qATestingLead);

                foreach (var senior in qaTestingTeamLead.Seniors)
                {
                    var qaTestingSenior = new ExpandoObject() as IDictionary<string, object>;

                    qaTestingSenior.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorDeveloper).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            qaTestingSenior.Add(property.Name, value);
                            continue;
                        }
                    }

                    qATestingSeniors.Add((dynamic)qaTestingSenior);
                }
            }

            PrepareForInvalidScenario(qATestingTeamLeads, validItems);

            PrepareForInvalidScenario(qATestingTeamLeads, validItems);

            #endregion


            #region ChiefProductOfficer

            productOfficer.Add("Id", chiefProductOfficer.Id.JsonElement());

            foreach (var property in typeof(ChiefProductOfficer).GetProperties())
            {
                if (propertyValues.TryGetValue(property.Name, out var value))
                {
                    productOfficer.Add(property.Name, value);
                }
            }

            var productTeamLeads = new List<ExpandoObject>();
            ((dynamic)productOfficer).ProductTeamLeads = productTeamLeads;

            var scrumMasterTeamLeads = new List<ExpandoObject>();
            ((dynamic)productOfficer).ScrumMasterTeamLeads = scrumMasterTeamLeads;

            foreach (var productTeamLead in chiefProductOfficer!.ProductTeamLeads)
            {
                var productMangerLead = new ExpandoObject() as IDictionary<string, object>;

                productMangerLead.Add("Id", productTeamLead.Id.JsonElement());

                foreach (var property in typeof(ProductTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        productMangerLead.Add(property.Name, value);
                    }
                }

                productTeamLeads.Add((dynamic)productMangerLead);

                var seniorProducts = new List<ExpandoObject>();
                ((dynamic)productMangerLead).Seniors = seniorProducts;

                foreach (var senior in productTeamLead.Seniors)
                {
                    var seniorProduct = new ExpandoObject() as IDictionary<string, object>;

                    seniorProduct.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorProductManager).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            seniorProduct.Add(property.Name, value);
                        }
                    }

                    seniorProducts.Add((dynamic)seniorProduct);

                    var midlevelProducts = new List<ExpandoObject>();
                    ((dynamic)seniorProduct).Midlevels = midlevelProducts;

                    foreach (var midlevel in senior.Midlevels)
                    {
                        var midlevelProduct = new ExpandoObject() as IDictionary<string, object>;

                        midlevelProduct.Add("Id", midlevel.Id.JsonElement());

                        foreach (var property in typeof(MidlevelProductManager).GetProperties())
                        {
                            if (propertyValues.TryGetValue(property.Name, out var value))
                            {
                                midlevelProduct.Add(property.Name, value);
                            }
                        }

                        midlevelProducts.Add((dynamic)midlevelProduct);

                        var juniorProducts = new List<ExpandoObject>();
                        ((dynamic)midlevelProduct).Juniors = juniorProducts;

                        foreach (var junior in midlevel.Juniors)
                        {
                            var juniorProduct = new ExpandoObject() as IDictionary<string, object>;

                            juniorProduct.Add("Id", junior.Id.JsonElement());

                            foreach (var property in typeof(JuniorProductManager).GetProperties())
                            {
                                if (propertyValues.TryGetValue(property.Name, out var value))
                                {
                                    juniorProduct.Add(property.Name, value);
                                }
                            }

                            juniorProducts.Add((dynamic)juniorProduct);

                            var fresherProducts = new List<ExpandoObject>();
                            ((dynamic)juniorProduct).Freshers = fresherProducts;

                            foreach (var fresher in junior.Freshers)
                            {
                                var fresherProduct = new ExpandoObject() as IDictionary<string, object>;

                                fresherProduct.Add("Id", fresher.Id.JsonElement());

                                foreach (var property in typeof(FresherProductManager).GetProperties())
                                {
                                    if (propertyValues.TryGetValue(property.Name, out var value))
                                    {
                                        fresherProduct.Add(property.Name, value);
                                    }
                                }

                                fresherProducts.Add((dynamic)fresherProduct);
                            }
                        }
                    }
                }
            }

            PrepareForInvalidScenario(productTeamLeads, validItems);

            foreach (var scrumTeamLead in chiefProductOfficer.ScrumMasterTeamLeads)
            {
                var scrumMasterTeamLead = new ExpandoObject() as IDictionary<string, object>;

                scrumMasterTeamLead.Add("Id", scrumTeamLead.Id.JsonElement());

                foreach (var property in typeof(ScrumMasterTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        scrumMasterTeamLead.Add(property.Name, value);
                    }
                }

                scrumMasterTeamLeads.Add((dynamic)scrumMasterTeamLead);

                var seniorScrumMasters = new List<ExpandoObject>();
                ((dynamic)scrumMasterTeamLead).Seniors = seniorScrumMasters;

                foreach (var senior in scrumTeamLead.Seniors)
                {
                    var seniorScrum = new ExpandoObject() as IDictionary<string, object>;

                    seniorScrum.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorScrumMaster).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            seniorScrum.Add(property.Name, value);
                        }
                    }

                    seniorScrumMasters.Add((dynamic)seniorScrum);
                }
            }

            PrepareForInvalidScenario(scrumMasterTeamLeads, validItems);

            #endregion


            #region ChiefMarketingOfficer

            var marketingTeamLeads = new List<ExpandoObject>();

            marketingOfficer.Add("Id", chiefMarketingOfficer.Id.JsonElement());

            foreach (var property in typeof(ChiefMarketingOfficer).GetProperties())
            {
                if (propertyValues.TryGetValue(property.Name, out var value))
                {
                    marketingOfficer.Add(property.Name, value);
                    continue;
                }

                if (property.Name is nameof(ChiefMarketingOfficer.MarketingTeamLeads))
                {
                    marketingOfficer.Add(property.Name, marketingTeamLeads);
                }
            }

            #region MarketingTeamLeads

            foreach (var marketingTeamLead in chiefMarketingOfficer!.MarketingTeamLeads)
            {
                var marketingSeniors = new List<ExpandoObject>();

                var marketingLead = new ExpandoObject() as IDictionary<string, object>;

                marketingLead.Add("Id", marketingTeamLead.Id.JsonElement());

                foreach (var property in typeof(MarketingTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        marketingLead.Add(property.Name, value);
                        continue;
                    }

                    if (property.Name is nameof(MarketingTeamLead.Seniors))
                    {
                        marketingLead.Add(property.Name, marketingSeniors);
                    }
                }

                marketingTeamLeads.Add((dynamic)marketingLead);

                foreach (var senior in marketingTeamLead.Seniors)
                {
                    var marketingMidlevels = new List<ExpandoObject>();

                    var marketingSenior = new ExpandoObject() as IDictionary<string, object>;

                    marketingSenior.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorMarketing).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            marketingSenior.Add(property.Name, value);
                            continue;
                        }

                        if (property.Name is nameof(SeniorMarketing.Midlevels))
                        {
                            marketingSenior.Add(property.Name, marketingMidlevels);
                        }
                    }

                    marketingSeniors.Add((dynamic)marketingSenior);

                    foreach (var midlevel in senior.Midlevels)
                    {
                        var marketingJuniors = new List<ExpandoObject>();

                        var marketingMidlevel = new ExpandoObject() as IDictionary<string, object>;

                        marketingMidlevel.Add("Id", midlevel.Id.JsonElement());

                        foreach (var property in typeof(MidlevelMarketing).GetProperties())
                        {
                            if (propertyValues.TryGetValue(property.Name, out var value))
                            {
                                marketingMidlevel.Add(property.Name, value);
                                continue;
                            }

                            if (property.Name is nameof(MidlevelMarketing.Juniors))
                            {
                                marketingMidlevel.Add(property.Name, marketingJuniors);
                            }
                        }

                        marketingMidlevels.Add((dynamic)marketingMidlevel);

                        foreach (var junior in midlevel.Juniors)
                        {
                            var marketingFreshers = new List<ExpandoObject>();

                            var marketingJunior = new ExpandoObject() as IDictionary<string, object>;

                            marketingJunior.Add("Id", junior.Id.JsonElement());

                            foreach (var property in typeof(JuniorMarketing).GetProperties())
                            {
                                if (propertyValues.TryGetValue(property.Name, out var value))
                                {
                                    marketingJunior.Add(property.Name, value);
                                    continue;
                                }

                                if (property.Name is nameof(JuniorMarketing.Freshers))
                                {
                                    marketingJunior.Add(property.Name, marketingFreshers);
                                }
                            }

                            marketingJuniors.Add((dynamic)marketingJunior);

                            foreach (var fresher in junior.Freshers)
                            {
                                var marketingFresher = new ExpandoObject() as IDictionary<string, object>;

                                marketingFresher.Add("Id", fresher.Id.JsonElement());

                                foreach (var property in typeof(FresherMarketing).GetProperties())
                                {
                                    if (propertyValues.TryGetValue(property.Name, out var value))
                                    {
                                        marketingFresher.Add(property.Name, value);
                                    }
                                }

                                marketingFreshers.Add((dynamic)marketingFresher);
                            }
                        }
                    }
                }
            }

            PrepareForInvalidScenario(marketingTeamLeads, validItems);

            #endregion


            #endregion


            executiveOfficers.Add((dynamic)executiveOfficer);
        }

        return executiveOfficers;
    }

    public static List<ExpandoObject> CreateSeniorInvalidPatchExecutiveOfficers(List<ChiefExecutiveOfficer> chiefExecutiveOfficers, int validItems)
    {
        var random = new Random();

        var executiveOfficers = new List<ExpandoObject>();

        foreach (var chiefExecutiveOfficer in chiefExecutiveOfficers)
        {
            ChiefProductOfficer? chiefProductOfficer = chiefExecutiveOfficer.ChiefProductOfficer;
            ChiefTechnicalOfficer? chiefTechnicalOfficer = chiefExecutiveOfficer.ChiefTechnicalOfficer;
            ChiefMarketingOfficer? chiefMarketingOfficer = chiefExecutiveOfficer.ChiefMarketingOfficer;

            int index = random.Next(0, 6);

            var propertyValues = new Dictionary<string, object?>();

            var age = Ages[index];
            var height = Heights[index];
            var weight = Weights[index];
            var address = Addresses[index];
            var eyeColor = EyeColors[index];
            var birthDate = BirthDates[index];
            var isFired = FireStatuses[index];
            var graduation = Graduations[index];
            var experience = Experiences[index];
            var officerName = OfficerNames[index];
            var modifiedDate = ModifiedDates[index];
            var organization = Organizations[index];
            var nationalCode = NationalCodes[index];
            var personalCode = PersonalCodes[index];
            var daysOfVacation = DaysOfVacation[index];
            var officerLastName = OfficerLastNames[index];
            var contractDateEnd = ContractDatesEnd[index];
            var uniqueIdentifier = UniqueIdentifiers[index];
            var contractDateStart = ContractDatesStart[index];

            propertyValues.Add(nameof(ChiefUnitIdentity.Age), 25.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Height), 188.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Weight), 85.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.IsFired), true.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Address), "Patched !".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Name), "Patched !".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.EyeColor), eyeColor?.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.BirthDate), birthDate.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Experience), Experience.Elementary.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Graduation), Graduation.Diploma.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.LastName), "Patched !".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.NationalCode), "99999999".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.PersonalCode), "Patched !".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.ModifiedDate), modifiedDate?.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.DaysOfVacation), daysOfVacation?.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.ContractDateEnd), contractDateEnd.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.UniqueIdentifier), uniqueIdentifier.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.AssignedOrganization), "SnappTrip".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.ContractDateStart), contractDateStart.JsonElement());

            var executiveOfficer = new ExpandoObject() as IDictionary<string, object>;
            var technicalOfficer = new ExpandoObject() as IDictionary<string, object>;
            var marketingOfficer = new ExpandoObject() as IDictionary<string, object>;
            var productOfficer = new ExpandoObject() as IDictionary<string, object>;


            #region ChiefExecutiveOfficer

            executiveOfficer.Add("Id", chiefExecutiveOfficer.Id.JsonElement());

            foreach (var property in typeof(ChiefExecutiveOfficer).GetProperties())
            {
                if (propertyValues.TryGetValue(property.Name, out var value))
                {
                    executiveOfficer.Add(property.Name, value);
                    continue;
                }

                if (property.Name is nameof(ChiefExecutiveOfficer.ChiefTechnicalOfficer))
                {
                    executiveOfficer.Add(property.Name, technicalOfficer);
                    continue;
                }

                if (property.Name is nameof(ChiefExecutiveOfficer.ChiefProductOfficer))
                {
                    executiveOfficer.Add(property.Name, productOfficer);
                    continue;
                }

                if (property.Name is nameof(ChiefExecutiveOfficer.ChiefMarketingOfficer))
                {
                    executiveOfficer.Add(property.Name, marketingOfficer);
                }
            }

            #endregion


            #region ChiefTechnicalOfficer

            var technicalTeamLeads = new List<ExpandoObject>();
            var qATestingTeamLeads = new List<ExpandoObject>();

            technicalOfficer.Add("Id", chiefTechnicalOfficer.Id.JsonElement());

            foreach (var property in typeof(ChiefTechnicalOfficer).GetProperties())
            {
                if (propertyValues.TryGetValue(property.Name, out var value))
                {
                    technicalOfficer.Add(property.Name, value);
                    continue;
                }

                if (property.Name is nameof(ChiefTechnicalOfficer.TechnicalTeamLeads))
                {
                    technicalOfficer.Add(property.Name, technicalTeamLeads);
                    continue;
                }

                if (property.Name is nameof(ChiefTechnicalOfficer.QaTestingTeamLeads))
                {
                    technicalOfficer.Add(property.Name, qATestingTeamLeads);
                }
            }

            foreach (var technicalTeamLead in chiefTechnicalOfficer!.TechnicalTeamLeads)
            {
                var technicalSeniors = new List<ExpandoObject>();

                var technicalLead = new ExpandoObject() as IDictionary<string, object>;

                technicalLead.Add("Id", technicalTeamLead.Id.JsonElement());

                foreach (var property in typeof(TechnicalTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        technicalLead.Add(property.Name, value);
                        continue;
                    }

                    if (property.Name is nameof(TechnicalTeamLead.Seniors))
                    {
                        technicalLead.Add(property.Name, technicalSeniors);
                    }
                }

                technicalTeamLeads.Add((dynamic)technicalLead);

                foreach (var senior in technicalTeamLead.Seniors)
                {
                    var technicalMidlevels = new List<ExpandoObject>();

                    var technicalSenior = new ExpandoObject() as IDictionary<string, object>;

                    technicalSenior.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorDeveloper).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            technicalSenior.Add(property.Name, value);
                            continue;
                        }

                        if (property.Name is nameof(SeniorDeveloper.Midlevels))
                        {
                            technicalSenior.Add(property.Name, technicalMidlevels);
                        }
                    }

                    technicalSeniors.Add((dynamic)technicalSenior);

                    foreach (var midlevel in senior.Midlevels)
                    {
                        var technicalJuniors = new List<ExpandoObject>();

                        var technicalMidlevel = new ExpandoObject() as IDictionary<string, object>;

                        technicalMidlevel.Add("Id", midlevel.Id.JsonElement());

                        foreach (var property in typeof(MidlevelDeveloper).GetProperties())
                        {
                            if (propertyValues.TryGetValue(property.Name, out var value))
                            {
                                technicalMidlevel.Add(property.Name, value);
                                continue;
                            }

                            if (property.Name is nameof(MidlevelDeveloper.Juniors))
                            {
                                technicalMidlevel.Add(property.Name, technicalJuniors);
                            }
                        }

                        technicalMidlevels.Add((dynamic)technicalMidlevel);

                        foreach (var junior in midlevel.Juniors)
                        {
                            var technicalFreshers = new List<ExpandoObject>();

                            var technicalJunior = new ExpandoObject() as IDictionary<string, object>;

                            technicalJunior.Add("Id", junior.Id.JsonElement());

                            foreach (var property in typeof(JuniorDeveloper).GetProperties())
                            {
                                if (propertyValues.TryGetValue(property.Name, out var value))
                                {
                                    technicalJunior.Add(property.Name, value);
                                    continue;
                                }

                                if (property.Name is nameof(JuniorDeveloper.Freshers))
                                {
                                    technicalJunior.Add(property.Name, technicalFreshers);
                                }
                            }

                            technicalJuniors.Add((dynamic)technicalJunior);

                            foreach (var fresher in junior.Freshers)
                            {
                                var technicalFresher = new ExpandoObject() as IDictionary<string, object>;

                                technicalFresher.Add("Id", fresher.Id.JsonElement());

                                foreach (var property in typeof(FresherDeveloper).GetProperties())
                                {
                                    if (propertyValues.TryGetValue(property.Name, out var value))
                                    {
                                        technicalFresher.Add(property.Name, value);
                                    }
                                }

                                technicalFreshers.Add((dynamic)technicalFresher);
                            }
                        }
                    }
                }

                PrepareForInvalidScenario(technicalSeniors, validItems);
            }

            foreach (var qaTestingTeamLead in chiefTechnicalOfficer.QaTestingTeamLeads)
            {
                var qATestingSeniors = new List<ExpandoObject>();

                var qATestingLead = new ExpandoObject() as IDictionary<string, object>;

                qATestingLead.Add("Id", qaTestingTeamLead.Id.JsonElement());

                foreach (var property in typeof(QaTestingTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        qATestingLead.Add(property.Name, value);
                        continue;
                    }

                    if (property.Name is nameof(QaTestingTeamLead.Seniors))
                    {
                        qATestingLead.Add(property.Name, qATestingSeniors);
                    }
                }

                qATestingTeamLeads.Add((dynamic)qATestingLead);

                foreach (var senior in qaTestingTeamLead.Seniors)
                {
                    var qaTestingSenior = new ExpandoObject() as IDictionary<string, object>;

                    qaTestingSenior.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorDeveloper).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            qaTestingSenior.Add(property.Name, value);
                            continue;
                        }
                    }

                    qATestingSeniors.Add((dynamic)qaTestingSenior);
                }

                PrepareForInvalidScenario(qATestingSeniors, validItems);
            }

            #endregion


            #region ChiefProductOfficer

            productOfficer.Add("Id", chiefProductOfficer.Id.JsonElement());

            foreach (var property in typeof(ChiefProductOfficer).GetProperties())
            {
                if (propertyValues.TryGetValue(property.Name, out var value))
                {
                    productOfficer.Add(property.Name, value);
                }
            }

            var productTeamLeads = new List<ExpandoObject>();
            ((dynamic)productOfficer).ProductTeamLeads = productTeamLeads;

            var scrumMasterTeamLeads = new List<ExpandoObject>();
            ((dynamic)productOfficer).ScrumMasterTeamLeads = scrumMasterTeamLeads;

            foreach (var productTeamLead in chiefProductOfficer!.ProductTeamLeads)
            {
                var productMangerLead = new ExpandoObject() as IDictionary<string, object>;

                productMangerLead.Add("Id", productTeamLead.Id.JsonElement());

                foreach (var property in typeof(ProductTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        productMangerLead.Add(property.Name, value);
                    }
                }

                productTeamLeads.Add((dynamic)productMangerLead);

                var seniorProducts = new List<ExpandoObject>();
                ((dynamic)productMangerLead).Seniors = seniorProducts;

                foreach (var senior in productTeamLead.Seniors)
                {
                    var seniorProduct = new ExpandoObject() as IDictionary<string, object>;

                    seniorProduct.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorProductManager).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            seniorProduct.Add(property.Name, value);
                        }
                    }

                    seniorProducts.Add((dynamic)seniorProduct);

                    var midlevelProducts = new List<ExpandoObject>();
                    ((dynamic)seniorProduct).Midlevels = midlevelProducts;

                    foreach (var midlevel in senior.Midlevels)
                    {
                        var midlevelProduct = new ExpandoObject() as IDictionary<string, object>;

                        midlevelProduct.Add("Id", midlevel.Id.JsonElement());

                        foreach (var property in typeof(MidlevelProductManager).GetProperties())
                        {
                            if (propertyValues.TryGetValue(property.Name, out var value))
                            {
                                midlevelProduct.Add(property.Name, value);
                            }
                        }

                        midlevelProducts.Add((dynamic)midlevelProduct);

                        var juniorProducts = new List<ExpandoObject>();
                        ((dynamic)midlevelProduct).Juniors = juniorProducts;

                        foreach (var junior in midlevel.Juniors)
                        {
                            var juniorProduct = new ExpandoObject() as IDictionary<string, object>;

                            juniorProduct.Add("Id", junior.Id.JsonElement());

                            foreach (var property in typeof(JuniorProductManager).GetProperties())
                            {
                                if (propertyValues.TryGetValue(property.Name, out var value))
                                {
                                    juniorProduct.Add(property.Name, value);
                                }
                            }

                            juniorProducts.Add((dynamic)juniorProduct);

                            var fresherProducts = new List<ExpandoObject>();
                            ((dynamic)juniorProduct).Freshers = fresherProducts;

                            foreach (var fresher in junior.Freshers)
                            {
                                var fresherProduct = new ExpandoObject() as IDictionary<string, object>;

                                fresherProduct.Add("Id", fresher.Id.JsonElement());

                                foreach (var property in typeof(FresherProductManager).GetProperties())
                                {
                                    if (propertyValues.TryGetValue(property.Name, out var value))
                                    {
                                        fresherProduct.Add(property.Name, value);
                                    }
                                }

                                fresherProducts.Add((dynamic)fresherProduct);
                            }
                        }
                    }
                }

                PrepareForInvalidScenario(seniorProducts, validItems);
            }

            foreach (var scrumTeamLead in chiefProductOfficer.ScrumMasterTeamLeads)
            {
                var scrumMasterTeamLead = new ExpandoObject() as IDictionary<string, object>;

                scrumMasterTeamLead.Add("Id", scrumTeamLead.Id.JsonElement());

                foreach (var property in typeof(ScrumMasterTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        scrumMasterTeamLead.Add(property.Name, value);
                    }
                }

                scrumMasterTeamLeads.Add((dynamic)scrumMasterTeamLead);

                var seniorScrumMasters = new List<ExpandoObject>();
                ((dynamic)scrumMasterTeamLead).Seniors = seniorScrumMasters;

                foreach (var senior in scrumTeamLead.Seniors)
                {
                    var seniorScrum = new ExpandoObject() as IDictionary<string, object>;

                    seniorScrum.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorScrumMaster).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            seniorScrum.Add(property.Name, value);
                        }
                    }

                    seniorScrumMasters.Add((dynamic)seniorScrum);
                }

                PrepareForInvalidScenario(seniorScrumMasters, validItems);
            }

            #endregion


            #region ChiefMarketingOfficer

            var marketingTeamLeads = new List<ExpandoObject>();

            marketingOfficer.Add("Id", chiefMarketingOfficer.Id.JsonElement());

            foreach (var property in typeof(ChiefMarketingOfficer).GetProperties())
            {
                if (propertyValues.TryGetValue(property.Name, out var value))
                {
                    marketingOfficer.Add(property.Name, value);
                    continue;
                }

                if (property.Name is nameof(ChiefMarketingOfficer.MarketingTeamLeads))
                {
                    marketingOfficer.Add(property.Name, marketingTeamLeads);
                }
            }

            foreach (var marketingTeamLead in chiefMarketingOfficer!.MarketingTeamLeads)
            {
                var marketingSeniors = new List<ExpandoObject>();

                var marketingLead = new ExpandoObject() as IDictionary<string, object>;

                marketingLead.Add("Id", marketingTeamLead.Id.JsonElement());

                foreach (var property in typeof(MarketingTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        marketingLead.Add(property.Name, value);
                        continue;
                    }

                    if (property.Name is nameof(MarketingTeamLead.Seniors))
                    {
                        marketingLead.Add(property.Name, marketingSeniors);
                    }
                }

                marketingTeamLeads.Add((dynamic)marketingLead);

                foreach (var senior in marketingTeamLead.Seniors)
                {
                    var marketingMidlevels = new List<ExpandoObject>();

                    var marketingSenior = new ExpandoObject() as IDictionary<string, object>;

                    marketingSenior.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorMarketing).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            marketingSenior.Add(property.Name, value);
                            continue;
                        }

                        if (property.Name is nameof(SeniorMarketing.Midlevels))
                        {
                            marketingSenior.Add(property.Name, marketingMidlevels);
                        }
                    }

                    marketingSeniors.Add((dynamic)marketingSenior);

                    foreach (var midlevel in senior.Midlevels)
                    {
                        var marketingJuniors = new List<ExpandoObject>();

                        var marketingMidlevel = new ExpandoObject() as IDictionary<string, object>;

                        marketingMidlevel.Add("Id", midlevel.Id.JsonElement());

                        foreach (var property in typeof(MidlevelMarketing).GetProperties())
                        {
                            if (propertyValues.TryGetValue(property.Name, out var value))
                            {
                                marketingMidlevel.Add(property.Name, value);
                                continue;
                            }

                            if (property.Name is nameof(MidlevelMarketing.Juniors))
                            {
                                marketingMidlevel.Add(property.Name, marketingJuniors);
                            }
                        }

                        marketingMidlevels.Add((dynamic)marketingMidlevel);

                        foreach (var junior in midlevel.Juniors)
                        {
                            var marketingFreshers = new List<ExpandoObject>();

                            var marketingJunior = new ExpandoObject() as IDictionary<string, object>;

                            marketingJunior.Add("Id", junior.Id.JsonElement());

                            foreach (var property in typeof(JuniorMarketing).GetProperties())
                            {
                                if (propertyValues.TryGetValue(property.Name, out var value))
                                {
                                    marketingJunior.Add(property.Name, value);
                                    continue;
                                }

                                if (property.Name is nameof(JuniorMarketing.Freshers))
                                {
                                    marketingJunior.Add(property.Name, marketingFreshers);
                                }
                            }

                            marketingJuniors.Add((dynamic)marketingJunior);

                            foreach (var fresher in junior.Freshers)
                            {
                                var marketingFresher = new ExpandoObject() as IDictionary<string, object>;

                                marketingFresher.Add("Id", fresher.Id.JsonElement());

                                foreach (var property in typeof(FresherMarketing).GetProperties())
                                {
                                    if (propertyValues.TryGetValue(property.Name, out var value))
                                    {
                                        marketingFresher.Add(property.Name, value);
                                    }
                                }

                                marketingFreshers.Add((dynamic)marketingFresher);
                            }
                        }
                    }
                }

                PrepareForInvalidScenario(marketingSeniors, validItems);
            }

            #endregion


            executiveOfficers.Add((dynamic)executiveOfficer);
        }

        return executiveOfficers;
    }

    public static List<ExpandoObject> CreateMidlevelInvalidPatchExecutiveOfficers(List<ChiefExecutiveOfficer> chiefExecutiveOfficers, int validItems)
    {
        var random = new Random();

        var executiveOfficers = new List<ExpandoObject>();

        foreach (var chiefExecutiveOfficer in chiefExecutiveOfficers)
        {
            ChiefProductOfficer? chiefProductOfficer = chiefExecutiveOfficer.ChiefProductOfficer;
            ChiefTechnicalOfficer? chiefTechnicalOfficer = chiefExecutiveOfficer.ChiefTechnicalOfficer;
            ChiefMarketingOfficer? chiefMarketingOfficer = chiefExecutiveOfficer.ChiefMarketingOfficer;

            int index = random.Next(0, 6);

            var propertyValues = new Dictionary<string, object?>();

            var age = Ages[index];
            var height = Heights[index];
            var weight = Weights[index];
            var address = Addresses[index];
            var eyeColor = EyeColors[index];
            var birthDate = BirthDates[index];
            var isFired = FireStatuses[index];
            var graduation = Graduations[index];
            var experience = Experiences[index];
            var officerName = OfficerNames[index];
            var modifiedDate = ModifiedDates[index];
            var organization = Organizations[index];
            var nationalCode = NationalCodes[index];
            var personalCode = PersonalCodes[index];
            var daysOfVacation = DaysOfVacation[index];
            var officerLastName = OfficerLastNames[index];
            var contractDateEnd = ContractDatesEnd[index];
            var uniqueIdentifier = UniqueIdentifiers[index];
            var contractDateStart = ContractDatesStart[index];

            propertyValues.Add(nameof(ChiefUnitIdentity.Age), 25.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Height), 188.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Weight), 85.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.IsFired), true.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Address), "Patched !".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Name), "Patched !".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.EyeColor), eyeColor?.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.BirthDate), birthDate.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Experience), Experience.Elementary.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Graduation), Graduation.Diploma.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.LastName), "Patched !".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.NationalCode), "99999999".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.PersonalCode), "Patched !".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.ModifiedDate), modifiedDate?.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.DaysOfVacation), daysOfVacation?.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.ContractDateEnd), contractDateEnd.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.UniqueIdentifier), uniqueIdentifier.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.AssignedOrganization), "SnappTrip".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.ContractDateStart), contractDateStart.JsonElement());

            var executiveOfficer = new ExpandoObject() as IDictionary<string, object>;
            var technicalOfficer = new ExpandoObject() as IDictionary<string, object>;
            var marketingOfficer = new ExpandoObject() as IDictionary<string, object>;
            var productOfficer = new ExpandoObject() as IDictionary<string, object>;


            #region ChiefExecutiveOfficer

            executiveOfficer.Add("Id", chiefExecutiveOfficer.Id.JsonElement());

            foreach (var property in typeof(ChiefExecutiveOfficer).GetProperties())
            {
                if (propertyValues.TryGetValue(property.Name, out var value))
                {
                    executiveOfficer.Add(property.Name, value);
                    continue;
                }

                if (property.Name is nameof(ChiefExecutiveOfficer.ChiefTechnicalOfficer))
                {
                    executiveOfficer.Add(property.Name, technicalOfficer);
                    continue;
                }

                if (property.Name is nameof(ChiefExecutiveOfficer.ChiefProductOfficer))
                {
                    executiveOfficer.Add(property.Name, productOfficer);
                    continue;
                }

                if (property.Name is nameof(ChiefExecutiveOfficer.ChiefMarketingOfficer))
                {
                    executiveOfficer.Add(property.Name, marketingOfficer);
                }
            }

            #endregion


            #region ChiefTechnicalOfficer

            var technicalTeamLeads = new List<ExpandoObject>();
            var qATestingTeamLeads = new List<ExpandoObject>();

            technicalOfficer.Add("Id", chiefTechnicalOfficer.Id.JsonElement());

            foreach (var property in typeof(ChiefTechnicalOfficer).GetProperties())
            {
                if (propertyValues.TryGetValue(property.Name, out var value))
                {
                    technicalOfficer.Add(property.Name, value);
                    continue;
                }

                if (property.Name is nameof(ChiefTechnicalOfficer.TechnicalTeamLeads))
                {
                    technicalOfficer.Add(property.Name, technicalTeamLeads);
                    continue;
                }

                if (property.Name is nameof(ChiefTechnicalOfficer.QaTestingTeamLeads))
                {
                    technicalOfficer.Add(property.Name, qATestingTeamLeads);
                }
            }

            foreach (var technicalTeamLead in chiefTechnicalOfficer!.TechnicalTeamLeads)
            {
                var technicalSeniors = new List<ExpandoObject>();

                var technicalLead = new ExpandoObject() as IDictionary<string, object>;

                technicalLead.Add("Id", technicalTeamLead.Id.JsonElement());

                foreach (var property in typeof(TechnicalTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        technicalLead.Add(property.Name, value);
                        continue;
                    }

                    if (property.Name is nameof(TechnicalTeamLead.Seniors))
                    {
                        technicalLead.Add(property.Name, technicalSeniors);
                    }
                }

                technicalTeamLeads.Add((dynamic)technicalLead);

                foreach (var senior in technicalTeamLead.Seniors)
                {
                    var technicalMidlevels = new List<ExpandoObject>();

                    var technicalSenior = new ExpandoObject() as IDictionary<string, object>;

                    technicalSenior.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorDeveloper).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            technicalSenior.Add(property.Name, value);
                            continue;
                        }

                        if (property.Name is nameof(SeniorDeveloper.Midlevels))
                        {
                            technicalSenior.Add(property.Name, technicalMidlevels);
                        }
                    }

                    technicalSeniors.Add((dynamic)technicalSenior);

                    foreach (var midlevel in senior.Midlevels)
                    {
                        var technicalJuniors = new List<ExpandoObject>();

                        var technicalMidlevel = new ExpandoObject() as IDictionary<string, object>;

                        technicalMidlevel.Add("Id", midlevel.Id.JsonElement());

                        foreach (var property in typeof(MidlevelDeveloper).GetProperties())
                        {
                            if (propertyValues.TryGetValue(property.Name, out var value))
                            {
                                technicalMidlevel.Add(property.Name, value);
                                continue;
                            }

                            if (property.Name is nameof(MidlevelDeveloper.Juniors))
                            {
                                technicalMidlevel.Add(property.Name, technicalJuniors);
                            }
                        }

                        technicalMidlevels.Add((dynamic)technicalMidlevel);

                        foreach (var junior in midlevel.Juniors)
                        {
                            var technicalFreshers = new List<ExpandoObject>();

                            var technicalJunior = new ExpandoObject() as IDictionary<string, object>;

                            technicalJunior.Add("Id", junior.Id.JsonElement());

                            foreach (var property in typeof(JuniorDeveloper).GetProperties())
                            {
                                if (propertyValues.TryGetValue(property.Name, out var value))
                                {
                                    technicalJunior.Add(property.Name, value);
                                    continue;
                                }

                                if (property.Name is nameof(JuniorDeveloper.Freshers))
                                {
                                    technicalJunior.Add(property.Name, technicalFreshers);
                                }
                            }

                            technicalJuniors.Add((dynamic)technicalJunior);

                            foreach (var fresher in junior.Freshers)
                            {
                                var technicalFresher = new ExpandoObject() as IDictionary<string, object>;

                                technicalFresher.Add("Id", fresher.Id.JsonElement());

                                foreach (var property in typeof(FresherDeveloper).GetProperties())
                                {
                                    if (propertyValues.TryGetValue(property.Name, out var value))
                                    {
                                        technicalFresher.Add(property.Name, value);
                                    }
                                }

                                technicalFreshers.Add((dynamic)technicalFresher);
                            }
                        }
                    }

                    PrepareForInvalidScenario(technicalMidlevels, validItems);
                }
            }

            foreach (var qaTestingTeamLead in chiefTechnicalOfficer.QaTestingTeamLeads)
            {
                var qATestingSeniors = new List<ExpandoObject>();

                var qATestingLead = new ExpandoObject() as IDictionary<string, object>;

                qATestingLead.Add("Id", qaTestingTeamLead.Id.JsonElement());

                foreach (var property in typeof(QaTestingTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        qATestingLead.Add(property.Name, value);
                        continue;
                    }

                    if (property.Name is nameof(QaTestingTeamLead.Seniors))
                    {
                        qATestingLead.Add(property.Name, qATestingSeniors);
                    }
                }

                qATestingTeamLeads.Add((dynamic)qATestingLead);

                foreach (var senior in qaTestingTeamLead.Seniors)
                {
                    var qaTestingSenior = new ExpandoObject() as IDictionary<string, object>;

                    qaTestingSenior.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorDeveloper).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            qaTestingSenior.Add(property.Name, value);
                            continue;
                        }
                    }

                    qATestingSeniors.Add((dynamic)qaTestingSenior);
                }
            }

            #endregion


            #region ChiefProductOfficer

            productOfficer.Add("Id", chiefProductOfficer.Id.JsonElement());

            foreach (var property in typeof(ChiefProductOfficer).GetProperties())
            {
                if (propertyValues.TryGetValue(property.Name, out var value))
                {
                    productOfficer.Add(property.Name, value);
                }
            }

            var productTeamLeads = new List<ExpandoObject>();
            ((dynamic)productOfficer).ProductTeamLeads = productTeamLeads;

            var scrumMasterTeamLeads = new List<ExpandoObject>();
            ((dynamic)productOfficer).ScrumMasterTeamLeads = scrumMasterTeamLeads;

            foreach (var productTeamLead in chiefProductOfficer!.ProductTeamLeads)
            {
                var productMangerLead = new ExpandoObject() as IDictionary<string, object>;

                productMangerLead.Add("Id", productTeamLead.Id.JsonElement());

                foreach (var property in typeof(ProductTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        productMangerLead.Add(property.Name, value);
                    }
                }

                productTeamLeads.Add((dynamic)productMangerLead);

                var seniorProducts = new List<ExpandoObject>();
                ((dynamic)productMangerLead).Seniors = seniorProducts;

                foreach (var senior in productTeamLead.Seniors)
                {
                    var seniorProduct = new ExpandoObject() as IDictionary<string, object>;

                    seniorProduct.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorProductManager).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            seniorProduct.Add(property.Name, value);
                        }
                    }

                    seniorProducts.Add((dynamic)seniorProduct);

                    var midlevelProducts = new List<ExpandoObject>();
                    ((dynamic)seniorProduct).Midlevels = midlevelProducts;

                    foreach (var midlevel in senior.Midlevels)
                    {
                        var midlevelProduct = new ExpandoObject() as IDictionary<string, object>;

                        midlevelProduct.Add("Id", midlevel.Id.JsonElement());

                        foreach (var property in typeof(MidlevelProductManager).GetProperties())
                        {
                            if (propertyValues.TryGetValue(property.Name, out var value))
                            {
                                midlevelProduct.Add(property.Name, value);
                            }
                        }

                        midlevelProducts.Add((dynamic)midlevelProduct);

                        var juniorProducts = new List<ExpandoObject>();
                        ((dynamic)midlevelProduct).Juniors = juniorProducts;

                        foreach (var junior in midlevel.Juniors)
                        {
                            var juniorProduct = new ExpandoObject() as IDictionary<string, object>;

                            juniorProduct.Add("Id", junior.Id.JsonElement());

                            foreach (var property in typeof(JuniorProductManager).GetProperties())
                            {
                                if (propertyValues.TryGetValue(property.Name, out var value))
                                {
                                    juniorProduct.Add(property.Name, value);
                                }
                            }

                            juniorProducts.Add((dynamic)juniorProduct);

                            var fresherProducts = new List<ExpandoObject>();
                            ((dynamic)juniorProduct).Freshers = fresherProducts;

                            foreach (var fresher in junior.Freshers)
                            {
                                var fresherProduct = new ExpandoObject() as IDictionary<string, object>;

                                fresherProduct.Add("Id", fresher.Id.JsonElement());

                                foreach (var property in typeof(FresherProductManager).GetProperties())
                                {
                                    if (propertyValues.TryGetValue(property.Name, out var value))
                                    {
                                        fresherProduct.Add(property.Name, value);
                                    }
                                }

                                fresherProducts.Add((dynamic)fresherProduct);
                            }
                        }
                    }

                    PrepareForInvalidScenario(midlevelProducts, validItems);
                }
            }

            foreach (var scrumTeamLead in chiefProductOfficer.ScrumMasterTeamLeads)
            {
                var scrumMasterTeamLead = new ExpandoObject() as IDictionary<string, object>;

                scrumMasterTeamLead.Add("Id", scrumTeamLead.Id.JsonElement());

                foreach (var property in typeof(ScrumMasterTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        scrumMasterTeamLead.Add(property.Name, value);
                    }
                }

                scrumMasterTeamLeads.Add((dynamic)scrumMasterTeamLead);

                var seniorScrumMasters = new List<ExpandoObject>();
                ((dynamic)scrumMasterTeamLead).Seniors = seniorScrumMasters;

                foreach (var senior in scrumTeamLead.Seniors)
                {
                    var seniorScrum = new ExpandoObject() as IDictionary<string, object>;

                    seniorScrum.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorScrumMaster).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            seniorScrum.Add(property.Name, value);
                        }
                    }

                    seniorScrumMasters.Add((dynamic)seniorScrum);
                }
            }

            #endregion


            #region ChiefMarketingOfficer

            var marketingTeamLeads = new List<ExpandoObject>();

            marketingOfficer.Add("Id", chiefMarketingOfficer.Id.JsonElement());

            foreach (var property in typeof(ChiefMarketingOfficer).GetProperties())
            {
                if (propertyValues.TryGetValue(property.Name, out var value))
                {
                    marketingOfficer.Add(property.Name, value);
                    continue;
                }

                if (property.Name is nameof(ChiefMarketingOfficer.MarketingTeamLeads))
                {
                    marketingOfficer.Add(property.Name, marketingTeamLeads);
                }
            }

            foreach (var marketingTeamLead in chiefMarketingOfficer!.MarketingTeamLeads)
            {
                var marketingSeniors = new List<ExpandoObject>();

                var marketingLead = new ExpandoObject() as IDictionary<string, object>;

                marketingLead.Add("Id", marketingTeamLead.Id.JsonElement());

                foreach (var property in typeof(MarketingTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        marketingLead.Add(property.Name, value);
                        continue;
                    }

                    if (property.Name is nameof(MarketingTeamLead.Seniors))
                    {
                        marketingLead.Add(property.Name, marketingSeniors);
                    }
                }

                marketingTeamLeads.Add((dynamic)marketingLead);

                foreach (var senior in marketingTeamLead.Seniors)
                {
                    var marketingMidlevels = new List<ExpandoObject>();

                    var marketingSenior = new ExpandoObject() as IDictionary<string, object>;

                    marketingSenior.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorMarketing).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            marketingSenior.Add(property.Name, value);
                            continue;
                        }

                        if (property.Name is nameof(SeniorMarketing.Midlevels))
                        {
                            marketingSenior.Add(property.Name, marketingMidlevels);
                        }
                    }

                    marketingSeniors.Add((dynamic)marketingSenior);

                    foreach (var midlevel in senior.Midlevels)
                    {
                        var marketingJuniors = new List<ExpandoObject>();

                        var marketingMidlevel = new ExpandoObject() as IDictionary<string, object>;

                        marketingMidlevel.Add("Id", midlevel.Id.JsonElement());

                        foreach (var property in typeof(MidlevelMarketing).GetProperties())
                        {
                            if (propertyValues.TryGetValue(property.Name, out var value))
                            {
                                marketingMidlevel.Add(property.Name, value);
                                continue;
                            }

                            if (property.Name is nameof(MidlevelMarketing.Juniors))
                            {
                                marketingMidlevel.Add(property.Name, marketingJuniors);
                            }
                        }

                        marketingMidlevels.Add((dynamic)marketingMidlevel);

                        foreach (var junior in midlevel.Juniors)
                        {
                            var marketingFreshers = new List<ExpandoObject>();

                            var marketingJunior = new ExpandoObject() as IDictionary<string, object>;

                            marketingJunior.Add("Id", junior.Id.JsonElement());

                            foreach (var property in typeof(JuniorMarketing).GetProperties())
                            {
                                if (propertyValues.TryGetValue(property.Name, out var value))
                                {
                                    marketingJunior.Add(property.Name, value);
                                    continue;
                                }

                                if (property.Name is nameof(JuniorMarketing.Freshers))
                                {
                                    marketingJunior.Add(property.Name, marketingFreshers);
                                }
                            }

                            marketingJuniors.Add((dynamic)marketingJunior);

                            foreach (var fresher in junior.Freshers)
                            {
                                var marketingFresher = new ExpandoObject() as IDictionary<string, object>;

                                marketingFresher.Add("Id", fresher.Id.JsonElement());

                                foreach (var property in typeof(FresherMarketing).GetProperties())
                                {
                                    if (propertyValues.TryGetValue(property.Name, out var value))
                                    {
                                        marketingFresher.Add(property.Name, value);
                                    }
                                }

                                marketingFreshers.Add((dynamic)marketingFresher);
                            }
                        }
                    }

                    PrepareForInvalidScenario(marketingMidlevels, validItems);
                }
            }

            #endregion


            executiveOfficers.Add((dynamic)executiveOfficer);
        }

        return executiveOfficers;
    }

    public static List<ExpandoObject> CreateJuniorInvalidPatchExecutiveOfficers(List<ChiefExecutiveOfficer> chiefExecutiveOfficers, int validItems)
    {
        var random = new Random();

        var executiveOfficers = new List<ExpandoObject>();

        foreach (var chiefExecutiveOfficer in chiefExecutiveOfficers)
        {
            ChiefProductOfficer? chiefProductOfficer = chiefExecutiveOfficer.ChiefProductOfficer;
            ChiefTechnicalOfficer? chiefTechnicalOfficer = chiefExecutiveOfficer.ChiefTechnicalOfficer;
            ChiefMarketingOfficer? chiefMarketingOfficer = chiefExecutiveOfficer.ChiefMarketingOfficer;

            int index = random.Next(0, 6);

            var propertyValues = new Dictionary<string, object?>();

            var age = Ages[index];
            var height = Heights[index];
            var weight = Weights[index];
            var address = Addresses[index];
            var eyeColor = EyeColors[index];
            var birthDate = BirthDates[index];
            var isFired = FireStatuses[index];
            var graduation = Graduations[index];
            var experience = Experiences[index];
            var officerName = OfficerNames[index];
            var modifiedDate = ModifiedDates[index];
            var organization = Organizations[index];
            var nationalCode = NationalCodes[index];
            var personalCode = PersonalCodes[index];
            var daysOfVacation = DaysOfVacation[index];
            var officerLastName = OfficerLastNames[index];
            var contractDateEnd = ContractDatesEnd[index];
            var uniqueIdentifier = UniqueIdentifiers[index];
            var contractDateStart = ContractDatesStart[index];

            propertyValues.Add(nameof(ChiefUnitIdentity.Age), 25.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Height), 188.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Weight), 85.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.IsFired), true.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Address), "Patched !".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Name), "Patched !".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.EyeColor), eyeColor?.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.BirthDate), birthDate.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Experience), Experience.Elementary.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.Graduation), Graduation.Diploma.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.LastName), "Patched !".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.NationalCode), "99999999".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.PersonalCode), "Patched !".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.ModifiedDate), modifiedDate?.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.DaysOfVacation), daysOfVacation?.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.ContractDateEnd), contractDateEnd.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.UniqueIdentifier), uniqueIdentifier.JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.AssignedOrganization), "SnappTrip".JsonElement());
            propertyValues.Add(nameof(ChiefUnitIdentity.ContractDateStart), contractDateStart.JsonElement());

            var executiveOfficer = new ExpandoObject() as IDictionary<string, object>;
            var technicalOfficer = new ExpandoObject() as IDictionary<string, object>;
            var marketingOfficer = new ExpandoObject() as IDictionary<string, object>;
            var productOfficer = new ExpandoObject() as IDictionary<string, object>;


            #region ChiefExecutiveOfficer

            executiveOfficer.Add("Id", chiefExecutiveOfficer.Id.JsonElement());

            foreach (var property in typeof(ChiefExecutiveOfficer).GetProperties())
            {
                if (propertyValues.TryGetValue(property.Name, out var value))
                {
                    executiveOfficer.Add(property.Name, value);
                    continue;
                }

                if (property.Name is nameof(ChiefExecutiveOfficer.ChiefTechnicalOfficer))
                {
                    executiveOfficer.Add(property.Name, technicalOfficer);
                    continue;
                }

                if (property.Name is nameof(ChiefExecutiveOfficer.ChiefProductOfficer))
                {
                    executiveOfficer.Add(property.Name, productOfficer);
                    continue;
                }

                if (property.Name is nameof(ChiefExecutiveOfficer.ChiefMarketingOfficer))
                {
                    executiveOfficer.Add(property.Name, marketingOfficer);
                }
            }

            #endregion


            #region ChiefTechnicalOfficer

            var technicalTeamLeads = new List<ExpandoObject>();
            var qATestingTeamLeads = new List<ExpandoObject>();

            technicalOfficer.Add("Id", chiefTechnicalOfficer.Id.JsonElement());

            foreach (var property in typeof(ChiefTechnicalOfficer).GetProperties())
            {
                if (propertyValues.TryGetValue(property.Name, out var value))
                {
                    technicalOfficer.Add(property.Name, value);
                    continue;
                }

                if (property.Name is nameof(ChiefTechnicalOfficer.TechnicalTeamLeads))
                {
                    technicalOfficer.Add(property.Name, technicalTeamLeads);
                    continue;
                }

                if (property.Name is nameof(ChiefTechnicalOfficer.QaTestingTeamLeads))
                {
                    technicalOfficer.Add(property.Name, qATestingTeamLeads);
                }
            }

            foreach (var technicalTeamLead in chiefTechnicalOfficer.TechnicalTeamLeads)
            {
                var technicalSeniors = new List<ExpandoObject>();

                var technicalLead = new ExpandoObject() as IDictionary<string, object>;

                technicalLead.Add("Id", technicalTeamLead.Id.JsonElement());

                foreach (var property in typeof(TechnicalTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        technicalLead.Add(property.Name, value);
                        continue;
                    }

                    if (property.Name is nameof(TechnicalTeamLead.Seniors))
                    {
                        technicalLead.Add(property.Name, technicalSeniors);
                    }
                }

                technicalTeamLeads.Add((dynamic)technicalLead);

                foreach (var senior in technicalTeamLead.Seniors)
                {
                    var technicalMidlevels = new List<ExpandoObject>();

                    var technicalSenior = new ExpandoObject() as IDictionary<string, object>;

                    technicalSenior.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorDeveloper).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            technicalSenior.Add(property.Name, value);
                            continue;
                        }

                        if (property.Name is nameof(SeniorDeveloper.Midlevels))
                        {
                            technicalSenior.Add(property.Name, technicalMidlevels);
                        }
                    }

                    technicalSeniors.Add((dynamic)technicalSenior);

                    foreach (var midlevel in senior.Midlevels)
                    {
                        var technicalJuniors = new List<ExpandoObject>();

                        var technicalMidlevel = new ExpandoObject() as IDictionary<string, object>;

                        technicalMidlevel.Add("Id", midlevel.Id.JsonElement());

                        foreach (var property in typeof(MidlevelDeveloper).GetProperties())
                        {
                            if (propertyValues.TryGetValue(property.Name, out var value))
                            {
                                technicalMidlevel.Add(property.Name, value);
                                continue;
                            }

                            if (property.Name is nameof(MidlevelDeveloper.Juniors))
                            {
                                technicalMidlevel.Add(property.Name, technicalJuniors);
                            }
                        }

                        technicalMidlevels.Add((dynamic)technicalMidlevel);

                        foreach (var junior in midlevel.Juniors)
                        {
                            var technicalFreshers = new List<ExpandoObject>();

                            var technicalJunior = new ExpandoObject() as IDictionary<string, object>;

                            technicalJunior.Add("Id", junior.Id.JsonElement());

                            foreach (var property in typeof(JuniorDeveloper).GetProperties())
                            {
                                if (propertyValues.TryGetValue(property.Name, out var value))
                                {
                                    technicalJunior.Add(property.Name, value);
                                    continue;
                                }

                                if (property.Name is nameof(JuniorDeveloper.Freshers))
                                {
                                    technicalJunior.Add(property.Name, technicalFreshers);
                                }
                            }

                            technicalJuniors.Add((dynamic)technicalJunior);

                            foreach (var fresher in junior.Freshers)
                            {
                                var technicalFresher = new ExpandoObject() as IDictionary<string, object>;

                                technicalFresher.Add("Id", fresher.Id.JsonElement());

                                foreach (var property in typeof(FresherDeveloper).GetProperties())
                                {
                                    if (propertyValues.TryGetValue(property.Name, out var value))
                                    {
                                        technicalFresher.Add(property.Name, value);
                                    }
                                }

                                technicalFreshers.Add((dynamic)technicalFresher);
                            }
                        }

                        PrepareForInvalidScenario(technicalJuniors, validItems);
                    }
                }
            }

            foreach (var qaTestingTeamLead in chiefTechnicalOfficer.QaTestingTeamLeads)
            {
                var qATestingSeniors = new List<ExpandoObject>();

                var qATestingLead = new ExpandoObject() as IDictionary<string, object>;

                qATestingLead.Add("Id", qaTestingTeamLead.Id.JsonElement());

                foreach (var property in typeof(QaTestingTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        qATestingLead.Add(property.Name, value);
                        continue;
                    }

                    if (property.Name is nameof(QaTestingTeamLead.Seniors))
                    {
                        qATestingLead.Add(property.Name, qATestingSeniors);
                    }
                }

                qATestingTeamLeads.Add((dynamic)qATestingLead);

                foreach (var senior in qaTestingTeamLead.Seniors)
                {
                    var qaTestingSenior = new ExpandoObject() as IDictionary<string, object>;

                    qaTestingSenior.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorDeveloper).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            qaTestingSenior.Add(property.Name, value);
                        }
                    }

                    qATestingSeniors.Add((dynamic)qaTestingSenior);
                }
            }

            #endregion


            #region ChiefProductOfficer

            productOfficer.Add("Id", chiefProductOfficer.Id.JsonElement());

            foreach (var property in typeof(ChiefProductOfficer).GetProperties())
            {
                if (propertyValues.TryGetValue(property.Name, out var value))
                {
                    productOfficer.Add(property.Name, value);
                }
            }

            var productTeamLeads = new List<ExpandoObject>();
            ((dynamic)productOfficer).ProductTeamLeads = productTeamLeads;

            var scrumMasterTeamLeads = new List<ExpandoObject>();
            ((dynamic)productOfficer).ScrumMasterTeamLeads = scrumMasterTeamLeads;

            foreach (var productTeamLead in chiefProductOfficer.ProductTeamLeads)
            {
                var productMangerLead = new ExpandoObject() as IDictionary<string, object>;

                productMangerLead.Add("Id", productTeamLead.Id.JsonElement());

                foreach (var property in typeof(ProductTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        productMangerLead.Add(property.Name, value);
                    }
                }

                productTeamLeads.Add((dynamic)productMangerLead);

                var seniorProducts = new List<ExpandoObject>();
                ((dynamic)productMangerLead).Seniors = seniorProducts;

                foreach (var senior in productTeamLead.Seniors)
                {
                    var seniorProduct = new ExpandoObject() as IDictionary<string, object>;

                    seniorProduct.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorProductManager).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            seniorProduct.Add(property.Name, value);
                        }
                    }

                    seniorProducts.Add((dynamic)seniorProduct);

                    var midlevelProducts = new List<ExpandoObject>();
                    ((dynamic)seniorProduct).Midlevels = midlevelProducts;

                    foreach (var midlevel in senior.Midlevels)
                    {
                        var midlevelProduct = new ExpandoObject() as IDictionary<string, object>;

                        midlevelProduct.Add("Id", midlevel.Id.JsonElement());

                        foreach (var property in typeof(MidlevelProductManager).GetProperties())
                        {
                            if (propertyValues.TryGetValue(property.Name, out var value))
                            {
                                midlevelProduct.Add(property.Name, value);
                            }
                        }

                        midlevelProducts.Add((dynamic)midlevelProduct);

                        var juniorProducts = new List<ExpandoObject>();
                        ((dynamic)midlevelProduct).Juniors = juniorProducts;

                        foreach (var junior in midlevel.Juniors)
                        {
                            var juniorProduct = new ExpandoObject() as IDictionary<string, object>;

                            juniorProduct.Add("Id", junior.Id.JsonElement());

                            foreach (var property in typeof(JuniorProductManager).GetProperties())
                            {
                                if (propertyValues.TryGetValue(property.Name, out var value))
                                {
                                    juniorProduct.Add(property.Name, value);
                                }
                            }

                            juniorProducts.Add((dynamic)juniorProduct);

                            var fresherProducts = new List<ExpandoObject>();
                            ((dynamic)juniorProduct).Freshers = fresherProducts;

                            foreach (var fresher in junior.Freshers)
                            {
                                var fresherProduct = new ExpandoObject() as IDictionary<string, object>;

                                fresherProduct.Add("Id", fresher.Id.JsonElement());

                                foreach (var property in typeof(FresherProductManager).GetProperties())
                                {
                                    if (propertyValues.TryGetValue(property.Name, out var value))
                                    {
                                        fresherProduct.Add(property.Name, value);
                                    }
                                }

                                fresherProducts.Add((dynamic)fresherProduct);
                            }
                        }

                        PrepareForInvalidScenario(juniorProducts, validItems);
                    }
                }
            }

            foreach (var scrumTeamLead in chiefProductOfficer.ScrumMasterTeamLeads)
            {
                var scrumMasterTeamLead = new ExpandoObject() as IDictionary<string, object>;

                scrumMasterTeamLead.Add("Id", scrumTeamLead.Id.JsonElement());

                foreach (var property in typeof(ScrumMasterTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        scrumMasterTeamLead.Add(property.Name, value);
                    }
                }

                scrumMasterTeamLeads.Add((dynamic)scrumMasterTeamLead);

                var seniorScrumMasters = new List<ExpandoObject>();
                ((dynamic)scrumMasterTeamLead).Seniors = seniorScrumMasters;

                foreach (var senior in scrumTeamLead.Seniors)
                {
                    var seniorScrum = new ExpandoObject() as IDictionary<string, object>;

                    seniorScrum.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorScrumMaster).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            seniorScrum.Add(property.Name, value);
                        }
                    }

                    seniorScrumMasters.Add((dynamic)seniorScrum);
                }
            }

            #endregion


            #region ChiefMarketingOfficer

            var marketingTeamLeads = new List<ExpandoObject>();

            marketingOfficer.Add("Id", chiefMarketingOfficer.Id.JsonElement());

            foreach (var property in typeof(ChiefMarketingOfficer).GetProperties())
            {
                if (propertyValues.TryGetValue(property.Name, out var value))
                {
                    marketingOfficer.Add(property.Name, value);
                    continue;
                }

                if (property.Name is nameof(ChiefMarketingOfficer.MarketingTeamLeads))
                {
                    marketingOfficer.Add(property.Name, marketingTeamLeads);
                }
            }

            foreach (var marketingTeamLead in chiefMarketingOfficer!.MarketingTeamLeads)
            {
                var marketingSeniors = new List<ExpandoObject>();

                var marketingLead = new ExpandoObject() as IDictionary<string, object>;

                marketingLead.Add("Id", marketingTeamLead.Id.JsonElement());

                foreach (var property in typeof(MarketingTeamLead).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        marketingLead.Add(property.Name, value);
                        continue;
                    }

                    if (property.Name is nameof(MarketingTeamLead.Seniors))
                    {
                        marketingLead.Add(property.Name, marketingSeniors);
                    }
                }

                marketingTeamLeads.Add((dynamic)marketingLead);

                foreach (var senior in marketingTeamLead.Seniors)
                {
                    var marketingMidlevels = new List<ExpandoObject>();

                    var marketingSenior = new ExpandoObject() as IDictionary<string, object>;

                    marketingSenior.Add("Id", senior.Id.JsonElement());

                    foreach (var property in typeof(SeniorMarketing).GetProperties())
                    {
                        if (propertyValues.TryGetValue(property.Name, out var value))
                        {
                            marketingSenior.Add(property.Name, value);
                            continue;
                        }

                        if (property.Name is nameof(SeniorMarketing.Midlevels))
                        {
                            marketingSenior.Add(property.Name, marketingMidlevels);
                        }
                    }

                    marketingSeniors.Add((dynamic)marketingSenior);

                    foreach (var midlevel in senior.Midlevels)
                    {
                        var marketingJuniors = new List<ExpandoObject>();

                        var marketingMidlevel = new ExpandoObject() as IDictionary<string, object>;

                        marketingMidlevel.Add("Id", midlevel.Id.JsonElement());

                        foreach (var property in typeof(MidlevelMarketing).GetProperties())
                        {
                            if (propertyValues.TryGetValue(property.Name, out var value))
                            {
                                marketingMidlevel.Add(property.Name, value);
                                continue;
                            }

                            if (property.Name is nameof(MidlevelMarketing.Juniors))
                            {
                                marketingMidlevel.Add(property.Name, marketingJuniors);
                            }
                        }

                        marketingMidlevels.Add((dynamic)marketingMidlevel);

                        foreach (var junior in midlevel.Juniors)
                        {
                            var marketingFreshers = new List<ExpandoObject>();

                            var marketingJunior = new ExpandoObject() as IDictionary<string, object>;

                            marketingJunior.Add("Id", junior.Id.JsonElement());

                            foreach (var property in typeof(JuniorMarketing).GetProperties())
                            {
                                if (propertyValues.TryGetValue(property.Name, out var value))
                                {
                                    marketingJunior.Add(property.Name, value);
                                    continue;
                                }

                                if (property.Name is nameof(JuniorMarketing.Freshers))
                                {
                                    marketingJunior.Add(property.Name, marketingFreshers);
                                }
                            }

                            marketingJuniors.Add((dynamic)marketingJunior);

                            foreach (var fresher in junior.Freshers)
                            {
                                var marketingFresher = new ExpandoObject() as IDictionary<string, object>;

                                marketingFresher.Add("Id", fresher.Id.JsonElement());

                                foreach (var property in typeof(FresherMarketing).GetProperties())
                                {
                                    if (propertyValues.TryGetValue(property.Name, out var value))
                                    {
                                        marketingFresher.Add(property.Name, value);
                                    }
                                }

                                marketingFreshers.Add((dynamic)marketingFresher);
                            }
                        }

                        PrepareForInvalidScenario(marketingJuniors, validItems);
                    }
                }
            }

            #endregion


            executiveOfficers.Add((dynamic)executiveOfficer);
        }

        return executiveOfficers;
    }

    private static void PrepareForInvalidScenario(IDictionary<string, object> officerOrLowerLevel)
    {
        officerOrLowerLevel[nameof(ChiefUnitIdentity.Experience)] = null!;
        officerOrLowerLevel[nameof(ChiefUnitIdentity.UniqueIdentifier)] = null!;
        officerOrLowerLevel[nameof(ChiefUnitIdentity.Age)] = "XXXXXX".JsonElement();
        officerOrLowerLevel[nameof(ChiefUnitIdentity.Height)] = "XXXXXX".JsonElement();
        officerOrLowerLevel[nameof(ChiefUnitIdentity.Weight)] = "XXXXXX".JsonElement();
        officerOrLowerLevel[nameof(ChiefUnitIdentity.EyeColor)] = "Purple".JsonElement();
        officerOrLowerLevel[nameof(ChiefUnitIdentity.BirthDate)] = "XXXXXX".JsonElement();
        officerOrLowerLevel[nameof(ChiefUnitIdentity.IsFired)] = "BooleanTrueOrFalse".JsonElement();
    }

    private static void PrepareForInvalidScenario(IEnumerable<ExpandoObject> officerOrLowerLevels, int validItems)
    {
        foreach (IDictionary<string, object> officerOrLowerLevel in officerOrLowerLevels.Skip(validItems))
        {
            officerOrLowerLevel[nameof(ChiefUnitIdentity.Experience)] = null!;
            officerOrLowerLevel[nameof(ChiefUnitIdentity.UniqueIdentifier)] = null!;
            officerOrLowerLevel[nameof(ChiefUnitIdentity.Age)] = "XXXXXX".JsonElement();
            officerOrLowerLevel[nameof(ChiefUnitIdentity.Name)] = "XXXXXX".JsonElement();
            officerOrLowerLevel[nameof(ChiefUnitIdentity.Height)] = "XXXXXX".JsonElement();
            officerOrLowerLevel[nameof(ChiefUnitIdentity.Weight)] = "XXXXXX".JsonElement();
            officerOrLowerLevel[nameof(ChiefUnitIdentity.EyeColor)] = "Purple".JsonElement();
            officerOrLowerLevel[nameof(ChiefUnitIdentity.BirthDate)] = "XXXXXX".JsonElement();
            officerOrLowerLevel[nameof(ChiefUnitIdentity.IsFired)] = "BooleanTrueOrFalse".JsonElement();
        }
    }
}
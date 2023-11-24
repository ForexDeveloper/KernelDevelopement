using Foodzilla.Kernel.UnitTest.Domain;
using Foodzilla.Kernel.UnitTest.Domain.ChiefOfficers;
using Foodzilla.Kernel.UnitTest.Domain.Freshers;
using Foodzilla.Kernel.UnitTest.Domain.Juniors;
using Foodzilla.Kernel.UnitTest.Domain.MidLevels;
using Foodzilla.Kernel.UnitTest.Domain.Seniors;
using Foodzilla.Kernel.UnitTest.Domain.TeamLeads;

namespace Foodzilla.Kernel.UnitTest;

public static class Engine
{
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

    private static readonly DateTimeOffset ModifiedDate = DateTimeOffset.Now;

    private static string[] Organizations => new[] { "Google", "Microsoft", "Apple", "SpaceX", "Amazon", "Facebook" };

    private static string[] OfficerNames => new[] { "Christopher", "David", "John", "Steve", "Leonardo", "Rafael" };

    private static string[] OfficerLastNames => new[] { "Stones", "Hamilton", "Rabin", "Anderson", "Kane", "Smith" };

    private static string[] OtherLevelNames => new[] { "Micheal", "Tom", "Victor", "Williams", "Sam", "Harry" };

    private static string[] OtherLevelLastNames => new[] { "Jordan", "Kennedy", "Graham", "Jenkins", "Garret", "Bradley" };

    private static string[] NationalCodes => new[] { "4120583732", "9182736455", "1324354657", "0897867564", "1425364758", "0896857463" };

    private static string[] PersonalCodes => new[] { "#MdEjdSjE$kEfKxD#", "#MdRkXkEk&jDjExS#", "#MdsSdkEk&jDjExS#", "#PoLefDkEk&jDjExS#", "#OldElXkEk&jDjExS#", "#QrEtyUeIkEk&jDjEx#" };

    private static string[] Addresses => new[] { "#MdEjdSjE$kEfKxD#", "#MdRkXkEk&jDjExS#", "#MdsSdkEk&jDjExS#", "#PoLefDkEk&jDjExS#", "#OldElXkEk&jDjExS#", "#QrEtyUeIkEk&jDjEx#" };

    private static int[] Ages => new[] { 25, 30, 35, 45, 55, 65 };

    private static decimal?[] Weights => new decimal?[] { 80, null, 78.5M, null, 85, null };

    private static decimal?[] Heights => new decimal?[] { 187.56M, null, 178.5M, null, 196, null };

    private static int?[] DaysOfVacation => new int?[] { 2, null, 3, null, 4, null };

    private static EyeColor?[] EyeColors => new EyeColor?[] { EyeColor.Black, null, EyeColor.Blue, null, EyeColor.Green, null };

    private static Graduation[] Graduations => new[] { Graduation.Diploma, Graduation.Bachelor, Graduation.Associate, Graduation.Master, Graduation.Phd, Graduation.Bachelor };

    private static Experience[] Experiences => new[] { Experience.Elementary, Experience.Intermediate, Experience.Advance, Experience.Elementary, Experience.Intermediate, Experience.Advance };

    private static DateTimeOffset[] BirthDates => new[] { BirthDate, BirthDate10, BirthDate20, BirthDate30, BirthDate40, BirthDate50 };

    private static DateTimeOffset?[] ModifiedDates => new DateTimeOffset?[] { ModifiedDate, ModifiedDate.AddDays(1), null, ModifiedDate.AddDays(-10), null, null };

    private static DateTimeOffset[] ContractDatesStart => new[] { ContractDateStart1, ContractDateStart2, ContractDateStart3, ContractDateStart4, ContractDateStart5, ContractDateStart6 };

    private static DateTimeOffset[] ContractDatesEnd => new[] { ContractDateEnd1, ContractDateEnd2, ContractDateEnd3, ContractDateEnd4, ContractDateEnd5, ContractDateEnd6 };

    private static Guid[] UniqueIdentifiers => new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

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
            var personalCodes = PersonalCodes[index];
            var daysOfVacation = DaysOfVacation[index];
            var otherLevelName = OtherLevelNames[index];
            var officerLastName = OfficerLastNames[index];
            var contractDateEnd = ContractDatesEnd[index];
            var uniqueIdentifier = UniqueIdentifiers[index];
            var contractDateStart = ContractDatesStart[index];
            var otherLevelLastName = OtherLevelLastNames[index];

            var chiefExecutiveOfficer = ChiefExecutiveOfficer.Create(officerId, officerName, officerLastName, nationalCode, personalCodes, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, organization);

            var chiefTechnicalOfficer = ChiefTechnicalOfficer.Create(officerId, officerName, officerLastName, nationalCode, personalCodes, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, organization);

            var chiefProductOfficer = ChiefProductOfficer.Create(officerId, officerName, officerLastName, nationalCode, personalCodes, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, organization);

            var chiefMarketingOfficer = ChiefMarketingOfficer.Create(officerId, officerName, officerLastName, nationalCode, personalCodes, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, organization);

            chiefExecutiveOfficers.Add(chiefExecutiveOfficer);
            chiefExecutiveOfficer.AddChiefProductOfficer(chiefProductOfficer);
            chiefExecutiveOfficer.AddChiefMarketingOfficer(chiefMarketingOfficer);
            chiefExecutiveOfficer.AddChiefTechnicalOfficer(chiefTechnicalOfficer);

            for (int teamLeadId = 1; teamLeadId <= count; teamLeadId++)
            {
                var technicalTeamLead = TechnicalTeamLead.Create(teamLeadId, otherLevelName, otherLevelLastName, nationalCode, personalCodes, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                    graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, chiefTechnicalOfficer.Id);

                var qATestingTeamLead = QaTestingTeamLead.Create(teamLeadId, otherLevelName, otherLevelLastName, nationalCode, personalCodes, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                    graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, chiefTechnicalOfficer.Id);

                chiefTechnicalOfficer.AddTechnicalLead(technicalTeamLead);
                chiefTechnicalOfficer.AddLeadQaTesting(qATestingTeamLead);

                var productTeamLead = ProductTeamLead.Create(teamLeadId, otherLevelName, otherLevelLastName, nationalCode, personalCodes, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                    graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, chiefProductOfficer.Id);

                var scrumMasterTeamLead = ScrumMasterTeamLead.Create(teamLeadId, otherLevelName, otherLevelLastName, nationalCode, personalCodes, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                    graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, chiefProductOfficer.Id);

                chiefProductOfficer.AddLeadProductManager(productTeamLead);
                chiefProductOfficer.AddLeadScrumMaster(scrumMasterTeamLead);

                var marketingTeamLead = MarketingTeamLead.Create(teamLeadId, otherLevelName, otherLevelLastName, nationalCode, personalCodes, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                    graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, chiefMarketingOfficer.Id);

                chiefMarketingOfficer.AddLeadMarketing(marketingTeamLead);

                for (int seniorId = 1; seniorId <= count; seniorId++)
                {
                    var seniorDeveloper = SeniorDeveloper.Create(seniorId, otherLevelName, otherLevelLastName, nationalCode, personalCodes, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                        graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, technicalTeamLead.Id);
                    technicalTeamLead.AddSeniorTechnical(seniorDeveloper);

                    var seniorQaTesting = SeniorQaTesting.Create(seniorId, otherLevelName, otherLevelLastName, nationalCode, personalCodes, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                        graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, qATestingTeamLead.Id);
                    qATestingTeamLead.AddSeniorQaTesting(seniorQaTesting);

                    var seniorProductManager = SeniorProductManager.Create(seniorId, otherLevelName, otherLevelLastName, nationalCode, personalCodes, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                        graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, productTeamLead.Id);
                    productTeamLead.AddSeniorProductManger(seniorProductManager);

                    var seniorScrumMaster = SeniorScrumMaster.Create(seniorId, otherLevelName, otherLevelLastName, nationalCode, personalCodes, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                        graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, scrumMasterTeamLead.Id);
                    scrumMasterTeamLead.AddSeniorScrumMaster(seniorScrumMaster);

                    var seniorMarketing = SeniorMarketing.Create(seniorId, otherLevelName, otherLevelLastName, nationalCode, personalCodes, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                        graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, marketingTeamLead.Id);
                    marketingTeamLead.AddSeniorMarketing(seniorMarketing);

                    for (int midlevelId = 1; midlevelId <= count; midlevelId++)
                    {
                        var midlevelDeveloper = MidlevelDeveloper.Create(midlevelId, otherLevelName, otherLevelLastName, nationalCode, personalCodes, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                            graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, seniorDeveloper.Id);
                        seniorDeveloper.AddMidlevelTechnical(midlevelDeveloper);

                        var midlevelProductManager = MidlevelProductManager.Create(midlevelId, otherLevelName, otherLevelLastName, nationalCode, personalCodes, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                            graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, seniorProductManager.Id);
                        seniorProductManager.AddMidlevelProductManager(midlevelProductManager);

                        var midlevelMarketing = MidlevelMarketing.Create(midlevelId, otherLevelName, otherLevelLastName, nationalCode, personalCodes, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                            graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, seniorMarketing.Id);
                        seniorMarketing.AddMidlevelMarketing(midlevelMarketing);

                        for (int juniorId = 1; juniorId <= count; juniorId++)
                        {
                            var juniorDeveloper = JuniorDeveloper.Create(juniorId, otherLevelName, otherLevelLastName, nationalCode, personalCodes, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                                graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, midlevelDeveloper.Id);
                            midlevelDeveloper.AddJuniorTechnical(juniorDeveloper);

                            var juniorProductManager = JuniorProductManager.Create(juniorId, otherLevelName, otherLevelLastName, nationalCode, personalCodes, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                                graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, midlevelProductManager.Id);
                            midlevelProductManager.AddJuniorProductManager(juniorProductManager);

                            var juniorMarketing = JuniorMarketing.Create(juniorId, otherLevelName, otherLevelLastName, nationalCode, personalCodes, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                                graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, midlevelMarketing.Id);
                            midlevelMarketing.AddJuniorMarketing(juniorMarketing);

                            for (int fresherId = 1; fresherId <= count; fresherId++)
                            {
                                var fresherDeveloper = FresherDeveloper.Create(juniorId, otherLevelName, otherLevelLastName, nationalCode, personalCodes, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                                    graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, juniorDeveloper.Id);
                                juniorDeveloper.AddFresherTechnical(fresherDeveloper);

                                var fresherProductManager = FresherProductManager.Create(juniorId, otherLevelName, otherLevelLastName, nationalCode, personalCodes, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
                                    graduation, experience, modifiedDate, birthDate, contractDateEnd, contractDateStart, juniorProductManager.Id);
                                juniorProductManager.AddFresherProductManager(fresherProductManager);

                                var fresherMarketing = FresherMarketing.Create(juniorId, otherLevelName, otherLevelLastName, nationalCode, personalCodes, address, age, daysOfVacation, height, weight, false, uniqueIdentifier, eyeColor,
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
}
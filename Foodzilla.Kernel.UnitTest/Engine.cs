using Foodzilla.Kernel.UnitTest.Domain.ChiefOfficers;

namespace Foodzilla.Kernel.UnitTest;

public static class Engine
{
    private static string[] Organizations => new[] { "Google", "Microsoft", "Apple", "SpaceX", "Amazon", "Facebook" };
    private static string[] OfficerNames => new[] { "Christopher", "David", "John", "Steve", "Leonardo", "Rafael" };
    private static string[] OfficerLastNames => new[] { "Stones", "Green", "Rabin", "Silva", "Kane", "Smith" };
    private static string[] NationalCodes => new[] { "4120583732", "9182736455", "1324354657", "0897867564", "1425364758", "0896857463" };
    private static string[] PersonalCodes => new[] { "#MdEjdSjE$kEfKxD#", "#MdRkXkEk&jDjExS#", "#MdsSdkEk&jDjExS#", "#PoLefDkEk&jDjExS#", "#OldElXkEk&jDjExS#", "#QrEtyUeIkEk&jDjEx#" };
    private static string[] Addresses => new[] { "#MdEjdSjE$kEfKxD#", "#MdRkXkEk&jDjExS#", "#MdsSdkEk&jDjExS#", "#PoLefDkEk&jDjExS#", "#OldElXkEk&jDjExS#", "#QrEtyUeIkEk&jDjEx#" };
    private static int[] Ages => new[] { 25, 30, 35, 45, 55, 65 };
    private static int?[] daysOfVacation => new int?[] { 2, null, 3, null, 4, null };
    private static Guid[] uniqueIdentifiers => new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

    public static async Task CreateChiefExecutiveOfficers(int count)
    {
        for (var id = 0; id < count; id++)
        {

        }
    }
}
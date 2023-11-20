using Microsoft.EntityFrameworkCore;
using Foodzilla.Kernel.UnitTest.Patch;

namespace Foodzilla.Kernel.UnitTest.Persistence;

public sealed class ApplicationDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }

    public DbSet<Customer> Customers { get; set; }
}
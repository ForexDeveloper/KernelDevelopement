using Foodzilla.Kernel.Domain;
using Microsoft.EntityFrameworkCore;

namespace Foodzilla.Kernel.Patch;

public sealed class PatchDbContext<TContext, TEntity> where TContext : DbContext where TEntity : Entity, IPatchValidator
{
}
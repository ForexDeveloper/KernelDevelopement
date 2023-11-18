using Foodzilla.Kernel.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Foodzilla.Kernel.Persistence.EF.Configurations;

public abstract class EntityConfiguration<TEntity> : DbSchemaConfiguration<TEntity>, IEntityTypeConfiguration<TEntity> where TEntity : Entity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        const string idPropertyString = nameof(Entity<long>.Id);
        builder.ToTable(SingularTableName);
        builder.HasKey(idPropertyString);
    }
}
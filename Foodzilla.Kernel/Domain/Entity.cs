namespace Foodzilla.Kernel.Domain;

using MediatR;

public abstract class Entity : IEntity
{
    public virtual Entity Clone()
    {
        return (Entity)MemberwiseClone();
    }
}

public abstract class Entity<TKey> : Entity where TKey : struct
{
    public TKey Id { get; set; }

    private List<INotification> _domainEvents;

    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

    public bool IsDeleted { get; protected set; } = false;

    public DateTimeOffset? ModifiedAt { get; protected set; }

    public DateTimeOffset CreatedAt { get; protected set; }

    public override Entity Clone()
    {
        return MemberwiseClone() as Entity;
    }

    protected Entity()
    {

    }

    protected Entity(TKey id)
    {
        Id = id;
    }

    protected void SetIdentity(TKey id)
    {
        Id = id;
    }
}
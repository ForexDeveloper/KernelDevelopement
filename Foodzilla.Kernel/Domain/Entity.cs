﻿namespace Foodzilla.Kernel.Domain;

using MediatR;

public abstract class Entity
{
    public virtual Entity Clone()
    {
        return (Entity)MemberwiseClone();
    }
}

public abstract class Entity<TKey> : Entity where TKey : struct
{
    public virtual TKey Id { get; protected set; }

    public bool IsDeleted { get; protected set; } = false;

    public DateTimeOffset? ModifiedAt { get; protected set; }

    public DateTimeOffset CreatedAt { get; protected set; }


    private readonly List<INotification> _domainEvents;

    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

    public override Entity Clone()
    {
        return MemberwiseClone() as Entity;
    }

    protected Entity()
    {
    }

    protected void SetIdentity(TKey id)
    {
        Id = id;
    }
}
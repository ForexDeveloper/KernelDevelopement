namespace Foodzilla.Kernel.Domain;

public interface IEntity<TKey> where TKey : struct
{
    public TKey Id { get; protected set; }
}
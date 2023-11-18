namespace Foodzilla.Kernel.Domain;

public interface ITrackableEntity
{
    int Version { get; }

    void UpdateVersion();

    void InitializeVersion();
}
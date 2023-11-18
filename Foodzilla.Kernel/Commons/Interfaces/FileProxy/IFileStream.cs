namespace Foodzilla.Kernel.Commons.Interfaces.FileProxy;

public interface IFileStream
{
    string Name { get; }

    string FileName { get; }

    string ContentType { get; }

    long Length { get; }

    Task CopyToAsync(Stream target, CancellationToken cancellationToken = default);

    Stream OpenReadStream();
}
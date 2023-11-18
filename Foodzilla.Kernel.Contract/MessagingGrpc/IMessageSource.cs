namespace Foodzilla.Kernel.Contract.MessagingGrpc;

public interface IMessageSource
{
    Task SendAsync();
}
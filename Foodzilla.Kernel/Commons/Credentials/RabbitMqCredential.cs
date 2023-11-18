namespace Foodzilla.Kernel.Commons.Credentials;

public sealed class RabbitMqCredential
{
    public int? RetryCount { get; set; }

    public string EventBusHostname { get; set; }

    public string EventBusUsername { get; set; }

    public string EventBusPassword { get; set; }

    public RabbitMqCredential()
    {

    }

    private RabbitMqCredential(int? retryCount, string eventBusHostname, string eventBusUsername, string eventBusPassword)
    {
        RetryCount = retryCount;
        EventBusHostname = eventBusHostname;
        EventBusUsername = eventBusUsername;
        EventBusPassword = eventBusPassword;
    }

    public static RabbitMqCredential Create(int? retryCount, string eventBusHostname, string eventBusUsername, string eventBusPassword)
    {
        return new RabbitMqCredential(retryCount, eventBusHostname, eventBusUsername, eventBusPassword);
    }
}
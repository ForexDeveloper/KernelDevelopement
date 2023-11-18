using Polly;
using RabbitMQ.Client;
using System.Net.Sockets;
using Foodzilla.Kernel.Commons.Credentials;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using Microsoft.Extensions.Logging;
using Foodzilla.Kernel.Commons.Interfaces.Bus;

namespace Foodzilla.Kernel.Persistence.RabbitMQ.Bus;

public sealed class RabbitMqPersistentConnection : IPersistentConnection<IModel>
{
    private bool _disposed;
    private IConnection _connection;
    private readonly int _retryCount;
    private readonly object sync_root = new object();
    private readonly IConnectionFactory _connectionFactory;
    private readonly ILogger<RabbitMqPersistentConnection> _logger;

    public RabbitMqPersistentConnection(RabbitMqCredential configuration, ILogger<RabbitMqPersistentConnection> logger)
    {
        _connectionFactory = CreateFactory(configuration);
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _retryCount = configuration.RetryCount ?? 5;
    }

    public bool IsConnected
    {
        get
        {
            return _connection != null && _connection.IsOpen && !_disposed;
        }
    }

    public IModel CreateModel()
    {
        if (!IsConnected)
        {
            throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
        }

        return _connection.CreateModel();
    }

    public bool TryConnect()
    {
        _logger.LogInformation("RabbitMQ Client is trying to connect");

        lock (sync_root)
        {
            var policy = Policy.Handle<SocketException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.LogWarning(ex.ToString());
                }
            );

            policy.Execute(() =>
            {
                _connection = _connectionFactory
                      .CreateConnection();
            });

            if (IsConnected)
            {
                _connection.ConnectionShutdown += OnConnectionShutdown;
                _connection.CallbackException += OnCallbackException;
                _connection.ConnectionBlocked += OnConnectionBlocked;

                _logger.LogInformation($"RabbitMQ persistent connection acquired a connection {_connection.Endpoint.HostName} and is subscribed to failure events");

                return true;
            }
            else
            {
                _logger.LogCritical("FATAL ERROR: RabbitMQ connections could not be created and opened");

                return false;
            }
        }
    }

    public void Dispose()
    {
        if (_disposed) return;

        _disposed = true;

        try
        {
            _connection.Dispose();
        }
        catch (IOException ex)
        {
            _logger.LogCritical(ex.ToString());
        }
    }

    private static IConnectionFactory CreateFactory(RabbitMqCredential configuration)
    {
        var factory = new ConnectionFactory()
        {
            HostName = configuration.EventBusHostname,
        };

        if (!string.IsNullOrEmpty(configuration.EventBusUsername))
        {
            factory.UserName = configuration.EventBusUsername;
        }

        if (!string.IsNullOrEmpty(configuration.EventBusPassword))
        {
            factory.Password = configuration.EventBusPassword;
        }

        return factory;
    }

    private void OnCallbackException(object sender, CallbackExceptionEventArgs e)
    {
        if (_disposed) return;

        _logger.LogWarning("A RabbitMQ connection throw exception. Trying to re-connect...");

        TryConnect();
    }

    private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
    {
        if (_disposed) return;

        _logger.LogWarning("A RabbitMQ connection is shutdown. Trying to re-connect...");

        TryConnect();
    }

    private void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
    {
        if (_disposed) return;

        _logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");

        TryConnect();
    }
}
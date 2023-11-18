﻿using Polly;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Reflection;
using System.Net.Sockets;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Foodzilla.Kernel.Commons.Interfaces.Bus;

namespace Foodzilla.Kernel.Persistence.RabbitMQ.Bus;

public class RabbitMqEventBus : IEventBus, IDisposable
{
    private readonly string QUEUE_NAME = "OFOOD_EVENT";
    private readonly string BROKER_NAME = "OFOOD_EVENT";


    private bool _disposed = false; // To detect redundant calls
    private readonly IEventDispatcher _eventDispatcher;
    private readonly IPersistentConnection<IModel> _persistentConnection;
    private readonly ILogger<RabbitMqEventBus> _logger;
    private static readonly Dictionary<string, Type> _subsManager = new();
    private readonly int _retryCount;


    private IModel _consumerChannel;

    public RabbitMqEventBus(IEventDispatcher eventDispatcher,
        IPersistentConnection<IModel> persistentConnection,
        ILogger<RabbitMqEventBus> logger,
        int retryCount = 5)
    {
        _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
        _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _retryCount = retryCount;
        _consumerChannel = CreateConsumerChannel();

        JsonConvert.DefaultSettings = () => new JsonSerializerSettings
        {
            ContractResolver = new NonPublicPropertiesResolver()
        };
    }

    public void Publish(IEvent @event)
    {
        if (!_persistentConnection.IsConnected)
        {
            _persistentConnection.TryConnect();
        }

        var policy = Policy.Handle<BrokerUnreachableException>()
            .Or<SocketException>()
            .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
            {
                _logger.LogWarning("Retrying message because of error {0}", ex.ToString());
            });

        using var channel = _persistentConnection.CreateModel();
        var eventName = @event.GetType().Name;

        channel.ExchangeDeclare(exchange: BROKER_NAME,
                            type: "direct");

        var message = JsonConvert.SerializeObject(@event);
        var body = Encoding.UTF8.GetBytes(message);

        policy.Execute(() =>
        {
            var properties = channel.CreateBasicProperties();
            properties.DeliveryMode = 2; // persistent

            channel.BasicPublish(exchange: BROKER_NAME,
                         routingKey: eventName,
                         mandatory: true,
                         basicProperties: properties,
                         body: body);
        });
    }

    public class NonPublicPropertiesResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);
            if (member is PropertyInfo pi)
            {
                prop.Readable = (pi.GetMethod != null);
                prop.Writable = (pi.SetMethod != null);
            }
            return prop;
        }
    }

    public void Subscribe<T>() where T : IEvent
    {
        var eventName = typeof(T).Name;
        var containsKey = _subsManager.ContainsKey(eventName);
        if (!containsKey)
        {
            _subsManager.Add(eventName, typeof(T));
        }

        if (!_persistentConnection.IsConnected)
        {
            _persistentConnection.TryConnect();
        }

        using var channel = _persistentConnection.CreateModel();
        channel.QueueBind(queue: QUEUE_NAME,
                          exchange: BROKER_NAME,
                          routingKey: eventName);
    }

    private IModel CreateConsumerChannel()
    {
        if (!_persistentConnection.IsConnected)
        {
            _persistentConnection.TryConnect();
        }

        var channel = _persistentConnection.CreateModel();

        channel.ExchangeDeclare(exchange: BROKER_NAME,
                             type: "direct");

        channel.QueueDeclare(queue: QUEUE_NAME,
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var eventName = ea.RoutingKey;
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());

            await ProcessEvent(eventName, message);

            channel.BasicAck(ea.DeliveryTag, multiple: false);
        };

        channel.BasicConsume(queue: QUEUE_NAME,
                             autoAck: false,
                             consumer: consumer);

        channel.CallbackException += (sender, ea) =>
        {
            _consumerChannel.Dispose();
            _consumerChannel = CreateConsumerChannel();
        };

        return channel;
    }

    private async Task ProcessEvent(string eventName, string message)
    {
        if (_subsManager.ContainsKey(eventName))
        {
            var @type = _subsManager[eventName];
            var @event = JsonConvert.DeserializeObject(message, @type) as IEvent;

            await _eventDispatcher.Dispatch(@event);
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                if (_consumerChannel != null)
                {
                    _consumerChannel.Dispose();
                }

                _subsManager.Clear();
            }

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }
}
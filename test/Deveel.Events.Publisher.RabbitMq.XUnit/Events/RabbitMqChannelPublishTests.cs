using CloudNative.CloudEvents;
using CloudNative.CloudEvents.SystemTextJson;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using System.Text;
using System.Text.Json;

using Xunit.Abstractions;

namespace Deveel.Events
{
    public class RabbitMqChannelPublishTests : IClassFixture<RabbitMqTestServer>, IAsyncLifetime
    {
        private readonly IModel _channel;

        public RabbitMqChannelPublishTests(RabbitMqTestServer testServer, ITestOutputHelper outputHelper)
        {
            var services = new ServiceCollection();
            services.AddEventPublisher()
                .UseRabbitMq(options => options.ConnectionString = testServer.ConnectionString);

            services.AddLogging(logging => logging.AddXUnit(outputHelper).SetMinimumLevel(LogLevel.Trace));

            var provider = services.BuildServiceProvider();

            Publisher = provider.GetRequiredService<EventPublisher>();

            var connection = provider.GetRequiredService<IConnection>();
            _channel = connection.CreateModel();
        }

        private EventPublisher Publisher { get; set; }

        private CloudEvent? ReceivedEvent { get; set; }

        public Task InitializeAsync()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, args) =>
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var json = JsonSerializer.Deserialize<JsonElement>(message);
                var formatter = new JsonEventFormatter();
                ReceivedEvent = formatter.ConvertFromJsonElement(json, null);
            };

            _channel.ExchangeDeclare("test", ExchangeType.Topic);
            _channel.QueueDeclare("test.queue1", true, false, false, null);
            _channel.QueueBind("test.queue1", "test", "test.event1");
            _channel.BasicConsume("test.queue1", true, consumer);

            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            _channel.Dispose();
            return Task.CompletedTask;
        }

        [Fact]
        public async Task PublishEventToRabbitMq()
        {
            var cloudEvent = new CloudNative.CloudEvents.CloudEvent
            {
                Subject = "test",
                DataContentType = "application/binary",
                Data = Encoding.UTF8.GetBytes(Convert.ToBase64String(Encoding.UTF8.GetBytes("Hello, World!"))),
                Source = new Uri("https://api.svc.deveel.com/test-service"),
                Type = "test.created",
                Time = DateTime.UtcNow,
                Id = Guid.NewGuid().ToString("N"),
                DataSchema = new Uri("http://example.com/schema/1.0"),
            };

            cloudEvent["amqproutingkey"] = "test.event1";
            cloudEvent["amqpexchange"] = "test";

            await Publisher.PublishEventAsync(cloudEvent);

            await Task.Delay(200);

            Assert.NotNull(ReceivedEvent);
            Assert.Equal(cloudEvent.Id, ReceivedEvent.Id);
            Assert.Equal(cloudEvent.Type, ReceivedEvent.Type);
            Assert.Equal(cloudEvent.Source, ReceivedEvent.Source);
            Assert.Equal(cloudEvent.Subject, ReceivedEvent.Subject);
        }
    }
}

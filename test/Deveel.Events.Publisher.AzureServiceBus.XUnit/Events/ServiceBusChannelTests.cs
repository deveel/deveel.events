using System.Text;

using Azure.Messaging.ServiceBus;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Xunit.Abstractions;

namespace Deveel.Events {
	public class ServiceBusChannelTests {
		public ServiceBusChannelTests(ITestOutputHelper outputHelper) {
			var services = new ServiceCollection();
			services.AddLogging(builder => builder.AddXUnit(outputHelper).SetMinimumLevel(LogLevel.Debug));

			services.AddSingleton<IServiceBusClientFactory>(new TestServiceBusClientFactory(OnMessageSent));

			services.AddEventPublisher(options => {
				options.Source = "test";
			})
			.AddServiceBusChannel(options => {
				options.ConnectionString = "Endpoint=sb://test.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=abc12345";
				options.QueueName = "test-queue";
			});

			var provider = services.BuildServiceProvider();
			Publisher = provider.GetRequiredService<EventPublisher>();
		}

		private EventPublisher Publisher { get; }

		private ServiceBusMessage? MessageSent { get; set; }

		private void OnMessageSent(ServiceBusMessage message) {
			MessageSent = message;
		}

		[Fact]
		public async Task PublishEventWithBinaryData() {
			await Publisher.PublishEventAsync(new Event("test.created", "1.0") {
				EventData = new EventData(Encoding.UTF8.GetBytes("Hello, World!"))
			});

			Assert.NotNull(MessageSent);
			Assert.Equal("test", MessageSent!.Subject);
			Assert.Equal("application/octet-stream", MessageSent.ContentType);
			Assert.NotNull(MessageSent.Body);
			Assert.NotNull(MessageSent.ApplicationProperties);
			Assert.NotEmpty(MessageSent.ApplicationProperties);
			Assert.NotNull(MessageSent.ApplicationProperties[ServiceBusMessageProperties.EventType]);
			Assert.Equal("test.created", MessageSent.ApplicationProperties[ServiceBusMessageProperties.EventType]);
		}
	}
}

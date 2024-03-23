using Azure.Messaging.ServiceBus;

namespace Deveel.Events {
	public class ServiceBusEventPublishChannelOptions {
		public string ConnectionString { get; set; }

		public string QueueName { get; set; }

		public ServiceBusClientOptions ClientOptions { get; set; } = new ServiceBusClientOptions();
	}
}

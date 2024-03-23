using Azure.Messaging.ServiceBus;

namespace Deveel.Events {
	class ServiceBusClientFactory : IServiceBusClientFactory {
		public ServiceBusClient CreateClient(string connectionString, ServiceBusClientOptions options)
			=> new ServiceBusClient(connectionString, options);
	}
}

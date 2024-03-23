using Azure.Messaging.ServiceBus;

namespace Deveel.Events {
	public interface IServiceBusClientFactory {
		ServiceBusClient CreateClient(string connectionString, ServiceBusClientOptions options);
	}
}

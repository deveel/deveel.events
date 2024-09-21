﻿using Azure.Messaging.ServiceBus;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Deveel.Events
{
    [Trait("Channel", "ServiceBus")]
    [Trait("Function", "Registration")]
    public static class ServiceRegistrationTests
    {
        [Fact]
        public static void AddServiceBusEventPublishChannel_WasSuccessful()
        {
            var services = new ServiceCollection();
            services.AddEventPublisher()
                .AddServiceBusChannel(options =>
                {
                    options.ConnectionString = "Endpoint=sb://my-namespace.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=abc12345";
                    options.QueueName = "my-queue";
                    options.ClientOptions.TransportType = ServiceBusTransportType.AmqpWebSockets;
                });

            var serviceProvider = services.BuildServiceProvider();

            Assert.NotNull(serviceProvider.GetService<EventPublisher>());
            Assert.NotNull(serviceProvider.GetService<IEventPublishChannel>());
            Assert.IsType<ServiceBusEventPublishChannel>(serviceProvider.GetService<IEventPublishChannel>());
            Assert.NotNull(serviceProvider.GetService<IServiceBusClientFactory>());

            var options = serviceProvider.GetService<IOptions<ServiceBusEventPublishChannelOptions>>();
            Assert.NotNull(options);
            Assert.Equal("my-queue", options.Value.QueueName);
            Assert.Equal("Endpoint=sb://my-namespace.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=abc12345", options.Value.ConnectionString);
            Assert.NotNull(options.Value.ClientOptions);
            Assert.Equal(ServiceBusTransportType.AmqpWebSockets, options.Value.ClientOptions.TransportType);
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using RabbitMQ.Client;

namespace Deveel.Events
{
    public static class EventPublisherBuilderExtensions
    {
        private static EventPublisherBuilder AddRabbitMqChannel(this EventPublisherBuilder builder)
        {
            builder.Services.AddSingleton<IEventPublishChannel, RabbitMqEventPublishChannel>();
            builder.Services.TryAddSingleton<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>();
            builder.Services.TryAddSingleton<IConnection>(sp =>
            {
                var factory = sp.GetRequiredService<IRabbitMqConnectionFactory>();
                return factory.CreateConnection();
            });

            return builder;
        }

        public static EventPublisherBuilder UseRabbitMq(this EventPublisherBuilder builder, Action<RabbitMqEventPublishChannelOptions> configure)
        {
            builder.Services.AddOptions<RabbitMqEventPublishChannelOptions>()
                .Configure(configure);

            return builder.AddRabbitMqChannel();
        }
    }
}

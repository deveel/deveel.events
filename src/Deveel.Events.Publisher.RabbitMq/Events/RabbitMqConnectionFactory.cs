using Microsoft.Extensions.Options;

using RabbitMQ.Client;

namespace Deveel.Events
{
    public class RabbitMqConnectionFactory : IRabbitMqConnectionFactory
    {
        private readonly ConnectionFactory _connectionFactory;

        public RabbitMqConnectionFactory(IOptions<RabbitMqEventPublishChannelOptions> options)
        {
            if (!Uri.TryCreate(options.Value.ConnectionString, UriKind.Absolute, out var connectionUri))
                throw new ArgumentException("The connection string is not a valid URI", nameof(options));

            _connectionFactory = new ConnectionFactory
            {
                Uri = connectionUri
            };
        }

        public IConnection CreateConnection()
        {
            return _connectionFactory.CreateConnection();
        }
    }
}

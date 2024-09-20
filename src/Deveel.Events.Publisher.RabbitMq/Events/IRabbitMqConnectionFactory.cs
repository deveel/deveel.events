using RabbitMQ.Client;

namespace Deveel.Events
{
    public interface IRabbitMqConnectionFactory
    {
        IConnection CreateConnection();
    }
}

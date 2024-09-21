
using Testcontainers.RabbitMq;

namespace Deveel.Events
{
    public class RabbitMqTestServer : IAsyncLifetime
    {
        private readonly RabbitMqContainer rabbitMq;

        public RabbitMqTestServer()
        {
            rabbitMq = new RabbitMqBuilder()
                .Build();
        }

        public string ConnectionString => rabbitMq.GetConnectionString();


        public async Task DisposeAsync()
        {
            await rabbitMq.StopAsync();
            await rabbitMq.DisposeAsync();
        }

        public async Task InitializeAsync()
        {
            await rabbitMq.StartAsync();
        }
    }
}


using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace RabbittMQCliente.Customers.API.Bus
{
    public class BusService : IBusService
    {
        private readonly IModel _channel;
        const string EXCHANGE = "exchange";
        public BusService()
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = "localhost",
            };
            var connection = connectionFactory.CreateConnection();
            _channel = connection.CreateModel();
        }
        public async Task Publish<T>(string rountingKey, T message)
        {
            var serialize = JsonSerializer.Serialize(message);
            var bytes = Encoding.UTF8.GetBytes(serialize);

            _channel.BasicPublish(EXCHANGE, rountingKey, null, bytes);
        }
    }
}

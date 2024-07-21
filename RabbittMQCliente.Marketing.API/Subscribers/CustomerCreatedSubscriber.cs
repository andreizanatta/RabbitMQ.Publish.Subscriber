
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace RabbittMQCliente.Marketing.API.Subscribers
{
    public class CustomerCreatedSubscriber : IHostedService
    {
        const string EXCHANGE = "exchange";
        const string QUEUE = "queue";
        private readonly IModel _channel;

        public CustomerCreatedSubscriber()
        {
            var connectionFactory = new ConnectionFactory { HostName =  "localhost" };
            var connection = connectionFactory.CreateConnection();
            _channel = connection.CreateModel();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, eventArgs) =>
            {
                var message = eventArgs.Body.ToArray();
                var getstring = Encoding.UTF8.GetString(message);
                Console.WriteLine(getstring);
                var @event = JsonSerializer.Deserialize<CustomerCreatedSubscriber>(getstring);

                _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            _channel.BasicConsume(QUEUE, false, consumer);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

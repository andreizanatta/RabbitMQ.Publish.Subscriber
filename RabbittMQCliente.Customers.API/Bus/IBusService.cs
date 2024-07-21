namespace RabbittMQCliente.Customers.API.Bus
{
    public interface IBusService
    {
        Task Publish<T>(string rountingKey, T message);
    }
}

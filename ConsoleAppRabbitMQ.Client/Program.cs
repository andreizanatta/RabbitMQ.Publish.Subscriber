// See https://aka.ms/new-console-template for more information

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

const string EXCHANGE = "exchange";

var person = new Person("Andrei", "999999999");

var connectionFactory = new ConnectionFactory
{
    HostName = "localhost",
};

var connection = connectionFactory.CreateConnection();

var channel = connection.CreateModel();

var personSerialize = JsonSerializer.Serialize<Person>(person);
var personBytes = Encoding.UTF8.GetBytes(personSerialize);

//channel.BasicPublish(EXCHANGE, "building", null, personBytes);
//
//Console.WriteLine($"json: {personSerialize}");

var consumerChannel = connection.CreateModel();

var consumer = new EventingBasicConsumer(consumerChannel);

consumer.Received += (sender, args) =>
{
    var arrayBytes = args.Body.ToArray();
    var arrayString = Encoding.UTF8.GetString(arrayBytes);
    var serialize = JsonSerializer.Deserialize<Person>(arrayString);
    Console.WriteLine(arrayString);

    consumerChannel.BasicAck(args.DeliveryTag, false);
};

consumerChannel.BasicConsume("queue", false, consumer);
Console.ReadLine();

public class Person
{
    public Person(string name, string document)
    {
        Name = name;
        Document = document;
    }

    public string Name { get; set; }
    public string Document { get; set; }
}
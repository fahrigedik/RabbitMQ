// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Text;

Console.WriteLine("Hello, Topic Exchange!");

ConnectionFactory factory = new()
{
    HostName = "localhost",
    Port = 5672,
    UserName = "guest",
    Password = "guest"
};
Console.WriteLine("Connection is created");

await using IConnection connection = await factory.CreateConnectionAsync();
Console.WriteLine("Connection is opened");

await using IChannel channel = await connection.CreateChannelAsync();
Console.WriteLine("Channel is opened");

await channel.ExchangeDeclareAsync("topic-exchange", ExchangeType.Topic);
Console.WriteLine("Exchange is declared");

while (true)
{
    Console.WriteLine("Mesaj Giriniz : ");
    string message = Console.ReadLine();
    byte[] messageBytes = Encoding.UTF8.GetBytes(message);

    Console.WriteLine("Routing Key (Topic) Giriniz : ");
    string topic = Console.ReadLine();

    await channel.BasicPublishAsync("topic-exchange", routingKey:topic, body: messageBytes);
}
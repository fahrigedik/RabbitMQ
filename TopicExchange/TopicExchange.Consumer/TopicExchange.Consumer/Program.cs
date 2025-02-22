// See https://aka.ms/new-console-template for more information

using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

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

string queueName = (await channel.QueueDeclareAsync()).QueueName;
Console.WriteLine("Queue is declared");

await channel.QueueBindAsync(queue: queueName, exchange: "topic-exchange", routingKey: "topic.*");
Console.WriteLine("Queue is binded");

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += async (sender, eventArgs) =>
{
    var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
    Console.WriteLine($"Received Message: {message}");
    Console.WriteLine($"Routing Key: {eventArgs.RoutingKey}");
    await Task.CompletedTask;

};

await channel.BasicConsumeAsync(
    queue: queueName,
    autoAck: true, // Mesajlar otomatik acknowledge edilsin
    consumer: consumer
);

Console.WriteLine("Mesajları dinlemeye başladı. Çıkmak için bir tuşa basın...");
Console.ReadLine();




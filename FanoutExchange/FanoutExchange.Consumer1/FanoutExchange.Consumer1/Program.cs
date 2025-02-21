// See https://aka.ms/new-console-template for more information

using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.WriteLine("Hello, Fanout Exchange.");

ConnectionFactory factory = new()
{
    HostName = "localhost",
    Port = 5672,
    UserName = "guest",
    Password = "guest"
};
Console.WriteLine("Connection is created");

await using IConnection connection = await factory.CreateConnectionAsync();
Console.WriteLine("Connection is enabled");

await using IChannel channel = await connection.CreateChannelAsync();
Console.WriteLine("Channel is created");

await channel.ExchangeDeclareAsync("fanout-exchange-example", ExchangeType.Fanout);

await channel.QueueDeclareAsync("fanout-queue-example2");
await channel.QueueBindAsync(
    "fanout-queue-example2",
    "fanout-exchange-example", "");

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += async (sender, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Message received: {message}");
    await Task.CompletedTask;
};


await channel.BasicConsumeAsync(
    queue: "fanout-queue-example2",
    autoAck: true, // Mesajlar otomatik acknowledge edilsin
    consumer: consumer
);
Console.WriteLine("Started consuming messages...");
Console.ReadLine(); // Uygulamanın kapanmaması için bekle

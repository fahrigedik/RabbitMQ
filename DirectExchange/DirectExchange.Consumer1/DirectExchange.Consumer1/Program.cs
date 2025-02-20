// See https://aka.ms/new-console-template for more information

using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.WriteLine("Hello, Direct Exchange Type!");

ConnectionFactory factory = new()
{
    HostName = "localhost",
    Port = 5672,
    UserName = "guest",
    Password = "guest"
};

await using IConnection connection = await factory.CreateConnectionAsync();
Console.WriteLine("Connection is created");

await using IChannel channel = await connection.CreateChannelAsync();
Console.WriteLine("Channel is created");

await channel.ExchangeDeclareAsync("direct-exchange-example", ExchangeType.Direct);
Console.WriteLine("Exchange is declared");

string queueName = (await channel.QueueDeclareAsync()).QueueName;
Console.WriteLine($"Queue is declared: {queueName}");

await channel.QueueBindAsync(queueName, "direct-exchange-example", "direct-queue-example");

Console.WriteLine("Waiting for messages...");

var consumer = new AsyncEventingBasicConsumer(channel);

await channel.BasicConsumeAsync(queue:queueName, false, consumer:consumer);

consumer.ReceivedAsync += async (sender, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Received message: {message}");
    await channel.BasicAckAsync(eventArgs.DeliveryTag, false);
};

Console.ReadLine();
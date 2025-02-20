using System.Text;
using RabbitMQ.Client;

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

while (true)
{
    Console.WriteLine("Mesaj Giriniz :");
    string message = Console.ReadLine();
    byte[] byteMessage = Encoding.UTF8.GetBytes(message);

    await channel.BasicPublishAsync(exchange: "direct-exchange-example", routingKey: "direct-queue-example",
        body: byteMessage);
    Console.WriteLine("Message is published");  
}
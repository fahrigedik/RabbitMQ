// See https://aka.ms/new-console-template for more information

using System.Text;
using RabbitMQ.Client;


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
Console.WriteLine("Exchange is declared");

while (true)
{
    Console.WriteLine("Mesaj Giriniz : ");
    string message = Console.ReadLine();
    byte[] messageBytes = Encoding.UTF8.GetBytes(message);

    await channel.BasicPublishAsync("fanout-exchange-example", "", body: messageBytes);
}


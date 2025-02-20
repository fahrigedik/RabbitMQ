
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

Console.WriteLine("Hello RabbitMQ");

ConnectionFactory factory = new()
{
    HostName = "localhost",
    Port = 5672,
    UserName = "guest",
    Password = "guest"
};

try
{
    // create a connection
    using IConnection connection = await factory.CreateConnectionAsync();
    Console.WriteLine("connection is created");

    // create a channel
    using IChannel channel = await connection.CreateChannelAsync();
    Console.WriteLine("channel is created");

    // declare a queue
    //
    await channel.QueueDeclareAsync(queue: "example-queue", exclusive: false);


    // publish a message
    //RabbitMQ kuyruğa atacağı mesajları byte türünden kabul etmektedir. Haliyle mesajları bizim byte'a dönüştürmemiz gerekmektedir.
    byte[] messageBodyBytes = Encoding.UTF8.GetBytes("Senayı Çok Seviyoruuuuum!");

    channel.BasicPublishAsync(exchange: "", routingKey: "example-queue", body: messageBodyBytes);
    Console.WriteLine("message is published");

    Console.WriteLine("RabbitMQ Connection ve Channel'ı açık tutmak için bekleme modu aktifleştirildi.");
    Console.ReadLine();
}
catch (RabbitMQClientException ex)
{
    throw;
}


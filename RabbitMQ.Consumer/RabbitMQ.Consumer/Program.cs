// See https://aka.ms/new-console-template for more information

using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.WriteLine("Hello, RabbitMQ");

//Bağlantı oluşturma
ConnectionFactory factory = new()
{
    HostName = "localhost",
    Port = 5672,
    UserName = "guest",
    Password = "guest"
};

//Bağlantı aktifleştirme
using IConnection connection = await factory.CreateConnectionAsync();
Console.WriteLine("connection is created");

//Kanal oluşturma
using IChannel channel = await connection.CreateChannelAsync();
Console.WriteLine("channel is created");

//Kuyruk oluşturma
await channel.QueueDeclareAsync(queue: "example-queue", exclusive: false);
//Consumer'da da kuyruk publisher'daki ile birebir aynı yapılandırma da tanımlanmalıdır.



AsyncEventingBasicConsumer consumer = new(channel);
channel.BasicConsumeAsync(queue: "example-queue", false, consumer);

consumer.ReceivedAsync += async (object sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());
    Console.WriteLine($"Gelen Mesaj: {message}");
    await channel.BasicAckAsync(e.DeliveryTag, false);  
};


Console.ReadLine();


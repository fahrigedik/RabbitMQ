// See https://aka.ms/new-console-template for more information

using MassTransit;
using RabbitMQ.ESB.MassTransit.Consumer.Consumers;
using RabbitMQ.ESB.MassTransit.Shared.Messages;

Console.WriteLine("Hello, MassTransit Consumer!");

string queueName = "example-queue";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(configure =>
{
    configure.Host("localhost", "/", h =>
    {
        h.Username("guest");
        h.Password("guest");
    });
    configure.ReceiveEndpoint(queueName, endpoint =>
    {
       endpoint.Consumer<ExampleMessageConsumer>();
    });
});

await bus.StartAsync();

Console.WriteLine("Press any key to exit...");
Console.ReadLine();

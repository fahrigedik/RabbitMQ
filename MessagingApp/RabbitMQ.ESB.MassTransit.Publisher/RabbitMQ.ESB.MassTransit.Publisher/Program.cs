// See https://aka.ms/new-console-template for more information

using MassTransit;
using RabbitMQ.ESB.MassTransit.Shared.Messages;

Console.WriteLine("Hello, MassTransit Publisher!");

string queueName = "example-queue";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(configure =>
{
    configure.Host("localhost", "/", h =>
    {
        h.Username("guest");
        h.Password("guest");
    });
});

await bus.StartAsync();
var endpoint = await bus.GetSendEndpoint(new Uri($"queue:{queueName}"));
await endpoint.Send<ExampleMessage>(new ExampleMessage
{
    Text = "Hello, this is a test message!"
});

Console.WriteLine("Message sent!");
Console.ReadLine();

await bus.StopAsync();


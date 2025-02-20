// See https://aka.ms/new-console-template for more information

using RabbitMQ.Client;

Console.WriteLine("Hello, RabbitMQ");

ConnectionFactory factory = new()
{
    HostName = "localhost",
    Port = 5672,
    UserName = "guest",
    Password = "guest"
};




using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using RabbitMQ.ESB.MassTransit.Shared.Messages;

namespace RabbitMQ.ESB.MassTransit.Consumer.Consumers
{
    public class ExampleMessageConsumer : IConsumer<IMessage>
    {
        public async Task Consume(ConsumeContext<IMessage> context)
        {
            Console.WriteLine("Gelen Mesaj : " + context.Message.Text);
            await Task.CompletedTask;
        }
    }
}

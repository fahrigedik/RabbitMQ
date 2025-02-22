using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MassTransit;
using MessageApp.MassTransit.Fah.Message;

namespace MessageApp.MassTransit.Fah.Consumer
{
    public class FahConsumer : IConsumer<IMessage>
    {
        public Task Consume(ConsumeContext<IMessage> context)
        {
            Form1 form = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (form != null)
            {
                form.Invoke((MethodInvoker)delegate
                {
                    form.listBox1.Items.Add($"{context.Message.Sender} : {context.Message.MessageText} ~ {context.Message.MessageDate}");
                });
            }
            return Task.CompletedTask;

        }
    }
}

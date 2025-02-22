using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageApp.MassTransit.Fah.Message
{
    public class Message : IMessage
    {
        public string MessageText { get; set; }
        public DateTime MessageDate { get; set; } = DateTime.Now;
        public string Sender { get; set; }
    }
}

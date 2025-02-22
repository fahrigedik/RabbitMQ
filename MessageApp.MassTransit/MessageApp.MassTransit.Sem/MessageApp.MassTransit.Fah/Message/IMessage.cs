using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageApp.MassTransit.Fah.Message
{
    public interface IMessage
    {
         string MessageText { get; set; }
         DateTime MessageDate { get; set; }
         string Sender { get; set; }
    }
}

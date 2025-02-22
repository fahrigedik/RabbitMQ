using MassTransit;
using MessageApp.MassTransit.Fah.Consumer;
using MessageApp.MassTransit.Fah.Message;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MessageApp.MassTransit.Fah
{
    public partial class Form1 : Form
    {
        private IBusControl _bus;
        private string _senderQueue = "FahQueue";
        private string _receiverQueue = "SemQueue";
        public Form1()
        {
            InitializeComponent();
            InitializeMassTransit();
        }

        private async void InitializeMassTransit()
        {
            try
            {
                _bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri("amqps://goose.rmq2.cloudamqp.com/qndjqakk"), h =>
                    {
                        h.Username("qndjqakk");
                        h.Password("4WuL1fE_Sk18b_dUpA4q9ddZoc2akvGK");

                        h.UseSsl(s =>
                        {
                            s.Protocol = System.Security.Authentication.SslProtocols.Tls12;
                        });
                    });

                    cfg.ReceiveEndpoint(queueName: _receiverQueue, endpoint =>
                    {
                        endpoint.Consumer<FahConsumer>();
                    });
                });

                await _bus.StartAsync();
                button1.BackColor = Color.Green;
            }
            catch (Exception ex)
            {
                button1.BackColor = Color.Red;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_DoubleClick(object sender, EventArgs e)
        {
            string imagePath = Path.Combine(Application.StartupPath, "Images", "image.jpeg");
            BackgroundImage = Image.FromFile(imagePath);
        }

        private async void button1_Click_1(object sender, EventArgs e)
        {
            if (_bus != null)
            {
                try
                {
                    var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{_senderQueue}")); 
                    await endpoint.Send<IMessage>(new
                    {
                        MessageText = richTextBox1.Text,
                        Sender = "Fahrican",
                        DateTime = DateTime.Now
                    });
                    listBox1.Items.Add($"Ben : {richTextBox1.Text} ~ {DateTime.Now:g}");
                    richTextBox1.Text = ""; 
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Mesaj gönderme hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        protected override async void OnFormClosing(FormClosingEventArgs e)
        {
            if (_bus != null)
            {
                try
                {
                    await _bus.StopAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Bus kapatma hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            base.OnFormClosing(e);
        }

    }
}

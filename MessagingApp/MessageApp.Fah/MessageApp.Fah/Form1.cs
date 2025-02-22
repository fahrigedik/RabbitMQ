using System;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MessageApp.Fah
{
    public partial class Form1 : Form
    {
        private IConnection _connection;
        private IChannel _channel;
        private readonly string _queueName = "message_queue";
        private readonly string _recipientQueueName = "message_queue_2";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            await SetupRabbitMQ();
        }

        private async Task SetupRabbitMQ()
        {
            var factory = new ConnectionFactory()
            {
                Port = 5672,
                Uri = new Uri("amqps://rndombxw:dxtF0hfgLPmhzpcED2CQecJ8QzIMXQoE@stingray.rmq.cloudamqp.com/rndombxw"),
                Password = "dxtF0hfgLPmhzpcED2CQecJ8QzIMXQoE",
                UserName = "rndombxw"
            };

            try
            {
                _connection = await factory.CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync();

                // Her iki kuyruğu da oluştur
                await CreateQueues();
                // Mesaj dinleyiciyi ayarla
                await SetupMessageConsumer();

                UpdateConnectionStatus(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"RabbitMQ bağlantı hatası: {ex.Message}");
                UpdateConnectionStatus(false);
            }
        }

        private async Task CreateQueues()
        {
            // Kendi kuyruğumuzu oluştur
            await _channel.QueueDeclareAsync(
                queue: _queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            // Karşı tarafın kuyruğunu oluştur
            await _channel.QueueDeclareAsync(
                queue: _recipientQueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        }

        private async Task SetupMessageConsumer()
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                this.Invoke((MethodInvoker)(() =>
                {
                    AddMessageToList($"[ALINAN] {DateTime.Now:HH:mm:ss}: {message}");
                }));

                await Task.CompletedTask;
            };

            await _channel.BasicConsumeAsync(
                queue: _queueName,
                autoAck: true,
                consumer: consumer
            );
        }

        private void UpdateConnectionStatus(bool isConnected)
        {
            label1.Text = isConnected ? "Bağlantı Aktif" : "Bağlantı Hatası!";
            label1.ForeColor = isConnected ? Color.Green : Color.Red;
        }

        private void AddMessageToList(string message)
        {
            listBox1.Items.Add(message);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMessage.Text)) return;

            try
            {
                var message = txtMessage.Text;
                var body = Encoding.UTF8.GetBytes(message);

                await _channel.BasicPublishAsync(
                    exchange: "",
                    routingKey: _recipientQueueName,
                    body: body
                );

                AddMessageToList($"[GÖNDERİLEN] {DateTime.Now:HH:mm:ss}: {message}");
                txtMessage.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Mesaj gönderme hatası: {ex.Message}");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                _channel?.CloseAsync().Wait();
                _connection?.CloseAsync().Wait();
            }
            catch { /* Bağlantı kapatma hataları görmezden gelinebilir */ }

            base.OnFormClosing(e);
        }
    }
}

using System;
using System.IO;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Rabbitmq.Subscriber
{
    public class App
    {
        private  ConnectionFactory _factory;
        private  IConnection _connection;
        private  IModel _channel;
        private EventingBasicConsumer _consumer;

        public void Run()
        {
            _factory = new ConnectionFactory()
            {
                HostName = "00000",
                RequestedHeartbeat = 30,
                UserName = "berwqerweqrewrew",
                Password = "B12312312!!"
            };

            Reconnect();

            //Console.ReadLine();

            Cleanup();
        }

        public void Connect()
        {
            _connection = _factory.CreateConnection();
            _connection.ConnectionShutdown += Connection_ConnectionShutdown;

            _channel = _connection.CreateModel();
            _channel.QueueDeclare("logs", true, false, false, null);

            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += Consumer_Received;
            _channel.BasicConsume("logs", true, _consumer);
        }


        private void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("Connection broke!");

            Reconnect();
        }

        private void Reconnect()
        {
            Cleanup();

            var mres = new ManualResetEventSlim(false); // state is initially false

            while (!mres.Wait(3000)) // loop until state is true, checking every 3s
            {
                try
                {
                    Connect();

                    Console.WriteLine("Connected!");
                    mres.Set(); // state set to true - breaks out of loop
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Connect failed!");
                }
            }
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body;
            var content = Encoding.UTF8.GetString(body);
            Console.WriteLine(content);
        }


        private void Cleanup()
        {
            try
            {
                if (_channel != null && _channel.IsOpen)
                {
                    _channel.Close();
                    _channel = null;
                }

                if (_connection != null && _connection.IsOpen)
                {
                    _connection.Close();
                    _connection = null;
                }
            }
            catch (IOException ex)
            {
                // Close() may throw an IOException if connection
                // dies - but that's ok (handled by reconnect)
            }
        }
    }
}

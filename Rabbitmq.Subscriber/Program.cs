using System;
using System.IO;
using System.Text;
using System.Threading;
using Rabbitmq.Core.Impl;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Rabbitmq.Subscriber
{
    class Program
    {
        private static ConnectionFactory factory;
        private static IConnection connection;
        private static IModel channel;
        private static EventingBasicConsumer consumer;

        static void Main(string[] args)
        {
            factory = new ConnectionFactory()
            {
                HostName = "52.214.177.146",
                RequestedHeartbeat = 30,
                UserName = "boyner",
                Password = "B12312312!!"
            };

            Reconnect();

            //Console.ReadLine();

            Cleanup();
        }

        static void Connect()
        {
            connection = factory.CreateConnection();
            connection.ConnectionShutdown += Connection_ConnectionShutdown;

            channel = connection.CreateModel();
            channel.QueueDeclare("logs", true, false, false, null);

            consumer = new EventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;
            channel.BasicConsume("logs", true, consumer);
        }


        private static void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("Connection broke!");

            Reconnect();
        }

        private static void Reconnect()
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

        private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body;
            var content = Encoding.UTF8.GetString(body);
            Console.WriteLine(content);
        }


        static void Cleanup()
        {
            try
            {
                if (channel != null && channel.IsOpen)
                {
                    channel.Close();
                    channel = null;
                }

                if (connection != null && connection.IsOpen)
                {
                    connection.Close();
                    connection = null;
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

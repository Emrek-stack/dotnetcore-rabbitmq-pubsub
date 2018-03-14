using System;
using Rabbitmq.Core.Impl;
using RabbitMQ.Client;

namespace Rabbitmq.Publisher
{
    public class App
    {
        public static string HostName = "52.214.177.146";
        public static string ExchangeName = "logs";
        private static Producer _producer;

        public void ConnectRabbit()
        {
            //Declare the producer
            _producer = new Producer(HostName, ExchangeName, ExchangeType.Fanout);
            //connect to RabbitMQ
            //_producer.Connect();
            _producer.Reconnect();

            Console.ReadLine();

            _producer.Cleanup();
            //{
            //    //Show a basic error if we fail
            //    Console.WriteLine("Could not connect to Broker");
            //}

        }

        private static int _count = 0;
        private void SendMessage(string message)
        {
            string sVal = String.Format("{0} - {1}", _count++, message);
            _producer.SendMessage(sVal);
            Console.WriteLine(sVal);
        }


        public void Run()
        {
            ConnectRabbit();
            for (int i = 0; i < 120; i++)
            {
                SendMessage($"{i} - test message");
            }

        }
    }
}
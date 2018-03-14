using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Rabbitmq.Core.Impl
{
    public class Producer : IProducer, IDisposable
    {
        private RabbitMqService _rabbitMqService;
        private const string QueueName = "logs";
        public Producer()
        {


            //Impl.Consumer = new EventingBasicConsumer(Channel);
            //Impl.Consumer.Received += OnReceived;
            //Channel.BasicConsume(QueueName, AutoAck, Impl.Consumer);
        }

        public void SendMessage(string message)
        {
            _rabbitMqService = new RabbitMqService("localhost", QueueName);
            var connection = _rabbitMqService.GetConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare(QueueName, false, false, false, null);        
            channel.BasicPublish("", QueueName, null, Encoding.UTF8.GetBytes(message));
            Console.WriteLine("{0} queue'su üzerine, \"{1}\" mesajı yazıldı.", QueueName, message);
            
            //IBasicProperties basicProperties = Channel.CreateBasicProperties();
            //basicProperties.Persistent = true;
            ////Model.BasicPublish(QueueName, "", basicProperties, message);
            //Channel.BasicPublish(exchange: "", routingKey: QueueName, basicProperties: null, body: message);
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //dispose managed resources
                }
            }
            //dispose unmanaged resources
            disposed = true;
        }

        public void Dispose()
        {
            _rabbitMqService?.Dispose();
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
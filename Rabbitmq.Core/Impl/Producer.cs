using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Rabbitmq.Core.Impl
{
    public class Producer : RabbitConnectionBase, IProducer
    {
        public Producer(string server, string queueName, string exchangeType) : base(server, queueName, exchangeType)
        {


            //Impl.Consumer = new EventingBasicConsumer(Channel);
            //Impl.Consumer.Received += OnReceived;
            //Channel.BasicConsume(QueueName, AutoAck, Impl.Consumer);
        }

        public void SendMessage(byte[] message)
        {
            IBasicProperties basicProperties = Channel.CreateBasicProperties();
            basicProperties.Persistent = true;
            //Model.BasicPublish(QueueName, "", basicProperties, message);
            Channel.BasicPublish(exchange: "", routingKey: QueueName, basicProperties: null, body: message);
        }
    }
}
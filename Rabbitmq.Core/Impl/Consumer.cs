using System;
using System.Text;
using RabbitMQ.Client.Events;

namespace Rabbitmq.Core.Impl
{
    public class Consumer : RabbitConnectionBase, IConsumer
    {
        public Consumer(string server, string queueName, string exchangeType) : base(server, queueName, exchangeType)
        {
            Reconnect();            
        }


        public override void OnReceived(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body;
            var content = Encoding.UTF8.GetString(body);
            Console.WriteLine(content);
        }
    }
}
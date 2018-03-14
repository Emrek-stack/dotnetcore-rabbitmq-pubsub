using System;
using System.Text;
using RabbitMQ.Client.Events;

namespace Rabbitmq.Core.Impl
{
    public class Consumer :  IConsumer
    {
        private RabbitMqService _rabbitMqService;
        private const string queueName = "logs";

        public Consumer() 
        {                
        }


        public override void OnReceived(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body;
            var content = Encoding.UTF8.GetString(body);
            Console.WriteLine(content);
        }
    }
}
using System;
using System.IO;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace Rabbitmq.Core
{
    public abstract class RabbitConnectionBase : IDisposable
    {                             
        private ConnectionFactory _factory;
        private IConnection _connection;
        protected IModel Channel;
        private EventingBasicConsumer _consumer;
        
        
        private const bool Durable = true;
        private const bool AutoAck = true;

        private readonly string _server;
        protected readonly string QueueName;


        protected RabbitConnectionBase(string server, string queueName, string exchangeType)
        {
            this._server = server;
            this.QueueName = queueName;
            //ExchangeTypeName = exchangeType;
        }
        //Create the connection, Model and Exchange(if one is required)
        private void Connect()
        {
            _factory = new ConnectionFactory { HostName = _server, UserName = "boyner", Password = "B12312312!!" };
            _connection = _factory.CreateConnection();
            _connection.ConnectionShutdown += Connection_ConnectionShutdown;


            Channel = _connection.CreateModel();
            Channel.QueueDeclare(QueueName, Durable, false, false, null);

            _consumer = new EventingBasicConsumer(Channel);
            _consumer.Received += OnReceived;
            Channel.BasicConsume(QueueName, AutoAck, _consumer);

            //Model = Connection.CreateModel();
            //bool durable = true;
            ////if (!string.IsNullOrEmpty(ExchangeName))
            ////    Model.ExchangeDeclare(ExchangeName, ExchangeTypeName, durable);
            //if (!string.IsNullOrEmpty(QueueName))
            //{
            //    Model.QueueDeclare(queue: QueueName,
            //        durable: durable,
            //        exclusive: false,
            //        autoDelete: false,
            //        arguments: null);
            //}        
        }

        public virtual void OnReceived(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body;
            var content = Encoding.UTF8.GetString(body);
            Console.WriteLine(content);
        }

        private void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("Connection broke!");

            Reconnect();
        }


        public void Reconnect()
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


        public void Cleanup()
        {
            try
            {
                if (Channel != null && Channel.IsOpen)
                {
                    Channel.Close();
                    Channel = null;
                }

                if (_connection == null || !_connection.IsOpen) return;
                _connection.Close();
                _connection = null;
            }
            catch (IOException ex)
            {
                // Close() may throw an IOException if connection
                // dies - but that's ok (handled by reconnect)
            }
        }

        public void Dispose()
        {
            _connection?.Close();
            Channel?.Abort();
        }
    }
}

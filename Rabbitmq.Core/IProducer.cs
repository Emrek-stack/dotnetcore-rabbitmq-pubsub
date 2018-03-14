namespace Rabbitmq.Core
{
    public interface IProducer
    {
        void SendMessage(string message);
    }
}
namespace Rabbitmq.Core
{
    public interface IProducer
    {
        void SendMessage(byte[] message);
    }
}
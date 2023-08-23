using RabbitMQ.Client;

namespace ServerClient.RabbitMQRPC.Lib.Interfaces
{
    public interface IConnectionProvider
    {
        IConnection GetConnection();
    }
}

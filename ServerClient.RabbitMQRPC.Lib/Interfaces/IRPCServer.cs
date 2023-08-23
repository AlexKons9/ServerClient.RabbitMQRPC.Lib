

namespace ServerClient.RabbitMQRPC.Lib.Interfaces
{
    public interface IRPCServer
    {
        void StartListening();
        void StopListening();
    }
}


namespace ServerClient.RabbitMQRPC.Lib.Interfaces
{
    public interface IRPCClient
    {
        string SendRequest(object request);
    }
}

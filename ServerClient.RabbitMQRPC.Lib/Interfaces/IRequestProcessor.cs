using ServerClient.RabbitMQRPC.Lib.Enums;


namespace ServerClient.RabbitMQRPC.Lib.Interfaces
{
    public interface IRequestProcessor
    {
        public TypeOfRequest Type { get; }
        Task<object> ProcessRequest(object request);
    }
}

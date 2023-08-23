using ServerClient.RabbitMQRPC.Lib.Enums;


namespace ServerClient.RabbitMQRPC.Lib
{
    public class RequestHandler<T> where T : class
    {
        public T Data { get; set; }
        public TypeOfRequest Type { get; set; }


        public RequestHandler(T data, TypeOfRequest type)
        {
            Data = data;
            Type = type;
        }
    }
}

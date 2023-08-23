using RabbitMQ.Client;
using ServerClient.RabbitMQRPC.Lib.Interfaces;

namespace ServerClient.RabbitMQRPC.Lib
{
    public class ConnectionProvider : IConnectionProvider
    {
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;

        public ConnectionProvider(string url)
        {
            _factory = new ConnectionFactory
            {
                Uri = new Uri(url)
            };

            _connection = _factory.CreateConnection();
        }

        public IConnection GetConnection()
        {
            return _connection;
        }
    }
}

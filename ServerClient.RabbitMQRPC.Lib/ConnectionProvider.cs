using RabbitMQ.Client;
using ServerClient.RabbitMQRPC.Lib.Interfaces;

namespace ServerClient.RabbitMQRPC.Lib
{
    // This class provides a connection to the RabbitMQ server.
    // It uses the provided URL to establish the connection.
    public class ConnectionProvider : IConnectionProvider
    {
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;

        public ConnectionProvider(string url)
        {
            // Initialize the connection factory with the provided URL
            _factory = new ConnectionFactory
            {
                Uri = new Uri(url)
            };

            // Create a connection using the factory
            _connection = _factory.CreateConnection();
        }

        // Get the established connection
        public IConnection GetConnection()
        {
            return _connection;
        }
    }
}

using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using ServerClient.RabbitMQRPC.Lib.Interfaces;
using System.Text;
using Newtonsoft.Json;

namespace ServerClient.RabbitMQRPC.Lib
{
    // This class represents an RPC client that sends requests to the RabbitMQ server
    // and waits for responses.
    public class RPCClient : IRPCClient, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IConnectionProvider _connectionProvider;
        private readonly IModel _channel;
        private readonly string _replyQueueName;
        private readonly EventingBasicConsumer _consumer;
        private TaskCompletionSource<string> _responseReceived;

        // This class represents an RPC client that sends requests to the RabbitMQ server
        // and waits for responses.
        public RPCClient(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;

            // Establish a connection to RabbitMQ using the connection provider
            _connection = _connectionProvider.GetConnection();

            // Create a channel for communication
            _channel = _connection.CreateModel();

            // Generate a unique queue name for receiving RPC responses
            _replyQueueName = _channel.QueueDeclare().QueueName;

            // Create a consumer to receive RPC responses
            _consumer = new EventingBasicConsumer(_channel);

            // Set up a task completion source to track the received response
            _responseReceived = new TaskCompletionSource<string>();

            // Register an event handler for the Received event of the consumer
            _consumer.Received += (sender, args) =>
            {
                // Check if the received response's correlation ID matches the task's ID
                if (args.BasicProperties.CorrelationId == _responseReceived.Task.Id.ToString())
                {
                    // Convert the response body to a string
                    var body = args.Body.ToArray();
                    var response = Encoding.UTF8.GetString(body);

                    // Set the result of the task completion source with the response
                    _responseReceived.TrySetResult(response);
                }
            };
        }

        // This method sends a request to the server and waits for a response.
        public string SendRequest(object request)
        {
            // Generate a unique correlation ID for the request
            var correlationId = Guid.NewGuid().ToString();

            // Generate a unique queue name for receiving the request's response
            var replyQueueName = _channel.QueueDeclare().QueueName;

            // Create basic properties for the request
            var props = _channel.CreateBasicProperties();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;

            // Convert the request object to JSON and encode it as bytes
            var messageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request));

            // Set up a task completion source to track the received response for this request
            var responseReceived = new TaskCompletionSource<string>();

            // Create a new consumer to receive the response
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, args) =>
            {
                // Check if the received response's correlation ID matches the request's correlation ID
                if (args.BasicProperties.CorrelationId == correlationId)
                {
                    // Convert the response body to a string
                    var response = Encoding.UTF8.GetString(args.Body.ToArray());

                    // Set the result of the task completion source with the response
                    responseReceived.SetResult(response);
                }
            };

            // Start consuming messages from the reply queue
            _channel.BasicConsume(
                consumer: consumer,
                queue: replyQueueName,
                autoAck: true);

            // Publish the request message to the RPC queue with the specified properties and body
            _channel.BasicPublish(
                exchange: "",
                routingKey: "rpc_queue",
                basicProperties: props,
                body: messageBytes);

            // Wait for the response task to complete and return the received response
            return responseReceived.Task.GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            // Close the channel and the connection when disposing of the client
            _channel.Close();
            _connection.Close();
        }
    }
}

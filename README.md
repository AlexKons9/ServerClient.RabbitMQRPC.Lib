# ServerClient.RabbitMQRPC.Lib

## Description
Library that uses RabbitMQ as a Message Broker. The client sends the message and the type of the process to the server, and the server gets the object with the type of request, (you have to create a dto to pass all the required information), it finds the proccesor and proccess the message accordingly. The results is being snd back to the client. 

## Example
Look at my repositories the Project.RabbitMQ.RPC.Final project as an example and more explanation in depth.

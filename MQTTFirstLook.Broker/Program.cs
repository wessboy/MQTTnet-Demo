
using MQTTnet;
using MQTTnet.Server;



MqttServerOptionsBuilder options = new MqttServerOptionsBuilder()
                                       .WithDefaultEndpoint()
                                       .WithDefaultEndpointPort(707)
                                       .WithConnectionValidator(BrokerMethods.OnNewConnection);
                                       //.WithApplicationMessageInterceptor(BrokerMethods.OnNewMessage);

IMqttServer mqttServer = new MqttFactory().CreateMqttServer();

mqttServer.StartAsync(options.Build()).GetAwaiter().GetResult();    

Console.ReadLine();





static class BrokerMethods
{
   

    public static void OnNewConnection(MqttConnectionValidatorContext context)
    {
        Console.WriteLine(
            "New connection: ClientId = {0},Endpoint = {1}",
            context.ClientId,
            context.Endpoint
            );


    }



}


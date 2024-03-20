
using MQTTnet;
using MQTTnet.Server;
using Newtonsoft.Json;



MqttServerOptionsBuilder options = new MqttServerOptionsBuilder()
                                       .WithDefaultEndpoint()
                                       .WithDefaultEndpointPort(707)
                                       .WithConnectionValidator(BrokerMethods.OnNewConnection);
                                       //.WithApplicationMessageInterceptor(BrokerMethods.OnNewMessage);

IMqttServer mqttServer = new MqttFactory().CreateMqttServer();

mqttServer.StartAsync(options.Build()).GetAwaiter().GetResult();

/*while (true)
{

   
    
        string message = JsonConvert.SerializeObject("New Client detected");
        mqttServer.PublishAsync("dev.to/topic/newClient", message).GetAwaiter().GetResult();
    

}*/




if (BrokerMethods.detector)
{
    string message = JsonConvert.SerializeObject("New Client detected");
    mqttServer.PublishAsync("dev.to/topic/newClient", message).GetAwaiter().GetResult();
}


Console.ReadLine();

 





static class BrokerMethods
{
    public static bool detector = false;
    
    public static void OnNewConnection(MqttConnectionValidatorContext context)
    {
        

        Console.WriteLine(
            "New connection: ClientId = {0},Endpoint = {1}",
            context.ClientId,
            context.Endpoint
        
            );
        
        detector = true;
        

    }



}


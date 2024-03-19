

using MQTTnet;
using MQTTnet.Server;

namespace MQTTFirstLook.Broker;

     public class MqttBrokerManager
    {
    private readonly IMqttServer _mqttServer;
        public MqttBrokerManager()
        {
           _mqttServer = new MqttFactory().CreateMqttServer();
         _mqttServer.ClientConnectedHandler = new MqttServerClientConnectedHandlerDelegate(OnClientConnected);
    }

    private IMqttServerOptions ConfigureBroker()
        {
            IMqttServerOptions  options = new MqttServerOptionsBuilder()
                                                    .WithDefaultEndpoint()
                                                    .WithDefaultEndpointPort(707)
                                                    .WithConnectionValidator(BrokerMethods.OnNewConnection)
                                                    .Build();
        
                return options;   
        }

      public async Task ConnectServer()
    {
         await _mqttServer.StartAsync(ConfigureBroker());
         await Task.CompletedTask;
    }

    /* static void OnNewConnection(MqttConnectionValidatorContext context)
    {
        Console.WriteLine(
            "New connection: ClientId = {0},Endpoint = {1}",
            context.ClientId,
            context.Endpoint
            );


    }*/

    static void OnClientConnected(MqttServerClientConnectedEventArgs obj)
    {
        Console.Write("New connection: ClientId = {0}",obj.ClientId);
    }
}


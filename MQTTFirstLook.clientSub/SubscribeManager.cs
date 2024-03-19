

using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Extensions.ManagedClient;
using System.Text;

namespace MQTTFirstLook.clientSub;

     public class SubscribeManager
    {
    private readonly IManagedMqttClient _mqttClient; 
        public SubscribeManager()
            {
                _mqttClient = new MqttFactory().CreateManagedMqttClient();
                _mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(OnConnected);
                _mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(OnDisconnected);
                _mqttClient.ConnectingFailedHandler = new ConnectingFailedHandlerDelegate(OnConnectingFailed);
                _mqttClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(OnMessageReceived);

    }


    private ManagedMqttClientOptions ConfigureClient()
        {
            MqttClientOptionsBuilder builder = new MqttClientOptionsBuilder()
                                                 .WithClientId("Dev.from")
                                                    .WithTcpServer("localhost", 707);
        ManagedMqttClientOptions options = new ManagedMqttClientOptionsBuilder()
                                            .WithAutoReconnectDelay(TimeSpan.FromSeconds(60))
                                            .WithClientOptions(builder.Build())
                                            .Build();

        return options; 
    }


        public async Task ConnectClient()
        {
           await _mqttClient.StartAsync(ConfigureClient());
            Task.Delay(1000).GetAwaiter().GetResult();

    }

      public async Task SubscribeToTopic(string topic)
        {
        
          await _mqttClient.SubscribeAsync(topic);
           Task.Delay(1000).GetAwaiter().GetResult();
        }



    static void OnConnected(MqttClientConnectedEventArgs obj)
    {

        Console.WriteLine("Subscriber Successfully connected.");
    }

    static void OnConnectingFailed(ManagedProcessFailedEventArgs obj)
    {
        Console.WriteLine("Couldn't connect to broker");
    }

    static void OnDisconnected(MqttClientDisconnectedEventArgs obj)
    {
        Console.WriteLine("Successfully disconnected.");

    }

    static void OnMessageReceived(MqttApplicationMessageReceivedEventArgs obj)
    {
        Console.WriteLine($"recived message from: {obj.ClientId}, topic: {obj.ApplicationMessage.Topic}, payload {Encoding.UTF8.GetString(obj.ApplicationMessage.Payload)},Time: {DateTime.Now}");
    }


}


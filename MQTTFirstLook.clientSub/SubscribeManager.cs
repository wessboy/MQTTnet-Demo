

using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Client.Subscribing;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;



namespace MQTTFirstLook.clientSub;

     public class SubscribeManager
    {
    private readonly IManagedMqttClient _mqttClient; 
    private List<MqttTopicFilter> _topics;
        public SubscribeManager()
            {
                _mqttClient = new MqttFactory().CreateManagedMqttClient();
                _mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(OnConnected);
                _mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(OnDisconnected);
                _mqttClient.ConnectingFailedHandler = new ConnectingFailedHandlerDelegate(OnConnectingFailed);
                _mqttClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(OnMessageReceived);
                _topics = new List<MqttTopicFilter>();

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

        _topics.Add(new MqttTopicFilter { Topic = topic });
        await _mqttClient.SubscribeAsync(_topics);
          //await _mqttClient.SubscribeAsync(topic);
           Task.Delay(1000).GetAwaiter().GetResult();
        }



    static void OnConnected(MqttClientConnectedEventArgs obj)
    {

        Console.WriteLine("Subscriber successfully connected.");
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
        if (obj.ApplicationMessage.Topic.Contains("json"))
        {
            var payload = Encoding.UTF8.GetString(obj.ApplicationMessage.Payload);
            var customerData = System.Text.Json.JsonSerializer.Deserialize<CustomerData>(payload);
            Console.WriteLine($"recived message from: {obj.ClientId}, topic: {obj.ApplicationMessage.Topic}, console content:{customerData?.ConsoleContent},Time: {customerData?.IssuedDate}");

            //responed to the broker 
            obj.ReasonCode = MqttApplicationMessageReceivedReasonCode.Success;

        }
        else if (obj.ApplicationMessage.Topic.Contains("newClient"))
        {
            Console.WriteLine($"Message from broker : {Encoding.UTF8.GetString(obj.ApplicationMessage.Payload)}");
        }
    }


}


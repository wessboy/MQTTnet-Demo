

using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;

namespace MQTTFirstLook.Client;

     public class PublisherManager
    {
    private readonly IManagedMqttClient _mqttClient;
       public PublisherManager() 
        {
          _mqttClient = new MqttFactory().CreateManagedMqttClient();

        _mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(OnConnected);
        _mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(OnDisconnected);
        _mqttClient.ConnectingFailedHandler = new ConnectingFailedHandlerDelegate(OnConnectingFailed);

        }



          private ManagedMqttClientOptions ConfigureClient()
            {
                                MqttClientOptionsBuilder builder = new MqttClientOptionsBuilder()
                               .WithClientId("Dev.To")
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
        }

        public async Task PublishToTopic(string topic,string payload)
    {
        CustomerData customerData = new CustomerData { ConsoleContent = payload, IssuedDate = DateTime.Now };

        string json = JsonConvert.SerializeObject(customerData);
         
          var message = new MqttApplicationMessageBuilder()
                            .WithTopic(topic)
                            .WithPayload(json)
                            .WithAtLeastOnceQoS()
                            .WithContentType("application/json")
                            .Build();
        await _mqttClient.PublishAsync(message);
        //await _mqttClient.PublishAsync(topic, json);
            Task.Delay(1000).GetAwaiter().GetResult();
        }
        

    static void OnConnected(MqttClientConnectedEventArgs obj)
    {

        Console.WriteLine("Publisher Successfully connected.");
    }

    static void OnConnectingFailed(ManagedProcessFailedEventArgs obj)
    {
        Console.WriteLine("Couldn't connect to broker");
    }

    static void OnDisconnected(MqttClientDisconnectedEventArgs obj)
    {
        Console.WriteLine("Successfully disconnected.");

    }



}



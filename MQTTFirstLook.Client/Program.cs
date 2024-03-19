
using MQTTFirstLook.Client;


PublisherManager publisherClient = new PublisherManager();
await publisherClient.ConnectClient();


Thread.Sleep(1000);

while (true)
{
    Console.WriteLine("enter your payload : ");

    string payload = Console.ReadLine() ?? string.Empty;

    if (payload != null)
    {

        await publisherClient.PublishToTopic("dev.to/topic/json",payload);


    }

}
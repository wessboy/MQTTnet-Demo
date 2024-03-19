
using MQTTFirstLook.clientSub;

SubscribeManager subscriberClient = new SubscribeManager();
await subscriberClient.ConnectClient();

while (true)
await subscriberClient.SubscribeToTopic("dev.to/topic/json");
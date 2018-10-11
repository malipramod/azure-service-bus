using System;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace SimpleBrokeredMessing.ChatConsole
{
    class ChatConsole
    {
        private static string ConnectionString = "Endpoint=sb://<AzureServiceBusNamespace>.servicebus.windows.net/;SharedAccessKeyName=<SharedAccessKeyName>;SharedAccessKey=<SharedAccessKey>";
        static string TopicPath = "chattopic";
        static void Main(string[] args)
        {
            Console.Write("Enter username (without spaces/special chars):");
            var userName = Console.ReadLine();

            //Create a namespace
            var manager = NamespaceManager.CreateFromConnectionString(ConnectionString);

            //Create a topic if it doesn't exists
            if(!manager.TopicExists(TopicPath))
            {
                manager.CreateTopic(TopicPath);
            }

            //Create a subscription for the user

            var description = new SubscriptionDescription(TopicPath, userName) {
                AutoDeleteOnIdle = TimeSpan.FromMinutes(5)
            };

            manager.CreateSubscription(description);


            //Create Clients
            var factory = MessagingFactory.CreateFromConnectionString(ConnectionString);
            var topicClient = factory.CreateTopicClient(TopicPath);
            var subscriptionClient = factory.CreateSubscriptionClient(TopicPath, userName);

            //Create a message pump for receiving messages
            subscriptionClient.OnMessage(msg => ProcessMessage(msg));


            //Send a message to say you are here
            var helloMessage = new BrokeredMessage("Has entered the room...") {
                Label = userName
            };
            topicClient.Send(helloMessage);

            while(true)
            {
                string text = Console.ReadLine();
                if(text.Equals("exit")) break;

                var chatMessage = new BrokeredMessage(text);
                chatMessage.Label = userName;
                topicClient.Send(chatMessage);
            }

            //Send a message to say you are leaving
            var goodbyeMessage = new BrokeredMessage("Has left the building...") {
                Label = userName
            };
            topicClient.Send(goodbyeMessage);

            //close the factory and clients it created
            factory.Close();
        }

        private static void ProcessMessage(BrokeredMessage msg)
        {
            var user = msg.Label;
            var text = msg.GetBody<string>();
            Console.WriteLine(user + " : " + text);
        }
    }
}

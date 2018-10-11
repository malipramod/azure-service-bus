using System;
using Microsoft.ServiceBus.Messaging;

namespace SimpleBrokeredMessing.Reciever
{
    class Reciever
    {
        private static string ConnectionString = "Endpoint=sb://<AzureServiceBusNamespace>.servicebus.windows.net/;SharedAccessKeyName=<SharedAccessKeyName>;SharedAccessKey=<SharedAccessKey>";
        static string QueuePath = "demoqueue";

        static void Main(string[] args)
        {
            //create a queue client
            var queueClient = QueueClient.CreateFromConnectionString(ConnectionString, QueuePath);

            //Create a message pump to reveive and process message.
            queueClient.OnMessage(msg => ProcessMessage(msg));

            Console.WriteLine("Press key exit.");
            Console.ReadKey();

            queueClient.Close();
        }

        private static void ProcessMessage(BrokeredMessage msg)
        {
            var text = msg.GetBody<string>();
            Console.WriteLine("Received: "+ text);
        }
    }
}

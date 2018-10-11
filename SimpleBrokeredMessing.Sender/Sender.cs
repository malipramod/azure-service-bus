using System;
using Microsoft.ServiceBus.Messaging;

namespace SimpleBrokeredMessing.Sender
{
    class Sender
    {
        private static string ConnectionString = "Endpoint=sb://<AzureServiceBusNamespace>.servicebus.windows.net/;SharedAccessKeyName=<SharedAccessKeyName>;SharedAccessKey=<SharedAccessKey>";

        static string QueuePath = "demoqueue";
        static void Main(string[] args){
            //create a queue client
            var queueClient = QueueClient.CreateFromConnectionString(ConnectionString, QueuePath);

            for(int i = 0; i < 10; i++)
            {
                var message = new BrokeredMessage("Message: " + GetTimestamp(DateTime.Now));
                queueClient.Send(message);
                Console.WriteLine("Sent: " + message);
            }

            queueClient.Close();
            //Console.ReadKey();
        }

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }
    }
}

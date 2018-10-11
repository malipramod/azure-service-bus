using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusManagementConsole
{
    class ManagementConsole
    {
        private static string ServiceBusConnectionString = "Endpoint=sb://<AzureServiceBusNamespace>.servicebus.windows.net/;SharedAccessKeyName=<SharedAccessKeyName>;SharedAccessKey=<SharedAccessKey>";
        static void Main(string[] args){
            ManagementHelper helper = new ManagementHelper(ServiceBusConnectionString);
            bool done = false;
            do{
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(">");
                string commandLine = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.Magenta;
                string[] commands = commandLine.Split(' ');

                try{
                    if(commands.Length > 0) {
                        switch(commands[0]){
                            case "cq":
                                if(commands.Length > 1){
                                    helper.CreateQueue(commands[1]);
                                } else
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Queue Path missing");
                                }
                                break;
                            case "lq":
                                if(commands.Length > 1)
                                {
                                    helper.ListQueue(commands[1]);
                                }else if(commands.Length == 1)
                                {
                                    helper.ListQueues();
                                } else
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Queue Path missing");
                                }
                                break;
                            case "dq":
                                if(commands.Length > 1)
                                {
                                    helper.DeleteQueue(commands[1]);
                                } else
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Queue Path missing");
                                }
                                break;
                            case "ls":
                                helper.ListTopicsAndSubscription();
                                break;
                            case "ct":
                                if(commands.Length > 1)
                                {
                                    helper.CreateTopic(commands[1]);
                                } else
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Topic Path missing");
                                }
                                break;
                            case "cs":
                                if(commands.Length > 2)
                                {
                                    helper.CreateSubscription(commands[1], commands[2]);
                                } else
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Subsription Path missing");
                                }
                                break;
                            case "exit":
                                done = true;
                                break;
                        }
                    }
                    
                } catch(Exception ex) {
                    Console.WriteLine("Error {0}", ex.Message);
                }

            } while(!done);
            Console.ReadKey();
        }
    }
}

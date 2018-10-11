using System;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System.Collections;
using System.Collections.Generic;

namespace ServiceBusManagementConsole
{
    class ManagementHelper
    {
        private NamespaceManager m_NamespaceManager;

        public ManagementHelper(string connectionString){
            m_NamespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            Console.WriteLine("Service bus address {0}", m_NamespaceManager.Address);
        }

        public void CreateQueue(string queuePath) {
            Console.Write("\tCreating queue: {0}...", queuePath);
            var description = GetQueueDescription(queuePath);
            var createDescription = m_NamespaceManager.CreateQueue(description);
            Console.WriteLine("Done!");
        }

        public QueueDescription GetQueueDescription(string path){
            return new QueueDescription(path) {
                RequiresDuplicateDetection = true,
                DuplicateDetectionHistoryTimeWindow = TimeSpan.FromMinutes(10),
                RequiresSession=true,
                MaxDeliveryCount=20,
                DefaultMessageTimeToLive=TimeSpan.FromHours(1),
                EnableDeadLetteringOnMessageExpiration=true
            };
        }

        public void ListQueues() {
            IEnumerable<QueueDescription> queueDescriptions = m_NamespaceManager.GetQueues();
            foreach(QueueDescription item in queueDescriptions) {
                Console.WriteLine("\t{0}", item.Path);
            }

            Console.WriteLine("Done!");
        }

        public void ListQueue(string queuePath){
            QueueDescription queueDescriptions = m_NamespaceManager.GetQueue(queuePath);
            Console.WriteLine("\tQueue Path: {0}", queueDescriptions.Path);
            Console.WriteLine("\tQueue MessageCount: {0}", queueDescriptions.MessageCount);  
            Console.WriteLine("\tQueue SizeInBytes: {0}", queueDescriptions.SizeInBytes);
            Console.WriteLine("\tQueue RequiresSession: {0}", queueDescriptions.RequiresSession);
            Console.WriteLine("\tQueue RequiresDuplicateDetection: {0}", queueDescriptions.RequiresDuplicateDetection);
            Console.WriteLine("\tQueue MaxDeliveryCount: {0}", queueDescriptions.MaxDeliveryCount);
            Console.WriteLine("\tQueue DefaultMessageTimeToLive: {0}", queueDescriptions.DefaultMessageTimeToLive);
            Console.WriteLine("\tQueue EnableDeadLetteringOnMessageExpiration: {0}", queueDescriptions.EnableDeadLetteringOnMessageExpiration);
            Console.WriteLine("Done!");
        }

        public void DeleteQueue(string queuePath){
            Console.Write("\tDeleting queue: {0}...", queuePath);
            m_NamespaceManager.DeleteQueue(queuePath);
            Console.WriteLine("Done!");
        }

        public void ListTopicsAndSubscription() {
            IEnumerable<TopicDescription> topicDescriptions = m_NamespaceManager.GetTopics();
            foreach(TopicDescription topicDescription in topicDescriptions) {
                Console.WriteLine("\t{0}", topicDescription.Path);
                IEnumerable<SubscriptionDescription> sunscriptionDescriptions = m_NamespaceManager.GetSubscriptions(topicDescription.Path);
                foreach(SubscriptionDescription subscriptionDescription in sunscriptionDescriptions) {
                    Console.WriteLine("\t\t{0}", subscriptionDescription.Name);
                }
            }
            Console.WriteLine("Done!");
        }

        public void CreateTopic(string topicPath){
            Console.Write("\tCreating topic: {0}...", topicPath);
            var description = m_NamespaceManager.CreateTopic(topicPath);
            Console.WriteLine("Done!");
        }
        
        public void CreateSubscription(string topicPath, string subscriptionName){
            Console.WriteLine("Creating subscription {0}/Subscriptions/{1}...", topicPath, subscriptionName);
            var description = m_NamespaceManager.CreateSubscription(topicPath, subscriptionName);
            Console.WriteLine("Done!");
        }

    }
}

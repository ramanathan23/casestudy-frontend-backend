using Azure.Messaging.ServiceBus;
using EAuction.Infrastructure.Common;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace EAuction.Messaging
{
    public static class MessagingConfiguration
    {
        public static void ConfigureMessaging(this IServiceCollection serviceCollection, ServiceBusSettings serviceBusSettings)
        {
            serviceCollection.AddSingleton<ServiceBusClient>((_) => new ServiceBusClient(serviceBusSettings.ConnectionString));
            CreateQueues(serviceCollection, serviceBusSettings.serviceBusQueues, serviceBusSettings.ConnectionString);
            CreateTopicPublishers(serviceCollection, serviceBusSettings.serviceBusTopics, serviceBusSettings.ConnectionString);
            CreateQueueConsumers(serviceCollection, serviceBusSettings.serviceBusQueueConsumers, serviceBusSettings.ConnectionString);
            CreateTopicSubscribers(serviceCollection, serviceBusSettings.serviceBusSubscribers, serviceBusSettings.ConnectionString);
        }

        public static void CreateQueues(IServiceCollection serviceCollection, IEnumerable<string> queues, string connectionString)
        {
            if (queues != null) 
            {

                foreach (var item in queues)
                {
                    serviceCollection.AddSingleton<IEventBusQueuePublisher>(provider =>
                                                                            new EventBusQueuePublisher(provider.GetRequiredService<ServiceBusClient>(), item));
                }
            }
        }

        public static void CreateTopicPublishers(IServiceCollection serviceCollection, IEnumerable<string> topics, string connectionString)
        {
            if (topics != null)
            {
                foreach (var item in topics)
                {
                    serviceCollection.AddSingleton<IEventBusTopicPublisher>(provider => new EventBusTopicPublisher(
                                                                                                provider.GetRequiredService<ServiceBusClient>(), item));
                }
            }
        }

        public static void CreateQueueConsumers(IServiceCollection serviceCollection, IEnumerable<string> consumers, string connectionString)
        {
            if (consumers != null)
            {
                foreach (var item in consumers)
                {
                    serviceCollection.AddSingleton<IEventBusQueueConsumer>(provider => new EventBusQueueConsumer(
                                                                                           provider.GetRequiredService<ServiceBusClient>(), item));
                }
            }
        }

        public static void CreateTopicSubscribers(IServiceCollection serviceCollection, IEnumerable<string> subscribers, string connectionString)
        {
            if (subscribers != null)
            {

                foreach (var item in subscribers)
                {
                    var results = item.Split(":");
                    serviceCollection.AddSingleton<IEventBusSubscriber>(provider => new EventBusSubscriber(
                                                                                 provider.GetRequiredService<ServiceBusClient>(), results[0], results[1]));
                }
            }
        }
    }
}
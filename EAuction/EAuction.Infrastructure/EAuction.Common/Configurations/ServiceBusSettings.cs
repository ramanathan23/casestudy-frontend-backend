using System.Collections.Generic;

namespace EAuction.Infrastructure.Common
{
    public class ServiceBusSettings
    {
        public string ConnectionString { get; set; }
        public IEnumerable<string> serviceBusQueues { get; set; }
        public IEnumerable<string> serviceBusTopics { get; set; }
        public IEnumerable<string> serviceBusSubscribers { get; set; }
        public IEnumerable<string> serviceBusQueueConsumers { get; set; }
    }
}
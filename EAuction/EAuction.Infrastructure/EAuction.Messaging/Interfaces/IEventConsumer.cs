using System;
using System.Threading.Tasks;

namespace EAuction.Messaging
{
    public interface IEventConsumer
    {
        Task StartProcessingAsync();

        event Func<string, Task> Consume;
    }
}
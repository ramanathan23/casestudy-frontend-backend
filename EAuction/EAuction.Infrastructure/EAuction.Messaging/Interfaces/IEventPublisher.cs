using EAuction.Infrastructure.Common;
using System.Threading.Tasks;

namespace EAuction.Messaging
{
    public interface IEventPublisher
    {
        Task PublishMessageAsync(EventMessage messageToPublish);
    }
}
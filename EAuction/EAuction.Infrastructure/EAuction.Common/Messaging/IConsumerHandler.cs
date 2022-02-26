using System.Threading.Tasks;

namespace EAuction.Infrastructure.Common
{
    public interface IConsumerHandler
    {
        public Task RegisterAsync();

        public Task HandleMessageAsync(string message);
    }
}
using EAuction.Buyer.Core.Consumers;
using EAuction.Buyer.Core.Domain;
using EAuction.Buyer.Core.Repositories;
using EAuction.Buyer.Core.Services;
using EAuction.Infrastructure.Common;
using EAuction.Persistence.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EAuction.Buyer.Core
{
    public static class BuyerDomainConfiguration
    {
        public static void ConfigureDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IBidRepository, BidRepository>();
            services.AddScoped<IRepository<AuctionBuyer, string>, BuyerRepository>();
            services.AddScoped<IBuyerService, BuyerService>();
            services.AddSingleton<IConsumerHandler, DeleteProductSubscriber>();
            services.AddSingleton<IConsumerHandler, AddOrUpdateBidConfirmSubscriber>();
        }

        public static void InitializeConsumers(this IApplicationBuilder app)
        {
            foreach (var consumer in app.ApplicationServices.GetServices<IConsumerHandler>())
            {
                consumer.RegisterAsync().Wait();
            }
        }
    }
}
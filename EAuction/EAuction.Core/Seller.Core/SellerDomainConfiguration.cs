using EAuction.Infrastructure.Common;
using EAuction.Persistence.Repositories;
using EAuction.Seller.Core.Consumers;
using EAuction.Seller.Core.Domain;
using EAuction.Seller.Core.Repositories;
using EAuction.Seller.Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EAuction.Seller.Core
{
    public static class SellerDomainConfiguration
    {
        public static void ConfigureDomainServices(this IServiceCollection services)
        {
            services.AddScoped<ISellerService, SellerService>();
            services.AddScoped<IRepository<AuctionProduct, string>, ProductRepository>();
            services.AddScoped<IRepository<AuctionProductSeller, string>, SellerRepository>();
            services.AddSingleton<IConsumerHandler, DeleteProductConfirmSubscriber>();
            services.AddSingleton<IConsumerHandler, ValidateBidRequestSubscriber>();
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
using EAuction.BidListing.Core.Consumers;
using EAuction.BidListing.Core.Domain;
using EAuction.BidListing.Core.Repositories;
using EAuction.BidListing.Core.Services;
using EAuction.Infrastructure.Common;
using EAuction.Persistence.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EAuction.BidListing.Core
{
    public static class BidlistingDomainConfiguration
    {
        public static void ConfigureDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository<ProductAndBidDetails, string>, ProductAndBidDetailsRepository>();
            services.AddScoped<IBidListingService, BidListingService>();
            services.AddSingleton<IConsumerHandler, AddProductConsumer>();
            services.AddSingleton<IConsumerHandler, BidAddedOrUpdatedConsumer>();
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
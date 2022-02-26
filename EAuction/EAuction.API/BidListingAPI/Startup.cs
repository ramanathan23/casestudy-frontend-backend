using EAuction.BidListing.Core;
using EAuction.Infrastructure.Common;
using EAuction.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Security.Authentication;

namespace EAuction.BidListing.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));

            services.AddCors();

            services.Configure<ServiceBusSettings>(Configuration.GetSection("ServiceBusSettings"));
            services.Configure<DatabaseSettings>(Configuration.GetSection("BidListingDatabase"));

            services.AddApplicationInsightsTelemetry();

            services.AddSingleton<MongoClient>((_) =>
            {
                MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(_.GetService<IOptions<DatabaseSettings>>().Value.ConnectionString));
                settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
                return new MongoClient(settings);
            });
            services.AddSingleton<IMongoDatabase>((_) => _.GetService<MongoClient>().GetDatabase(_.GetService<IOptions<DatabaseSettings>>().Value.Store));

            services.ConfigureMessaging(Configuration.GetSection("ServiceBusSettings").Get<ServiceBusSettings>());
            services.ConfigureDomainServices();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BidListingAPI", Version = "v1" });
                c.OperationFilter<AddRequiredHeaderParameter>();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BidListingAPI v1"));
            }

            app.UsePathBase(new PathString("/e-auction"));

            app.UseRouting();

            app.UseAuthorization();

            app.InitializeConsumers();

            app.UseCors((_) => _.AllowAnyOrigin().AllowAnyHeader().WithMethods("GET"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
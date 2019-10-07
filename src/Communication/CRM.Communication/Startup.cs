using CRM.IntegrationEvents;
using CRM.Shared;
using CRM.Shared.EventBus;
using CRM.Shared.EventBus.Nats;
using CRM.Shared.Jaeger;
using CRM.Communication.IntegrationHandlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;

namespace CRM.Communication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            IdentityModelEventSource.ShowPII = true;
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<NatsOptions>(Configuration.GetSection("NATS"));
            services.AddJaeger();

            RegisterServiceBus(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            var appOptions = Configuration.GetOptions<AppOptions>("App");
            eventBus.Subscribe<ContactCreatedEvent, ContactCreatedEventHandler>(appOptions.Name);
        }

        private static void RegisterServiceBus(IServiceCollection services)
        {
            services.AddSingleton<INatsConnection, DefaultNatsConnection>();
            services.AddSingleton<IEventBus, EventBusNats>();
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            services.AddTransient<ContactCreatedEventHandler>();
        }
    }
}

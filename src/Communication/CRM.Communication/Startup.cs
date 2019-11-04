using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using CRM.Tracing.Jaeger;
using MassTransit.AspNetCoreIntegration;
using MassTransit;
using CRM.Communication.IntegrationHandlers;
using System;
using CRM.Shared.Types;
using MassTransit.Definition;
using CRM.Shared;
using CRM.MassTransit.Tracing;

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
            services.AddJaeger();

            services.AddMassTransit(ConfigureBus, (cfg) =>
           {
               cfg.AddConsumersFromNamespaceContaining<ConsumerAnchor>();
           });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        }

        private static IBusControl ConfigureBus(IServiceProvider provider)
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.UseSerilog();
                var rabbitmqOption = provider.GetService<IConfiguration>().GetOptions<RabbitMqOptions>("rabbitMQ");
                var host = cfg.Host(new Uri(rabbitmqOption.Url), "/", hc =>
                {
                    hc.Username(rabbitmqOption.UserName);
                    hc.Password(rabbitmqOption.Password);
                });

                cfg.ReceiveEndpoint(host, "communication", x =>
                {
                    x.Consumer<ContactCreatedConsumer>(provider);
                    x.Consumer<ContactUpdatedConsumer>(provider);
                });

                cfg.ConfigureEndpoints(provider, new KebabCaseEndpointNameFormatter());
                cfg.PropagateOpenTracingContext();
            });
        }
    }
}

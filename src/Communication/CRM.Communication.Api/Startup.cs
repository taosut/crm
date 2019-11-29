using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CRM.Tracing.Jaeger;
using MassTransit.AspNetCoreIntegration;
using MassTransit;
using CRM.Communication.IntegrationHandlers;
using System;
using CRM.Shared.Types;
using CRM.Shared;
using CRM.MassTransit.Tracing;
using CRM.Metrics;
using Microsoft.AspNetCore.Http;
using MassTransit.Context;

namespace CRM.Communication.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        private RabbitMqOptions RabbitMqOption
        {
            get
            {
                return _configuration.GetOptions<RabbitMqOptions>("rabbitMQ");
            }
        }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddJaeger();
            services.AddAppMetrics();
            services.AddHealthChecks()
                .AddRabbitMQ(RabbitMqOption.Url, name: "comnunication-rabbitmqbus-check", tags: new string[] { "rabbitmqbus" });

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

            app.UseAppMetrics();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client");
                });
            });
        }

        private IBusControl ConfigureBus(IServiceProvider provider)
        {
            MessageCorrelation.UseCorrelationId<IMessage>(x=>x.CorrelationId);
            
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.UseSerilog();
                var host = cfg.Host(new Uri(RabbitMqOption.Url), "/", hc =>
                {
                    hc.Username(RabbitMqOption.UserName);
                    hc.Password(RabbitMqOption.Password);
                });

                cfg.ReceiveEndpoint(host, "communication", x =>
                {
                    x.Consumer<ContactCreatedConsumer>(provider);
                    x.Consumer<ContactUpdatedConsumer>(provider);
                });

                cfg.PropagateOpenTracingContext();
                cfg.PropagateCorrelationIdContext();
            });
        }
    }
}

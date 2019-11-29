using CRM.Contact.Services;
using CRM.Shared.Interceptors;
using CRM.Shared.Repository;
using CRM.Tracing.Jaeger;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using OpenTracing.Contrib.Grpc.Interceptors;
using MassTransit;
using System;
using MassTransit.AspNetCoreIntegration;
using CRM.Shared.Types;
using CRM.Shared;
using CRM.Contact.IntegrationHandlers;
using MassTransit.Definition;
using CRM.Shared.CorrelationId;
using CRM.MassTransit.Tracing;
using MassTransit.Context;
using CRM.Dapper;
using CRM.Contact.Validators;
using CRM.Metrics;
using Microsoft.AspNetCore.Http;

namespace CRM.Contact.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        private string ConnString
        {
            get
            {
                return _configuration.GetConnectionString("contact");
            }
        }
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
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.PostgreSQL);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCorrelationId();
            services.AddJaeger();
            services.AddMediatR(typeof(ContactService));
            services.AddAppMetrics();
            services.AddHealthChecks()
                .AddNpgSql(ConnString, name: "contactdb-check", tags: new string[] { "contactdb" })
                .AddRabbitMQ(RabbitMqOption.Url, name: "contact-rabbitmqbus-check", tags: new string[] { "rabbitmqbus" });

            RegisterGrpc(services);
            // RegisterAuth(services);
            RegisterRepository(services);

            services.Scan(scan => scan
               .FromAssemblyOf<CreateContactRequestValidator>()
               .AddClasses(c => c.AssignableTo(typeof(IValidator<>)))
               .AsImplementedInterfaces()
               .WithTransientLifetime());

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

            app.UseCorrelationId();
            app.UseAppMetrics();
            app.UseRouting();
            // app.UseAuthentication();
            // app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // Communication with gRPC endpoints must be made through a gRPC client.
                // To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909
                endpoints.MapGrpcService<LeadService>();
                endpoints.MapGrpcService<ContactService>();

                endpoints.MapHealthChecks("/health");

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client");
                });
            });
        }

        private void RegisterRepository(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork>(sp =>
            {
                return new UnitOfWork(() => new NpgsqlConnection(ConnString));
            });
        }

        private static void RegisterGrpc(IServiceCollection services)
        {
            services.AddGrpc(options =>
            {                
                options.Interceptors.Add<ExceptionInterceptor>();                
                options.Interceptors.Add<ServerTracingInterceptor>();
                options.EnableDetailedErrors = true;
            });
        }

        private IBusControl ConfigureBus(IServiceProvider provider)
        {
            MessageCorrelation.UseCorrelationId<IMessage>(x => x.CorrelationId);

            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.UseSerilog();
                var host = cfg.Host(new Uri(RabbitMqOption.Url), "/", hc =>
                {
                    hc.Username(RabbitMqOption.UserName);
                    hc.Password(RabbitMqOption.Password);
                });

                cfg.ReceiveEndpoint(host, "contact", x =>
                {
                    x.ConfigureConsumer<ContactCreatedConsumer>(provider);
                });

                cfg.PropagateOpenTracingContext();
                cfg.PropagateCorrelationIdContext();
            });
        }

        
        private static void RegisterAuth(IServiceCollection services)
        {
            services.AddAuthorization();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer((options) =>
                {
                    options.Authority = "http://localhost:8080/auth/realms/master";
                    options.RequireHttpsMetadata = false;
                    options.Audience = "account";
                });
        }
    }
}

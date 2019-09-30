using CRM.Graph.Gateway.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using CRM.Shared;
using CRM.Shared.Jaeger;
using OpenTracing.Contrib.Grpc.Interceptors;
using CRM.Shared.Services;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Playground;
using CRM.Graph.Gateway.Types;
using static CRM.Protobuf.Contact.V1.ContactApi;
using static CRM.Protobuf.Contact.V1.LeadApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CRM.Graph.Gateway
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            RegisterAuth(services);

            services.AddJaeger();

            GraphQLRegister(services);

            GrpcRegister(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseCors("MyPolicy");

            app.UseRouting();
            app.UseGraphQL("/graphql")
                .UsePlayground(new PlaygroundOptions()
                {
                    QueryPath = "/graphql",
                    Path = "/ui/playground"
                });
        }

        private void GraphQLRegister(IServiceCollection services)
        {
            services.AddGraphQL(sp => Schema.Create(c =>
            {
                c.RegisterServiceProvider(sp);
                c.RegisterQueryType<QueryType>();
                c.RegisterAuthorizeDirectiveType();
                c.RegisterType<AddressType>();
            }));
        }

        private void GrpcRegister(IServiceCollection services)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            services.Scan(scan => scan
                            .FromCallingAssembly()
                            .AddClasses(x => x.AssignableTo(typeof(ServiceBase)))
                            .AsImplementedInterfaces()
                            .WithScopedLifetime());

            services.AddTransient<ClientTracingInterceptor>();
            var serviceOptions = Configuration.GetOptions<ServiceOptions>("Services");

            services.AddGrpcClient<ContactApiClient>(o =>
            {
                o.Address = new Uri(serviceOptions.ContactService.Url);
            })
            .AddInterceptor<ClientTracingInterceptor>();

            services.AddGrpcClient<LeadApiClient>(o =>
            {
                o.Address = new Uri(serviceOptions.ContactService.Url);
            });
        }

        private void RegisterAuth(IServiceCollection services)
        {
            var identityUrl = Configuration.GetValue<string>("IdentityUrl");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer((options) =>
                {
                    options.Authority = identityUrl;
                    options.RequireHttpsMetadata = false;
                    options.Audience = "account";
                });
        }
    }
}

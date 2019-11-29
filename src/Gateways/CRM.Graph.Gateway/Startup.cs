using System;
using System.Threading.Tasks;
using CRM.Graph.Gateway.Options;
using CRM.Graph.Gateway.Types;
using CRM.Metrics;
using CRM.Shared;
using CRM.Shared.CorrelationId;
using CRM.Shared.Interceptors;
using CRM.Shared.Services;
using CRM.Tracing.Jaeger;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Playground;
using HotChocolate.Execution.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using OpenTracing.Contrib.Grpc.Interceptors;
using static CRM.Protobuf.Contacts.V1.ContactApi;
using static CRM.Protobuf.Contacts.V1.LeadApi;

namespace CRM.Graph.Gateway
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private IHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        private ServiceOptions ServiceOptions
        {
            get
            {
                return Configuration.GetOptions<ServiceOptions>("Services");
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                var cors = Configuration.GetValue<string>("Cors:Origins").Split(',');
                options.AddPolicy("CorsPolicy",
                          policy =>
                          {
                              var builder = policy
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials()
                                .WithOrigins(cors)
                                /* https://github.com/aspnet/AspNetCore/issues/4457 */
                                .SetIsOriginAllowed(host => true);
                          });
            });

            services.AddCorrelationId();
            services.AddJaeger();
            services.AddAppMetrics();

            services.AddHealthChecks()
                    .AddCheck("self", () => HealthCheckResult.Healthy())
                    .AddUrlGroup(new Uri(System.IO.Path.Combine(ServiceOptions.CommunicationService.Url, "health")), name: "communicationapi-check", tags: new string[] { "communicationapi" })
                    .AddUrlGroup(new Uri(System.IO.Path.Combine(ServiceOptions.ContactService.Url, "health")), name: "contactapi-check", tags: new string[] { "contactapi" })
                    .AddUrlGroup(new Uri(System.IO.Path.Combine(ServiceOptions.IdentityService.Url, "health")), name: "identityapi-check", tags: new string[] { "idsvrapi" });

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

            app.UsePathBase(Configuration["PathBase"]);
            app.UseCorrelationId();
            app.UseAppMetrics();
            app.UseCors("CorsPolicy");
            app.UseRouting();
            app.UseGraphQL("/graphql")
                .UsePlayground(new PlaygroundOptions()
                {
                    QueryPath = "/graphql",
                    Path = "/ui/playground"
                });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");

                if (Environment.IsDevelopment())
                {
                    endpoints.MapGet(Configuration["PathBase"], context =>
                    {
                        context.Response.Redirect($"{Configuration["PathBase"]}ui/playground");
                        return Task.CompletedTask;
                    });
                }
            });
        }

        private void GraphQLRegister(IServiceCollection services)
        {
            services.AddGraphQL(sp => Schema.Create(c =>
            {
                c.RegisterServiceProvider(sp);
                c.RegisterQueryType<QueryType>();
                c.RegisterMutationType<MutationType>();
            }), new QueryExecutionOptions
            {
                IncludeExceptionDetails = true,
                TracingPreference = TracingPreference.Never
            });
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
            services.AddTransient<ClientLoggerInterceptor>();
            var serviceOptions = Configuration.GetOptions<ServiceOptions>("Services");

            services.AddGrpcClient<ContactApiClient>(o =>
            {
                o.Address = new Uri(serviceOptions.ContactService.Url);
            })
            .AddInterceptor<ClientLoggerInterceptor>()
            .AddInterceptor<ClientTracingInterceptor>();

            services.AddGrpcClient<LeadApiClient>(o =>
            {
                o.Address = new Uri(serviceOptions.ContactService.Url);
            });
        }
    }
}

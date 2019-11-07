using App.Metrics;
using CRM.Shared;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using System;
using App.Metrics.Formatters.InfluxDB;

namespace CRM.Metrics
{
    public static class Extensions
    {
        private static bool _initialized;
        private const string SectionName = "metrics";

        public static IServiceCollection AddAppMetrics(this IServiceCollection services, string sectionName = SectionName)
        {
            if (_initialized)
            {
                return services;
            }
            _initialized = true;

            var options = GetAppMetricOptions(services, sectionName);
            services.AddSingleton(options);

            var metricsBuilder = new MetricsBuilder().Configuration.Configure(cfg =>
            {
                var tags = options.Tags;
                if (tags == null)
                {
                    return;
                }
                tags.TryGetValue("app", out var app);
                tags.TryGetValue("env", out var env);
                tags.Add("server", Environment.MachineName);

                foreach (var tag in tags)
                {
                    if (!cfg.GlobalTags.ContainsKey(tag.Key))
                    {
                        cfg.GlobalTags.Add(tag.Key, tag.Value);
                    }
                }
            });

            if (options.InfluxEnabled)
            {
                metricsBuilder.Report.ToInfluxDb(o =>
                {
                    o.InfluxDb.Database = options.Database;
                    o.InfluxDb.BaseUri = new Uri(options.InfluxUrl);
                    o.InfluxDb.CreateDataBaseIfNotExists = true;
                    o.FlushInterval = TimeSpan.FromSeconds(options.Interval);
                    o.MetricsOutputFormatter = new MetricsInfluxDbLineProtocolOutputFormatter();
                });
            }

            var metrics = metricsBuilder.Build();
            var metricsWebHostOptions = GetMetricsWebHostOptions(options);

            using var seriveProvider = services.BuildServiceProvider();
            var configuration = seriveProvider.GetService<IConfiguration>();
            services.AddMetricsTrackingMiddleware(configuration);
            services.AddMetricsEndpoints(configuration);
            services.AddMetricsReportingHostedService(metricsWebHostOptions.UnobservedTaskExceptionHandler);
            services.AddMetricsTrackingMiddleware(configuration);
            services.AddMetricsEndpoints(configuration);
            services.AddMetrics(metrics);

            return services;
        }

        public static IApplicationBuilder UseAppMetrics(this IApplicationBuilder app)
        {
            MetricsOptions options;
            using (var scope = app.ApplicationServices.CreateScope())
            {
                options = scope.ServiceProvider.GetService<MetricsOptions>();
            }

            return !options.Enabled
                ? app
                : app
                    // .UseHealthAllEndpoints()
                    .UseMetricsAllEndpoints()
                    .UseMetricsAllMiddleware();
        }

        private static MetricsWebHostOptions GetMetricsWebHostOptions(MetricsOptions metricsOptions)
        {
            var options = new MetricsWebHostOptions();
            if (!metricsOptions.Enabled)
            {
                return options;
            }
            if (!metricsOptions.PrometheusEnabled)
            {
                return options;
            }

            options.EndpointOptions = endpointOptions =>
            {
                switch (metricsOptions.PrometheusFormatter?.ToLowerInvariant() ?? string.Empty)
                {
                    case "protobuf":
                        endpointOptions.MetricsEndpointOutputFormatter =
                            new MetricsPrometheusProtobufOutputFormatter();
                        break;
                    default:
                        endpointOptions.MetricsEndpointOutputFormatter =
                            new MetricsPrometheusTextOutputFormatter();
                        break;
                }
            };

            return options;
        }

        private static MetricsOptions GetAppMetricOptions(IServiceCollection services, string sectionName)
        {
            using (var seriveProvider = services.BuildServiceProvider())
            {
                var configuration = seriveProvider.GetService<IConfiguration>();
                return configuration.GetOptions<MetricsOptions>(sectionName);
            }
        }
    }
}

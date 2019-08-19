
using System;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Enrichers;
using CRM.Shared.Logging.Enrichers;

namespace CRM.Shared.Logging
{
    public static class Extensions
    {
        public static IHostBuilder UseLogging(this IHostBuilder hostBuilder, string applicationName = "")
        {
            hostBuilder.UseSerilog((context, loggerConfiguration) =>
            {
                var appOptions = context.Configuration.GetOptions<AppOptions>("App");
                var loggingOptions = context.Configuration.GetOptions<LoggingOptions>("Logging");

                if (!Enum.TryParse<LogEventLevel>(loggingOptions.Level, true, out var level))
                {
                    level = LogEventLevel.Information;
                }
                applicationName = string.IsNullOrWhiteSpace(applicationName) ? appOptions.Name : applicationName;
                loggerConfiguration.Enrich.FromLogContext()
                    .MinimumLevel.Is(level)
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                    .MinimumLevel.Override("System", LogEventLevel.Warning)
                    .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                    .Enrich.WithProperty("ApplicationName", applicationName)
                    .Enrich.With<OpenTracingContextLogEventEnricher>();

                Configure(loggerConfiguration, level, loggingOptions);
            });

            return hostBuilder;
        }

        private static void Configure(LoggerConfiguration loggerConfiguration, LogEventLevel level, 
            LoggingOptions loggingOptions)
        {
            if (loggingOptions.ConsoleEnabled)
            {
                loggerConfiguration
                    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3} {Properties:j}] {Message:lj}{NewLine}{Exception}");
            }
        }
    }
}
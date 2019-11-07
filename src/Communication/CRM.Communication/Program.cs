using System;
using App.Metrics;
using App.Metrics.AspNetCore;
using CRM.Configuration.Vault;
using CRM.Shared.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace CRM.Communication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true);
                    config.AddEnvironmentVariables();
                    config.AddCommandLine(args);
                })
                .UseLogging()
                .UseVault()
                // .ConfigureMetricsWithDefaults(buildder => {
                //     buildder.Report.ToInfluxDb("http://localhost:8086", "crm2", TimeSpan.FromSeconds(1));
                // })
                // .UseMetrics()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

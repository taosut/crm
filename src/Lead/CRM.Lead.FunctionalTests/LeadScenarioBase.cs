
using CRM.Lead.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace CRM.Lead.FunctionalTests
{
    public class LeadScenarioBase
    {
        public IHost CreateTestHost()
        {
            return new HostBuilder()
                .ConfigureWebHost(webBuilder => {
                    webBuilder.UseTestServer()
                        .ConfigureAppConfiguration((context, config) =>
                        {
                            config.AddJsonFile("appsettings.json", optional: true);
                            config.AddEnvironmentVariables();
                        })
                        .UseStartup<Startup>();
                })
                .Build();
        }
    }
}
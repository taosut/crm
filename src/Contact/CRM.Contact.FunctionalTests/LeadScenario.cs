using Xunit;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace CRM.Contact.FunctionalTests
{
    public class LeadScenario : IClassFixture<WebApplicationFactory<CRM.Contact.Startup>>
    {
        protected readonly WebApplicationFactory<Startup> _factory;

        public LeadScenario(WebApplicationFactory<Startup> factory)
        {
            var projectDir = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(projectDir, "appsettings.json");

            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((SourceContext, config) =>
                {
                    config.AddJsonFile(configPath);
                });
            });
        }

        [Fact]
        public async Task Call_ping_and_return_pong()
        {
            GrpcChannelOptions options = new GrpcChannelOptions()
            {
                HttpClient = _factory.CreateClient()
            };
            var channel = GrpcChannel.ForAddress("http://localhost", options);
            var client = new CRM.Contact.V1.LeadApi.LeadApiClient(channel);

            var result = await client.PingAsync(new Empty());

            Assert.NotNull(result);
            Assert.Equal("Pong", result.Message);
        }
    }
}


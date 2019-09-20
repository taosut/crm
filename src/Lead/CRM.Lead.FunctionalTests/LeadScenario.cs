using Xunit;
using Microsoft.AspNetCore.TestHost;
using Grpc.Net.Client;
using static LeadApi.Lead;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;

namespace CRM.Lead.FunctionalTests
{
    public class LeadScenario : LeadScenarioBase
    {
        [Fact]
        public async Task Call_ping_and_return_pong()
        {
            using var host = CreateTestHost();
            await host.StartAsync();

            var grpcClient = GrpcClient.Create<LeadClient>(host.GetTestClient());
            var result = await grpcClient.PingAsync(new Empty());

            Assert.NotNull(result);
            Assert.Equal("Pong", result.Message);
        }

        [Fact]
        public async Task Call_ping_and_return_pong1()
        {
            using var host = CreateTestHost();
            await host.StartAsync();

            var grpcClient = GrpcClient.Create<LeadClient>(host.GetTestClient());
            var result = await grpcClient.PingAsync(new Empty());

            Assert.NotNull(result);
            Assert.NotEqual("Pong", result.Message);
        }
    }
}

using System;
using System.Threading.Tasks;
using CRM.Migration;
using DbUp;
using DbUp.Engine;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace crm.migration
{
    class Program
    {
        private static IConfiguration _configuration;

        private enum DBName
        {
            Contact = 0,
            Identity = 1
        }

        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var lastArg = 0;
            for (; lastArg < args.Length; lastArg++)
            {
                if (IsArg(args[lastArg], "contact"))
                {
                    Log.Information("Run migration - Contact Db");
                    Run(DBName.Contact);
                    continue;
                }
                if (IsArg(args[lastArg], "identity"))
                {
                    Log.Information("Run migration - Identity Db");
                    Run(DBName.Identity);
                    await IdentityConfig.SeedData(_configuration.GetConnectionString(DBName.Identity.ToString()));
                    continue;
                }
                else
                {
                    throw new ArgumentOutOfRangeException($"{args[lastArg]} not found.");
                }
            }
        }

        private static void Run(DBName dBName)
        {
            var connString = _configuration.GetConnectionString(dBName.ToString());
            var scriptFolderPath = $"./Scripts/{dBName.ToString()}";

            EnsureDatabase.For.PostgresqlDatabase(connString);

            var upgrader = DeployChanges.To
                .PostgresqlDatabase(connString)
                .WithScriptsFromFileSystem(scriptFolderPath, new SqlScriptOptions
                {
                    RunGroupOrder = DbUpDefaults.DefaultRunGroupOrder + 1
                })
                .LogToAutodetectedLog()
                .Build();

            upgrader.PerformUpgrade();
        }

        private static bool IsArg(string candidate, string name)
        {
            return (name != null && candidate.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}

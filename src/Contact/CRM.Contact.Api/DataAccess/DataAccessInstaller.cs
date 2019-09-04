using CRM.Contact.Api.Domain;
using CRM.Shared.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace CRM.Contact.Api.DataAccess
{
    public static class DataAccessInstaller
    {
        public static IServiceCollection AddDataAccessConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork>(sp =>
            {
                return new UnitOfWork(() => new NpgsqlConnection(configuration.GetConnectionString("default")));
            });

            services.AddScoped<IContactRepository, ContactRepository>();

            return services;
        }
    }
}
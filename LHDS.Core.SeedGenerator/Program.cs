// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Services.Foundations.Audits;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using LHDS.Core.Services.Foundations.Suppliers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LHDS.Core.SeedGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var environmentName = args.FirstOrDefault() ?? "Development";

            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("local.appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfiguration configuration = configurationBuilder.Build();

            var serviceProvider = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.AddApplicationInsights();
                })
                .AddTransient<IStorageBroker, StorageBroker>()
                .AddTransient<IAuditService, AuditService>()
                .AddTransient<IIngestionTrackingService, IngestionTrackingService>()
                .AddTransient<ISupplierService, SupplierService>()
                .BuildServiceProvider();
        }
    }
}
// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.SeedGenerator.Services;
using LHDS.Core.Services.Foundations.IngestionTrackingAudits;
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
            var logfile = $"{AppDomain.CurrentDomain.BaseDirectory}\\CrashDump.txt";
            AppDomain.CurrentDomain.UnhandledException += (sender, args1) =>
            {
                try
                {
                    File.WriteAllText(logfile, args1.ExceptionObject.ToString());
                }
                catch
                {
                }
            };

            var environmentName = args.FirstOrDefault() ?? "Development";

            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("local.appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables("");

            IConfiguration configuration = configurationBuilder.Build();

            var serviceProvider = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddConsole();
                })
                .AddSingleton<IConfiguration>(_ => configuration)
                .AddScoped<IStorageBroker, StorageBroker>()
                .AddTransient<IDateTimeBroker, DateTimeBroker>()
                .AddTransient<ILoggingBroker, LoggingBroker>()
                .AddTransient<IIngestionTrackingAuditService, IngestionTrackingAuditService>()
                .AddTransient<IIngestionTrackingService, IngestionTrackingService>()
                .AddTransient<ISupplierService, SupplierService>()
                .AddTransient<IGenerate, Generate>()
                .BuildServiceProvider();

            var generate = serviceProvider.GetService<IGenerate>();

            try
            {
                if (generate != null)
                {
                    generate.GenerateRecords(2, 5, 3);
                }
            }
            catch (Exception ex)
            {
                ILoggingBroker logger = (ILoggingBroker)serviceProvider.GetService(typeof(ILoggingBroker));
                logger?.LogError(ex);
                Console.WriteLine(ex.Message);
            }
        }
    }
}
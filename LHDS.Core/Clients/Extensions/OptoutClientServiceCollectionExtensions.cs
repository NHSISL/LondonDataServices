// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Core.Brokers.CsvMappers;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Mesh;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Orchestrations.OptOuts;
using LHDS.Core.Services.Foundations.Audits;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using LHDS.Core.Services.Foundations.Mesh;
using LHDS.Core.Services.Processings.CsvMappers;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.Mesh;
using LHDS.Core.Services.Processings.OptOuts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LHDS.Core.Clients.Extensions
{
    public static class OptOutClientServiceCollectionExtensions
    {
        public static IServiceCollection AddOptutClient(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IConfiguration>(_ => configuration);

            var optOptOutConfiguration = new OptOutConfiguration
            {
                ExpiredAfterDays = int.Parse(GetSettings(configuration, "OptOutSettings:ExpiredAfterDays", true)),
                InputFolder = GetSettings(configuration, "OptOutSettings:InputFolder", true),

                OptOutFileHasHeader =
                    bool.Parse(GetSettings(configuration, "OptOutSettings:OptOutFileHasHeader", true)),

                OutputFolder = GetSettings(configuration, "OptOutSettings:OutputFolder"),

                OptOutFileRequireTrailingComma =
                    bool.Parse(GetSettings(configuration, "OptOutSettings:OptOutFileRequireTrailingComma", true)),
            };

            services.AddSingleton(optOptOutConfiguration);
            services.AddTransient<IOptOutClient, OptOutClient>();

            AddProcessingServices(services);
            AddBrokers(services);
            AddServices(services);

            return services;
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddSingleton<IMeshService, MeshService>();
            services.AddSingleton<ICsvMapperService, CsvMapperService>();
            services.AddTransient<IIngestionTrackingService, IngestionTrackingService>();
            services.AddSingleton<IAuditService, AuditService>();
        }

        private static void AddBrokers(IServiceCollection services)
        {
            services.AddTransient<IStorageBroker, StorageBroker>();
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            services.AddSingleton<ILoggingBroker, LoggingBroker>();
            services.AddTransient<IBlobStorageBroker, BlobStorageBroker>();
            services.AddTransient<IStorageBroker, StorageBroker>();
            services.AddTransient<IBlobStorageBrokerSettings, BlobStorageBrokerSettings>();
            services.AddSingleton<IMeshBroker, MeshBroker>();
            services.AddSingleton<ICsvMapperBroker, CsvMapperBroker>();
        }

        private static void AddProcessingServices(IServiceCollection services)
        {
            services.AddSingleton<IOptOutProcessingService, OptOutProcessingService>();
            services.AddSingleton<IDocumentProcessingService, DocumentProcessingService>();
            services.AddSingleton<IMeshProcessingService, MeshProcessingService>();
            services.AddSingleton<ICsvMapperProcessingService, CsvMapperProcessingService>();
        }

        private static string GetSettings(IConfiguration configuration, string configurationKey, bool mandatory = true)
        {
            var value = configuration[configurationKey];

            if (string.IsNullOrEmpty(value))
            {
                if (mandatory)
                {
                    throw new Exception($"Configuration value {configurationKey} does not exist");
                }
            }

            return value;
        }
    }
}

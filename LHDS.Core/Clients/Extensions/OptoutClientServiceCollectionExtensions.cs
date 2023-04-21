// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Net.Http;
using Azure.Core.Pipeline;
using Azure.Identity;
using Azure.Storage.Blobs;
using LHDS.Core.Brokers.CsvMappers;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Mesh;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Orchestrations.OptOuts;
using LHDS.Core.Services.Foundations.Audits;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using LHDS.Core.Services.Foundations.Mesh;
using LHDS.Core.Services.Foundations.OptOuts;
using LHDS.Core.Services.Orchestrations.OptOuts;
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
            var blobServiceUri = GetSettings(configuration, "BlobStorage:azureBlobServiceUri", true);
            var azureTenantId = GetSettings(configuration, "BlobStorage:azureTenantId", true);
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

            var meshConfig = new NEL.MESH.Models.Configurations.MeshConfiguration
            {
                MailboxId = GetSettings(configuration, "MeshConfiguration:MailboxId", true),
                Password = GetSettings(configuration, "MeshConfiguration:Password", true),
                Key = GetSettings(configuration, "MeshConfiguration:Key", true),
                //RootCertificate = GetSettings(configuration, "MeshConfiguration:RootCertificate", true),
                //IntermediateCertificates = GetSettings(configuration, "MeshConfiguration:IntermediateCertificates", true),
            };

            services.AddSingleton(meshConfig);

            AddClients(services, blobServiceUri, azureTenantId, optOptOutConfiguration);
            AddProcessingServices(services);
            AddBrokers(services);
            AddServices(services);
            AddOrchestrations(services);
            services.AddSingleton(meshConfig);

            return services;
        }

        private static void AddOrchestrations(IServiceCollection services)
        {
            services.AddSingleton<IOptOutOrchestrationService, OptOutOrchestrationService>();
        }

        private static void AddClients(IServiceCollection services, string blobServiceUri, string azureTenantId, OptOutConfiguration optOptOutConfiguration)
        {
            var blobServiceClientOptions = new BlobClientOptions()
            {
                Transport = new HttpClientTransport(new HttpClient { Timeout = new TimeSpan(1, 0, 0) }),
                Retry = { NetworkTimeout = new TimeSpan(1, 0, 0) },
                EnableTenantDiscovery = true
            };

            services.AddSingleton(
                new BlobServiceClient(
                    serviceUri: new Uri(blobServiceUri),
                    credential: new DefaultAzureCredential(
                        new DefaultAzureCredentialOptions
                        {
                            VisualStudioTenantId = azureTenantId,
                        }),
                    options: blobServiceClientOptions));

            services.AddSingleton(optOptOutConfiguration);
            services.AddTransient<IOptOutClient, OptOutClient>();
            services.AddTransient<IAzureBlobClient, AzureBlobClient>();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddSingleton<IAuditService, AuditService>();
            services.AddSingleton<ICsvMapperService, CsvMapperService>();
            services.AddSingleton<IDocumentService, DocumentService>();
            services.AddTransient<IIngestionTrackingService, IngestionTrackingService>();
            services.AddSingleton<IMeshService, MeshService>();
            services.AddSingleton<IOptOutService, OptOutService>();
        }

        private static void AddBrokers(IServiceCollection services)
        {
            services.AddSingleton<ILoggingBroker, LoggingBroker>();
            services.AddTransient<IBlobStorageBroker, BlobStorageBroker>();
            services.AddTransient<IBlobStorageBrokerSettings, BlobStorageBrokerSettings>();
            services.AddSingleton<ICsvMapperBroker, CsvMapperBroker>();
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            services.AddTransient<IMeshBroker, MeshBroker>();
            services.AddTransient<IStorageBroker, StorageBroker>();
        }

        private static void AddProcessingServices(IServiceCollection services)
        {
            services.AddSingleton<ICsvMapperProcessingService, CsvMapperProcessingService>();
            services.AddSingleton<IDocumentProcessingService, DocumentProcessingService>();
            services.AddSingleton<IMeshProcessingService, MeshProcessingService>();
            services.AddSingleton<IOptOutProcessingService, OptOutProcessingService>();
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

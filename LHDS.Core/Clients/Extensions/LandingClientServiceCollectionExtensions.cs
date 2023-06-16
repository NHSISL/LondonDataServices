// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Net.Http;
using Azure.Core.Pipeline;
using Azure.Identity;
using Azure.Storage.Blobs;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Downloads;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Orchestrations.Downloads;
using LHDS.Core.Providers.Downloads;
using LHDS.Core.Services.Foundations.Audits;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.Downloads;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using LHDS.Core.Services.Orchestrations.Downloads;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LHDS.Core.Clients.Extensions
{
    public static class LandingClientServiceCollectionExtensions
    {
        public static IServiceCollection AddLandingClient(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return AddLandingClient(services, configuration, acceptanceTest: false);
        }

        internal static IServiceCollection AddLandingClientForAcceptance(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return AddLandingClient(services, configuration, acceptanceTest: true);
        }

        private static IServiceCollection AddLandingClient(
            this IServiceCollection services,
            IConfiguration configuration,
            bool acceptanceTest)
        {
            services.AddSingleton<IConfiguration>(_ => configuration);

            AddProviders(services);
            AddBrokers(services, configuration, acceptanceTest);
            AddServices(services);
            AddProcessingServices(services);
            AddOrchestrations(services);
            AddClients(services);

            return services;
        }

        private static void AddProviders(IServiceCollection services)
        {
            services.AddTransient<IDownloadAbstractProvider, DownloadAbstractProvider>();
        }

        private static void AddBrokers(IServiceCollection services, IConfiguration configuration, bool acceptanceTest)
        {
            services.AddTransient<ILoggingBroker, LoggingBroker>();
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            services.AddTransient<IIdentifierBroker, IdentifierBroker>();
            services.AddTransient<IStorageBroker, StorageBroker>();

            if (!acceptanceTest)
            {
                services.AddTransient<IBlobStorageBroker, BlobStorageBroker>();
                services.AddTransient<IBlobStorageBrokerSettings, BlobStorageBrokerSettings>();
                services.AddTransient<IDownloadBroker, DownloadBroker>();

                var blobServiceUri = GetSettings(configuration, "BlobStorage:azureBlobServiceUri", true);
                var azureTenantId = GetSettings(configuration, "BlobStorage:azureTenantId", true);

                var blobServiceClientOptions = new BlobClientOptions()
                {
                    Transport = new HttpClientTransport(new HttpClient { Timeout = new TimeSpan(1, 0, 0) }),
                    Retry = { NetworkTimeout = new TimeSpan(1, 0, 0) },
                    EnableTenantDiscovery = true
                };

                var landingConfiguration = new LandingConfiguration
                {
                    LandingSupplierId = Guid.Parse(GetSettings(configuration, "LandingSettings:LandingSupplierId", true)),
                    EncryptedFolder = GetSettings(configuration, "LandingSettings:EncryptedFolder", true),
                    DecryptedFolder = GetSettings(configuration, "LandingSettings:DecryptedFolder", true),
                };

                services.AddSingleton(landingConfiguration);

                services.AddSingleton(
                    new BlobServiceClient(
                        serviceUri: new Uri(blobServiceUri),
                        credential: new DefaultAzureCredential(
                            new DefaultAzureCredentialOptions
                            {
                                VisualStudioTenantId = azureTenantId,
                            }),
                        options: blobServiceClientOptions));

                services.AddTransient<IAzureBlobClient, AzureBlobClient>();
            }
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IDownloadService, DownloadService>();
            services.AddTransient<IIngestionTrackingService, IngestionTrackingService>();
            services.AddSingleton<IAuditService, AuditService>();
        }

        private static void AddProcessingServices(IServiceCollection services)
        { }

        private static void AddOrchestrations(IServiceCollection services)
        {
            services.AddTransient<IDownloadOrchestrationService, DownloadOrchestrationService>();
        }

        private static void AddClients(IServiceCollection services)
        {
            services.AddTransient<ILandingClient, LandingClient>();
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

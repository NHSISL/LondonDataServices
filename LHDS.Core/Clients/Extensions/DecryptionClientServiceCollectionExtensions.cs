// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Net.Http;
using Azure.Core.Pipeline;
using Azure.Identity;
using Azure.Storage.Blobs;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Decryptions;
using LHDS.Core.Brokers.Downloads;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Providers.Cryptography;
using LHDS.Core.Providers.Cryptography.Gpg;
using LHDS.Core.Providers.Downloads;
using LHDS.Core.Services.Foundations.Audits;
using LHDS.Core.Services.Foundations.Decryptions;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.Downloads;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using LHDS.Core.Services.Foundations.Suppliers;
using LHDS.Core.Services.Orchestrations.Decryptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LHDS.Core.Clients.Extensions
{
    public static class DecryptionClientServiceCollectionExtensions
    {
        public static IServiceCollection AddDecryptionClient(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return AddDecryptionClient(services, configuration, acceptanceTest: false);
        }

        internal static IServiceCollection AddDecryptionClientForAcceptance(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return AddDecryptionClient(services, configuration, acceptanceTest: true);
        }

        private static IServiceCollection AddDecryptionClient(
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
            services.AddTransient<ICryptographyAbstractProvider, CryptographyAbstractProvider>();
            services.AddTransient<ICryptographyProvider, GpgCryptographyProvider>();
            services.AddTransient<IGpgCryptographyProviderSettings, GpgCryptographyProviderSettings>();
        }

        private static void AddBrokers(IServiceCollection services, IConfiguration configuration, bool acceptanceTest)
        {
            services.AddTransient<IStorageBroker, StorageBroker>();
            services.AddTransient<IDecryptionBroker, DecryptionBroker>();
            services.AddTransient<ILoggingBroker, LoggingBroker>();
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            services.AddTransient<IIdentifierBroker, IdentifierBroker>();

            if (!acceptanceTest)
            {
                var blobServiceUri = GetSettings(configuration, "BlobStorage:azureBlobServiceUri", true);
                var azureTenantId = GetSettings(configuration, "BlobStorage:azureTenantId", true);

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

                services.AddTransient<IBlobStorageBroker, BlobStorageBroker>();
                services.AddTransient<IDownloadBroker, DownloadBroker>();
                services.AddTransient<IBlobStorageBrokerSettings, BlobStorageBrokerSettings>();
                services.AddTransient<IAzureBlobClient, AzureBlobClient>();
            }
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IDownloadService, DownloadService>();
            services.AddTransient<IIngestionTrackingService, IngestionTrackingService>();
            services.AddTransient<ISupplierService, SupplierService>();
            services.AddTransient<IAuditService, AuditService>();
            services.AddTransient<IDecryptionOrchestrationService, DecryptionOrchestrationService>();
            services.AddTransient<IDecryptionService, DecryptionService>();
        }

        private static void AddProcessingServices(IServiceCollection services)
        { }

        private static void AddOrchestrations(IServiceCollection services)
        { }

        private static void AddClients(IServiceCollection services)
        {
            services.AddTransient<IDecryptionClient, DecryptionClient>();
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

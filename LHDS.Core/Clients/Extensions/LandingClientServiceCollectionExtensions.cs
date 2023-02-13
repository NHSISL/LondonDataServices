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
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Providers.Downloads;
using LHDS.Core.Providers.Downloads.FtpDownloads;
using LHDS.Core.Services.Foundations.Audits;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.Downloads;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using LHDS.Core.Services.Orchestrations.Downloads;
using LHDS.Decryptions.Client.Clients;
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
            var blobServiceUri = GetSettings(configuration, "blobStorage:azureBlobStoreUri", true);
            var azureTenantId = GetSettings(configuration, "blobStorage:azureTenantId", true);

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

            services.AddSingleton<IConfiguration>(_ => configuration);
            services.AddTransient<ILandingClient, LandingClient>();
            services.AddTransient<IDownloadOrchestrationService, DownloadOrchestrationService>();
            services.AddSingleton<ILoggingBroker, LoggingBroker>();
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IDownloadService, DownloadService>();
            services.AddTransient<IIngestionTrackingBroker, IngestionTrackingService>();
            services.AddSingleton<IAuditService, AuditService>();
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            services.AddTransient<IBlobStorageBroker, BlobStorageBroker>();
            services.AddTransient<IDownloadBroker, DownloadBroker>();
            services.AddTransient<IStorageBroker, StorageBroker>();
            services.AddTransient<IBlobStorageBrokerSettings, BlobStorageBrokerSettings>();
            services.AddTransient<IDownloadAbstractProvider, DownloadAbstractProvider>();
            services.AddTransient<IAzureBlobClient, AzureBlobClient>();

            return services;
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

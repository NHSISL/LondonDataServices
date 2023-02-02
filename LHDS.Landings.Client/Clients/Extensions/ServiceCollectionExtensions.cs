// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Net.Http;
using Azure.Core.Pipeline;
using Azure.Identity;
using Azure.Storage.Blobs;
using LHDS.Landings.Client.Brokers.DateTimes;
using LHDS.Landings.Client.Brokers.Downloads;
using LHDS.Landings.Client.Brokers.Loggings;
using LHDS.Landings.Client.Brokers.Storages.Blobs;
using LHDS.Landings.Client.Brokers.Storages.Sql;
using LHDS.Landings.Client.Providers.Downloads;
using LHDS.Landings.Client.Providers.Downloads.FtpDownloads;
using LHDS.Landings.Client.Services.Foundations.Audits;
using LHDS.Landings.Client.Services.Foundations.Documents;
using LHDS.Landings.Client.Services.Foundations.Downloads;
using LHDS.Landings.Client.Services.Foundations.IngestionTrackings;
using LHDS.Landings.Client.Services.Orchestrations.Downloads;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NEL.DDS.InterfaceLayer.Function.Download.Client.AzureBlobs;

namespace LHDS.Landings.Client.Clients.Extensions
{
    public static class LandingClientServiceCollectionExtensions
    {
        public static IServiceCollection AddLandingClient(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var blobServiceUri = GetSettings(configuration, "blobStorage:azureBlobStoreUri", true);

            var blobServiceClientOptions = new BlobClientOptions()
            {
                Transport = new HttpClientTransport(new HttpClient { Timeout = new TimeSpan(1, 0, 0) }),
                Retry = { NetworkTimeout = new TimeSpan(1, 0, 0) },
            };

            services.AddSingleton(
                new BlobServiceClient(
                    serviceUri: new Uri(blobServiceUri),
                    credential: new DefaultAzureCredential(),
                    options: blobServiceClientOptions));

            services.AddTransient<ILandingClient, LandingClient>();
            services.AddTransient<IDownloadOrchestrationService, DownloadOrchestrationService>();
            services.AddTransient<ILoggingBroker, LoggingBroker>();
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IDownloadService, DownloadService>();
            services.AddTransient<IIngestionTrackingService, IngestionTrackingService>();
            services.AddTransient<IAuditService, AuditService>();
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

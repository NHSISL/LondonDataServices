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
using LHDS.Landings.Client.Brokers.Storages;
using LHDS.Landings.Client.Brokers.Storages.Blobs;
using LHDS.Landings.Client.Providers.Downloads;
using LHDS.Landings.Client.Services.Foundations.Audits;
using LHDS.Landings.Client.Services.Foundations.Documents;
using LHDS.Landings.Client.Services.Foundations.Downloads;
using LHDS.Landings.Client.Services.Foundations.IngestionTrackings;
using LHDS.Landings.Client.Services.Orchestrations.Downloads;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LHDS.Landings.Client.Clients.Extensions
{
    public static class LandingClientServiceCollectionExtensions
    {
        public static IServiceCollection AddLandingClient(
            this IServiceCollection services,
            IConfiguration configuration)
        {
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
            services.AddTransient<IDownloadAbstractProvider, DownloadAbstractProvider>();

            var blobServiceUri = configuration["blobStorage:AzureBlobStoreUri"];

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

            return services;
        }
    }
}

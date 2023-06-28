// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Azure.Core.Pipeline;
using Azure.Identity;
using Azure.Storage.Blobs;
using LHDS.Core.Brokers.CsvMappers;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Mesh;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Brokers.Mesh;
using LHDS.Core.Models.Orchestrations.Pds;
using LHDS.Core.Services.Foundations.Audits;
using LHDS.Core.Services.Foundations.CsvMappers;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using LHDS.Core.Services.Foundations.Mesh;
using LHDS.Core.Services.Foundations.PdsAudits;
using LHDS.Core.Services.Orchestrations.Pds;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LHDS.Core.Clients.Extensions
{
    public static class PdsClientServiceCollectionExtensions
    {
        public static IServiceCollection AddPdsClient(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return AddPdsClient(services, configuration, acceptanceTest: false);
        }

        internal static IServiceCollection AddPdsClientForAcceptance(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return AddPdsClient(services, configuration, acceptanceTest: true);
        }

        private static IServiceCollection AddPdsClient(
            this IServiceCollection services,
            IConfiguration configuration,
            bool acceptanceTest)
        {
            var blobServiceUri = GetSettings(configuration, "blobStorage:azureBlobServiceUri", true);
            var azureTenantId = GetSettings(configuration, "blobStorage:azureTenantId", true);

            services.AddSingleton<IConfiguration>(_ => configuration);
            var pdsConfiguration = new PdsConfiguration
            {
                InputFolder = GetSettings(configuration, "pdsSettings:inputFolder", true),

                PdsFileHasHeader =
                    bool.Parse(GetSettings(configuration, "pdsSettings:pdsFileHasHeader", true)),

                OutputFolder = GetSettings(configuration, "pdsSettings:outputFolder"),

                PdsFileRequireTrailingComma =
                    bool.Parse(GetSettings(configuration, "pdsSettings:pdsFileRequireTrailingComma", true)),

                To = GetSettings(configuration, "pdsSettings:to"),
                WorkflowId = GetSettings(configuration, "pdsSettings:workflowId"),
            };

            var meshConfig = new MeshConfiguration
            {
                MailboxId = GetSettings(configuration, "meshConfiguration:mailboxId", true),
                Password = GetSettings(configuration, "meshConfiguration:password", true),
                Key = GetSettings(configuration, "meshConfiguration:key", true),
                Url = GetSettings(configuration, "meshConfiguration:url", true),
                MexClientVersion = GetSettings(configuration, "meshConfiguration:mexClientVersion", true),
                MexOSName = GetSettings(configuration, "meshConfiguration:mexOSName", true),
                MexOSVersion = GetSettings(configuration, "meshConfiguration:mexOSVersion", true),


                MaxChunkSizeInMegabytes = int.Parse(
                    GetSettings(configuration, "meshConfiguration:mxChunkSizeInMegabytes", true)),
            };

            if (!acceptanceTest)
            {
                meshConfig.RootCertificate =
                    GetCertificate(configuration, "meshConfiguration:rootCertificate", true);

                meshConfig.IntermediateCertificates =
                    GetCertificates(configuration, "meshConfiguration:intermediateCertificates", false);

                meshConfig.ClientCertificate =
                    GetCertificate(configuration, "meshConfiguration:clientCertificate", true);
            }

            services.AddSingleton(meshConfig);
            AddClients(services, blobServiceUri, azureTenantId, pdsConfiguration);
            AddBrokers(services, acceptanceTest);
            AddOrchestrations(services);
            AddServices(services);

            return services;
        }

        private static void AddClients(
            IServiceCollection services,
            string blobServiceUri,
            string azureTenantId,
            PdsConfiguration pdsConfiguration)
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

            services.AddSingleton(pdsConfiguration);
            services.AddTransient<IPdsClient, PdsClient>();
            services.AddTransient<IAzureBlobClient, AzureBlobClient>();
        }

        private static void AddBrokers(IServiceCollection services, bool acceptanceTest)
        {
            services.AddTransient<ILoggingBroker, LoggingBroker>();
            services.AddTransient<ICsvMapperBroker, CsvMapperBroker>();
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            services.AddTransient<IIdentifierBroker, IdentifierBroker>();
            services.AddTransient<IStorageBroker, StorageBroker>();

            if (!acceptanceTest)
            {
                services.AddTransient<IBlobStorageBroker, BlobStorageBroker>();
                services.AddTransient<IBlobStorageBrokerSettings, BlobStorageBrokerSettings>();
                services.AddTransient<IMeshBroker, MeshBroker>();
            }
        }

        private static void AddOrchestrations(IServiceCollection services)
        {
            services.AddTransient<IPdsOrchestrationService, PdsOrchestrationService>();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddTransient<IAuditService, AuditService>();
            services.AddTransient<ICsvMapperService, CsvMapperService>();
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IIngestionTrackingService, IngestionTrackingService>();
            services.AddTransient<IMeshService, MeshService>();
            services.AddTransient<IPdsAuditService, PdsAuditService>();
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

        private static X509Certificate2 GetCertificate(IConfiguration configuration, string configurationKey, bool mandatory = true)
        {
            var value = configuration[configurationKey];

            if (string.IsNullOrEmpty(value))
            {
                if (mandatory)
                {
                    throw new Exception($"Configuration value {configurationKey} does not exist");
                }
            }

            byte[] certBytes = Convert.FromBase64String(value);

            return new X509Certificate2(certBytes);
        }

        private static X509Certificate2Collection GetCertificates(IConfiguration configuration, string configurationKey, bool mandatory = true)
        {
            List<string> intermediateCertificates =
               configuration.GetSection(configurationKey)
                   .Get<List<string>>();

            if (intermediateCertificates == null)
            {
                intermediateCertificates = new List<string>();
            }

            if (mandatory && intermediateCertificates.Count == 0)
            {
                throw new Exception($"Configuration value {configurationKey} does not exist");
            }

            var certificates = new X509Certificate2Collection();

            foreach (string item in intermediateCertificates)
            {
                byte[] certBytes = Convert.FromBase64String(item);
                certificates.Add(new X509Certificate2(certBytes));
            }

            return certificates;
        }
    }
}

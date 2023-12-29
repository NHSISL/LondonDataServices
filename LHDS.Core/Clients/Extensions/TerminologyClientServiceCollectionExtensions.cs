// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Azure.Core.Pipeline;
using Azure.Identity;
using Azure.Storage.Blobs;
using Hl7.Fhir.Specification.Terminology;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Hashing;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Ontologies;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Configurations;
using LHDS.Core.Models.Orchestrations.TerminologyMedata;
using LHDS.Core.Providers.Downloads;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.Ontologies;
using LHDS.Core.Services.Foundations.TerminologyArtifacts;
using LHDS.Core.Services.Foundations.TerminologyPolls;
using LHDS.Core.Services.Orchestrations.TerminologyDetails;
using LHDS.Core.Services.Orchestrations.TerminologyMetadata;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.Ontologies;
using LHDS.Core.Services.Processings.TerminologyArtifacts;
using LHDS.Core.Services.Processings.TerminologyPolls;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LHDS.Core.Clients.Extensions
{
    public static class TerminologyClientServiceCollectionExtensions
    {
        public static IServiceCollection AddTerminologyClient(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return AddTerminologyClient(services, configuration, acceptanceTest: false);
        }

        internal static IServiceCollection AddTerminologyClientForAcceptance(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return AddTerminologyClient(services, configuration, acceptanceTest: true);
        }

        private static IServiceCollection AddTerminologyClient(
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
            AddClients(services, configuration);

            return services;
        }

        private static void AddProviders(IServiceCollection services)
        {
            services.AddTransient<IDownloadAbstractProvider, DownloadAbstractProvider>();
        }

        private static void AddBrokers(IServiceCollection services, IConfiguration configuration, bool acceptanceTest)
        {
            if(!acceptanceTest) 
            {
                services.AddTransient<IOntologyBroker, OntologyBroker>();
            }

            services.AddTransient<ILoggingBroker, LoggingBroker>();
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            services.AddTransient<IBlobStorageBroker, BlobStorageBroker>();
            services.AddTransient<IIdentifierBroker, IdentifierBroker>();
            services.AddTransient<IStorageBroker, StorageBroker>();
            services.AddTransient<IHashBroker, HashBroker>();

            var terminologyMetdataConfiguration =
                configuration.GetSection("terminologyMetadataSettings").Get<TerminologyMetadataConfiguration>();

            ValidateTerminologyConfiguration(terminologyMetdataConfiguration);
            services.AddTransient<TerminologyMetadataConfiguration>(_ => terminologyMetdataConfiguration);
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddTransient<IOntologyService, OntologyService>();
            services.AddTransient<ITerminologyArtifactService, TerminologyArtifactService>();
            services.AddTransient<ITerminologyPollService, TerminologyPollService>();
            services.AddTransient<IDocumentService, DocumentService>();
        }

        private static void AddProcessingServices(IServiceCollection services)
        {
            services.AddTransient<IOntologyProcessingService, OntologyProcessingService>();
            services.AddTransient<ITerminologyArtifactProcessingService, TerminologyArtifactProcessingService>();
            services.AddTransient<ITerminologyPollProcessingService, TerminologyPollProcessingService>();
            services.AddTransient<IDocumentProcessingService, DocumentProcessingService>();
        }

        private static void AddOrchestrations(IServiceCollection services)
        {
            services.AddTransient<ITerminologyDetailOrchestrationService, TerminologyDetailOrchestrationService>();
            services.AddTransient<ITerminologyMetadataOrchestrationService, TerminologyMetadataOrchestrationService>();
        }

        private static void AddClients(IServiceCollection services, IConfiguration configuration)
        {
            var blobStorageSettings = configuration
                .GetSection("blobStorage").Get<BlobStorageSettings>();

            var blobServiceClientOptions = new BlobClientOptions()
            {
                Transport = new HttpClientTransport(new HttpClient { Timeout = new TimeSpan(1, 0, 0) }),
                Retry = { NetworkTimeout = new TimeSpan(1, 0, 0) },
                EnableTenantDiscovery = true
            };

            services.AddSingleton(
                new BlobServiceClient(
                    serviceUri: new Uri(blobStorageSettings.AzureBlobServiceUri),
                    credential: new DefaultAzureCredential(
                        new DefaultAzureCredentialOptions
                        {
                            VisualStudioTenantId = blobStorageSettings.AzureTenantId,
                        }),
                    options: blobServiceClientOptions));

            services.AddTransient<IAzureBlobClient, AzureBlobClient>();
            services.AddTransient<ITerminologyClient, TerminologyClient>();
        }

        private static void ValidateTerminologyConfiguration(
            TerminologyMetadataConfiguration? terminologyMetadataConfiguration)
        {
            if (terminologyMetadataConfiguration == null)
            {
                throw new InvalidConfigurationException(
                    "Configuration section 'terminologyMetadataSettings' not defined.");
            }

            Validate(
                (Rule: IsInvalid(terminologyMetadataConfiguration.ResourceURL),
                    Parameter: "terminologyMetadataSettings__ResourceURL"));
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Configuration value does not exist"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            StringBuilder validationErrors = new StringBuilder();
            validationErrors.AppendLine("Configuration error(s):");
            IDictionary errors = new Dictionary<string, List<string>>();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    validationErrors.AppendLine(
                        $"{parameter} -> Configuration value does not exist or does not meet validation criteria");

                    if (errors.Contains(parameter))
                    {
                        (errors[parameter] as List<string>)?.Add(rule.Message);
                        
                        return;
                    }

                    errors.Add(parameter, new List<string> { rule.Message });
                }
            }

            var invalidConfigurationException = new InvalidConfigurationException(
                message: validationErrors.ToString(),
                data: errors);

            invalidConfigurationException.ThrowIfContainsErrors();
        }
    }
}

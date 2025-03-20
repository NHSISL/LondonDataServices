// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using Azure.Core.Pipeline;
using Azure.Identity;
using Azure.Storage.Blobs;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Ontologies;
using LHDS.Core.Brokers.Securities;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Brokers.Ontologies;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Configurations;
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
            services.AddSingleton<IConfiguration>(_ => configuration);

            AddProviders(services);
            AddBrokers(services, claimsPrincipal: null, configuration);
            AddServices(services);
            AddProcessingServices(services);
            AddOrchestrations(services);
            AddCoordinations(services);
            AddClients(services, configuration);

            return services;
        }

        public static IServiceCollection AddTerminologyClient(
            this IServiceCollection services,
            IConfiguration configuration,
            ClaimsPrincipal claimsPrincipal)
        {
            services.AddSingleton<IConfiguration>(_ => configuration);

            AddProviders(services);
            AddBrokers(services, claimsPrincipal, configuration);
            AddServices(services);
            AddProcessingServices(services);
            AddOrchestrations(services);
            AddCoordinations(services);
            AddClients(services, configuration);

            return services;
        }

        private static void AddProviders(IServiceCollection services)
        { }

        private static void AddBrokers(
            IServiceCollection services,
            ClaimsPrincipal claimsPrincipal,
            IConfiguration configuration)
        {
            services.AddTransient<IBlobStorageBroker, BlobStorageBroker>();
            services.AddTransient<ILoggingBroker, LoggingBroker>();
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            services.AddTransient<IIdentifierBroker, IdentifierBroker>();
            services.AddTransient<IStorageBroker, StorageBroker>();
            services.AddTransient<IOntologyBroker, OntologyBroker>();

            OntologyConfiguration? ontologyConfiguration =
                configuration.GetSection("ontologySettings").Get<OntologyConfiguration>();

            ValidateOntologyConfiguration(ontologyConfiguration);

            if (ontologyConfiguration != null)
            {
                services.AddSingleton(ontologyConfiguration);
            }

            if (claimsPrincipal != null)
            {
                var securityBroker = new SecurityBroker(claimsPrincipal);
                services.AddTransient<ISecurityBroker>(_ => securityBroker);
            }
            else
            {
                services.AddTransient<ISecurityBroker, SecurityBroker>();
            }
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddTransient<ITerminologyPollService, TerminologyPollService>();
            services.AddTransient<ITerminologyArtifactService, TerminologyArtifactService>();
            services.AddTransient<IOntologyService, OntologyService>();
            services.AddTransient<IDocumentService, DocumentService>();

        }

        private static void AddProcessingServices(IServiceCollection services)
        {
            services.AddTransient<ITerminologyPollProcessingService, TerminologyPollProcessingService>();
            services.AddTransient<ITerminologyArtifactProcessingService, TerminologyArtifactProcessingService>();
            services.AddTransient<IOntologyProcessingService, OntologyProcessingService>();
            services.AddTransient<IDocumentProcessingService, DocumentProcessingService>();
        }

        private static void AddOrchestrations(IServiceCollection services)
        {
            services.AddTransient<ITerminologyMetadataOrchestrationService, TerminologyMetadataOrchestrationService>();
            services.AddTransient<ITerminologyDetailOrchestrationService, TerminologyDetailOrchestrationService>();
        }

        private static void AddCoordinations(IServiceCollection services)
        { }

        private static void AddClients(IServiceCollection services, IConfiguration configuration)
        {
            var blobStorageSettings = configuration.GetSection("blobStorage").Get<BlobStorageSettings>();
            ValidateBlobStorageSettings(blobStorageSettings);

            if (blobStorageSettings != null)
            {
                services.AddSingleton(blobStorageSettings.BlobContainers);

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
            }

            services.AddTransient<IAzureBlobClient, AzureBlobClient>();
            services.AddTransient<ITerminologyClient, TerminologyClient>();
        }

        private static void ValidateBlobStorageSettings(BlobStorageSettings? blobStorageSettings)
        {
            if (blobStorageSettings == null)
            {
                throw new InvalidConfigurationException(
                    "Configuration section 'blobStorage' not defined.");
            }

            Validate(
                (Rule: IsInvalid(blobStorageSettings.AzureBlobServiceUri),
                    Parameter: "blobStorage__azureBlobServiceUri"),

                (Rule: IsInvalid(blobStorageSettings.AzureTenantId),
                    Parameter: "blobStorage__azureTenantId"));
        }

        private static void ValidateOntologyConfiguration(OntologyConfiguration? ontologyConfiguration)
        {
            if (ontologyConfiguration == null)
            {
                throw new InvalidConfigurationException("Configuration section 'ontologySettings' not defined.");
            }

            Validate(
                (Rule: IsInvalid(ontologyConfiguration.TerminologyServerBaseUrl),
                    Parameter: "ontologySettings__terminologyServerBaseUrl"),

                (Rule: IsInvalid(ontologyConfiguration.TerminologyServerAuthenticationRelativeUrl),
                    Parameter: "ontologySettings__terminologyServerAuthenticationRelativeUrl"),

                (Rule: IsInvalid(ontologyConfiguration.TerminologyServerResourceRelativeUrl),
                    Parameter: "ontologySettings__terminologyServerResourceRelativeUrl"),

                (Rule: IsInvalid(ontologyConfiguration.ClientId),
                    Parameter: "ontologySettings__clientId"),

                (Rule: IsInvalid(ontologyConfiguration.ClientSecret),
                    Parameter: "ontologySettings__clientSecret"));
        }

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Configuration value does not exist"
        };

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
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

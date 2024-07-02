// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Azure.Core.Pipeline;
using Azure.Identity;
using Azure.Storage.Blobs;
using LHDS.Core.Brokers.CryptographyKeys;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Downloads;
using LHDS.Core.Brokers.Files;
using LHDS.Core.Brokers.Hashing;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.KeyVaults;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Configurations;
using LHDS.Core.Models.Orchestrations.EmisLandings;
using LHDS.Core.Providers.Downloads;
using LHDS.Core.Services.Coordinations.EmisLandings;
using LHDS.Core.Services.Foundations.CryptographicKeys;
using LHDS.Core.Services.Foundations.DataSetSpecifications;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.Downloads;
using LHDS.Core.Services.Foundations.IngestionTrackingAudits;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using LHDS.Core.Services.Foundations.SecureDatas;
using LHDS.Core.Services.Foundations.SubscriberAgreements;
using LHDS.Core.Services.Foundations.Suppliers;
using LHDS.Core.Services.Orchestrations.Downloads;
using LHDS.Core.Services.Orchestrations.EmisLandings;
using LHDS.Core.Services.Orchestrations.SubscriberCredentials;
using LHDS.Core.Services.Processings.CryptographicKeys;
using LHDS.Core.Services.Processings.DataSetSpecifications;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.Downloads;
using LHDS.Core.Services.Processings.IngestionTrackingAudits;
using LHDS.Core.Services.Processings.IngestionTrackings;
using LHDS.Core.Services.Processings.OptOuts;
using LHDS.Core.Services.Processings.SecureDatas;
using LHDS.Core.Services.Processings.SubscriberAgreements;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LHDS.Core.Clients.Extensions
{
    public static class EmisLandingClientServiceCollectionExtensions
    {
        public static IServiceCollection AddEmisLandingClient(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IConfiguration>(_ => configuration);

            AddProviders(services);
            AddBrokers(services, configuration);
            AddServices(services);
            AddProcessingServices(services);
            AddOrchestrations(services);
            AddCoordinations(services);
            AddClients(services);

            return services;
        }

        private static void AddProviders(IServiceCollection services)
        {
            services.AddTransient<IDownloadAbstractionProvider, DownloadAbstractionProvider>();
        }

        private static void AddBrokers(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ILoggingBroker, LoggingBroker>();
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            services.AddTransient<IIdentifierBroker, IdentifierBroker>();
            services.AddTransient<IStorageBroker, StorageBroker>();
            services.AddTransient<IHashBroker, HashBroker>();
            services.AddTransient<ICryptographyKeyBroker, GpgKeyBroker>();
            services.AddTransient<ICryptographyKeyBroker, SshKeyBroker>();
            services.AddTransient<IFileBroker, FileBroker>();

            LandingConfiguration landingConfiguration =
                configuration.GetSection("landingSettings").Get<LandingConfiguration>();

            ValidateLandingConfiguration(landingConfiguration);

            var blobStorageSettings = configuration
                .GetSection("blobStorage").Get<BlobStorageSettings>();

            ValidateBlobStorageSettings(blobStorageSettings);
            services.AddSingleton<BlobContainers>(blobStorageSettings.BlobContainers);
            services.AddSingleton(landingConfiguration);

            services.AddTransient<IBlobStorageBroker, BlobStorageBroker>();
            services.AddTransient<IDownloadBroker, DownloadBroker>();

            services.AddTransient<IKeyVaultSecretBroker>((LandingConfiguration) =>
                new KeyVaultSecretBroker(landingConfiguration.KeyVaultUrl));

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
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IDownloadService, DownloadService>();
            services.AddTransient<IIngestionTrackingService, IngestionTrackingService>();
            services.AddTransient<ISupplierService, SupplierService>();
            services.AddTransient<IDataSetSpecificationService, DataSetSpecificationService>();
            services.AddTransient<IIngestionTrackingAuditService, IngestionTrackingAuditService>();
            services.AddTransient<ISubscriberAgreementService, SubscriberAgreementService>();
            services.AddTransient<ISecureDataService, SecureDataService>();
            services.AddTransient<ICryptographyKeyService, CryptographyKeyService>();
        }

        private static void AddProcessingServices(IServiceCollection services)
        {
            services.AddTransient<IOptOutProcessingService, OptOutProcessingService>();
            services.AddTransient<IDataSetSpecificationProcessingService, DataSetSpecificationProcessingService>();
            services.AddTransient<IDocumentProcessingService, DocumentProcessingService>();
            services.AddTransient<IDownloadProcessingService, DownloadProcessingService>();
            services.AddTransient<IIngestionTrackingProcessingService, IngestionTrackingProcessingService>();
            services.AddTransient<IIngestionTrackingAuditProcessingService, IngestionTrackingAuditProcessingService>();
            services.AddTransient<ISubscriberAgreementProcessingService, SubscriberAgreementProcessingService>();
            services.AddTransient<ISecureDataProcessingService, SecureDataProcessingService>();
            services.AddTransient<ICryptographyKeyProcessingService, CryptographyKeyProcessingService>();
        }

        private static void AddOrchestrations(IServiceCollection services)
        {
            services.AddTransient<IEmisLandingOrchestrationService, EmisLandingOrchestrationService>();
            services.AddTransient<ISubscriberCredentialOrchestration, SubscriberCredentialOrchestration>();
        }

        private static void AddCoordinations(IServiceCollection services)
        {
            services.AddTransient<IEmisLandingCoordinationService, EmisLandingCoordinationService>();
        }

        private static void AddClients(IServiceCollection services)
        {
            services.AddTransient<IEmisLandingClient, EmisLandingClient>();
        }

        private static void ValidateLandingConfiguration(LandingConfiguration landingConfiguration)
        {
            if (landingConfiguration == null)
            {
                throw new InvalidConfigurationException("Configuration section 'landingSettings' not defined.");
            }

            Validate(
                (Rule: IsInvalid(landingConfiguration.LandingSupplierId),
                    Parameter: "landingSettings__landingSupplierId"),

                (Rule: IsInvalid(landingConfiguration.EncryptedFolder),
                    Parameter: "landingSettings:encryptedFolder"),

                (Rule: IsInvalid(landingConfiguration.DecryptedFolder),
                    Parameter: "landingSettings:decryptedFolder"),

                (Rule: IsInvalid(landingConfiguration.KeyVaultUrl),
                        Parameter: "landingSettings:keyVaultUrl"));
        }

        private static void ValidateBlobStorageSettings(BlobStorageSettings blobStorageSettings)
        {
            if (blobStorageSettings == null)
            {
                throw new InvalidConfigurationException("Configuration section 'blobStorage' not defined.");
            }

            Validate(
                (Rule: IsInvalid(blobStorageSettings.AzureBlobServiceUri),
                    Parameter: "blobStorage__azureBlobServiceUri"),

                (Rule: IsInvalid(blobStorageSettings.AzureTenantId),
                    Parameter: "blobStorage__azureTenantId"));
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

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
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Decryptions;
using LHDS.Core.Brokers.Downloads;
using LHDS.Core.Brokers.Hashing;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Configurations;
using LHDS.Core.Models.Orchestrations.Downloads;
using LHDS.Core.Providers.Cryptography;
using LHDS.Core.Providers.Cryptography.Gpg;
using LHDS.Core.Providers.Downloads;
using LHDS.Core.Services.Foundations.Decryptions;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.Downloads;
using LHDS.Core.Services.Foundations.IngestionTrackingAudits;
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
            if (configuration == null)
            {
                new Exception("No configuration found");
            }

            return AddDecryptionClient(services, configuration, acceptanceTest: true);
        }

        private static IServiceCollection AddDecryptionClient(
            this IServiceCollection services,
            IConfiguration configuration,
            bool acceptanceTest)
        {
            services.AddSingleton<IConfiguration>(_ => configuration);
            var landingConfiguration = configuration.GetSection("landingSettings").Get<LandingConfiguration>();
            ValidateLandingConfiguration(landingConfiguration);
            services.AddSingleton<LandingConfiguration>(landingConfiguration);

            AddProviders(services, configuration);
            AddBrokers(services, configuration, acceptanceTest);
            AddServices(services);
            AddProcessingServices(services);
            AddOrchestrations(services);
            AddClients(services);

            return services;
        }

        private static void AddProviders(IServiceCollection services, IConfiguration configuration)
        {
            IGpgCryptographyProviderSettings gpgCryptographyProviderSettings =
                configuration.GetSection("cryptography").Get<GpgCryptographyProviderSettings>();

            ValidateCryptographyProviderSettings(gpgCryptographyProviderSettings);
            services.AddSingleton<IGpgCryptographyProviderSettings>(gpgCryptographyProviderSettings);
            services.AddTransient<IDownloadAbstractProvider, DownloadAbstractProvider>();
            services.AddTransient<ICryptographyAbstractProvider, CryptographyAbstractProvider>();
            services.AddTransient<ICryptographyProvider, GpgCryptographyProvider>();
        }

        private static void AddBrokers(IServiceCollection services, IConfiguration configuration, bool acceptanceTest)
        {
            services.AddTransient<IStorageBroker, StorageBroker>();
            services.AddTransient<IDecryptionBroker, DecryptionBroker>();
            services.AddTransient<ILoggingBroker, LoggingBroker>();
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            services.AddTransient<IIdentifierBroker, IdentifierBroker>();
            services.AddTransient<IHashBroker, HashBroker>();

            if (!acceptanceTest)
            {
                var blobStorageSettings = configuration.GetSection("blobStorage").Get<BlobStorageSettings>();
                ValidateBlobStorageSettings(blobStorageSettings);
                services.AddSingleton<BlobContainers>(blobStorageSettings.BlobContainers);

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

                services.AddTransient<IBlobStorageBroker, BlobStorageBroker>();
                services.AddTransient<IDownloadBroker, DownloadBroker>();
                services.AddTransient<IAzureBlobClient, AzureBlobClient>();
            }
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IDownloadService, DownloadService>();
            services.AddTransient<IIngestionTrackingService, IngestionTrackingService>();
            services.AddTransient<ISupplierService, SupplierService>();
            services.AddTransient<IIngestionTrackingAuditService, IngestionTrackingAuditService>();
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

        private static void ValidateCryptographyProviderSettings(
            IGpgCryptographyProviderSettings cryptographyProviderSettings)
        {
            if (cryptographyProviderSettings == null)
            {
                throw new InvalidConfigurationException("Configuration section 'cryptography' not defined.");
            }

            Validate(
                (Rule: IsInvalid(cryptographyProviderSettings.PrivateKey),
                    Parameter: "cryptography__privateKey"),

                (Rule: IsInvalid(cryptographyProviderSettings.PublicKey),
                    Parameter: "cryptography__publicKey"),

                (Rule: IsInvalid(cryptographyProviderSettings.Passphrase),
                    Parameter: "cryptography__passphrase"));
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
                    Parameter: "landingSettings:decryptedFolder"));
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

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Configuration value does not exist"
        };

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

// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using Azure.Core.Pipeline;
using Azure.Identity;
using Azure.Storage.Blobs;
using ISL.Security.Client.Models.Clients;
using LHDS.Core.Brokers.Audits;
using LHDS.Core.Brokers.Cryptographies;
using LHDS.Core.Brokers.CryptographyKeys;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Downloads;
using LHDS.Core.Brokers.Hashing;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.KeyVaults;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Securities;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Configurations;
using LHDS.Core.Models.Orchestrations.EmisLandings;
using LHDS.Core.Providers.Cryptography;
using LHDS.Core.Providers.Cryptography.Gpg;
using LHDS.Core.Providers.Downloads;
using LHDS.Core.Providers.Downloads.FtpDownloads;
using LHDS.Core.Services.Coordinations.Decryptions;
using LHDS.Core.Services.Foundations.Audits;
using LHDS.Core.Services.Foundations.CryptographicKeys;
using LHDS.Core.Services.Foundations.Cryptographies;
using LHDS.Core.Services.Foundations.DataSetSpecifications;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.Downloads;
using LHDS.Core.Services.Foundations.IngestionTrackingAudits;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using LHDS.Core.Services.Foundations.SecureDatas;
using LHDS.Core.Services.Foundations.SpecificationObjects;
using LHDS.Core.Services.Foundations.SubscriberAgreements;
using LHDS.Core.Services.Foundations.Suppliers;
using LHDS.Core.Services.Orchestrations.Decryptions;
using LHDS.Core.Services.Orchestrations.Ingress;
using LHDS.Core.Services.Orchestrations.SubscriberCredentials;
using LHDS.Core.Services.Processings.CryptographicKeys;
using LHDS.Core.Services.Processings.DataSetSpecifications;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.Downloads;
using LHDS.Core.Services.Processings.IngestionTrackings;
using LHDS.Core.Services.Processings.SecureDatas;
using LHDS.Core.Services.Processings.SpecificationObjects;
using LHDS.Core.Services.Processings.SubscriberAgreements;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xeptions;

namespace LHDS.Core.Clients.Extensions
{
    public static class DecryptionClientServiceCollectionExtensions
    {
        public static IServiceCollection AddDecryptionClient(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return AddDecryptionClient(services, configuration, null, acceptanceTest: false);
        }

        public static IServiceCollection AddDecryptionClient(
            this IServiceCollection services,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            var claimsPrincipal = httpContextAccessor.HttpContext.User;

            return AddDecryptionClient(services, configuration, claimsPrincipal, acceptanceTest: false);
        }

        public static IServiceCollection AddDecryptionClient(
            this IServiceCollection services,
            IConfiguration configuration,
            string accessToken)
        {
            var claimsPrincipal = GetClaimsPrincipalFromToken(accessToken);

            return AddDecryptionClient(services, configuration, claimsPrincipal, acceptanceTest: false);
        }

        public static IServiceCollection AddDecryptionClient(
            this IServiceCollection services,
            IConfiguration configuration,
            ClaimsPrincipal claimsPrincipal)
        {
            return AddDecryptionClient(services, configuration, claimsPrincipal, acceptanceTest: false);
        }

        internal static IServiceCollection AddDecryptionClientForAcceptance(
            this IServiceCollection services,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            if (configuration == null)
            {
                new Exception("No configuration found");
            }

            var claimsPrincipal = httpContextAccessor.HttpContext.User;

            return AddDecryptionClient(services, configuration, claimsPrincipal, acceptanceTest: true);
        }

        internal static IServiceCollection AddDecryptionClientForAcceptance(
            this IServiceCollection services,
            IConfiguration configuration,
            string accessToken)
        {
            if (configuration == null)
            {
                new Exception("No configuration found");
            }

            var claimsPrincipal = GetClaimsPrincipalFromToken(accessToken);

            return AddDecryptionClient(services, configuration, claimsPrincipal, acceptanceTest: true);
        }

        internal static IServiceCollection AddDecryptionClientForAcceptance(
            this IServiceCollection services,
            IConfiguration configuration,
            ClaimsPrincipal claimsPrincipal)
        {
            if (configuration == null)
            {
                new Exception("No configuration found");
            }

            return AddDecryptionClient(services, configuration, claimsPrincipal, acceptanceTest: true);
        }

        private static IServiceCollection AddDecryptionClient(
            this IServiceCollection services,
            IConfiguration configuration,
            ClaimsPrincipal claimsPrincipal,
            bool acceptanceTest)
        {
            services.AddSingleton(_ => configuration);
            var landingConfiguration = configuration.GetSection("landingSettings").Get<LandingConfiguration>();
            ValidateLandingConfiguration(landingConfiguration);

            if (landingConfiguration != null)
            {
                services.AddSingleton(landingConfiguration);
            }

            AddProviders(services, configuration);
            AddBrokers(services, configuration, claimsPrincipal, acceptanceTest);
            AddServices(services);
            AddProcessingServices(services);
            AddOrchestrations(services);
            AddCoordinations(services);
            AddClients(services);

            return services;
        }

        private static void AddProviders(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDownloadAbstractionProvider, DownloadAbstractionProvider>();
            services.AddTransient<ICryptographyAbstractProvider, CryptographyAbstractProvider>();
            services.AddTransient<ICryptographyProvider, GpgCryptographyProvider>();

            IFtpDownloadProviderSettings? ftpDownloadProviderSettings =
                configuration.GetSection("ftpDownload").Get<FtpDownloadProviderSettings>();

            ValidateFtpProviderSettings(ftpDownloadProviderSettings);

            if (ftpDownloadProviderSettings != null)
            {
                services.AddSingleton(ftpDownloadProviderSettings);
            }

            services.AddTransient<IDownloadProvider, FtpDownloadProvider>();
        }

        private static void AddBrokers(
            IServiceCollection services,
            IConfiguration configuration,
            ClaimsPrincipal claimsPrincipal,
            bool acceptanceTest)
        {
            if (claimsPrincipal != null)
            {
                services.AddTransient<ISecurityBroker>(_ => new SecurityBroker(claimsPrincipal));

                services.AddTransient<ISecurityAuditBroker>(_ =>
                    new SecurityAuditBroker(claimsPrincipal, new SecurityConfigurations()));
            }
            else
            {
                services.AddTransient<ISecurityBroker, SecurityBroker>();
                services.AddTransient<ISecurityAuditBroker, SecurityAuditBroker>();
            }

            services.AddTransient<IStorageBroker>(sp =>
            {
                var factory = sp.GetRequiredService<IDbContextFactory<StorageBroker>>();

                return factory.CreateDbContext();
            });

            services.AddTransient<ICryptographyBroker, CryptographyBroker>();
            services.AddTransient<ICryptographyKeyBroker, GpgKeyBroker>();
            services.AddTransient<ICryptographyKeyBroker, SshKeyBroker>();
            services.AddTransient<ILoggingBroker, LoggingBroker>();
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            services.AddTransient<IIdentifierBroker, IdentifierBroker>();
            services.AddTransient<IHashBroker, HashBroker>();
            services.AddTransient<IAuditBroker, AuditBroker>();

            LandingConfiguration? landingConfiguration =
                configuration.GetSection("landingSettings").Get<LandingConfiguration>();

            ValidateLandingConfiguration(landingConfiguration);

            if (!acceptanceTest)
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

                services.AddTransient<IBlobStorageBroker, BlobStorageBroker>();
                services.AddTransient<IDownloadBroker, DownloadBroker>();

                if (landingConfiguration != null)
                {
                    services.AddTransient<IKeyVaultSecretBroker>((LandingConfiguration) =>
                        new KeyVaultSecretBroker(landingConfiguration.KeyVaultUrl));
                }

                services.AddTransient<IAzureBlobClient, AzureBlobClient>();
            }
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddTransient<IAuditService, AuditService>();
            services.AddTransient<IDataSetSpecificationService, DataSetSpecificationService>();
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IDownloadService, DownloadService>();
            services.AddTransient<IIngestionTrackingService, IngestionTrackingService>();
            services.AddTransient<ISupplierService, SupplierService>();
            services.AddTransient<IIngestionTrackingAuditService, IngestionTrackingAuditService>();
            services.AddTransient<IDecryptionOrchestrationService, DecryptionOrchestrationService>();
            services.AddTransient<ICryptographyService, CryptographyService>();
            services.AddTransient<ISubscriberAgreementService, SubscriberAgreementService>();
            services.AddTransient<ISecureDataService, SecureDataService>();
            services.AddTransient<ICryptographyKeyService, CryptographyKeyService>();
            services.AddTransient<ISpecificationObjectService, SpecificationObjectService>();
        }

        private static void AddProcessingServices(IServiceCollection services)
        {
            services.AddTransient<IDownloadProcessingService, DownloadProcessingService>();
            services.AddTransient<IDocumentProcessingService, DocumentProcessingService>();
            services.AddTransient<ISubscriberAgreementProcessingService, SubscriberAgreementProcessingService>();
            services.AddTransient<ISecureDataProcessingService, SecureDataProcessingService>();
            services.AddTransient<ICryptographyKeyProcessingService, CryptographyKeyProcessingService>();
            services.AddTransient<IIngestionTrackingProcessingService, IngestionTrackingProcessingService>();
            services.AddTransient<IDataSetSpecificationProcessingService, DataSetSpecificationProcessingService>();
            services.AddTransient<ISpecificationObjectProcessingService, SpecificationObjectProcessingService>();
        }

        private static void AddOrchestrations(IServiceCollection services)
        {
            services.AddTransient<ISubscriberCredentialOrchestration, SubscriberCredentialOrchestration>();
            services.AddTransient<IIngressOrchestrationService, IngressOrchestrationService>();
        }

        private static void AddCoordinations(IServiceCollection services)
        {
            services.AddTransient<IDecryptionCoordinationService, DecryptionCoordinationService>();
        }

        private static void AddClients(IServiceCollection services)
        {
            services.AddTransient<IDecryptionClient, DecryptionClient>();
            services.AddTransient<IAuditClient, AuditClient>();
        }

        private static void ValidateFtpProviderSettings(IFtpDownloadProviderSettings? ftpDownloadProviderSettings)
        {
            if (ftpDownloadProviderSettings is null)
            {
                throw new InvalidConfigurationException("ftpDownload configuration section missing");
            }

            Validate(
                createException: (message, errors) => new InvalidConfigurationException(message, null, errors),

                (Rule: IsInvalid(ftpDownloadProviderSettings.FtpPort),
                    Parameter: "ftpDownload__ftpPort"),

                (Rule: IsInvalid(ftpDownloadProviderSettings.IncludeSubDirectories),
                    Parameter: "ftpDownload__includeSubDirectories"));
        }

        private static void ValidateLandingConfiguration(LandingConfiguration? landingConfiguration)
        {
            if (landingConfiguration == null)
            {
                throw new InvalidConfigurationException("Configuration section 'landingSettings' not defined.");
            }

            Validate(
                createException: (message, errors) => new InvalidConfigurationException(message, null, errors),

                (Rule: IsInvalid(landingConfiguration.LandingSupplierId),
                    Parameter: "landingSettings__landingSupplierId"),

                (Rule: IsInvalid(landingConfiguration.EncryptedFolder),
                    Parameter: "landingSettings:encryptedFolder"),

                (Rule: IsInvalid(landingConfiguration.DecryptedFolder),
                    Parameter: "landingSettings:decryptedFolder"),

                (Rule: IsInvalid(landingConfiguration.KeyVaultUrl),
                        Parameter: "landingSettings:keyVaultUrl"));
        }

        private static void ValidateBlobStorageSettings(BlobStorageSettings? blobStorageSettings)
        {
            if (blobStorageSettings == null)
            {
                throw new InvalidConfigurationException("Configuration section 'blobStorage' not defined.");
            }

            Validate(
                createException: (message, errors) => new InvalidConfigurationException(message, null, errors),

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

        private static dynamic IsInvalid(int value) => new
        {
            Condition = value == 0,
            Message = "Configuration value does not exist"
        };

        private static dynamic IsInvalid(bool? value) => new
        {
            Condition = value == null,
            Message = "Configuration value does not exist"
        };

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Configuration value does not exist"
        };

        private static void Validate<T>(
            Func<string, IDictionary, T> createException,
            params (dynamic Rule, string Parameter)[] validations)
            where T : Xeption
        {
            StringBuilder validationErrors = new StringBuilder();
            validationErrors.AppendLine("Configuration error(s):");
            IDictionary errors = new Dictionary<string, List<string>>();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    validationErrors.AppendLine($"{parameter}");

                    if (errors.Contains(parameter))
                    {
                        (errors[parameter] as List<string>)?.Add(rule.Message);
                        return;
                    }

                    errors.Add(parameter, new List<string> { rule.Message });
                }
            }

            T invalidDataException = createException(
                validationErrors.ToString(),
                errors);

            invalidDataException.ThrowIfContainsErrors();
        }

        /// <summary>
        /// Extracts a <see cref="ClaimsPrincipal"/> from a given JWT token.
        /// </summary>
        /// <param name="token">The JWT token.</param>
        /// <returns>A <see cref="ClaimsPrincipal"/> containing claims from the token.</returns>
        private static ClaimsPrincipal GetClaimsPrincipalFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");

            return new ClaimsPrincipal(identity);
        }
    }
}

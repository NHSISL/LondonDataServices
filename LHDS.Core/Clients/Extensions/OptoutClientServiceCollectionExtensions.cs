// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Azure.Core.Pipeline;
using Azure.Identity;
using Azure.Storage.Blobs;
using ISL.Security.Client.Models.Clients;
using LHDS.Core.Brokers.Audits;
using LHDS.Core.Brokers.CsvHelpers;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Files;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Mesh;
using LHDS.Core.Brokers.Securities;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Brokers.TempLocations;
using LHDS.Core.Models.Brokers.Mesh;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Configurations;
using LHDS.Core.Models.Orchestrations.OptOuts;
using LHDS.Core.Services.Foundations.Audits;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.IngestionTrackingAudits;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using LHDS.Core.Services.Foundations.Mesh;
using LHDS.Core.Services.Foundations.OptOuts;
using LHDS.Core.Services.Orchestrations.OptOuts;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.Mesh;
using LHDS.Core.Services.Processings.OptOuts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xeptions;

namespace LHDS.Core.Clients.Extensions
{
    public static class OptOutClientServiceCollectionExtensions
    {
        public static IServiceCollection AddOptOutClient(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return AddOptOutClient(services, configuration, claimsPrincipal: null, acceptanceTest: false);
        }

        public static IServiceCollection AddOptOutClient(
            this IServiceCollection services,
            IConfiguration configuration,
            ClaimsPrincipal claimsPrincipal)
        {
            return AddOptOutClient(services, configuration, claimsPrincipal, acceptanceTest: false);
        }

        internal static IServiceCollection AddOptOutClientForAcceptance(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return AddOptOutClient(services, configuration, claimsPrincipal: null, acceptanceTest: true);
        }

        internal static IServiceCollection AddOptOutClientForAcceptance(
            this IServiceCollection services,
            IConfiguration configuration,
            ClaimsPrincipal claimsPrincipal)
        {
            return AddOptOutClient(services, configuration, claimsPrincipal, acceptanceTest: true);
        }

        private static IServiceCollection AddOptOutClient(
            this IServiceCollection services,
            IConfiguration configuration,
            ClaimsPrincipal claimsPrincipal,
            bool acceptanceTest)
        {
            services.AddSingleton<IConfiguration>(_ => configuration);

            var meshConfigurationSettings =
                configuration.GetSection("meshConfiguration").Get<MeshConfigurationSettings>();

            ValidateMeshConfigurationSettings(meshConfigurationSettings, acceptanceTest);

            if (meshConfigurationSettings != null)
            {
                var meshConfig = new MeshConfiguration
                {
                    MailboxId = meshConfigurationSettings.MailboxId,
                    Password = meshConfigurationSettings.Password,
                    SharedKey = meshConfigurationSettings.SharedKey,
                    Url = meshConfigurationSettings.Url,
                    MexClientVersion = meshConfigurationSettings.MexClientVersion,
                    MexOSName = meshConfigurationSettings.MexOSName,
                    MexOSVersion = meshConfigurationSettings.MexOSVersion,
                    MaxChunkSizeInMegabytes = meshConfigurationSettings.MaxChunkSizeInMegabytes,
                };

                if (!acceptanceTest)
                {
                    meshConfig.TlsRootCertificates =
                        GetCertificates(values: meshConfigurationSettings.TlsRootCertificates);

                    meshConfig.TlsIntermediateCertificates =
                        GetCertificates(values: meshConfigurationSettings.TlsIntermediateCertificates);

                    meshConfig.ClientSigningCertificate =
                        GetCertificate(value: meshConfigurationSettings.ClientSigningCertificate);
                }

                services.AddSingleton(meshConfig);
            }

            AddBrokers(services, claimsPrincipal, acceptanceTest);
            AddServices(services);
            AddProcessingServices(services);
            AddOrchestrations(services);
            AddClients(services, configuration);

            return services;
        }

        public static IServiceCollection AddOptOutClient(
            this IServiceCollection services,
            IConfiguration configuration,
            string accessToken,
            bool acceptanceTest)
        {
            services.AddSingleton<IConfiguration>(_ => configuration);

            var meshConfigurationSettings =
                configuration.GetSection("meshConfiguration").Get<MeshConfigurationSettings>();

            ValidateMeshConfigurationSettings(meshConfigurationSettings, acceptanceTest);

            if (meshConfigurationSettings != null)
            {
                var meshConfig = new MeshConfiguration
                {
                    MailboxId = meshConfigurationSettings.MailboxId,
                    Password = meshConfigurationSettings.Password,
                    SharedKey = meshConfigurationSettings.SharedKey,
                    Url = meshConfigurationSettings.Url,
                    MexClientVersion = meshConfigurationSettings.MexClientVersion,
                    MexOSName = meshConfigurationSettings.MexOSName,
                    MexOSVersion = meshConfigurationSettings.MexOSVersion,
                    MaxChunkSizeInMegabytes = meshConfigurationSettings.MaxChunkSizeInMegabytes,
                };

                if (!acceptanceTest)
                {
                    meshConfig.TlsRootCertificates =
                        GetCertificates(values: meshConfigurationSettings.TlsRootCertificates);

                    meshConfig.TlsIntermediateCertificates =
                        GetCertificates(values: meshConfigurationSettings.TlsIntermediateCertificates);

                    meshConfig.ClientSigningCertificate =
                        GetCertificate(value: meshConfigurationSettings.ClientSigningCertificate);
                }

                services.AddSingleton(meshConfig);
            }

            AddBrokers(services, GetClaimsPrincipalFromToken(accessToken), acceptanceTest);
            AddServices(services);
            AddProcessingServices(services);
            AddOrchestrations(services);
            AddClients(services, configuration);

            return services;
        }

        private static void AddBrokers(IServiceCollection services, ClaimsPrincipal claimsPrincipal, bool acceptanceTest)
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

            services.AddTransient<IAuditBroker, AuditBroker>();
            services.AddTransient<ILoggingBroker, LoggingBroker>();
            services.AddTransient<ICsvHelperBroker, CsvHelperBroker>();
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            services.AddTransient<IIdentifierBroker, IdentifierBroker>();
            services.AddTransient<ITempLocationBroker, TempLocationBroker>();
            services.AddTransient<IFileBroker, FileBroker>();

            if (!acceptanceTest)
            {
                services.AddTransient<IBlobStorageBroker, BlobStorageBroker>();
                services.AddTransient<IMeshBroker, MeshBroker>();
            }
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddTransient<IIngestionTrackingAuditService, IngestionTrackingAuditService>();
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IIngestionTrackingService, IngestionTrackingService>();
            services.AddTransient<IMeshService, MeshService>();
            services.AddTransient<IOptOutService, OptOutService>();
            services.AddTransient<IAuditService, AuditService>();
        }

        private static void AddProcessingServices(IServiceCollection services)
        {
            services.AddTransient<IDocumentProcessingService, DocumentProcessingService>();
            services.AddTransient<IMeshProcessingService, MeshProcessingService>();
            services.AddTransient<IOptOutProcessingService, OptOutProcessingService>();
        }

        private static void AddOrchestrations(IServiceCollection services)
        {
            services.AddTransient<IOptOutOrchestrationService, OptOutOrchestrationService>();
        }

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

            var optOptOutConfiguration = configuration.GetSection("optOutSettings").Get<OptOutConfiguration>();
            ValidateOptOutConfigurationSettings(optOptOutConfiguration);

            if (optOptOutConfiguration != null)
            {
                services.AddSingleton(optOptOutConfiguration);
            }

            services.AddTransient<IAuditClient, AuditClient>();
            services.AddTransient<IOptOutClient, OptOutClient>();
            services.AddTransient<IAzureBlobClient, AzureBlobClient>();
        }

        public static X509Certificate2 LoadCertificate(byte[] certBytes, string password)
        {
            try
            {
                // Try to load as PKCS#12
                return X509CertificateLoader.LoadPkcs12(
                    certBytes,
                    password,
                    X509KeyStorageFlags.EphemeralKeySet |
                        X509KeyStorageFlags.Exportable);
            }
            catch (CryptographicException)
            {
                // If it fails, try to load as a regular certificate
                return X509CertificateLoader.LoadCertificate(certBytes);
            }
        }

        private static X509Certificate2? GetCertificate(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    byte[] certBytes = Convert.FromBase64String(value);
                    return LoadCertificate(certBytes, string.Empty);
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"Invalid Base64 string: {ex.Message}");
                }
                catch (CryptographicException ex)
                {
                    Console.WriteLine($"Error loading certificate: {ex.Message}");
                }
            }

            return null;
        }

        private static X509Certificate2Collection GetCertificates(List<string> values)
        {
            values ??= new List<string>();

            var certificates = new X509Certificate2Collection();

            if (values.Any())
            {
                foreach (string item in values)
                {
                    byte[] certBytes = Convert.FromBase64String(item);
                    certificates.Add(LoadCertificate(certBytes, string.Empty));
                }
            }

            return certificates;
        }

        private static void ValidateMeshConfigurationSettings(
            MeshConfigurationSettings? meshConfigurationSettings,
            bool acceptanceTest)
        {
            if (meshConfigurationSettings == null)
            {
                throw new InvalidConfigurationException(
                    "Configuration section 'meshConfiguration' not defined.");
            }

            Validate(
                createException: (message, errors) => new InvalidConfigurationException(message, null, errors),

                (Rule: IsInvalid(meshConfigurationSettings.MailboxId),
                    Parameter: "meshConfiguration__mailboxId"),

                (Rule: IsInvalid(meshConfigurationSettings.Password),
                    Parameter: "meshConfiguration__password"),

                (Rule: IsInvalid(meshConfigurationSettings.SharedKey),
                    Parameter: "meshConfiguration__sharedKey"),

                (Rule: IsInvalid(meshConfigurationSettings.Url),
                    Parameter: "meshConfiguration__url"),

                (Rule: IsInvalid(meshConfigurationSettings.MexClientVersion),
                    Parameter: "meshConfiguration__mexClientVersion"),

                (Rule: IsInvalid(meshConfigurationSettings.MexOSName),
                    Parameter: "meshConfiguration__mexOSName"),

                (Rule: IsInvalid(meshConfigurationSettings.MexOSVersion),
                    Parameter: "meshConfiguration__mexOSVersion"));

            if (acceptanceTest)
            {
                Validate(
                createException: (message, errors) => new InvalidConfigurationException(message, null, errors),

                    (Rule: IsInvalid(meshConfigurationSettings.TlsRootCertificates),
                        Parameter: "meshConfiguration__tlsRootCertificates__0"),

                    (Rule: IsInvalid(meshConfigurationSettings.TlsIntermediateCertificates),
                        Parameter: "meshConfiguration__tlsIntermediateCertificates__0"),

                    (Rule: IsInvalid(meshConfigurationSettings.ClientSigningCertificate),
                        Parameter: "meshConfiguration__clientSigningCertificate"));
            }
        }

        private static void ValidateOptOutConfigurationSettings(OptOutConfiguration? optOutConfiguration)
        {
            if (optOutConfiguration == null)
            {
                throw new InvalidConfigurationException(
                    "Configuration section 'optOutSettings' not defined.");
            }

            Validate(
                createException: (message, errors) => new InvalidConfigurationException(message, null, errors),

                (Rule: IsInvalid(optOutConfiguration.ExpiredAfterDays),
                    Parameter: "optOutSettings__expiredAfterDays"),

                (Rule: IsInvalid(optOutConfiguration.InputFolder),
                    Parameter: "optOutSettings__inputFolder"),

                (Rule: IsInvalid(optOutConfiguration.OutputFolder),
                    Parameter: "optOutSettings__outputFolder"),

                (Rule: IsInvalid(optOutConfiguration.OptOutFileHasHeader),
                    Parameter: "optOutSettings__optOutFileHasHeader"),

                (Rule: IsInvalid(optOutConfiguration.OptOutFileRequireTrailingComma),
                    Parameter: "optOutSettings__optOutFileRequireTrailingComma"),

                (Rule: IsInvalid(optOutConfiguration.To),
                    Parameter: "optOutSettings__to"),

                (Rule: IsInvalid(optOutConfiguration.WorkflowId),
                    Parameter: "optOutSettings__workflowId"));
        }

        private static void ValidateBlobStorageSettings(BlobStorageSettings? blobStorageSettings)
        {
            if (blobStorageSettings == null)
            {
                throw new InvalidConfigurationException(
                    "Configuration section 'blobStorage' not defined.");
            }

            Validate(
                createException: (message, errors) => new InvalidConfigurationException(message, null, errors),

                (Rule: IsInvalid(blobStorageSettings.AzureBlobServiceUri),
                    Parameter: "blobStorage__azureBlobServiceUri"),

                (Rule: IsInvalid(blobStorageSettings.AzureTenantId),
                    Parameter: "blobStorage__azureTenantId"));
        }

        private static dynamic IsInvalid(int number) => new
        {
            Condition = number <= 0,
            Message = "Configuration value does not exist"
        };

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Configuration value does not exist"
        };

        private static dynamic IsInvalid(List<string> stringList) => new
        {
            Condition = stringList == null,
            Message = "String list is required"
        };

        private static dynamic IsInvalid(bool? value) => new
        {
            Condition = value == null,
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

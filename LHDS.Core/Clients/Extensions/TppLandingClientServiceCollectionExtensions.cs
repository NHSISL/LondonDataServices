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
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Files;
using LHDS.Core.Brokers.Hashing;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Securities;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Configurations;
using LHDS.Core.Models.Orchestrations.EmisLandings;
using LHDS.Core.Services.Coordinations.TppLandings;
using LHDS.Core.Services.Foundations.Audits;
using LHDS.Core.Services.Foundations.DataSets;
using LHDS.Core.Services.Foundations.DataSetSpecifications;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.IngestionTrackingAudits;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using LHDS.Core.Services.Foundations.ObjectColumns;
using LHDS.Core.Services.Foundations.SpecificationObjects;
using LHDS.Core.Services.Foundations.SubscriberAgreements;
using LHDS.Core.Services.Foundations.Suppliers;
using LHDS.Core.Services.Orchestrations.Ingress;
using LHDS.Core.Services.Orchestrations.Tpp;
using LHDS.Core.Services.Orchestrations.TppLandings;
using LHDS.Core.Services.Processings.DataSets;
using LHDS.Core.Services.Processings.DataSetSpecifications;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.IngestionTrackingAudits;
using LHDS.Core.Services.Processings.IngestionTrackings;
using LHDS.Core.Services.Processings.SpecificationObjects;
using LHDS.Core.Services.Processings.SubscriberAgreements;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xeptions;

namespace LHDS.Core.Clients.Extensions
{
    public static class TppLandingClientServiceCollectionExtensions
    {
        public static IServiceCollection AddTppLandingClient(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return AddTppLandingClient(services, configuration, claimsPrincipal: null);
        }

        public static IServiceCollection AddTppLandingClient(
            this IServiceCollection services,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            services.AddSingleton<IConfiguration>(_ => configuration);
            AddBrokers(services, configuration, httpContextAccessor.HttpContext.User);
            AddServices(services);
            AddProcessingServices(services);
            AddOrchestrations(services);
            AddCoordinations(services);
            AddClients(services);

            return services;
        }

        public static IServiceCollection AddTppLandingClient(
            this IServiceCollection services,
            IConfiguration configuration,
            string accessToken)
        {
            services.AddSingleton<IConfiguration>(_ => configuration);
            AddBrokers(services, configuration, GetClaimsPrincipalFromToken(accessToken));
            AddServices(services);
            AddProcessingServices(services);
            AddOrchestrations(services);
            AddCoordinations(services);
            AddClients(services);

            return services;
        }

        public static IServiceCollection AddTppLandingClient(
            this IServiceCollection services,
            IConfiguration configuration,
            ClaimsPrincipal claimsPrincipal)
        {
            services.AddSingleton<IConfiguration>(_ => configuration);
            AddBrokers(services, configuration, claimsPrincipal);
            AddServices(services);
            AddProcessingServices(services);
            AddOrchestrations(services);
            AddCoordinations(services);
            AddClients(services);

            return services;
        }

        private static void AddBrokers(
            IServiceCollection services,
            IConfiguration configuration,
            ClaimsPrincipal claimsPrincipal)
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

            services.AddTransient<ILoggingBroker, LoggingBroker>();
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            services.AddTransient<IIdentifierBroker, IdentifierBroker>();
            services.AddTransient<IHashBroker, HashBroker>();
            services.AddTransient<IAuditBroker, AuditBroker>();
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
            services.AddTransient<IAuditService, AuditService>();
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IIngestionTrackingService, IngestionTrackingService>();
            services.AddTransient<ISupplierService, SupplierService>();
            services.AddTransient<IDataSetService, DataSetService>();
            services.AddTransient<IDataSetSpecificationService, DataSetSpecificationService>();
            services.AddTransient<IIngestionTrackingAuditService, IngestionTrackingAuditService>();
            services.AddTransient<ISpecificationObjectService, SpecificationObjectService>();
            services.AddTransient<IObjectColumnService, ObjectColumnService>();
            services.AddTransient<ISubscriberAgreementService, SubscriberAgreementService>();
        }

        private static void AddProcessingServices(IServiceCollection services)
        {
            services.AddTransient<IDataSetSpecificationProcessingService, DataSetSpecificationProcessingService>();
            services.AddTransient<IDataSetProcessingService, DataSetProcessingService>();
            services.AddTransient<IDocumentProcessingService, DocumentProcessingService>();
            services.AddTransient<IIngestionTrackingProcessingService, IngestionTrackingProcessingService>();
            services.AddTransient<IIngestionTrackingAuditProcessingService, IngestionTrackingAuditProcessingService>();
            services.AddTransient<ISpecificationObjectProcessingService, SpecificationObjectProcessingService>();
            services.AddTransient<ISubscriberAgreementProcessingService, SubscriberAgreementProcessingService>();
        }

        private static void AddOrchestrations(IServiceCollection services)
        {
            services.AddTransient<ITppLandingOrchestrationService, TppLandingOrchestrationService>();
            services.AddTransient<IIngressOrchestrationService, IngressOrchestrationService>();
        }

        private static void AddCoordinations(IServiceCollection services)
        {
            services.AddTransient<ITppLandingCoordinationService, TppLandingCoordinationService>();
        }

        private static void AddClients(IServiceCollection services)
        {
            services.AddTransient<ITppLandingClient, TppLandingClient>();
            services.AddTransient<IAuditClient, AuditClient>();
        }

        private static void ValidateLandingConfiguration(LandingConfiguration landingConfiguration)
        {
            if (landingConfiguration == null)
            {
                throw new InvalidConfigurationException("Configuration section 'landingSettings' not defined.");
            }

            Validate(
                createException: (message, errors) => new InvalidConfigurationException(message, null, errors),

                (Rule: IsInvalid(landingConfiguration.LandingSupplierId),
                    Parameter: "landingSettings__landingSupplierId"),

                (Rule: IsInvalid(landingConfiguration.DecryptedFolder),
                    Parameter: "landingSettings:decryptedFolder"),

                (Rule: IsInvalid(landingConfiguration.BatchDownloadedFile),
                    Parameter: "landingSettings:batchDownloadedFile"),

                (Rule: IsInvalid(landingConfiguration.BatchReadyFile),
                    Parameter: "landingSettings:batchReadyFile"));
        }

        private static void ValidateBlobStorageSettings(BlobStorageSettings blobStorageSettings)
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

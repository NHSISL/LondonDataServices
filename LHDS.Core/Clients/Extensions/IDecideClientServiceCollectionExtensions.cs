// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using Azure.Core.Pipeline;
using Azure.Identity;
using Azure.Storage.Blobs;
using ISL.Security.Client.Models.Clients;
using LHDS.Core.Brokers.CsvHelpers;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Decisions;
using LHDS.Core.Brokers.Hashing;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Securities;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Brokers.DecisionConfigurations;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Configurations;
using LHDS.Core.Services.Foundations.DecisionPolls;
using LHDS.Core.Services.Foundations.Decisions;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Orchestrations.Decisions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LHDS.Core.Clients.Extensions
{
    public static class IDecideClientServiceCollectionExtensions
    {
        public static IServiceCollection AddIDecideClient(
            this IServiceCollection services,
            IConfiguration configuration,
            string accessToken)
        {
            services.AddSingleton<IConfiguration>(_ => configuration);

            AddProviders(services);
            AddBrokers(services, configuration, GetClaimsPrincipalFromToken(accessToken));
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
            IConfiguration configuration,
            ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal is not null)
            {
                services.AddTransient<ISecurityAuditBroker>(_ =>
                    new SecurityAuditBroker(claimsPrincipal, new SecurityConfigurations()));
            }
            else
            {
                services.AddTransient<ISecurityAuditBroker, SecurityAuditBroker>();
            }

            services.AddTransient<IStorageBroker>(sp =>
            {
                var factory = sp.GetRequiredService<IDbContextFactory<StorageBroker>>();

                return factory.CreateDbContext();
            });

            services.AddTransient<ILoggingBroker, LoggingBroker>();
            services.AddTransient<ICsvHelperBroker, CsvHelperBroker>();
            services.AddTransient<IHashBroker, HashBroker>();
            services.AddTransient<IIdentifierBroker, IdentifierBroker>();
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            services.AddTransient<IDecisionBroker, DecisionBroker>();

            DecisionConfiguration decisionConfiguration =
                configuration.GetSection("IDecide").Get<DecisionConfiguration>();

            ValidateDecisionConfiguration(decisionConfiguration);
            services.AddSingleton<DecisionConfiguration>(decisionConfiguration);

            var blobStorageSettings = configuration
                .GetSection("blobStorage").Get<BlobStorageSettings>();

            ValidateBlobStorageSettings(blobStorageSettings);
            services.AddSingleton<BlobContainers>(blobStorageSettings.BlobContainers);
            services.AddTransient<IBlobStorageBroker, BlobStorageBroker>();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddTransient<IDecisionPollService, DecisionPollService>();
            services.AddTransient<IDecisionService, DecisionService>();
            services.AddTransient<IDocumentService, DocumentService>();
        }

        private static void AddProcessingServices(IServiceCollection services)
        { }

        private static void AddOrchestrations(IServiceCollection services)
        {
            services.AddTransient<IDecisionOrchestrationService, DecisionOrchestrationService>();
        }

        private static void AddCoordinations(IServiceCollection services)
        { }

        private static void AddClients(IServiceCollection services, IConfiguration configuration)
        {
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
                services.AddTransient<IIDecideClient, IDecideClient>();
            }
        }

        private static void ValidateDecisionConfiguration(DecisionConfiguration decisionConfiguration)
        {
            Validate((Rule: IsInvalid(decisionConfiguration.HashPepper), Parameter: "IDecide__hashPepper"));
            Validate((Rule: IsInvalid(decisionConfiguration.FolderName), Parameter: "IDecide__folderName"));
            Validate((Rule: IsInvalid(decisionConfiguration.FilePrefix), Parameter: "IDecide__filePrefix"));
        }

        private static void ValidateBlobStorageSettings(BlobStorageSettings? blobStorageSettings)
        {
            if (blobStorageSettings is null)
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

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Configuration value does not exist"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidConfigurationException = new InvalidConfigurationException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidConfigurationException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidConfigurationException.ThrowIfContainsErrors();
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

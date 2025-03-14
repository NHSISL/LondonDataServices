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
using LHDS.Core.Brokers.Assigns;
using LHDS.Core.Brokers.Audits;
using LHDS.Core.Brokers.CsvHelpers;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Files;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Securities;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Brokers.Assigns;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Configurations;
using LHDS.Core.Models.Coordinations.AddressCoordinations;
using LHDS.Core.Services.Coordinations.AddressCoordinations;
using LHDS.Core.Services.Foundations.Addresses;
using LHDS.Core.Services.Foundations.Assigns;
using LHDS.Core.Services.Foundations.Audits;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.ResolvedAddresses;
using LHDS.Core.Services.Orchestrations.Addresses;
using LHDS.Core.Services.Orchestrations.ResolvedAddresses;
using LHDS.Core.Services.Processings.Addresses;
using LHDS.Core.Services.Processings.Assigns;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.ResolvedAddresses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LHDS.Core.Clients.Extensions
{
    public static class AddressClientServiceCollectionExtensions
    {
        public static IServiceCollection AddAddressClient(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return AddAddressClient(services, configuration, claimsPrincipal: null, acceptanceTest: false);
        }

        public static IServiceCollection AddAddressClient(
            this IServiceCollection services,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            services.AddSingleton<IConfiguration>(_ => configuration);
            AddClients(services, configuration);
            AddBrokers(services, httpContextAccessor.HttpContext.User);
            AddServices(services);
            AddProcessings(services);
            AddOrchestrations(services);
            AddCoordinations(services);

            return services;
        }

        public static IServiceCollection AddAddressClient(
            this IServiceCollection services,
            IConfiguration configuration,
            string accessToken)
        {
            services.AddSingleton<IConfiguration>(_ => configuration);
            AddClients(services, configuration);
            AddBrokers(services, GetClaimsPrincipalFromToken(accessToken));
            AddServices(services);
            AddProcessings(services);
            AddOrchestrations(services);
            AddCoordinations(services);

            return services;
        }

        public static IServiceCollection AddAddressClient(
            this IServiceCollection services,
            IConfiguration configuration,
            ClaimsPrincipal claimsPrincipal)
        {
            services.AddSingleton<IConfiguration>(_ => configuration);
            AddClients(services, configuration);
            AddBrokers(services, claimsPrincipal);
            AddServices(services);
            AddProcessings(services);
            AddOrchestrations(services);
            AddCoordinations(services);

            return services;
        }

        private static void AddClients(
            IServiceCollection services,
            IConfiguration configuration)
        {
            var blobStorageSettings = configuration.GetSection("blobStorage").Get<BlobStorageSettings>();
            ValidateBlobStorageSettings(blobStorageSettings);

            AddressConfiguration addressConfiguration =
                configuration.GetSection("addressSettings").Get<AddressConfiguration>();

            services.AddSingleton(addressConfiguration);

            AssignConfiguration assignConfiguration =
                configuration.GetSection("assignConfiguration").Get<AssignConfiguration>();

            ValidateAssingConfiguration(assignConfiguration);
            services.AddSingleton(assignConfiguration);

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

            services.AddTransient<IAddressClient, AddressClient>();
            services.AddTransient<IAzureBlobClient, AzureBlobClient>();
            services.AddTransient<IAuditClient, AuditClient>();
        }

        private static void AddBrokers(IServiceCollection services, ClaimsPrincipal claimsPrincipal)
        {
            services.AddTransient<IFileBroker, FileBroker>();
            services.AddTransient<IAssignBroker, AssignBroker>();
            services.AddTransient<ILoggingBroker, LoggingBroker>();
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            services.AddTransient<IIdentifierBroker, IdentifierBroker>();
            services.AddTransient<IStorageBroker, StorageBroker>();
            services.AddTransient<IBlobStorageBroker, BlobStorageBroker>();
            services.AddTransient<IAuditBroker, AuditBroker>();
            services.AddTransient<ICsvHelperBroker, CsvHelperBroker>();
            
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
            services.AddTransient<IAssignService, AssignService>();
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IResolvedAddressService, ResolvedAddressService>();
            services.AddTransient<IAuditService, AuditService>();
            services.AddTransient<IAddressService, AddressService>();
        }

        private static void AddProcessings(IServiceCollection services)
        {
            services.AddTransient<IAssignProcessingService, AssignProcessingService>();
            services.AddTransient<IAddressProcessingService, AddressProcessingService>();
            services.AddTransient<IDocumentProcessingService, DocumentProcessingService>();
            services.AddTransient<IResolvedAddressProcessingService, ResolvedAddressProcessingService>();
        }

        private static void AddOrchestrations(IServiceCollection services)
        {
            services.AddTransient<IAddressOrchestrationService, AddressOrchestrationService>();
            services.AddTransient<IResolvedAddressOrchestrationService, ResolvedAddressOrchestrationService>();
        }

        private static void AddCoordinations(IServiceCollection services)
        {
            services.AddTransient<IAddressCoordinationService, AddressCoordinationService>();
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

        private static void ValidateAssingConfiguration(AssignConfiguration? assignConfiguration)
        {
            if (assignConfiguration == null)
            {
                throw new InvalidConfigurationException(
                    "Configuration section 'assignConfiguration' not defined.");
            }

            Validate(
                (Rule: IsInvalid(assignConfiguration.ApiUrl),
                    Parameter: "assignConfiguration__apiUrl"));
        }

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Configuration value does not exist"
        };

        private static dynamic IsInvalid(bool? value) => new
        {
            Condition = value == null,
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

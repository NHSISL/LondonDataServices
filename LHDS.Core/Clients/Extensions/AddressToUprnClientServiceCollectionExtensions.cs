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
using Azure.Storage.Queues;
using ISL.Security.Client.Models.Clients;
using LHDS.Core.Brokers.Audits;
using LHDS.Core.Brokers.CsvHelpers;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Securities;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Configurations;
using LHDS.Core.Models.Orchestrations.AddressToUprns;
using LHDS.Core.Services.Foundations.AddressToUprnFileLogs;
using LHDS.Core.Services.Foundations.Assigns;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Orchestrations.AddressToUprns;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xeptions;

namespace LHDS.Core.Clients.Extensions
{
    public static class AddressToUprnClientServiceCollectionExtensions
    {
        public static IServiceCollection AddAddressToUprnClient(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return AddAddressToUprnClient(services, configuration, claimsPrincipal: null);
        }

        public static IServiceCollection AddAddressToUprnClient(
            this IServiceCollection services,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            services.AddSingleton<IConfiguration>(_ => configuration);
            AddClients(services, configuration);
            AddBrokers(services, httpContextAccessor.HttpContext.User, configuration);
            AddServices(services);
            AddOrchestrations(services);

            return services;
        }

        public static IServiceCollection AddAddressToUprnClient(
            this IServiceCollection services,
            IConfiguration configuration,
            string accessToken)
        {
            services.AddSingleton<IConfiguration>(_ => configuration);
            AddClients(services, configuration);
            AddBrokers(services, GetClaimsPrincipalFromToken(accessToken), configuration);
            AddServices(services);
            AddOrchestrations(services);

            return services;
        }

        public static IServiceCollection AddAddressToUprnClient(
            this IServiceCollection services,
            IConfiguration configuration,
            ClaimsPrincipal claimsPrincipal)
        {
            services.AddSingleton<IConfiguration>(_ => configuration);
            AddClients(services, configuration);
            AddBrokers(services, claimsPrincipal, configuration);
            AddServices(services);
            AddOrchestrations(services);

            return services;
        }

        private static void AddClients(
            IServiceCollection services,
            IConfiguration configuration)
        {
            AddressToUprnConfiguration addressToUprnConfiguration =
                configuration.GetSection("addressToUprnSettings").Get<AddressToUprnConfiguration>();

            ValidateAddressToUprnConfiguration(addressToUprnConfiguration);
            services.AddSingleton(addressToUprnConfiguration);

            var blobStorageSettings = configuration.GetSection("blobStorage").Get<BlobStorageSettings>();
            ValidateBlobStorageSettings(blobStorageSettings);
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

            services.AddTransient<IAddressToUprnClient, AddressToUprnClient>();
        }

        private static void AddBrokers(
            IServiceCollection services,
            ClaimsPrincipal claimsPrincipal,
            IConfiguration configuration)
        {
            services.AddTransient<IStorageBroker>(sp =>
            {
                var factory = sp.GetRequiredService<IDbContextFactory<StorageBroker>>();

                return factory.CreateDbContext();
            });

            services.AddTransient<ILoggingBroker, LoggingBroker>();
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            services.AddTransient<IIdentifierBroker, IdentifierBroker>();
            services.AddTransient<IBlobStorageBroker, BlobStorageBroker>();
            services.AddTransient<IAuditBroker, AuditBroker>();
            services.AddTransient<ICsvHelperBroker, CsvHelperBroker>();

            if (claimsPrincipal != null)
            {
                var securityBroker = new SecurityBroker(claimsPrincipal);
                var securityAuditBroker = new SecurityAuditBroker(claimsPrincipal, new SecurityConfigurations());
                services.AddTransient<ISecurityBroker>(_ => securityBroker);
                services.AddTransient<ISecurityAuditBroker>(_ => securityAuditBroker);
            }
            else
            {
                services.AddTransient<ISecurityBroker, SecurityBroker>();
                services.AddTransient<ISecurityAuditBroker, SecurityAuditBroker>();
            }
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddTransient<IAssignService, AssignService>();
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IAddressToUprnFileLogService, AddressToUprnFileLogService>();
        }

        private static void AddOrchestrations(IServiceCollection services)
        {
            services.AddTransient<IAddressToUprnOrchestrationService, AddressToUprnOrchestrationService>();
        }

        private static void ValidateAddressToUprnConfiguration(AddressToUprnConfiguration? addressToUprnConfiguration)
        {
            if (addressToUprnConfiguration == null)
            {
                throw new InvalidConfigurationException(
                    "Configuration section 'addressToUprnSettings' not defined.");
            }

            Validate(
                createException: (message, errors) => new InvalidConfigurationException(message, null, errors),

                (Rule: IsInvalid(addressToUprnConfiguration.InboxFolder),
                    Parameter: "addressToUprnSettings__inboxFolder"),

                (Rule: IsInvalid(addressToUprnConfiguration.OutboxFolder),
                    Parameter: "addressToUprnSettings__outboxFolder"),

                (Rule: IsInvalid(addressToUprnConfiguration.ErrorFolder),
                    Parameter: "addressToUprnSettings__errorFolder"));
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

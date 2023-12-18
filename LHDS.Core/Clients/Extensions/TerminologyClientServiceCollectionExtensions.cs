// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.Text;
using Hl7.Fhir.Specification.Terminology;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Hashing;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
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
            AddClients(services);

            return services;
        }

        private static void AddProviders(IServiceCollection services)
        {
            services.AddTransient<IDownloadAbstractProvider, DownloadAbstractProvider>();
        }

        private static void AddBrokers(IServiceCollection services, IConfiguration configuration, bool acceptanceTest)
        {
            services.AddTransient<ILoggingBroker, LoggingBroker>();
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            services.AddTransient<IIdentifierBroker, IdentifierBroker>();
            services.AddTransient<IStorageBroker, StorageBroker>();
            services.AddTransient<IHashBroker, HashBroker>();

            var terminologyMetdataConfiguration =
                configuration.GetSection("terminologyMetadataSettings").Get<TerminologyMetadataConfiguration>();

            ValidateTerminologyConfiguration(terminologyMetdataConfiguration);
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

        private static void AddClients(IServiceCollection services)
        {
            services.AddTransient<ILandingClient, LandingClient>();
        }

        private static void ValidateTerminologyConfiguration(
            TerminologyMetadataConfiguration terminologyMetadataConfiguration)
        {
            if (terminologyMetadataConfiguration == null)
            {
                throw new InvalidConfigurationException(
                    "Configuration section 'terminologyMetadataSettings' not defined.");
            }

            Validate(
                (Rule: IsInvalid(terminologyMetadataConfiguration.ResourceURL),
                    Parameter: "landingSettings__landingSupplierId"));
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

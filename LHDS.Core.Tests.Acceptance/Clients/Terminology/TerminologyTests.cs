// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Brokers.Ontologies;
using LHDS.Core.Services.Orchestrations.TerminologyMetadata;
using LHDS.Core.Services.Processings.Ontologies;
using LHDS.Core.Services.Processings.TerminologyArtifacts;
using LHDS.Core.Services.Processings.TerminologyPolls;
using LHDS.Core.Tests.Acceptance.Brokers.DependencyBrokers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tynamix.ObjectFiller;
using WireMock.Server;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Terminology
{
    [Collection(nameof(CoreTestCollection))]
    public partial class TerminologyTests
    {
        private readonly DependencyBroker dependencyBroker;
        private readonly ITerminologyMetadataOrchestrationService terminologyMetadataOrchestrationService;
        private readonly ITerminologyArtifactProcessingService terminologyArtifactProcessingService;
        private readonly ITerminologyPollProcessingService terminologyPollProcessingService;
        private readonly IOntologyProcessingService ontologyProcessingService;
        private readonly ITerminologyClient terminologyClient;
        private readonly OntologyConfiguration ontologyConfigurations;
        private readonly ICompareLogic compareLogic;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IIdentifierBroker identifierBroker;
        private readonly WireMockServer wireMockServer;

        public TerminologyTests(DependencyBroker dependencyBroker)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("local.appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables("LHDS_TERMINOLOGY_CLIENT_ACCEPTANCE_");

            IConfiguration configuration = configurationBuilder.Build();
            this.wireMockServer = WireMockServer.Start();
            bool RunAcceptanceTests = configuration.GetSection("RunAcceptanceTests").Get<bool>();
            bool RunIntegrationTests = configuration.GetSection("RunIntegrationTests").Get<bool>();
            var terminologyServerBaseUrl = configuration["ontologySettings:terminologyServerBaseUrl"];
            var terminologyServerAuthenticationRelativeUrl = configuration["ontologySettings:terminologyServerAuthenticationRelativeUrl"];
            var terminologyServerResourceRelativeUrl = configuration["ontologySettings:terminologyServerResourceRelativeUrl"];
            var clientId = configuration["ontologySettings:ClientId"];
            var clientSecret = configuration["ontologySettings:ClientSecret"];

            this.dependencyBroker = dependencyBroker;
            this.compareLogic = new CompareLogic();
            var serviceCollection = new ServiceCollection();

            serviceCollection
                .AddTransient<ITerminologyPollProcessingService, TerminologyPollProcessingService>()
                .AddTransient<ITerminologyMetadataOrchestrationService, TerminologyMetadataOrchestrationService>()
                .AddTransient<ITerminologyArtifactProcessingService, TerminologyArtifactProcessingService>()
                .AddTransient<IOntologyProcessingService, OntologyProcessingService>();

            serviceCollection.AddLogging(builder =>
            {
                builder.AddConsole();
            });

            this.ontologyConfigurations = new OntologyConfiguration
            {
                TerminologyServerBaseUrl = this.wireMockServer.Url,
                TerminologyServerAuthenticationRelativeUrl = terminologyServerAuthenticationRelativeUrl,
                TerminologyServerResourceRelativeUrl = terminologyServerResourceRelativeUrl,
                ClientId = clientId,
                ClientSecret = clientSecret,
            };

            serviceCollection.AddTerminologyClient(this.dependencyBroker.Configuration);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            this.dateTimeBroker = serviceProvider.GetService<IDateTimeBroker>();
            this.identifierBroker = serviceProvider.GetService<IIdentifierBroker>();
            this.loggingBroker = serviceProvider.GetService<ILoggingBroker>();
            terminologyClient = serviceProvider.GetService<ITerminologyClient>();

            this.terminologyMetadataOrchestrationService = new TerminologyMetadataOrchestrationService(
                ontologyConfiguration: this.ontologyConfigurations,
                terminologyPollProcessingService: this.terminologyPollProcessingService,
                ontologyProcessingService: this.ontologyProcessingService,
                terminologyArtifactProcessingService: this.terminologyArtifactProcessingService,
                dateTimeBroker: this.dateTimeBroker,
                loggingBroker: this.loggingBroker,
                identifierBroker: this.identifierBroker
                );
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();
    }
}

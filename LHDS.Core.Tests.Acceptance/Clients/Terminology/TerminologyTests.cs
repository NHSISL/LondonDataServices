// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Brokers.Ontologies;
using LHDS.Core.Services.Foundations.Ontologies;
using LHDS.Core.Services.Foundations.TerminologyArtifacts;
using LHDS.Core.Services.Foundations.TerminologyPolls;
using LHDS.Core.Tests.Acceptance.Brokers.DependencyBrokers;
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
        private readonly ITerminologyPollService terminologyPollService;
        private readonly ITerminologyArtifactService terminologyArtifactService;
        private readonly ITerminologyClient terminologyClient;
        private readonly OntologyConfiguration ontologyConfiguration;
        private readonly ICompareLogic compareLogic;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly WireMockServer wireMockServer;

        public TerminologyTests(DependencyBroker dependencyBroker)
        {
            this.wireMockServer = WireMockServer.Start();
            this.dependencyBroker = dependencyBroker;
            this.compareLogic = new CompareLogic();
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging(builder =>
            {
                builder.AddConsole();
            });

            this.dependencyBroker.Configuration["ontologySettings:terminologyServerBaseUrl"] = this.wireMockServer.Url;
            serviceCollection.AddTerminologyClient(this.dependencyBroker.Configuration);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            this.dateTimeBroker = serviceProvider.GetService<IDateTimeBroker>();
            terminologyClient = serviceProvider.GetService<ITerminologyClient>();
            ontologyConfiguration = serviceProvider.GetService<OntologyConfiguration>();
            terminologyPollService = serviceProvider.GetService<ITerminologyPollService>();
            terminologyArtifactService = serviceProvider.GetService<ITerminologyArtifactService>();
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();
    }
}
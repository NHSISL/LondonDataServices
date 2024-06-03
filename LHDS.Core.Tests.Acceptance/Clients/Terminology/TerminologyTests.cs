// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Services.Foundations.TerminologyPolls;
using LHDS.Core.Tests.Acceptance.Brokers.DependencyBrokers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Terminology
{
    [Collection(nameof(CoreTestCollection))]
    public partial class TerminologyTests
    {
        private readonly DependencyBroker dependencyBroker;
        private readonly ITerminologyPollService terminologyPollService;
        private readonly ITerminologyClient terminologyClient;
        private readonly ICompareLogic compareLogic;
        private readonly IDateTimeBroker dateTimeBroker;

        public TerminologyTests(DependencyBroker dependencyBroker)
        {
            this.dependencyBroker = dependencyBroker;
            this.compareLogic = new CompareLogic();
            var serviceCollection = new ServiceCollection();

            serviceCollection
                .AddTransient<ITerminologyPollService, TerminologyPollService>();

            serviceCollection.AddLogging(builder =>
            {
                builder.AddConsole();
            });

            serviceCollection.AddTerminologyClient(this.dependencyBroker.Configuration);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            this.terminologyPollService = serviceProvider.GetService<ITerminologyPollService>();
            this.dateTimeBroker = serviceProvider.GetService<IDateTimeBroker>();
            terminologyClient = serviceProvider.GetService<ITerminologyClient>();
        }
    }
}

// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using KellermanSoftware.CompareNetObjects;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Coordinations.AddressCoordinations;
using LHDS.Core.Services.Foundations.TerminologyPolls;
using LHDS.Core.Tests.Acceptance.Brokers.DependencyBrokers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tynamix.ObjectFiller;
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
        private readonly AddressConfiguration addressConfiguration;
        private readonly BlobContainers blobContainers;

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

            this.terminologyPollService =
                serviceProvider.GetService<ITerminologyPollService>();

            this.dateTimeBroker = serviceProvider.GetService<IDateTimeBroker>();
            terminologyClient = serviceProvider.GetService<ITerminologyClient>();
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static int GetRandomNumber(int min = 2, int max = 10) =>
            new IntRange(min, max).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime().AddDays(7)).GetValue();
    }
}

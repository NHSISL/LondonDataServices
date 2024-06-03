// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Brokers.Ontologies;
using LHDS.Core.Models.Foundations.Ontologies;
using LHDS.Core.Services.Foundations.TerminologyPolls;
using LHDS.Core.Services.Processings.Ontologies;
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
        private readonly IOntologyProcessingService ontologyProcessingService;
        private readonly ITerminologyClient terminologyClient;
        private readonly ICompareLogic compareLogic;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly OntologyConfiguration ontologyConfiguration;

        public TerminologyTests(DependencyBroker dependencyBroker)
        {
            this.dependencyBroker = dependencyBroker;
            this.compareLogic = new CompareLogic();
            var serviceCollection = new ServiceCollection();

            serviceCollection
                .AddTransient<ITerminologyPollService, TerminologyPollService>()
                .AddTransient<IOntologyProcessingService, OntologyProcessingService>();

            serviceCollection.AddLogging(builder =>
            {
                builder.AddConsole();
            });

            serviceCollection.AddTerminologyClient(this.dependencyBroker.Configuration);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            this.terminologyPollService = serviceProvider.GetService<ITerminologyPollService>();
            this.ontologyProcessingService = serviceProvider.GetService<IOntologyProcessingService>();
            this.ontologyConfiguration = serviceProvider.GetService<OntologyConfiguration>();
            this.dateTimeBroker = serviceProvider.GetService<IDateTimeBroker>();
            terminologyClient = serviceProvider.GetService<ITerminologyClient>();
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static int GetRandomNumber(int min = 2, int max = 10) =>
            new IntRange(min, max).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime().AddDays(7)).GetValue();

        private static List<dynamic> CreateRandomArtifactProperties(
            string resourceType, DateTimeOffset dateTimeOffset, Guid id)
        {
            string user = GetRandomString();
            return Enumerable.Range(1, GetRandomNumber())
                .Select(item =>
                {
                    return new
                    {
                        Id = id,
                        FullUrl = GetRandomString(),
                        ResourceType = resourceType,
                        Version = GetRandomString(),
                        Name = GetRandomString(),
                        Title = GetRandomString(),
                        Status = "active",
                        LastUpdated = dateTimeOffset,
                        IsCore = false,
                        IsDownloaded = false,
                        CreatedBy = "System",
                        UpdatedBy = "System",
                        UpdatedDate = dateTimeOffset,
                        CreatedDate = dateTimeOffset,
                        NextPage = GetRandomString()
                    };
                })
                .ToList<dynamic>();
        }

        private static OntologyAssets CreateOntologyAssetFromRandomData(List<dynamic> randomArtifactProperties)
        {
            var ontologyAssets = new OntologyAssets
            {
                Assets = new List<OntologyAsset>(),
                NextPage = randomArtifactProperties.First().NextPage,
            };

            foreach (var item in randomArtifactProperties)
            {
                ontologyAssets.Assets.Add(
                    new OntologyAsset
                    {
                        FullUrl = item.FullUrl,
                        ResourceType = item.ResourceType,
                        Version = item.Version,
                        Name = item.Name,
                        Title = item.Title,
                        Status = item.Status,
                        LastUpdated = item.LastUpdated
                    });

                ontologyAssets.NextPage = item.NextPage;
            }

            return ontologyAssets;
        }
    }
}

// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Brokers.Ontologies;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.TerminologyArtifacts;
using LHDS.Core.Services.Foundations.TerminologyPolls;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tynamix.ObjectFiller;
using Xunit.Abstractions;

namespace LHDS.Core.Tests.Integration.Terminology
{
    public partial class TerminologyTests
    {
        private readonly ITestOutputHelper output;
        private readonly ITerminologyPollService terminologyPollService;
        private readonly ITerminologyArtifactService terminologyArtifactService;
        private readonly IDocumentService documentService;
        private readonly ITerminologyClient terminologyClient;
        private readonly OntologyConfiguration ontologyConfiguration;
        private readonly IDateTimeBroker dateTimeBroker;

        public TerminologyTests(ITestOutputHelper output)
        {
            this.output = output;
            var environmentName = "Development";

            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile("local.appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfiguration configuration = configurationBuilder.Build();

            var serviceProvider = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.AddApplicationInsights();
                })
                .AddTerminologyClient(configuration)
                .BuildServiceProvider();

            this.dateTimeBroker = serviceProvider.GetService<IDateTimeBroker>();
            terminologyClient = serviceProvider.GetService<ITerminologyClient>();
            ontologyConfiguration = serviceProvider.GetService<OntologyConfiguration>();
            terminologyPollService = serviceProvider.GetService<ITerminologyPollService>();
            documentService = serviceProvider.GetService<IDocumentService>();
            terminologyArtifactService = serviceProvider.GetService<ITerminologyArtifactService>();
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static IQueryable<TerminologyArtifact> CreateRandomTerminologyArtifacts(DateTimeOffset dateTimeOffset)
        {
            return CreateTerminologyArtifactFiller(dateTimeOffset)
                .Create(count: 1)
                    .AsQueryable();
        }

        private static TerminologyArtifact CreateRandomTerminologyArtifact(DateTimeOffset dateTimeOffset) =>
            CreateTerminologyArtifactFiller(dateTimeOffset).Create();

        private static Filler<TerminologyArtifact> CreateTerminologyArtifactFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<TerminologyArtifact>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(terminologyArtifact => terminologyArtifact.IsDownloaded).Use(false)
                .OnProperty(terminologyArtifact => terminologyArtifact.IsError).Use(false)
                .OnProperty(terminologyArtifact => terminologyArtifact.CreatedBy).Use(user)
                .OnProperty(terminologyArtifact => terminologyArtifact.UpdatedBy).Use(user);

            return filler;
        }
    }
}
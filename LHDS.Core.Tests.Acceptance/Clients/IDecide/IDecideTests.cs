// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.CsvHelpers;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Decisions;
using LHDS.Core.Brokers.Hashing;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Brokers.Decisions;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.Decisions;
using LHDS.Core.Models.Foundations.DecisionTypes;
using LHDS.Core.Models.Foundations.Patients;
using LHDS.Core.Services.Foundations.Decisions;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Orchestrations.Decisions;
using LHDS.Core.Tests.Acceptance.Brokers.DependencyBrokers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tynamix.ObjectFiller;
using WireMock.Server;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.IDecide
{
    [Collection(nameof(CoreTestCollection))]
    public partial class IDecideTests
    {
        private readonly DependencyBroker dependencyBroker;
        private readonly IDecisionOrchestrationService decisionOrchestrationService;
        private readonly IDecisionService decisionService;
        private readonly IDocumentService documentService;
        private readonly ICsvHelperBroker csvHelperBroker;
        private readonly IHashBroker hashBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IDecisionBroker decisionBroker;
        private readonly IStorageBroker storageBroker;
        private readonly IBlobStorageBroker blobStorageBroker;
        private readonly IIDecideClient iDecideClient;
        private readonly ICompareLogic compareLogic;
        private readonly DecisionConfiguration decisionConfiguration;
        private readonly BlobContainers blobContainers;
        private readonly WireMockServer wireMockServer;

        public IDecideTests(DependencyBroker dependencyBroker)
        {
            this.wireMockServer = WireMockServer.Start();
            this.dependencyBroker = dependencyBroker;
            this.compareLogic = new CompareLogic();
            var serviceCollection = new ServiceCollection();
            var claimsPrincipal = new ClaimsPrincipal();

            claimsPrincipal.AddIdentity(new ClaimsIdentity(new List<Claim>
            {
                new Claim("oid", Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.GivenName, "GivenName"),
                new Claim(ClaimTypes.Surname, "Surname"),
                new Claim("displayName", "DisplayName"),
                new Claim(ClaimTypes.Email, "some@email.com"),
                new Claim("jobTitle", "job title"),
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, "ISL.LDS.AdminApi.Administrators"),
                new Claim(ClaimTypes.Role, "ISL.LDS.AdminApi.Configurations")
            }));

            serviceCollection.AddLogging(builder =>
            {
                builder.AddConsole();
            });

            this.dependencyBroker.Configuration["IDecide:iDecideBaseUrl"] = this.wireMockServer.Url;

            serviceCollection
                .AddDbContextFactory<StorageBroker>()
                .AddTransient<IDecisionOrchestrationService, DecisionOrchestrationService>()
                .AddTransient<IDecisionService, DecisionService>()
                .AddTransient<IDocumentService, DocumentService>()
                .AddTransient<ICsvHelperBroker, CsvHelperBroker>()
                .AddTransient<IHashBroker, HashBroker>()
                .AddTransient<IDateTimeBroker, DateTimeBroker>()
                .AddTransient<IDecisionBroker, DecisionBroker>()
                .AddTransient<IStorageBroker, StorageBroker>()
                .AddTransient<IBlobStorageBroker, BlobStorageBroker>()
                .AddIDecideClient(this.dependencyBroker.Configuration, claimsPrincipal);

            var serviceProvider = serviceCollection.BuildServiceProvider(new ServiceProviderOptions
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            });

            this.decisionOrchestrationService = serviceProvider.GetService<IDecisionOrchestrationService>();
            this.decisionService = serviceProvider.GetService<IDecisionService>();
            this.documentService = serviceProvider.GetService<IDocumentService>();
            this.csvHelperBroker = serviceProvider.GetService<ICsvHelperBroker>();
            this.hashBroker = serviceProvider.GetService<IHashBroker>();
            this.dateTimeBroker = serviceProvider.GetService<IDateTimeBroker>();
            this.decisionBroker = serviceProvider.GetService<IDecisionBroker>();
            this.storageBroker = serviceProvider.GetService<IStorageBroker>();
            this.blobStorageBroker = serviceProvider.GetService<IBlobStorageBroker>();
            this.decisionConfiguration = serviceProvider.GetService<DecisionConfiguration>();
            this.blobContainers = serviceProvider.GetService<BlobContainers>();
            iDecideClient = serviceProvider.GetService<IIDecideClient>();
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static string GetRandomStringWithLengthOf(int length)
        {
            string result = new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

            return result.Length > length ? result.Substring(0, length) : result;
        }

        private static int GetRandomNumber(int min = 2, int max = 10) =>
            new IntRange(min, max).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime().AddDays(7)).GetValue();

        private static List<Decision> CreateRandomDecisions()
        {
            return CreateDecisionFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                .ToList();
        }

        private static Filler<Decision> CreateDecisionFiller(DateTimeOffset dateTimeOffset, string userId = "")
        {
            userId = string.IsNullOrEmpty(userId) ? Guid.NewGuid().ToString() : userId;

            DecisionType decisionType = new()
            {
                Id = Guid.NewGuid()
            };

            Patient patient = new()
            {
                Id = Guid.NewGuid()
            };

            var filler = new Filler<Decision>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(decision => decision.DecisionChoice).Use(GetRandomStringWithLengthOf(255))
                .OnProperty(decision => decision.CreatedBy).Use(userId)
                .OnProperty(decision => decision.UpdatedBy).Use(userId);

            return filler;
        }
    }
}

// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Securities;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Brokers.Ontologies;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Services.Foundations.Documents;
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
        private readonly IDocumentService documentService;
        private readonly ITerminologyClient terminologyClient;
        private readonly OntologyConfiguration ontologyConfiguration;
        private readonly ICompareLogic compareLogic;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ISecurityBroker securityBroker;
        private readonly WireMockServer wireMockServer;

        public TerminologyTests(DependencyBroker dependencyBroker)
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
                new Claim(ClaimTypes.Role, "ISL.LDS.AdminSpa.Administrators"),
                new Claim(ClaimTypes.Role, "ISL.LDS.AdminSpa.Configurations")
            }));

            serviceCollection.AddLogging(builder =>
            {
                builder.AddConsole();
            });

            this.dependencyBroker.Configuration["ontologySettings:terminologyServerBaseUrl"] = this.wireMockServer.Url;
            serviceCollection.AddAddressClient(this.dependencyBroker.Configuration, claimsPrincipal);
            serviceCollection.AddTerminologyClient(this.dependencyBroker.Configuration);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            this.dateTimeBroker = serviceProvider.GetService<IDateTimeBroker>();
            terminologyClient = serviceProvider.GetService<ITerminologyClient>();
            ontologyConfiguration = serviceProvider.GetService<OntologyConfiguration>();
            terminologyPollService = serviceProvider.GetService<ITerminologyPollService>();
            documentService = serviceProvider.GetService<IDocumentService>();
            this.securityBroker = serviceProvider.GetService<ISecurityBroker>();
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
                .OnProperty(terminologyArtifact => terminologyArtifact.IsCore).Use(false)
                .OnProperty(terminologyArtifact => terminologyArtifact.IsDownloaded).Use(false)
                .OnProperty(terminologyArtifact => terminologyArtifact.IsError).Use(false)
                .OnProperty(terminologyArtifact => terminologyArtifact.CreatedBy).Use(user)
                .OnProperty(terminologyArtifact => terminologyArtifact.UpdatedBy).Use(user);

            return filler;
        }
    }
}
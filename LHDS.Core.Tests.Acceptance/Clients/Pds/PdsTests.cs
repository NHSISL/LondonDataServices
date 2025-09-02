// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Security.Claims;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Mesh;
using LHDS.Core.Brokers.Securities;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Orchestrations.Pds;
using LHDS.Core.Services.Foundations.PdsAudits;
using LHDS.Core.Tests.Acceptance.Brokers.DependencyBrokers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NEL.MESH.Models.Foundations.Mesh;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Pds
{
    [Collection(nameof(CoreTestCollection))]
    public partial class PdsTests
    {
        private readonly DependencyBroker dependencyBroker;
        private readonly Mock<IMeshBroker> meshBrokerMock;
        private readonly Mock<IBlobStorageBroker> blobStorageBrokerMock;
        private readonly IdentifierBroker identifierBroker;
        private readonly ISecurityBroker securityBroker;
        private readonly IPdsClient pdsClient;
        private readonly PdsConfiguration pdsConfiguration;
        private readonly IPdsAuditService pdsAuditService;

        public PdsTests(DependencyBroker dependencyBroker)
        {
            this.dependencyBroker = dependencyBroker;
            this.meshBrokerMock = new Mock<IMeshBroker>();
            this.blobStorageBrokerMock = new Mock<IBlobStorageBroker>();
            this.identifierBroker = new IdentifierBroker();
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

            var blobStorageSettings = dependencyBroker.Configuration
                .GetSection("blobStorage").Get<BlobStorageSettings>();

            serviceCollection
                .AddDbContextFactory<StorageBroker>()
                .AddSingleton<BlobContainers>(blobStorageSettings.BlobContainers)
                .AddTransient<IMeshBroker>(serviceProvider => meshBrokerMock.Object)
                .AddTransient<IBlobStorageBroker>(serviceProvider => blobStorageBrokerMock.Object)
                .AddPdsClientForAcceptance(this.dependencyBroker.Configuration, claimsPrincipal);

            var serviceProvider = serviceCollection.BuildServiceProvider(new ServiceProviderOptions
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            });

            this.pdsConfiguration = serviceProvider.GetService<PdsConfiguration>();
            this.pdsClient = serviceProvider.GetService<IPdsClient>();
            this.securityBroker = serviceProvider.GetService<ISecurityBroker>();
            this.pdsAuditService = serviceProvider.GetService<IPdsAuditService>();
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static List<string> GetRandomStrings(int count)
        {
            var messages = new List<string>();

            for (int i = 0; i < count; i++)
            {
                var message = GetRandomString();
                messages.Add(message);
            }

            return messages;
        }

        private static Message CreateRandomMessage() =>
            CreateMessageFiller().Create();

        private static Filler<Message> CreateMessageFiller()
        {
            var filler = new Filler<Message>();
            filler.Setup().OnProperty(message => message.Headers)
                .Use(new Dictionary<string, List<string>>());

            return filler;
        }
    }
}

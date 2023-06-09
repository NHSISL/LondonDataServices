// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using LHDS.Core.Brokers.Mesh;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.PdsAudits;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Tynamix.ObjectFiller;

namespace LHDS.Core.Tests.Acceptance.Clients.Pds
{
    public partial class PdsTests
    {
        private readonly Mock<IMeshBroker> meshBrokerMock;
        private readonly Mock<IBlobStorageBroker> blobStorageBrokerMock;
        private readonly IPdsClient pdsClient;

        public PdsTests()
        {
            this.meshBrokerMock = new Mock<IMeshBroker>();
            this.blobStorageBrokerMock = new Mock<IBlobStorageBroker>();

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
                .AddOptOutClientForAcceptance(configuration)
                .AddTransient<IMeshBroker>(serviceProvider => meshBrokerMock.Object)
                .AddTransient<IBlobStorageBroker>(serviceProvider => blobStorageBrokerMock.Object)
                .BuildServiceProvider();

            this.pdsClient = serviceProvider.GetService<IPdsClient>();
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static List<MeshMessage> GetRandomMessages(string filename)
        {
            int count = GetRandomNumber();
            var messages = new List<MeshMessage>();

            for (int i = 0; i < count;)
            {
                MeshMessage message = ComposeMessage.CreateMeshMessage(
                    mexTo: GetRandomString(),
                    mexWorkflowId: GetRandomString(),
                    fileContent: Encoding.ASCII.GetBytes(GetRandomString()),
                    mexSubject: GetRandomString(),
                    mexLocalId: Guid.NewGuid().ToString(),
                    mexFileName: filename);

                messages.Add(message);
            };

            return messages;
        }

        private static PdsAudit GetRandomPdsAudit(
            Guid identifier,
            Guid correlationIdentifier,
            string fileName,
            DateTimeOffset randomDate,
            string messageId)
        {
            PdsAudit pdsAudit = new PdsAudit
            {
                Id = identifier,
                CorrelationId = correlationIdentifier,
                FileName = fileName,
                Message = $"Sent message to mesh with id {messageId}",
                CreatedDate = randomDate,
                UpdatedDate = randomDate,
                CreatedBy = "System",
                UpdatedBy = "System"
            };

            return pdsAudit;
        }
    }
}

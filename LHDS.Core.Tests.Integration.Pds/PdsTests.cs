// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Brokers.Mesh;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Orchestrations.Pds;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.Mesh;
using LHDS.Core.Services.Foundations.PdsAudits;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace LHDS.Core.Tests.Integration.Pds
{
    public partial class PdsTests
    {
        private readonly IPdsClient pdsClient;
        private readonly IPdsAuditService pdsAuditService;
        private readonly IDocumentService documentService;
        private readonly IMeshService meshService;
        private readonly IIdentifierBroker identifierBroker;
        private readonly IBlobStorageBroker blobStorageBroker;
        private readonly PdsConfiguration pdsConfiguration;
        private readonly MeshConfiguration meshConfiguration;
        private readonly ITestOutputHelper output;

        public PdsTests(ITestOutputHelper output)
        {
            this.output = output;
            var environmentName = "Development";

            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile("local.appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfiguration configuration = configurationBuilder.Build();
            var windowsIdentity = WindowsIdentity.GetCurrent();
            var claimsPrincipal = new ClaimsPrincipal(windowsIdentity);

            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.AddApplicationInsights();
                })
                .AddDbContextFactory<StorageBroker>()
                .AddPdsClient(configuration, claimsPrincipal)
                .BuildServiceProvider();

            this.pdsClient = serviceProvider.GetService<IPdsClient>();
            this.documentService = serviceProvider.GetService<IDocumentService>();
            this.meshService = serviceProvider.GetService<IMeshService>();
            this.identifierBroker = serviceProvider.GetService<IIdentifierBroker>();
            this.blobStorageBroker = serviceProvider.GetService<IBlobStorageBroker>();
            this.pdsConfiguration = serviceProvider.GetService<PdsConfiguration>();
            this.meshConfiguration = serviceProvider.GetService<MeshConfiguration>();
            this.pdsAuditService = serviceProvider.GetService<IPdsAuditService>();
        }

        private static string GetHeaderValue(MeshMessage message, string keyToFind)
        {
            List<string> value = new List<string>();

            foreach (var key in message.Headers.Keys)
            {
                if (key.ToLower() == keyToFind.ToLower())
                {
                    message.Headers.TryGetValue(key, out value);

                    break;
                }
            }

            return value.FirstOrDefault();
        }
    }
}

// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Brokers.Mesh;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Orchestrations.OptOuts;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.Mesh;
using LHDS.Core.Services.Foundations.OptOuts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace LHDS.Core.Tests.Integration.OptOuts
{
    public partial class OptOutTests
    {
        private readonly IOptOutClient optOutClient;
        private readonly IDocumentService documentService;
        private readonly IMeshService meshService;
        private readonly IOptOutService optOutService;
        private readonly OptOutConfiguration optOutConfiguration;
        private readonly MeshConfiguration meshConfiguration;
        private readonly ITestOutputHelper output;

        public OptOutTests(ITestOutputHelper output)
        {
            this.output = output;
            var environmentName = "Development";

            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile("local.appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfiguration configuration = configurationBuilder.Build();

            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.AddApplicationInsights();
                })
                .AddOptOutClient(configuration)
                .BuildServiceProvider();

            this.optOutClient = serviceProvider.GetService<IOptOutClient>();
            this.documentService = serviceProvider.GetService<IDocumentService>();
            this.meshService = serviceProvider.GetService<IMeshService>();
            this.meshConfiguration = serviceProvider.GetService<MeshConfiguration>();
            this.optOutService = serviceProvider.GetService<IOptOutService>();
            this.optOutConfiguration = serviceProvider.GetService<OptOutConfiguration>();
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

        private async ValueTask<List<OptOut>> SetupTestNhsNumbersForRetrieveUpdatedMesh(string batchReference)
        {
            DateTimeOffset currentDateTime = DateTimeOffset.UtcNow;
            List<OptOut> setupOptOut = new List<OptOut>();

            setupOptOut.Add(new OptOut
            {
                Id = Guid.NewGuid(),
                NhsNumber = "9694116414",
                LastSentToMesh = currentDateTime,
                OptOutStatus = "Unknown",
                CacheTime = currentDateTime,
                CreatedDate = currentDateTime,
                UpdatedDate = currentDateTime,
                CreatedBy = "System",
                UpdatedBy = "System",
                BatchReference = batchReference
            });

            setupOptOut.Add(new OptOut
            {
                Id = Guid.NewGuid(),
                NhsNumber = "9694116422",
                LastSentToMesh = currentDateTime,
                OptOutStatus = "Unknown",
                CacheTime = currentDateTime,
                CreatedDate = currentDateTime,
                UpdatedDate = currentDateTime,
                CreatedBy = "System",
                UpdatedBy = "System",
                BatchReference = batchReference
            });

            setupOptOut.Add(new OptOut
            {
                Id = Guid.NewGuid(),
                NhsNumber = "9694116430",
                LastSentToMesh = currentDateTime,
                OptOutStatus = "Unknown",
                CacheTime = currentDateTime,
                CreatedDate = currentDateTime,
                UpdatedDate = currentDateTime,
                CreatedBy = "System",
                UpdatedBy = "System",
                BatchReference = batchReference
            });

            foreach (var optOut in setupOptOut)
            {
                var maybeOptOut = this.optOutService.RetrieveAllOptOuts()
                    .Where(opt => opt.NhsNumber == optOut.NhsNumber)
                        .FirstOrDefault();

                if (maybeOptOut != null)
                {
                    await this.optOutService.RemoveOptOutByIdAsync(maybeOptOut.Id);
                }

                await this.optOutService.AddOptOutAsync(optOut);
            }

            return setupOptOut;
        }

        private async ValueTask<List<OptOut>> SetupExpiredTestNhsNumbersForRetrieveUpdatedMesh(string batchReference)
        {
            DateTimeOffset auditDateTime = DateTimeOffset.UtcNow;
            DateTimeOffset expiredDateTime = DateTimeOffset.UtcNow.AddDays(-14);
            List<OptOut> setupOptOut = new List<OptOut>();

            setupOptOut.Add(new OptOut
            {
                Id = Guid.NewGuid(),
                NhsNumber = "9694116481",
                LastSentToMesh = expiredDateTime,
                OptOutStatus = "Unknown",
                CacheTime = expiredDateTime,
                CreatedDate = auditDateTime,
                UpdatedDate = auditDateTime,
                CreatedBy = "System",
                UpdatedBy = "System",
                BatchReference = batchReference
            });

            setupOptOut.Add(new OptOut
            {
                Id = Guid.NewGuid(),
                NhsNumber = "9694116511",
                LastSentToMesh = expiredDateTime,
                OptOutStatus = "Unknown",
                CacheTime = expiredDateTime,
                CreatedDate = auditDateTime,
                UpdatedDate = auditDateTime,
                CreatedBy = "System",
                UpdatedBy = "System",
                BatchReference = batchReference
            });

            setupOptOut.Add(new OptOut
            {
                Id = Guid.NewGuid(),
                NhsNumber = "9694116473",
                LastSentToMesh = expiredDateTime,
                OptOutStatus = "Unknown",
                CacheTime = expiredDateTime,
                CreatedDate = auditDateTime,
                UpdatedDate = auditDateTime,
                CreatedBy = "System",
                UpdatedBy = "System",
                BatchReference = batchReference
            });

            foreach (var optOut in setupOptOut)
            {
                var maybeOptOut = this.optOutService.RetrieveAllOptOuts()
                    .Where(opt => opt.NhsNumber == optOut.NhsNumber)
                        .FirstOrDefault();

                if (maybeOptOut != null)
                {
                    await this.optOutService.RemoveOptOutByIdAsync(maybeOptOut.Id);
                }

                await this.optOutService.AddOptOutAsync(optOut);
            }

            return setupOptOut;
        }

        private async ValueTask<string> SetupSimulatedMeshMessage(string batchReference)
        {
            MeshMessage simulatedMeshReply = new MeshMessage
            {
                StringContent = "9694116414," + Environment.NewLine
                                    + "9694116422," + Environment.NewLine
                                    + "9694116430," + Environment.NewLine,
                Headers = {
                        { "Content-Type", new List<string> { "text/plain" } },
                        { "Mex-From", new List<string> { this.meshConfiguration.MailboxId } },
                        { "Mex-To", new List<string> { this.meshConfiguration.MailboxId } },
                        { "Mex-WorkflowID", new List<string> { this.optOutConfiguration.WorkflowId } },
                        { "Mex-LocalID", new List<string> { batchReference } },
                        { "Mex-FileName", new List<string> { batchReference } },
                    }
            };

            await this.meshService.SendMessageAsync(simulatedMeshReply);

            return simulatedMeshReply.StringContent;
        }


    }
}
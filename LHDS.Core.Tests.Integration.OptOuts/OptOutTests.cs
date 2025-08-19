// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Storages.Sql;
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
using Tynamix.ObjectFiller;
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
            var windowsIdentity = WindowsIdentity.GetCurrent();
            var claimsPrincipal = new ClaimsPrincipal(windowsIdentity);

            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.AddApplicationInsights();
                })
                .AddDbContext<StorageBroker>()
                .AddScoped<IStorageBroker>(service => service.GetRequiredService<StorageBroker>())
                .AddOptOutClient(configuration, claimsPrincipal)
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
            List<OptOut> setupOptOut = new List<OptOut>
            {
                new OptOut
                {
                    Id = Guid.NewGuid(),
                    NhsNumber = GenerateValidNhsNumber(),
                    LastSentToMesh = currentDateTime,
                    Status = "Unknown",
                    UniqueReference = "4d30a841-05f2-43e1-afa1-4280498841ff",
                    CacheTime = currentDateTime,
                    CreatedDate = currentDateTime,
                    UpdatedDate = currentDateTime,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    BatchReference = batchReference
                },

                new OptOut
                {
                    Id = Guid.NewGuid(),
                    NhsNumber = GenerateValidNhsNumber(),
                    LastSentToMesh = currentDateTime,
                    Status = "Opt-Out",
                    UniqueReference = "8a935b2d-9e7d-4655-8488-66d4837c81c1",
                    CacheTime = currentDateTime,
                    CreatedDate = currentDateTime,
                    UpdatedDate = currentDateTime,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    BatchReference = batchReference
                },

                new OptOut
                {
                    Id = Guid.NewGuid(),
                    NhsNumber = GenerateValidNhsNumber(),
                    LastSentToMesh = currentDateTime,
                    Status = "Opt-In",
                    UniqueReference = "8a935b2d-9e7d-4655-8488-66d4837c81c2",
                    CacheTime = currentDateTime,
                    CreatedDate = currentDateTime,
                    UpdatedDate = currentDateTime,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    BatchReference = batchReference
                },

                new OptOut
                {
                    Id = Guid.NewGuid(),
                    NhsNumber = GenerateValidNhsNumber(),
                    LastSentToMesh = currentDateTime,
                    Status = "Unknown",
                    UniqueReference = "8a935b2d-9e7d-4655-8488-66d4837c81c43",
                    CacheTime = currentDateTime,
                    CreatedDate = currentDateTime,
                    UpdatedDate = currentDateTime,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    BatchReference = batchReference
                }
            };

            foreach (var optOut in setupOptOut)
            {
                var allOptOuts = await this.optOutService.RetrieveAllOptOutsAsync();

                var maybeOptOut = allOptOuts
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
            List<OptOut> setupOptOut = new List<OptOut>
            {
                new OptOut
                {
                    Id = Guid.NewGuid(),
                    NhsNumber = "9694116481",
                    LastSentToMesh = expiredDateTime,
                    Status = "Unknown",
                    UniqueReference = "9e48851c-76c1-4db1-aa78-c36c0eb86223",
                    CacheTime = expiredDateTime,
                    CreatedDate = auditDateTime,
                    UpdatedDate = auditDateTime,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    BatchReference = batchReference
                },

                new OptOut
                {
                    Id = Guid.NewGuid(),
                    NhsNumber = "9694116511",
                    LastSentToMesh = expiredDateTime,
                    Status = "Unknown",
                    UniqueReference = "9e48851c-76c1-4db1-aa78-c36c0eb86224",
                    CacheTime = expiredDateTime,
                    CreatedDate = auditDateTime,
                    UpdatedDate = auditDateTime,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    BatchReference = batchReference
                },

                new OptOut
                {
                    Id = Guid.NewGuid(),
                    NhsNumber = "9694116473",
                    LastSentToMesh = expiredDateTime,
                    Status = "Unknown",
                    UniqueReference = "9e48851c-76c1-4db1-aa78-c36c0eb86225",
                    CacheTime = expiredDateTime,
                    CreatedDate = auditDateTime,
                    UpdatedDate = auditDateTime,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    BatchReference = batchReference
                }
            };

            foreach (var optOut in setupOptOut)
            {
                var allOptOuts = await this.optOutService.RetrieveAllOptOutsAsync();

                var maybeOptOut = allOptOuts
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

        private async ValueTask<string> SetupSimulatedMeshMessage(string batchReference, List<string> idsFromMesh)
        {
            StringBuilder stringContent = new StringBuilder();

            foreach (var id in idsFromMesh)
            {
                stringContent.AppendLine($"{id},");
            }

            await this.meshService.SendMessageAsync(
                mexTo: this.meshConfiguration.MailboxId,
                mexWorkflowId: this.optOutConfiguration.WorkflowId,
                fileContent: Encoding.UTF8.GetBytes(stringContent.ToString()),
                mexLocalId: batchReference,
                mexFileName: batchReference,
                contentType: "text/plain");

            return stringContent.ToString();
        }

        private static string GenerateValidNhsNumber()
        {
            int total = 10;
            string formattedNhsNumber = string.Empty;

            while (total == 10)
            {
                var randomNumber = new LongRange(100000000, 999999999);
                formattedNhsNumber = randomNumber.GetValue().ToString();
                int[] multiplers = new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                int currentNumber;
                int currentSum = 0;
                int currentMultipler;
                string currentString;
                int remainder;

                for (int i = 0; i <= 8; i++)
                {
                    currentString = formattedNhsNumber.Substring(i, 1);

                    currentNumber = Convert.ToInt16(currentString);
                    currentMultipler = multiplers[i];
                    currentSum = currentSum + (currentNumber * currentMultipler);
                }

                remainder = currentSum % 11;
                total = 11 - remainder;

                if (total.Equals(11))
                {
                    total = 0;
                }

                if (total != 10)
                {
                    break;
                }
            }

            string checkNumber = total.ToString();

            return $"{formattedNhsNumber}{checkNumber}";
        }
    }
}
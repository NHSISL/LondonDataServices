// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LHDS.Core.Brokers.CsvHelpers;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Mesh;
using LHDS.Core.Brokers.Securities;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Brokers.Mesh;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Orchestrations.OptOuts;
using LHDS.Core.Services.Foundations.OptOuts;
using LHDS.Core.Tests.Acceptance.Brokers.DependencyBrokers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.OptOuts
{
    [Collection(nameof(CoreTestCollection))]
    public partial class OptOutTests
    {
        private readonly DependencyBroker dependencyBroker;
        private readonly Mock<IBlobStorageBroker> blobStorageBrokerMock;
        private readonly Mock<IMeshBroker> meshBrokerMock;
        private readonly IOptOutClient optOutClient;
        private readonly ICsvHelperBroker csvHelperBroker;
        private readonly MeshConfiguration meshConfiguration;
        private readonly OptOutConfiguration optOutConfiguration;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly ISecurityBroker securityBroker;
        private readonly IOptOutService optOutService;

        public OptOutTests(DependencyBroker dependencyBroker)
        {
            this.dependencyBroker = dependencyBroker;
            this.blobStorageBrokerMock = new Mock<IBlobStorageBroker>();
            this.meshBrokerMock = new Mock<IMeshBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging(builder =>
            {
                builder.AddConsole();
            });

            var blobStorageSettings = dependencyBroker.Configuration
                .GetSection("blobStorage").Get<BlobStorageSettings>();

            serviceCollection.AddSingleton<BlobContainers>(blobStorageSettings.BlobContainers);

            serviceCollection
                .AddTransient<IMeshBroker>(serviceProvider => meshBrokerMock.Object)
                .AddTransient<IBlobStorageBroker>(serviceProvider => blobStorageBrokerMock.Object)
                .AddTransient<IDateTimeBroker>(serviceProvider => dateTimeBrokerMock.Object);

            serviceCollection.AddOptOutClientForAcceptance(this.dependencyBroker.Configuration);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            this.optOutConfiguration = serviceProvider.GetService<OptOutConfiguration>();
            this.meshConfiguration = serviceProvider.GetService<MeshConfiguration>();
            this.csvHelperBroker = serviceProvider.GetService<ICsvHelperBroker>();
            this.securityBroker = serviceProvider.GetService<ISecurityBroker>();
            this.optOutService = serviceProvider.GetService<IOptOutService>();
            optOutClient = serviceProvider.GetService<IOptOutClient>();
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static int GetRandomNumber(int min = 2, int max = 10) =>
            new IntRange(min, max).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime().AddDays(7)).GetValue();

        private static List<OptOut> CreateRandomOptOuts(int count, DateTimeOffset dateTimeOffset)
        {
            List<OptOut> optOuts = new List<OptOut>();

            for (int i = 0; i < count; i++)
            {
                var reference = Guid.NewGuid();

                var optOut = new OptOut
                {
                    Id = reference,
                    NhsNumber = GenerateValidNhsNumber(),
                    Status = "Unknown",
                    UniqueReference = reference.ToString(),
                    CacheTime = dateTimeOffset.AddDays(-50),
                    LastSentToMesh = dateTimeOffset.AddDays(-50),
                    CreatedDate = dateTimeOffset.AddSeconds(i),
                    CreatedBy = "System",
                    UpdatedDate = dateTimeOffset.AddSeconds(i),
                    UpdatedBy = "System",
                };

                optOuts.Add(optOut);
            }

            return optOuts.OrderBy(optOut => optOut.CreatedDate).ToList();
        }

        private static List<OptOutIdentifier> CreateRandomOptOutIdentifiersList()
        {
            return CreateOptOutIdentifierFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: 1)
                    .ToList();
        }

        private static Filler<OptOutIdentifier> CreateOptOutIdentifierFiller(DateTimeOffset dateTimeOffset)
        {
            var filler = new Filler<OptOutIdentifier>();

            filler.Setup()
                .OnProperty(optOut => optOut.NhsNumber).Use(GenerateValidNhsNumber())
                .OnProperty(optOut => optOut.UniqueReference).Use(GetRandomString())
                .OnProperty(optOut => optOut.Status).Use(GetRandomString())
                .OnProperty(optOut => optOut.StatusChangedDateTime).Use(GetRandomDateTimeOffset());
            return filler;
        }

        private static List<OptOut> CreateRandomOptOutsList(
            int count,
            DateTimeOffset dateTimeOffset,
            string batchReference
            )
        {
            var optOuts = new List<OptOut>();

            for (int i = 0; i < count; i++)
            {
                var optOut = CreateOptOutFiller(dateTimeOffset).Create();
                optOut.BatchReference = batchReference;
                optOuts.Add(optOut);
            }

            return optOuts;
        }

        private static List<OptOut> CreateRandomOptOutsList(
            int count,
            DateTimeOffset dateTimeOffset,
            string batchReference,
            string status
            )
        {
            var optOuts = new List<OptOut>();

            for (int i = 0; i < count; i++)
            {
                var optOut = CreateOptOutFiller(dateTimeOffset, status).Create();
                optOut.BatchReference = batchReference;
                optOuts.Add(optOut);
            }

            return optOuts;
        }

        private static Filler<OptOut> CreateOptOutFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<OptOut>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(optOut => optOut.NhsNumber).Use(GenerateValidNhsNumber())
                .OnProperty(optOut => optOut.Status).Use("Unknown")
                .OnProperty(optOut => optOut.CreatedBy).Use(user)
                .OnProperty(optOut => optOut.UpdatedBy).Use(user);

            return filler;
        }

        private static Filler<OptOut> CreateOptOutFiller(DateTimeOffset dateTimeOffset, string status)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<OptOut>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(optOut => optOut.NhsNumber).Use(GenerateValidNhsNumber())
                .OnProperty(optOut => optOut.Status).Use(status)
                .OnProperty(optOut => optOut.CreatedBy).Use(user)
                .OnProperty(optOut => optOut.UpdatedBy).Use(user);

            return filler;
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

        public static string GenerateCsv(
            List<OptOut> optOuts,
            bool hasHeaderRecord,
            bool shouldAddTrailingComma)
        {
            StringBuilder csvBuilder = new StringBuilder();

            if (hasHeaderRecord)
            {
                csvBuilder.AppendLine("UniqueReference,NHSNo");
            }

            foreach (var optOut in optOuts)
            {
                csvBuilder.Append($"{optOut.UniqueReference},");
                string nhsNumber = $"{optOut.NhsNumber}";

                if (shouldAddTrailingComma)
                {
                    nhsNumber += ",";
                }

                csvBuilder.AppendLine(nhsNumber);
            }

            return csvBuilder.ToString();
        }
    }
}

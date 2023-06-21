// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using LHDS.Core.Brokers.CsvMappers;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Mesh;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Brokers.Mesh;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Orchestrations.OptOuts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Tynamix.ObjectFiller;

namespace LHDS.Core.Tests.Acceptance.Clients.OptOuts
{
    public partial class OptOutTests
    {
        private readonly Mock<IBlobStorageBroker> blobStorageBrokerMock;
        private readonly Mock<IMeshBroker> meshBrokerMock;
        private readonly IOptOutClient optOutClient;
        private readonly CsvMapperBroker csvMapperBroker;
        private readonly DateTimeBroker dateTimeBroker;
        private readonly OptOutConfiguration optOutConfiguration;
        private readonly MeshConfiguration meshConfiguration;

        public OptOutTests()
        {
            this.blobStorageBrokerMock = new Mock<IBlobStorageBroker>();
            this.meshBrokerMock = new Mock<IMeshBroker>();
            this.csvMapperBroker = new CsvMapperBroker();

            string aspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var args = Environment.GetCommandLineArgs();
            var environmentArg = args.FirstOrDefault(arg => arg.StartsWith("--environment="));

            var environmentName = !string.IsNullOrEmpty(aspNetCoreEnvironment)
                ? aspNetCoreEnvironment
                : !string.IsNullOrEmpty(environmentArg)
                    ? environmentArg
                    : "Development";

            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile("local.appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfiguration configuration = configurationBuilder.Build();

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging(builder =>
            {
                builder.AddConsole();
            });

            serviceCollection.AddOptOutClientForAcceptance(configuration);

            serviceCollection
                .AddTransient<IMeshBroker>(serviceProvider => meshBrokerMock.Object)
                .AddTransient<IBlobStorageBroker>(serviceProvider => blobStorageBrokerMock.Object);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            this.optOutConfiguration = serviceProvider.GetService<OptOutConfiguration>();
            this.meshConfiguration = serviceProvider.GetService<MeshConfiguration>();
            optOutClient = serviceProvider.GetRequiredService<IOptOutClient>();
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static int GetRandomNumber(int min = 2, int max = 10) =>
            new IntRange(min, max).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime().AddDays(7)).GetValue();

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

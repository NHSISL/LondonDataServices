// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Text;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Orchestrations.Downloads;
using LHDS.Core.Providers.Downloads.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tynamix.ObjectFiller;
using Xunit.Abstractions;

namespace LHDS.Core.Tests.Integration.Landings
{
    public partial class LandingTests
    {
        private readonly ITestOutputHelper output;
        private readonly ILandingClient landingClient;
        private readonly ILoggingBroker loggingBroker;
        private readonly IBlobStorageBroker blobStorageBroker;
        private readonly LandingConfiguration landingConfiguration;


        public LandingTests(ITestOutputHelper output)
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
                .AddLandingClient(configuration)
                .UseFtpDownloadProvider(configuration, builder => builder.AddFtpDownloadProvider())
                .BuildServiceProvider();

            landingClient = serviceProvider.GetService<ILandingClient>();
            loggingBroker = serviceProvider.GetService<ILoggingBroker>();
            blobStorageBroker = serviceProvider.GetService<IBlobStorageBroker>();
            landingConfiguration = serviceProvider.GetService<LandingConfiguration>();
        }

        private static int GetRandomNumber(int min = 2, int max = 10) =>
            new IntRange(min, max).GetValue();

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

        private static string CreateRandomListNhsNumbers(int count)
        {
            StringBuilder list = new StringBuilder();

            for (int i = 0; i < count; i++)
            {
                list.AppendLine($"{GenerateValidNhsNumber()},");
            }

            return list.ToString();
        }
    }
}

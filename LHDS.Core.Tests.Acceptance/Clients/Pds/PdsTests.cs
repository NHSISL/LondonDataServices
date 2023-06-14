// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Mesh;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Orchestrations.Pds;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NEL.MESH.Models.Foundations.Mesh;
using Tynamix.ObjectFiller;

namespace LHDS.Core.Tests.Acceptance.Clients.Pds
{
    public partial class PdsTests
    {
        private readonly Mock<IMeshBroker> meshBrokerMock;
        private readonly Mock<IBlobStorageBroker> blobStorageBrokerMock;
        private readonly IPdsClient pdsClient;
        private readonly PdsConfiguration pdsConfiguration;

        public PdsTests()
        {
            this.meshBrokerMock = new Mock<IMeshBroker>();
            this.blobStorageBrokerMock = new Mock<IBlobStorageBroker>();

            string aspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            string env = Environment.GetEnvironmentVariable("environmentName");
            var args = Environment.GetCommandLineArgs();
            var environmentNameArg = args.FirstOrDefault(arg => arg.StartsWith("--environmentName="));

            var environmentName = !string.IsNullOrEmpty(aspNetCoreEnvironment)
                ? aspNetCoreEnvironment
                : !string.IsNullOrEmpty(env)
                    ? env
                    : "Development";

            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile("local.appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfiguration configuration = configurationBuilder.Build();
            var serviceCollection = new ServiceCollection();

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            var logger = loggerFactory.CreateLogger<LoggingBroker>();
            serviceCollection.AddTransient(serviceProvider => logger);
            serviceCollection.AddPdsClient(configuration);

            serviceCollection
                .AddTransient<IMeshBroker>(serviceProvider => meshBrokerMock.Object)
                .AddTransient<IBlobStorageBroker>(serviceProvider => blobStorageBrokerMock.Object);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            this.pdsConfiguration = serviceProvider.GetService<PdsConfiguration>();
            this.pdsClient = serviceProvider.GetService<IPdsClient>();
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

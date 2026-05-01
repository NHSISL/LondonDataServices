// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using Azure.Core;
using Azure.Identity;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


using Xunit;

namespace LHDS.Core.Tests.Integration.IDecide
{
    public partial class IDecideTests
    {
        private readonly IIDecideClient iDecideClient;
        private readonly IStorageBroker storageBroker;
        private readonly BlobContainers blobContainers;
        private readonly ITestOutputHelper output;
        private readonly TokenCredential credential;

        public IDecideTests(ITestOutputHelper output)
        {
            this.output = output;
            var environmentName = "Development";

            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfiguration configuration = configurationBuilder.Build();

            //TODO: [26630] - Remove internal constructor and apply config for test managed identity 
            // in appsettings.Development and GitHub secrets [DH]  use config to get managed identity token
            this.credential = new DefaultAzureCredential();

            var tokenRequestContext = new TokenRequestContext(
                new[] { "https://graph.microsoft.com/.default" });

            AccessToken accessToken = this.credential
                .GetTokenAsync(tokenRequestContext, default)
                .GetAwaiter()
                .GetResult();

            var serviceProvider = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.AddApplicationInsights();
                })
                .AddDbContextFactory<StorageBroker>()

                //TODO: [26630] - Remove internal constructor and apply config for test managed identity 
                // in appsettings.Development and GitHub secrets [DH]
                .AddSingleton<TokenCredential>(credential)

                //TODO: [26630] - Remove internal constructor and apply config for test managed identity 
                // in appsettings.Development and GitHub secrets [DH]
                .AddIDecideClient(configuration, accessToken.Token, includeInteractiveCredentials: true)

                .BuildServiceProvider();

            this.storageBroker = serviceProvider.GetRequiredService<StorageBroker>();
            this.blobContainers = serviceProvider.GetRequiredService<BlobContainers>();
            this.iDecideClient = serviceProvider.GetRequiredService<IIDecideClient>();
        }
    }
}


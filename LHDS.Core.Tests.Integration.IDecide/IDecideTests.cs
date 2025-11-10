// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Principal;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace LHDS.Core.Tests.Integration.IDecide
{
    public partial class IDecideTests
    {
        private readonly IIDecideClient iDecideClient;
        private readonly IStorageBroker storageBroker;
        private readonly BlobContainers blobContainers;
        private readonly ITestOutputHelper output;

        public IDecideTests(ITestOutputHelper output)
        {
            this.output = output;
            var environmentName = "Development";

            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfiguration configuration = configurationBuilder.Build();

            ClaimsPrincipal claimsPrincipal;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var windowsIdentity = WindowsIdentity.GetCurrent();
                claimsPrincipal = new ClaimsPrincipal(windowsIdentity);
            }
            else
            {
                claimsPrincipal = new ClaimsPrincipal();
            }

            var serviceProvider = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.AddApplicationInsights();
                })
                .AddDbContextFactory<StorageBroker>()
                .AddIDecideClient(configuration, claimsPrincipal)
                .BuildServiceProvider();

            this.storageBroker = serviceProvider.GetRequiredService<StorageBroker>();
            this.blobContainers = serviceProvider.GetRequiredService<BlobContainers>();
            iDecideClient = serviceProvider.GetRequiredService<IIDecideClient>();
        }
    }
}

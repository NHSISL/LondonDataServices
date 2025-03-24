// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Security.Claims;
using System.Security.Principal;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Providers.Cryptography.Extensions;
using LHDS.Core.Providers.Downloads;
using LHDS.Core.Providers.Downloads.MockDownloads;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace LHDS.Core.Tests.Integration.Decryptions
{
    public partial class DecryptionTests
    {
        private readonly ITestOutputHelper output;
        private readonly IDecryptionClient decryptionClient;
        private readonly IDocumentService documentService;
        private readonly IIngestionTrackingService ingestionTrackingService;

        public DecryptionTests(ITestOutputHelper output)
        {
            this.output = output;
            var environmentName = "Production";

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
                .AddDecryptionClient(configuration, claimsPrincipal)
                .UseGpgCryptographyProvider(configuration, builder => builder.AddGpgCryptographyProvider())
                .AddTransient<IDownloadProvider, MockDownloadProvider>()
                .BuildServiceProvider();

            decryptionClient = serviceProvider.GetService<IDecryptionClient>();
            ingestionTrackingService = serviceProvider.GetService<IIngestionTrackingService>();
            documentService = serviceProvider.GetService<IDocumentService>();
        }
    }
}

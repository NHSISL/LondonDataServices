// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Providers.Cryptography.Extensions;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LHDS.Clients.Tests.Decryption.Manual
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var environmentName = args.FirstOrDefault() ?? "Development";

            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("local.appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfiguration configuration = configurationBuilder.Build();

            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.AddApplicationInsights();
                })
                .AddDecryptionClient(configuration)
                .UseGpgCryptographyProvider(configuration, builder => builder.AddGpgCryptographyProvider())
                .BuildServiceProvider();

            var decryptionClient = serviceProvider.GetService<IDecryptionClient>();

            IIngestionTrackingService ingestionTrackingService =
                serviceProvider.GetService<IIngestionTrackingService>();

            var items = ingestionTrackingService.RetrieveAllIngestionTracking()
                .Where(ingestionTrackingService => ingestionTrackingService.Decrypted == false);

            foreach (IngestionTracking item in items)
            {
                await decryptionClient.DecryptAsync(item.Id);
            }
        }
    }
}
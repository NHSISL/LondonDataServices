// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------
using System.Linq;
using System.Threading.Tasks;
using LHDS.Landings.Client.Clients;
using LHDS.Landings.Client.Clients.Extensions;
using LHDS.Landings.Client.Models.Foundations.IngestionTrackings;
using LHDS.Landings.Client.Providers.Cryptography.Extensions;
using LHDS.Landings.Client.Services.Foundations.IngestionTrackings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
                .AddLogging()
                .AddDecryptionClient(configuration)
                .UseGpgCryptographyProvider(configuration, builder => builder.AddGpgCryptographyProvider())
                .BuildServiceProvider();

            var decryptionClient = serviceProvider.GetService<IDecryptionClient>();

            IIngestionTrackingService ingestionTrackingService =
                (IIngestionTrackingService)serviceProvider.GetServices<IIngestionTrackingService>();

            var items = ingestionTrackingService.RetrieveAllIngestionTracking()
                .Where(ingestionTrackingService => ingestionTrackingService.Decrypted == true);

            foreach (IngestionTracking item in items)
            {
                await decryptionClient.DecryptAsync(item.Id);
            }
        }
    }
}
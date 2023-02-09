// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Providers.Downloads.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LHDS.Core.Landings.Tests.Manual
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
                .AddLandingClient(configuration)
                .UseFtpDownloadProvider(configuration, builder => builder.AddFtpDownloadProvider())
                .BuildServiceProvider();

            var landingClient = serviceProvider.GetService<ILandingClient>();

            if (landingClient != null)
            {
                await landingClient.ProcessAsync();
            }
        }
    }
}
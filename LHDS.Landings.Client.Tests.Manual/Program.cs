// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Landings.Client.Clients;
using LHDS.Landings.Client.Clients.Extensions;
using LHDS.Landings.Client.Providers.Downloads.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LHDS.Landings.Client.Tests.Manual
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var environmentName = args.FirstOrDefault() ?? "Development";

                var configurationBuilder = new ConfigurationBuilder()
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true);

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
                    Task.Run(async () => await landingClient.ProcessAsync());
                }
            }
            catch (global::System.Exception ex)
            {
                throw ex;
            }
            
        }
    }
}
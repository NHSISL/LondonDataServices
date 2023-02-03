// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Landings.Client.Clients.Extensions;
using LHDS.Landings.Client.Providers.Downloads.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NEL.DDS.InterfaceLayer.Function.Download.Client.AzureBlobs;

namespace LHDS.Landings.Client.Tests.Manual
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var environmentName = args.FirstOrDefault() ?? "Development";

            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("local.appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true);

            IConfiguration configuration = configurationBuilder.Build();

            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddLandingClient(configuration)
                .UseFtpDownloadProvider(configuration, builder => builder.AddFtpDownloadProvider())
                .BuildServiceProvider();


            //var landingClient = serviceProvider.GetService<ILandingClient>();

            /*if (landingClient != null)
            {
                Task.Run(async () => await landingClient.ProcessAsync());
            }*/

            var blobClient = serviceProvider.GetService<IAzureBlobClient>();
            var testFile = File.ReadAllBytes(@"C:\Temp\LHDS\LHDS.pdf");
            await blobClient.UploadFileAsync("test1.pdf", new MemoryStream(testFile), "emislanding");
            await blobClient.UploadFileAsync("test2.pdf", new MemoryStream(testFile), "emislanding");
            await blobClient.UploadFileAsync("test3.pdf", new MemoryStream(testFile), "emislanding");


        }
    }
}
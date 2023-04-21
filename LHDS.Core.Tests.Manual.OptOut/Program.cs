// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LHDS.Core.Tests.Manual.OptOut
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfiguration configuration = configurationBuilder.Build();

            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.AddApplicationInsights();
                })
                .AddOptutClient(configuration)
                .BuildServiceProvider();

            var optOutClient = serviceProvider.GetService<IOptOutClient>();

            try
            {
                if (optOutClient != null)
                {
                    while (true)
                    {
                        Console.WriteLine("Enter the number of the method you want to run:");
                        Console.WriteLine("1. RetrieveOptOutStatusAsync()");
                        Console.WriteLine("2. PushExpiredOptOutsToMeshForRenewalAsync()");
                        Console.WriteLine("3. RetrieveUpdatedMeshOptOutStatusChangesAsync()");
                        Console.WriteLine("Enter any other key to exit.");

                        string input = Console.ReadLine();
                        if (!int.TryParse(input, out int selectedMethod))
                        {
                            break;
                        }

                        switch (selectedMethod)
                        {
                            case 1:
                                byte[] fileBytes = File.ReadAllBytes(@"Resources\testfile.csv");

                                FileInfo fi = new FileInfo(@"Resources\testfile.csv");
                                var fileName = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);

                                await optOutClient.RetrieveOptOutStatusAsync(optOutFile: fileBytes, fileName: fileName);
                                break;
                            case 2:
                                await optOutClient.PushExpiredOptOutsToMeshForRenewalAsync();
                                break;
                            case 3:
                                await optOutClient.RetrieveUpdatedMeshOptOutStatusChangesAsync();
                                break;
                            default:
                                Console.WriteLine("Invalid selection. Exiting...");
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ILoggingBroker logger = (ILoggingBroker)serviceProvider.GetService(typeof(ILoggingBroker));
                logger.LogError(ex);
                Console.WriteLine(ex.Message);
            }
        }
    }
}
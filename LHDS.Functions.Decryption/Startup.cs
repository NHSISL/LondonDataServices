// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Landings.Client.Clients.Extensions;
using LHDS.Landings.Client.Providers.Cryptography.Extensions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(LHDS.Functions.Decryption.Startup))]
namespace LHDS.Functions.Decryption
{
    /// <summary>
    /// The Startup class for the Azure Function.
    /// </summary>
    public class Startup : FunctionsStartup
    {
        /// <summary>
        /// The Configuration Method.
        /// </summary>
        /// <param name="builder">The Builder.</param>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            FunctionsHostBuilderContext context = builder.GetContext();

            var configurationBuilder = new ConfigurationBuilder()
                   .AddJsonFile(
                        path: Path.Combine(context.ApplicationRootPath, "local.settings.json"),
                        optional: true, reloadOnChange: true)
                   .AddJsonFile(path: Path.Combine(context.ApplicationRootPath, "appsettings.json"))
                   .AddJsonFile(
                        path: Path.Combine(context.ApplicationRootPath, $"appsettings.{context.EnvironmentName}.json"),
                        optional: true)
                   .AddEnvironmentVariables();

            IConfiguration configuration = configurationBuilder.Build();
            builder.Services.AddTransient<IConfiguration>(_ => configuration);

            builder.Services
                .AddLogging()
                .AddDecryptionClient(configuration)
                .UseGpgCryptographyProvider(configuration, builder => builder.AddGpgCryptographyProvider());
        }
    }
}

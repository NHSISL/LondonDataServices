// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.IO;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Providers.Downloads.Extensions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: FunctionsStartup(typeof(LHDS.Functions.Landings.Tpp.Startup))]
namespace LHDS.Functions.Landings.Tpp
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
                   .AddEnvironmentVariables("LHDS_");

            IConfiguration configuration = configurationBuilder.Build();
            builder.Services.AddTransient<IConfiguration>(_ => configuration);

            builder.Services
                .AddLogging(setup =>
                {
                    setup.AddApplicationInsights();
                    setup.AddConsole();
                })
                .AddLandingClient(configuration)
                .UseRestDownloadProvider(builder => builder.AddRestDownloadProvider());
        }
    }
}

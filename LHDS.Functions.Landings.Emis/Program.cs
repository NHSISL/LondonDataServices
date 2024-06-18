// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.IO;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Providers.Cryptography.Extensions;
using LHDS.Core.Providers.Downloads.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = new HostBuilder()
    .ConfigureAppConfiguration(config =>
    {
        var env = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT");
        config.SetBasePath(Directory.GetCurrentDirectory())

        .AddJsonFile(path: "appsettings.json")
        .AddJsonFile(
            path: $"appsettings.{env}.json",
            optional: true)
        .AddJsonFile(
            path: "appsettings.local.json",
            optional: true,
            reloadOnChange: true)
        .AddEnvironmentVariables();
    })
    .ConfigureServices((context, services) =>
    {

        services
            .AddLogging(setup =>
            {
                setup.AddApplicationInsights();
                setup.AddConsole();
            })
           .AddEmisLandingClient(context.Configuration)
           .AddDecryptionClient(context.Configuration)
           .UseGpgCryptographyProvider(context.Configuration, builder => builder.AddGpgCryptographyProvider())
           .UseFtpDownloadProvider(context.Configuration, builder => builder.AddFtpDownloadProvider());
    })
    .UseDefaultServiceProvider(options => options.ValidateScopes = false)
    .ConfigureFunctionsWorkerDefaults()
    .Build();

await host.RunAsync();

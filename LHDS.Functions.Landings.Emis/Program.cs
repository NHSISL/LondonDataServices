// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using Azure.Core;
using Azure.Identity;
using LHDS.Core.Brokers.Storages.Sql;
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
        .AddEnvironmentVariables();
    })
    .ConfigureServices((context, services) =>
    {
        var credential = new DefaultAzureCredential();
        var tokenRequestContext = new TokenRequestContext(new[] { "https://graph.microsoft.com/.default" });
        AccessToken accessToken = credential.GetTokenAsync(tokenRequestContext).Result;

        services
            .AddLogging(setup =>
            {
                setup.AddApplicationInsights();
                setup.AddConsole();
            })
            .AddDbContextFactory<StorageBroker>()
            .AddEmisLandingClient(context.Configuration, accessToken.Token)
            .AddDecryptionClient(context.Configuration, accessToken.Token)
            .UseGpgCryptographyProvider(context.Configuration, builder => builder.AddGpgCryptographyProvider())
            .UseFtpDownloadProvider(context.Configuration, builder => builder.AddFtpDownloadProvider());
    })
    .UseDefaultServiceProvider(options => options.ValidateScopes = false)
    .ConfigureFunctionsWorkerDefaults()
    .Build();

await host.RunAsync();

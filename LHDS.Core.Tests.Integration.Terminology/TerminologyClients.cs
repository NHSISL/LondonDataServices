// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Security.Claims;
using System.Security.Principal;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace LHDS.Core.Tests.Integration.Terminology
{
    public partial class TerminologyClients
    {
        private readonly ITerminologyClient terminologyClient;
        private readonly ITestOutputHelper output;

        public TerminologyClients(ITestOutputHelper output)
        {
            this.output = output;
            var environmentName = "Development";

            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile("local.appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfiguration configuration = configurationBuilder.Build();
            var windowsIdentity = WindowsIdentity.GetCurrent();
            var claimsPrincipal = new ClaimsPrincipal(windowsIdentity);

            var serviceProvider = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.AddApplicationInsights();
                })
                .AddDbContext<StorageBroker>()
                .AddScoped<IStorageBroker>(service => service.GetRequiredService<StorageBroker>())
                .AddTerminologyClient(configuration, claimsPrincipal)
                .BuildServiceProvider();

            this.terminologyClient = serviceProvider.GetService<ITerminologyClient>(); ;
        }
    }
}

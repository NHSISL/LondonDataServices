// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Security.Claims;
using System.Security.Principal;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace LHDS.Core.Tests.Integration.Terminology
{
    public partial class TerminologyClientTests
    {
        private readonly ITerminologyClient terminologyClient;
        private readonly ITestOutputHelper output;

        public TerminologyClientTests(ITestOutputHelper output)
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

            SqlAuthenticationProvider.SetProvider(
                SqlAuthenticationMethod.ActiveDirectoryManagedIdentity,
                new ActiveDirectoryAuthenticationProvider());

            SqlAuthenticationProvider.SetProvider(
                SqlAuthenticationMethod.ActiveDirectoryDefault,
                new ActiveDirectoryAuthenticationProvider());

            var serviceProvider = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.AddApplicationInsights();
                })
                .AddDbContextFactory<StorageBroker>()
                .AddTerminologyClient(configuration, claimsPrincipal)
                .BuildServiceProvider();

            this.terminologyClient = serviceProvider.GetService<ITerminologyClient>();
        }
    }
}
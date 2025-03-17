// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LHDS.Core.Tests.Integration.Addresses.Console
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var environmentName = "Development";

            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile("local.appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfiguration configuration = configurationBuilder.Build();
            var claimsPrincipal = new ClaimsPrincipal();

            claimsPrincipal.AddIdentity(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, "ISL.LDS.AdminSpa.Administrators")
            }, "TestAuthType"));

            var serviceProvider = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.AddApplicationInsights();
                })
                .AddAddressClient(configuration, claimsPrincipal)
                .BuildServiceProvider();

            var addressClient =
                serviceProvider.GetService<IAddressClient>();

            await Task.CompletedTask;

            throw new NotImplementedException();
        }
    }
}

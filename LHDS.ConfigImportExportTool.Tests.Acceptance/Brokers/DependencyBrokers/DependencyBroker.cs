// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using LHDS.Core.Brokers.Storages.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LHDS.ConfigImportExportTool.Tests.Acceptance.Brokers.DependencyBrokers
{
    public class DependencyBroker
    {
        public readonly IConfiguration Configuration;

        public DependencyBroker()
        {
            string aspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var args = Environment.GetCommandLineArgs();
            var environmentArg = args.FirstOrDefault(arg => arg.StartsWith("--environment="));

            var environmentName = !string.IsNullOrEmpty(aspNetCoreEnvironment)
                ? aspNetCoreEnvironment
                : !string.IsNullOrEmpty(environmentArg)
                    ? environmentArg
                    : "Development";

            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile("local.appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            this.Configuration = configurationBuilder.Build();

            if (this.Configuration == null)
            {
                throw new Exception("No configuration");
            }

            var storageBroker = new StorageBroker(this.Configuration);
            storageBroker.Database.Migrate();
            bool canConnect = storageBroker.Database.CanConnect();
        }
    }
}

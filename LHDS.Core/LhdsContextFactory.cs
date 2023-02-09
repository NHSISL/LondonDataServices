// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Linq;
using LHDS.Core.Brokers.Storages.Sql;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LHDS.Core
{
    internal class LhdsContextFactory : IDesignTimeDbContextFactory<StorageBroker>
    {
        public StorageBroker CreateDbContext(string[] args)
        {
            var environmentName = args.FirstOrDefault() ?? "Development";

            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("local.appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true);

            IConfiguration configuration = configurationBuilder.Build();
            return new StorageBroker(configuration);
        }
    }
}

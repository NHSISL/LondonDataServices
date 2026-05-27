// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using LHDS.Core.Brokers.Storages.Sql;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LHDS.Core
{
    internal class LhdsContextFactory : IDesignTimeDbContextFactory<StorageBroker>
    {
        public StorageBroker CreateDbContext(string[] args)
        {
            // This connection string is used exclusively for EF Core design-time tooling
            // (e.g., Add-Migration, Update-Database on a developer machine). It is a
            // LocalDB instance and is NOT used in any deployed environment. This is safe
            // to commit — no production credentials are present.
            List<KeyValuePair<string, string>> config = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(
                    key: "ConnectionStrings:DefaultConnection",
                    value: "Server=(localdb)\\MSSQLLocalDB;Database=LondonDataServices;" +
                        "Trusted_Connection=True;MultipleActiveResultSets=true"),
            };

            var configurationBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(initialData: config);

            IConfiguration configuration = configurationBuilder.Build();
            return new StorageBroker(configuration);
        }
    }
}

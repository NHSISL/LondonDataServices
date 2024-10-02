// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------
using Microsoft.EntityFrameworkCore.Design;


namespace LHDS.ConfigImportExportTool
{
    internal class LhdsContextFactory : IDesignTimeDbContextFactory<StorageBroker>
    {
        public StorageBroker CreateDbContext(string[] args)
        {
            List<KeyValuePair<string, string>> config = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(
                    key: "ConnectionStrings:DefaultConnection",
                    value: "Server=(localdb)\\MSSQLLocalDB;Database=LHDS;" +
                        "Trusted_Connection=True;MultipleActiveResultSets=true"),
            };

            var configurationBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(initialData: config);

            IConfiguration configuration = configurationBuilder.Build();
            return new StorageBroker(configuration);
        }
    }
}

// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Foundations.AddressToUprnFileLogs;
using LHDS.Core.Services.Foundations.AddressToUprnFileLogs;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Xunit;

namespace LHDS.Core.Tests.Integration.Addresses
{
    public partial class AddressToUprnTests
    {
        private readonly IAddressToUprnClient addressToUprnClient;
        private readonly IAddressToUprnFileLogService addressToUprnFileLogService;
        private readonly ITestOutputHelper output;

        public AddressToUprnTests(ITestOutputHelper output)
        {
            this.output = output;
            var environmentName = "Development";

            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile("local.appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfiguration configuration = configurationBuilder.Build();
            WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();
            var claimsPrincipal = new ClaimsPrincipal(windowsIdentity);

            SqlAuthenticationProvider.SetProvider(
                SqlAuthenticationMethod.ActiveDirectoryManagedIdentity,
                new ActiveDirectoryAuthenticationProvider());

            SqlAuthenticationProvider.SetProvider(
                SqlAuthenticationMethod.ActiveDirectoryDefault,
                new ActiveDirectoryAuthenticationProvider());

            ServiceProvider serviceProvider = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.AddApplicationInsights();
                })
                .AddDbContextFactory<StorageBroker>()
                .AddAddressToUprnClient(configuration, claimsPrincipal)
                .BuildServiceProvider();

            this.addressToUprnClient = serviceProvider.GetService<IAddressToUprnClient>();
            this.addressToUprnFileLogService = serviceProvider.GetService<IAddressToUprnFileLogService>();
        }

        private static string GetRandomString() =>
            Guid.NewGuid().ToString("N");

        private static Stream CreateAddressCsvStream(string unstructuredAddress)
        {
            string csv =
                "UnstructuredAddress" + Environment.NewLine +
                unstructuredAddress + Environment.NewLine;

            byte[] bytes = Encoding.UTF8.GetBytes(csv);

            return new MemoryStream(bytes);
        }

        private async ValueTask<AddressToUprnFileLog> RetrieveAddressToUprnFileLogByFileNameAsync(string fileName)
        {
            IQueryable<AddressToUprnFileLog> fileLogs =
                await this.addressToUprnFileLogService.RetrieveAllAddressToUprnFileLogsAsync();

            return fileLogs.FirstOrDefault(log => log.FileName == fileName);
        }
    }
}

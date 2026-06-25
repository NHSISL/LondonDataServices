// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Text;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.AddressToUprnFileLogs;
using LHDS.Core.Models.Foundations.AssignAddresses;
using LHDS.Core.Models.Orchestrations.AddressToUprns;
using LHDS.Core.Services.Foundations.AddressToUprnFileLogs;
using LHDS.Core.Tests.Acceptance.Brokers.DependencyBrokers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tynamix.ObjectFiller;
using WireMock.Server;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.AddressToUprn
{
    [Collection(nameof(CoreTestCollection))]
    public partial class AddressToUprnTests
    {
        private readonly DependencyBroker dependencyBroker;
        private readonly IAddressToUprnClient addressToUprnClient;
        private readonly IAddressToUprnFileLogService addressToUprnFileLogService;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IStorageBroker storageBroker;
        private readonly ICompareLogic compareLogic;
        private readonly AddressToUprnConfiguration addressToUprnConfiguration;
        private readonly BlobContainers blobContainers;
        private readonly WireMockServer wireMockServer;

        public AddressToUprnTests(DependencyBroker dependencyBroker)
        {
            this.wireMockServer = WireMockServer.Start();
            this.dependencyBroker = dependencyBroker;
            this.compareLogic = new CompareLogic();
            var serviceCollection = new ServiceCollection();
            var claimsPrincipal = new ClaimsPrincipal();

            claimsPrincipal.AddIdentity(new ClaimsIdentity(new List<Claim>
            {
                new Claim("oid", Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.GivenName, "GivenName"),
                new Claim(ClaimTypes.Surname, "Surname"),
                new Claim("displayName", "DisplayName"),
                new Claim(ClaimTypes.Email, "some@email.com"),
                new Claim("jobTitle", "job title"),
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, "ISL.LDS.AdminApi.Administrators"),
                new Claim(ClaimTypes.Role, "ISL.LDS.AdminApi.Configurations")
            }));

            serviceCollection.AddLogging(builder =>
            {
                builder.AddConsole();
            });

            this.dependencyBroker.Configuration["assignConfiguration:apiUrl"] = this.wireMockServer.Url;

            serviceCollection
                .AddDbContextFactory<StorageBroker>()
                .AddTransient<IStorageBroker, StorageBroker>()
                .AddAddressToUprnClient(this.dependencyBroker.Configuration, claimsPrincipal);

            var serviceProvider = serviceCollection.BuildServiceProvider(new ServiceProviderOptions
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            });

            this.addressToUprnClient = serviceProvider.GetService<IAddressToUprnClient>();
            this.addressToUprnFileLogService = serviceProvider.GetService<IAddressToUprnFileLogService>();
            this.dateTimeBroker = serviceProvider.GetService<IDateTimeBroker>();
            this.storageBroker = serviceProvider.GetService<IStorageBroker>();
            this.addressToUprnConfiguration = serviceProvider.GetService<AddressToUprnConfiguration>();
            this.blobContainers = serviceProvider.GetService<BlobContainers>();
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static Guid GetRandomGuid() =>
            Guid.NewGuid();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime().AddDays(1)).GetValue();

        private static Stream CreateAddressCsvStream(string unstructuredAddress)
        {
            var csvContent = new StringBuilder();
            csvContent.AppendLine("UnstructuredAddress");
            csvContent.AppendLine(unstructuredAddress);
            byte[] bytes = Encoding.UTF8.GetBytes(csvContent.ToString());

            return new MemoryStream(bytes);
        }

        private static AssignAddress CreateRandomAssignAddress(DateTimeOffset dateTimeOffset) =>
            CreateAssignAddressFiller(dateTimeOffset).Create();

        private static Filler<AssignAddress> CreateAssignAddressFiller(DateTimeOffset dateTimeOffset)
        {
            var filler = new Filler<AssignAddress>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset);

            return filler;
        }

        private static Filler<AddressToUprnFileLog> CreateAddressToUprnFileLogFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<AddressToUprnFileLog>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<TimeSpan>().Use(TimeSpan.Zero)
                .OnProperty(log => log.CreatedBy).Use(user)
                .OnProperty(log => log.UpdatedBy).Use(user);

            return filler;
        }
    }
}

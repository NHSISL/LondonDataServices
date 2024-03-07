// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Foundations.Suppliers;
using LHDS.Core.Models.Orchestrations.EmisLandings;
using LHDS.Core.Providers.Downloads.Extensions;
using LHDS.Core.Services.Foundations.DataSets;
using LHDS.Core.Services.Foundations.DataSetSpecifications;
using LHDS.Core.Services.Foundations.IngestionTrackingAudits;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using LHDS.Core.Services.Foundations.SubscriberAgreements;
using LHDS.Core.Services.Foundations.Suppliers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace LHDS.Core.Tests.Integration.Landings
{
    public partial class LandingTests
    {
        private readonly ITestOutputHelper output;
        private readonly IEmisLandingClient landingClient;
        private readonly ILoggingBroker loggingBroker;
        private readonly IBlobStorageBroker blobStorageBroker;
        private readonly LandingConfiguration landingConfiguration;
        private readonly IIngestionTrackingService ingestionTrackingService;
        private readonly ISupplierService supplierService;
        private readonly IDataSetService dataSetService;
        private readonly IDataSetSpecificationService dataSetSpecificationService;
        private readonly IIngestionTrackingAuditService auditService;
        private readonly ISubscriberAgreementService subscriberAgreementService;

        public LandingTests(ITestOutputHelper output)
        {
            this.output = output;
            var environmentName = "Development";

            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile("local.appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfiguration configuration = configurationBuilder.Build();

            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.AddApplicationInsights();
                })
                .AddLandingClient(configuration)
                .UseFtpDownloadProvider(configuration, builder => builder.AddFtpDownloadProvider())
                .BuildServiceProvider();

            landingConfiguration = serviceProvider.GetService<LandingConfiguration>();
            loggingBroker = serviceProvider.GetService<ILoggingBroker>();
            blobStorageBroker = serviceProvider.GetService<IBlobStorageBroker>();
            ingestionTrackingService = serviceProvider.GetService<IIngestionTrackingService>();
            auditService = serviceProvider.GetService<IIngestionTrackingAuditService>();
            supplierService = serviceProvider.GetService<ISupplierService>();
            dataSetService = serviceProvider.GetService<IDataSetService>();
            dataSetSpecificationService = serviceProvider.GetService<IDataSetSpecificationService>();
            subscriberAgreementService = serviceProvider.GetService<ISubscriberAgreementService>();
            landingClient = serviceProvider.GetService<IEmisLandingClient>();
        }

        private async ValueTask<Supplier> SetupSupplier()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;

            Supplier supplier = new Supplier
            {
                Id = this.landingConfiguration.LandingSupplierId,
                Name = "Test Supplier",
                Description = "Test Supplier Description",
                CreatedDate = now,
                CreatedBy = "Test User",
                UpdatedDate = now,
                UpdatedBy = "Test User",
                FriendlyName = "Test Supplier Friendly Name",
            };

            Supplier maybeSupplier = supplierService.RetrieveAllSuppliers()
                .FirstOrDefault(s => s.Id == supplier.Id);

            if (maybeSupplier == null)
            {
                return await supplierService.AddSupplierAsync(supplier);
            }

            return maybeSupplier;
        }

        private async ValueTask<SubscriberAgreement> SetupSubscriberAgreement()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;

            SubscriberAgreement subscriberAgreement = new SubscriberAgreement
            {
                Id = new Guid("2B086EB6-4666-45C1-BAA8-1CBDDA532E5C"),
                SupplierSharingAgreementShortName = "Test EMIS Supplier Sharing Agreement",
                SupplierSharingAgreementGuid = new Guid("6263EBC7-D8CC-4AA9-8849-60DCEDB63974"),
                FtpUserName = "sftp_user10222",
                FtpPublicKey = "LS0tLS1CRUdJTiBSU0EgUFJJVkFURSBLRVktLS0tLQpQcm9jLVR5cGU6IDQsRU5DUllQVEVECkRFSy1JbmZvOiBERVMtRURFMy1DQkMsMUYyM0U0MDZCMUJDNzVDRAoKL0lnb1FsT2RXcmhOTit1QVdvZzJ0Q0p2R21kZTBSMG9CczRnZFp3aDIxcC9XUlJ2SExFTTNKZy81VzU4NEFoZAovUmFlZzlFWGxqcFBneHpWVDFJSURhR1M0NFRSbTV5T1NYczY2VTJPRWRCalpuUG1rL2REdUtwNEZwbEhFeWlXCjhkWEdGL2JKeEF4WnpLcWh2QlM0QTZ5UzZKLzlvQ293VVJnbEJCRk82ZE52UVI2c09VcXc2T0NtcGwzK2xTNkcKN0gxUnpQY2RSMWJtd1UvOFQ4MW1KamV0VDJFSzB3cXgrNHhPUXBiaGF0bURabUNMbXd3ekNyQ0ZyODNMem5qMQpmTXUvSzJHcUNRa2RXS29HcFpC",
                GpgPublicKey = "LS0tLS1CRUdJTiBQR1AgUFVCTElDIEtFWSBCTE9DSy0tLS0tCgptRE1FWlVFQTJ4WUpLd1lCQkFIYVJ3OEJBUWRBaWxLWEk2ay83K2tha2lwNXlxdmV4QXNGMVNVTHNERnI1aVg5CnFvOEZ5ZGUwQjA1RlRDQkpRMEtJbVFRVEZnb0FRUlloQk5TUEZqNW1RTXU2ajhTNytoS2hHL2taZmRpRUJRSmwKUVFEYkFoc0RCUWtEdzZQbEJRc0pDQWNDQWlJQ0JoVUtDUWdMQWdRV0FnTUJBaDRIQWhlQUFBb0pFQktoRy9rWgpmZGlFZkJzQS8xdFZSLzlZN3hIVm9xOVd6dnVNWUtCY3JQcTkzMUpJWUp1MmpDUkVLL1ZJQVFDUE13SDBDSnhnCjV1SWlBc2c3ZzhIL2FTR0xueUY5MUs3ZThEaE1DMlFTRHJnNEJHVkJBTnNTQ2lzR0FRUUJsMVVCQlFFQkIwQmoKalJoTGpnM3EvbGNwcFBqdkc2dEVjVGwyVHhvdTRRaE5uemF4dnhuaEdRTUJDQWVJZmdRWUZnb0FKaFloQk5TUApGajVtUU11Nmo4UzcraEtoRy9rWmZkaUVCUUpsUVFEYkFoc01CUWtEdzZQbEFBb0pFQktoRy9rWmZkaUVOSjhBCi9BNEVoSDY4eExzUG9MUExFVTBpZHVPYUdJdjNjczI3WkhaRmFPNWsyY1VjQVA5RndxdVVyYUxjNnpaUUw2by8KTzJ0RmducFAyY3JGOEFZb3RiQVpFM1QwQ2c9PQo9aU9lNwotLS0tLUVORCBQR1AgUFVCTElDIEtFWSBCTE9DSy0tLS0t",
                IsActive = true,
                LastPollStartDate = null,
                LastPollEndDate = null,
                CreatedDate = now,
                CreatedBy = "Test User",
                UpdatedDate = now,
                UpdatedBy = "Test User"
            };

            SubscriberAgreement maybeSubscriberAgreement = subscriberAgreementService.RetrieveAllSubscriberAgreements()
                .FirstOrDefault(s => s.Id == subscriberAgreement.Id);

            if (maybeSubscriberAgreement == null)
            {
                return await subscriberAgreementService.AddSubscriberAgreementAsync(subscriberAgreement);
            }

            return maybeSubscriberAgreement;
        }
    }
}

// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Brokers.Storages.Blobs;
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


using Xunit;

namespace LHDS.Core.Tests.Integration.EmisLandings
{
    public partial class LandingTests
    {
        private readonly ITestOutputHelper output;
        private readonly IEmisLandingClient landingClient;
        private readonly IDecryptionClient decryptionClient;
        private readonly ILoggingBroker loggingBroker;
        private readonly IBlobStorageBroker blobStorageBroker;
        private readonly LandingConfiguration landingConfiguration;
        private readonly IIngestionTrackingService ingestionTrackingService;
        private readonly ISupplierService supplierService;
        private readonly IDataSetService dataSetService;
        private readonly IDataSetSpecificationService dataSetSpecificationService;
        private readonly IIngestionTrackingAuditService auditService;
        private readonly ISubscriberAgreementService subscriberAgreementService;
        private readonly BlobContainers blobContainers;

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
            var windowsIdentity = WindowsIdentity.GetCurrent();
            var claimsPrincipal = new ClaimsPrincipal(windowsIdentity);

            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.AddApplicationInsights();
                })
                .AddDbContextFactory<StorageBroker>()
                .AddEmisLandingClient(configuration, claimsPrincipal)
                .AddDecryptionClient(configuration, claimsPrincipal)
                .UseFtpDownloadProvider(configuration, builder => builder.AddFtpDownloadProvider())
                .BuildServiceProvider();

            blobContainers = serviceProvider.GetService<BlobContainers>();
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
            decryptionClient = serviceProvider.GetService<IDecryptionClient>();
        }

        private async ValueTask<Supplier> GetEmisSupplierAsync()
        {
            IQueryable<Supplier> retrievedSuppliers = await supplierService.RetrieveAllSuppliersAsync();

            return retrievedSuppliers.First(s => s.Name == "EMIS");
        }
    }
}


// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Foundations.Suppliers;
using LHDS.Core.Models.Orchestrations.Downloads;
using LHDS.Core.Providers.Downloads.Extensions;
using LHDS.Core.Services.Foundations.Audits;
using LHDS.Core.Services.Foundations.IngestionTrackings;
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
        private readonly ILandingClient landingClient;
        private readonly ILoggingBroker loggingBroker;
        private readonly IBlobStorageBroker blobStorageBroker;
        private readonly LandingConfiguration landingConfiguration;
        private readonly IIngestionTrackingService ingestionTrackingService;
        private readonly ISupplierService supplierService;
        private readonly IIngestionTrackingAuditService auditService;

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
            landingClient = serviceProvider.GetService<ILandingClient>();
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
                LandingManualTriggerUrl = "hjkhsd",
                DecryptionManualTriggerUrl = "hjkhsd",
            };

            Supplier maybeSupplier = supplierService.RetrieveAllSuppliers()
                .FirstOrDefault(s => s.Id == supplier.Id);

            if (maybeSupplier == null)
            {
                return await supplierService.AddSupplierAsync(supplier);
            }

            return maybeSupplier;
        }
    }
}

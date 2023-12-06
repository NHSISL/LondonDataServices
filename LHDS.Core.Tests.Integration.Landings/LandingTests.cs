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
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.Suppliers;
using LHDS.Core.Models.Orchestrations.Downloads;
using LHDS.Core.Providers.Downloads.Extensions;
using LHDS.Core.Services.Foundations.DataSets;
using LHDS.Core.Services.Foundations.DataSetSpecifications;
using LHDS.Core.Services.Foundations.IngestionTrackingAudits;
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
        private readonly IDataSetService dataSetService;
        private readonly IDataSetSpecificationService dataSetSpecificationService;
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
            dataSetService = serviceProvider.GetService<IDataSetService>();
            dataSetSpecificationService = serviceProvider.GetService<IDataSetSpecificationService>();
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
            };

            Supplier maybeSupplier = supplierService.RetrieveAllSuppliers()
                .FirstOrDefault(s => s.Id == supplier.Id);

            if (maybeSupplier == null)
            {
                return await supplierService.AddSupplierAsync(supplier);
            }

            return maybeSupplier;
        }

        private async ValueTask<DataSet> SetupDataSet()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;

            DataSet dataSet = new DataSet
            {
                Id = Guid.Parse("6a62313a-7442-462e-b6e8-dec541ddd0ba"),
                SupplierId = this.landingConfiguration.LandingSupplierId,
                DataSetName = "PrimaryCareEMISDEV",
                DataSetAliases = "",
                DataSetAuthor = "EMISDEV",
                SpecifiedBy = "EMIS",
                IsNationallySpecified = false,
                CollectedBy = "EMIS",
                IsNationallyCollected = false,
                DataSourceType = "",
                IsActive = true,
                CreatedBy = "System",
                CreatedDate = new DateTime(year: 2023, month: 1, day: 1, hour: 0, minute: 0, second: 0),
                UpdatedBy = "System",
                UpdatedDate = new DateTime(year: 2023, month: 1, day: 1, hour: 0, minute: 0, second: 0)
            };

            DataSet maybeDataSet = dataSetService.RetrieveAllDataSets()
                .FirstOrDefault(s => s.Id == supplier.Id);

            if (maybeDataSet == null)
            {
                return await supplierService.AddSupplierAsync(supplier);
            }

            return maybeDataSet;
        }

    }
}

// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.Suppliers;
using LHDS.Core.Models.Orchestrations.EmisLandings;
using LHDS.Core.Services.Foundations.DataSets;
using LHDS.Core.Services.Foundations.DataSetSpecifications;
using LHDS.Core.Services.Foundations.IngestionTrackingAudits;
using LHDS.Core.Services.Foundations.Suppliers;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.IngestionTrackingAudits;
using LHDS.Core.Services.Processings.IngestionTrackings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace LHDS.Core.Tests.Integration.TppLandings
{
    public partial class TppLandingTests
    {
        private readonly ITestOutputHelper output;
        private readonly ITppLandingClient tppLandingClient;
        private readonly ILoggingBroker loggingBroker;
        private readonly IBlobStorageBroker blobStorageBroker;
        private readonly LandingConfiguration landingConfiguration;
        private readonly IIngestionTrackingProcessingService ingestionTrackingService;
        private readonly IIngestionTrackingAuditService ingestionTrackingAuditService;
        private readonly IDocumentProcessingService documentService;
        private readonly ISupplierService supplierService;
        private readonly IDataSetService dataSetService;
        private readonly IDataSetSpecificationService dataSetSpecificationService;
        private readonly IIngestionTrackingAuditProcessingService auditService;
        private readonly BlobContainers blobContainers;

        public TppLandingTests(ITestOutputHelper output)
        {
            this.output = output;
            var environmentName = "Development";

            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
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
                .AddDbContext<StorageBroker>()
                .AddScoped<IStorageBroker>(service => service.GetRequiredService<StorageBroker>())
                .AddTppLandingClient(configuration, claimsPrincipal)
                .BuildServiceProvider();

            landingConfiguration = serviceProvider.GetService<LandingConfiguration>();
            loggingBroker = serviceProvider.GetService<ILoggingBroker>();
            blobStorageBroker = serviceProvider.GetService<IBlobStorageBroker>();
            ingestionTrackingService = serviceProvider.GetService<IIngestionTrackingProcessingService>();
            ingestionTrackingAuditService = serviceProvider.GetService<IIngestionTrackingAuditService>();
            documentService = serviceProvider.GetService<IDocumentProcessingService>();
            auditService = serviceProvider.GetService<IIngestionTrackingAuditProcessingService>();
            supplierService = serviceProvider.GetService<ISupplierService>();
            dataSetService = serviceProvider.GetService<IDataSetService>();
            dataSetSpecificationService = serviceProvider.GetService<IDataSetSpecificationService>();
            tppLandingClient = serviceProvider.GetService<ITppLandingClient>();
            this.blobContainers = serviceProvider.GetService<BlobContainers>();
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

            IQueryable<Supplier> retrievedSuppliers = await supplierService.RetrieveAllSuppliersAsync();

            Supplier maybeSupplier = retrievedSuppliers
                .FirstOrDefault(s => s.Id == supplier.Id);

            if (maybeSupplier == null)
            {
                return await supplierService.AddSupplierAsync(supplier);
            }

            return maybeSupplier;
        }

        private async ValueTask<Supplier> GetTppSupplier()
        {
            IQueryable<Supplier> retrievedSuppliers = await supplierService.RetrieveAllSuppliersAsync();

            return retrievedSuppliers.First(s => s.Name == "TPP");
        }

        private async ValueTask<DataSet> SetupDataSet(Guid SupplierId)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;

            DataSet dataSet = new DataSet
            {
                Id = Guid.NewGuid(),
                SupplierId = SupplierId,
                DataSetName = "TPP",
                DataSetAliases = "Test Dat Set Aliases",
                DataSetAuthor = "Test Data Set Author",
                SpecifiedBy = "Test Specified By",
                IsNationallySpecified = true,
                CollectedBy = "Test Collected By",
                IsNationallyCollected = true,
                DataSourceType = "Test Data Source Type",
                IsActive = true,
                ActiveFrom = now.AddDays(2),
                ActiveTo = now.AddDays(200),
                CreatedBy = "Test User",
                UpdatedBy = "Test User",
                UpdatedDate = now,
                CreatedDate = now
            };

            IQueryable<DataSet> retrievedDataSets = await dataSetService.RetrieveAllDataSetsAsync();
            DataSet maybeDataSet = retrievedDataSets.FirstOrDefault(s => s.Id == dataSet.Id);

            if (maybeDataSet == null)
            {
                return await dataSetService.AddDataSetAsync(dataSet);
            }

            return maybeDataSet;
        }

        private async ValueTask<DataSetSpecification> SetupDataSetSpecification(Guid dataSet)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;

            DataSetSpecification dataSetSpecification = new DataSetSpecification
            {
                Id = Guid.NewGuid(),
                DataSetId = dataSet,
                SupplierSpecificationVersion = "1",
                OurSpecificationVersion = "2",
                Notes = "Test Notes",
                IsMultiAuthorPerBatch = true,
                EntityChangeSynchronisation = "Test Entity Change Synchronisation",
                DateReleased = now,
                DateImplemented = now,
                DateSuperseded = null,
                SupersededById = null,
                PresededById = null,
                IsPublished = false,
                IsActive = true,
                ActiveFrom = now.AddDays(2),
                ActiveTo = now.AddDays(200),
                CreatedBy = "Test User",
                UpdatedBy = "Test User",
                UpdatedDate = now,
                CreatedDate = now
            };

            IQueryable<DataSetSpecification> allDataSetSpecification =
                await dataSetSpecificationService.RetrieveAllDataSetSpecificationsAsync();

            DataSetSpecification maybeDataSetSpecification = allDataSetSpecification
                .FirstOrDefault(s => s.Id == dataSetSpecification.Id);

            if (maybeDataSetSpecification == null)
            {
                return await dataSetSpecificationService.AddDataSetSpecificationAsync(dataSetSpecification);
            }

            return maybeDataSetSpecification;
        }

        // Helper method to calculate SHA256 hash from byte array
        private static string CalculateSHA256Hash(byte[] data)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(data);
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}

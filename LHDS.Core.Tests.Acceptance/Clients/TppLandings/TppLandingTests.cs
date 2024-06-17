// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Clients;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.Suppliers;
using LHDS.Core.Models.Orchestrations.EmisLandings;
using LHDS.Core.Services.Foundations.DataSets;
using LHDS.Core.Services.Foundations.DataSetSpecifications;
using LHDS.Core.Services.Foundations.IngestionTrackingAudits;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using LHDS.Core.Services.Foundations.Suppliers;
using LHDS.Core.Services.Processings.DataSetSpecifications;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Tests.Acceptance.Brokers.DependencyBrokers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.TppLandings
{
    [Collection(nameof(CoreTestCollection))]
    public partial class TppLandingTests
    {
        private readonly Mock<IBlobStorageBroker> blobStorageBrokerMock;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIngestionTrackingService ingestionTrackingService;
        private readonly IDataSetSpecificationService dataSetSpecificationService;
        private readonly ISupplierService supplierService;
        private readonly IDataSetService dataSetService;
        private readonly IDocumentProcessingService documentProcessingService;
        private readonly ITppLandingClient tppLandingClient;
        private readonly LandingConfiguration landingConfiguration;
        private readonly BlobContainers blobContainers;
        private readonly IIngestionTrackingAuditService ingestionTrackingAuditService;

        private readonly DependencyBroker dependencyBroker;

        public TppLandingTests(DependencyBroker dependencyBroker)
        {
            this.dependencyBroker = dependencyBroker;
            this.blobStorageBrokerMock = new Mock<IBlobStorageBroker>();
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging(builder =>
            {
                builder.AddConsole();
            });

            var blobStorageSettings = dependencyBroker.Configuration
                .GetSection("blobStorage").Get<BlobStorageSettings>();

            serviceCollection.AddSingleton<BlobContainers>(blobStorageSettings.BlobContainers);

            serviceCollection
                .AddTransient<IBlobStorageBroker>(serviceProvider => blobStorageBrokerMock.Object)
                .AddTransient<ISupplierService, SupplierService>()
                .AddTransient<IDataSetService, DataSetService>()
                .AddTransient<IDataSetSpecificationService, DataSetSpecificationService>()
                .AddTransient<IDataSetSpecificationProcessingService, DataSetSpecificationProcessingService>()
                .AddTransient<IDocumentProcessingService, DocumentProcessingService>();

            var serviceProvider = serviceCollection.BuildServiceProvider();
            this.ingestionTrackingService = serviceProvider.GetService<IIngestionTrackingService>();
            this.supplierService = serviceProvider.GetService<ISupplierService>();
            this.dataSetService = serviceProvider.GetService<IDataSetService>();
            this.dataSetSpecificationService = serviceProvider.GetService<IDataSetSpecificationService>();
            this.documentProcessingService = serviceProvider.GetService<IDocumentProcessingService>();
            this.ingestionTrackingAuditService = serviceProvider.GetService<IIngestionTrackingAuditService>();
            this.landingConfiguration = serviceProvider.GetService<LandingConfiguration>();
            this.blobContainers = serviceProvider.GetService<BlobContainers>();
            this.dateTimeBroker = serviceProvider.GetService<IDateTimeBroker>();
            tppLandingClient = serviceProvider.GetService<ITppLandingClient>();
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static string GetRandomFileName()
        {
            string filename = GetRandomString();

            for (int i = 0; i < 6; i++)
            {
                filename = $"{filename}_{GetRandomString(10)}";
            }

            return filename;
        }

        private static string GetRandomString(int length) =>
               new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static int GetRandomNumber(int min = 2, int max = 10) =>
            new IntRange(min, max).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime().AddDays(7)).GetValue();

        private static Document CreateRandomDocument() =>
            CreateDocumentFiller().Create();

        private static Filler<Document> CreateDocumentFiller()
        {
            var filler = new Filler<Document>();
            string filename = GetRandomString();

            filler.Setup()
                .OnProperty(dataSet => dataSet.FileName).Use(() => filename);

            return filler;
        }

        private static IngestionTracking CreateRandomIngestionTracking(
           DateTimeOffset dateTimeOffset,
           Document document,
           Guid supplierId)
        {
            IngestionTracking ingestionTracking = CreateIngestionTrackingFiller(
                dateTimeOffset,
                fileName: document.FileName,
                supplierId)
                    .Create();

            return ingestionTracking;
        }

        private async ValueTask<List<IngestionTracking>> CreateRandomIngestionTrackings(
            DateTimeOffset dateTimeOffset,
            List<Document> documents,
            Guid supplierId)
        {
            List<IngestionTracking> items = new List<IngestionTracking>();

            foreach (var document in documents)
            {
                var item = CreateIngestionTrackingFiller(
                    dateTimeOffset,
                    fileName: document.FileName,
                    supplierId)
                        .Create();

                await this.ingestionTrackingService.AddIngestionTrackingAsync(item);
                items.Add(item);
            }

            return items;
        }

        private static Filler<IngestionTracking> CreateIngestionTrackingFiller(
            DateTimeOffset dateTimeOffset, string fileName, Guid supplierId)
        {
            string user = "System";
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnProperty(ingestionTracking => ingestionTracking.FileName).Use(fileName)
                .OnProperty(ingestionTracking => ingestionTracking.CreatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.SupplierId).Use(supplierId)
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset);

            return filler;
        }

        private static Supplier CreateRandomSupplier(Guid supplierId, DateTimeOffset dateTimeOffset) =>
            CreateSupplierFiller(supplierId, dateTimeOffset).Create();

        private static Filler<Supplier> CreateSupplierFiller(Guid supplierId, DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<Supplier>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(supplier => supplier.Id).Use(supplierId)
                .OnProperty(supplier => supplier.CreatedBy).Use(user)
                .OnProperty(supplier => supplier.UpdatedBy).Use(user)
                .OnProperty(supplier => supplier.IngestionTrackings).IgnoreIt()
                .OnProperty(supplier => supplier.DataSets).IgnoreIt();

            return filler;
        }

        private static DataSet CreateRandomDataSet(Guid supplierId) =>
            CreateDataSetFiller(supplierId).Create();

        private static Filler<DataSet> CreateDataSetFiller(Guid supplierId)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<DataSet>();
            var now = DateTimeOffset.UtcNow;

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(dataSet => dataSet.SupplierId).Use(supplierId)
                .OnProperty(dataSet => dataSet.IsActive).Use(true)
                .OnProperty(dataSet => dataSet.ActiveFrom).Use(now.AddDays(-2))
                .OnProperty(dataSet => dataSet.ActiveTo).Use(now.AddDays(2))
                .OnProperty(dataSet => dataSet.CreatedBy).Use(user)
                .OnProperty(dataSet => dataSet.UpdatedBy).Use(user)
                .OnProperty(dataSet => dataSet.ActiveTo).Use(now.AddDays(GetRandomNumber()));

            return filler;
        }

        private static DataSetSpecification CreateRandomDataSetSpecification(DataSet dataSet) =>
            CreateDataSetSpecificationFiller(dataSet).Create();

        private static Filler<DataSetSpecification> CreateDataSetSpecificationFiller(DataSet dataSet)
        {
            string user = GetRandomString(255);
            var filler = new Filler<DataSetSpecification>();
            var now = DateTimeOffset.UtcNow;

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(dataSetSpecification => dataSetSpecification.DataSetId).Use(dataSet.Id)
                .OnProperty(dataSetSpecification => dataSetSpecification.DataSet).Use(dataSet)
                .OnProperty(dataSetSpecification => dataSetSpecification.IsActive).Use(true)
                .OnProperty(dataSetSpecification => dataSetSpecification.ActiveFrom).Use(now.AddDays(-2))
                .OnProperty(dataSetSpecification => dataSetSpecification.ActiveTo).Use(now.AddDays(2))

                .OnProperty(dataSetSpecification =>
                    dataSetSpecification.OurSpecificationVersion).Use(GetRandomString(10))

                .OnProperty(dataSetSpecification =>
                    dataSetSpecification.SupplierSpecificationVersion).Use(GetRandomString(10))

                .OnProperty(dataSetSpecification => dataSetSpecification.PresededById).IgnoreIt()
                .OnProperty(dataSetSpecification => dataSetSpecification.SupersededById).IgnoreIt()
                .OnProperty(dataSetSpecification => dataSetSpecification.CreatedBy).Use(user)
                .OnProperty(dataSetSpecification => dataSetSpecification.CreatedBy).Use(user)
                .OnProperty(dataSetSpecification => dataSetSpecification.UpdatedBy).Use(user);

            return filler;
        }
    }
}

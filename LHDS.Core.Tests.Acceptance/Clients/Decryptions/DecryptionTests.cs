// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using LHDS.Core.Models.Foundations.Suppliers;
using LHDS.Core.Models.Orchestrations.EmisLandings;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Providers.Cryptography;
using LHDS.Core.Services.Foundations.DataSets;
using LHDS.Core.Services.Foundations.DataSetSpecifications;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.IngestionTrackingAudits;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using LHDS.Core.Services.Foundations.SpecificationObjects;
using LHDS.Core.Services.Foundations.Suppliers;
using LHDS.Core.Services.Orchestrations.SubscriberCredentials;
using LHDS.Core.Tests.Acceptance.Brokers.DependencyBrokers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Decryptions
{
    [Collection(nameof(CoreTestCollection))]
    public partial class DecryptionTests
    {
        private readonly DependencyBroker dependencyBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIngestionTrackingService ingestionTrackingService;
        private readonly IDataSetService dataSetService;
        private readonly IDataSetSpecificationService dataSetSpecificationService;
        private readonly ISpecificationObjectService specificationObjectService;
        private readonly IDecryptionClient decryptionClient;
        private readonly ISupplierService supplierService;
        private readonly IDocumentService documentService;
        private readonly LandingConfiguration landingConfiguration;
        private readonly BlobContainers blobContainers;
        private readonly ICryptographyProvider cryptographyProvider;
        private readonly IIngestionTrackingAuditService auditService;
        private readonly ISubscriberCredentialOrchestration subscriberCredentialOrchestration;

        public DecryptionTests(DependencyBroker dependencyBroker)
        {
            this.dependencyBroker = dependencyBroker;
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging(builder =>
            {
                builder.AddConsole();
            });

            serviceCollection.AddDecryptionClient(this.dependencyBroker.Configuration);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            this.ingestionTrackingService = serviceProvider.GetService<IIngestionTrackingService>();
            this.dataSetService = serviceProvider.GetService<IDataSetService>();
            this.dataSetSpecificationService = serviceProvider.GetService<IDataSetSpecificationService>();
            this.specificationObjectService = serviceProvider.GetService<ISpecificationObjectService>();
            this.supplierService = serviceProvider.GetService<ISupplierService>();
            this.auditService = serviceProvider.GetService<IIngestionTrackingAuditService>();
            this.dateTimeBroker = serviceProvider.GetService<IDateTimeBroker>();
            this.landingConfiguration = serviceProvider.GetRequiredService<LandingConfiguration>();
            this.blobContainers = serviceProvider.GetRequiredService<BlobContainers>();
            this.documentService = serviceProvider.GetService<IDocumentService>();
            this.cryptographyProvider = serviceProvider.GetRequiredService<ICryptographyProvider>();
            this.decryptionClient = serviceProvider.GetService<IDecryptionClient>();
            this.subscriberCredentialOrchestration = serviceProvider.GetService<ISubscriberCredentialOrchestration>();
        }

        static byte[] ReadAllBytesFromStream(Stream stream)
        {
            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static int GetRandomNumber(int min = 2, int max = 10) =>
            new IntRange(min, max).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime().AddDays(7)).GetValue();

        private static string GetRandomWordString() =>
           new MnemonicString(wordCount: 1).GetValue();

        private static string GetRandomString(int length) =>
               new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static string CreateRandomFileName(Guid subscriberCredentialId)
        {
            string fileName = "acceptance-test-only-should-be-deleted-soon";

            for (int i = 0; i < 6; i++)
            {
                if (i == 1)
                {
                    fileName += $"/{subscriberCredentialId.ToString()}";
                }
                if (i == 5)
                {
                    fileName += $"/{subscriberCredentialId.ToString()}.csv";
                }
                else
                {
                    fileName += $"/{GetRandomWordString()}";
                }
            }

            return fileName;
        }

        private static IngestionTracking CreateRandomIngestionTracking(
           DateTimeOffset dateTimeOffset,
           string encryptedFileName,
           string decryptedFileName,
           Guid supplierId)
        {
            IngestionTracking ingestionTracking = CreateIngestionTrackingFiller(
                dateTimeOffset,
                encryptedFileName,
                decryptedFileName,
                supplierId)
                    .Create();

            return ingestionTracking;
        }

        private static Filler<IngestionTracking> CreateIngestionTrackingFiller(
            DateTimeOffset dateTimeOffset, string encryptedFileName, string decryptedFileName, Guid supplierId)
        {
            string user = "System";
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnProperty(ingestionTracking => ingestionTracking.EncryptedFileName).Use(encryptedFileName)
                .OnProperty(ingestionTracking => ingestionTracking.DecryptedFileName).Use(decryptedFileName)
                .OnProperty(ingestionTracking => ingestionTracking.CreatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.SupplierId).Use(supplierId)
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset);

            return filler;
        }

        private static SubscriberCredential CreateRandomSubscriberCredential() =>
            CreateSubscriberCredentialFiller().Create();

        private static Filler<SubscriberCredential> CreateSubscriberCredentialFiller()
        {
            var filler = new Filler<SubscriberCredential>();
            string user = Guid.NewGuid().ToString();
            var now = DateTimeOffset.UtcNow;

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(subscriberCredential => subscriberCredential.IsActive).Use(true)
                .OnProperty(subscriberCredential => subscriberCredential.CreatedBy).Use(user)
                .OnProperty(subscriberCredential => subscriberCredential.UpdatedBy).Use(user);

            return filler;
        }

        private static DataSet CreateRandomDataSet(Guid supplierId) =>
            CreateDataSetFiller(supplierId).Create();

        private static Filler<DataSet> CreateDataSetFiller(Guid supplierId)
        {
            var filler = new Filler<DataSet>();
            string user = Guid.NewGuid().ToString();
            var now = DateTimeOffset.UtcNow;

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(dataSet => dataSet.SupplierId).Use(supplierId)
                .OnProperty(dataSet => dataSet.CreatedBy).Use(user)
                .OnProperty(dataSet => dataSet.UpdatedBy).Use(user);

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

        private static SpecificationObject CreateRandomSpecificationObject(Guid datasetSpecificationId) =>
            CreateSpecificationObjectFiller(datasetSpecificationId).Create();

        private static Filler<SpecificationObject> CreateSpecificationObjectFiller(Guid datasetSpecificationId)
        {
            var filler = new Filler<SpecificationObject>();
            string user = Guid.NewGuid().ToString();
            var now = DateTimeOffset.UtcNow;

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)

                .OnProperty(specificationObject => 
                    specificationObject.DataSetSpecificationId).Use(datasetSpecificationId)

                .OnProperty(specificationObject => specificationObject.CreatedBy).Use(user)
                .OnProperty(specificationObject => specificationObject.UpdatedBy).Use(user);

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
    }
}

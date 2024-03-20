// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Hashing;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Configurations;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Foundations.Suppliers;
using LHDS.Core.Models.Orchestrations.EmisLandings;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Providers.Cryptography;
using LHDS.Core.Providers.Downloads;
using LHDS.Core.Providers.Downloads.DiskDownloads;
using LHDS.Core.Providers.Downloads.FtpDownloads;
using LHDS.Core.Services.Foundations.DataSets;
using LHDS.Core.Services.Foundations.DataSetSpecifications;
using LHDS.Core.Services.Foundations.IngestionTrackingAudits;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using LHDS.Core.Services.Foundations.Suppliers;
using LHDS.Core.Services.Orchestrations.SubscriberCredentials;
using LHDS.Core.Services.Processings.DataSetSpecifications;
using LHDS.Core.Services.Processings.Documents;
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
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIngestionTrackingService ingestionTrackingService;
        private readonly IDataSetSpecificationService dataSetSpecificationService;
        private readonly ISupplierService supplierService;
        private readonly IDataSetService dataSetService;
        private readonly IDocumentProcessingService documentProcessingService;
        private readonly IDataSetSpecificationProcessingService dataSetSpecificationProcessingService;
        private readonly ISubscriberCredentialOrchestration subscriberCredentialOrchestration;
        private readonly IDecryptionClient decryptionClient;
        private readonly LandingConfiguration landingConfiguration;
        private readonly IIngestionTrackingAuditService ingestionTrackingAuditService;
        private readonly DependencyBroker dependencyBroker;
        private readonly BlobContainers blobContainers;
        private readonly IBlobStorageBroker blobStorageBroker;
        private readonly IStorageBroker storageBroker;
        private readonly IHashBroker hashBroker;
        private readonly ICryptographyProvider cryptographyProvider;

        public DecryptionTests(DependencyBroker dependencyBroker)
        {
            this.dependencyBroker = dependencyBroker;
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging(builder =>
            {
                builder.AddConsole();
            });

            serviceCollection
                .AddTransient<ISupplierService, SupplierService>()
                .AddTransient<IDataSetService, DataSetService>()
                .AddTransient<IDataSetSpecificationService, DataSetSpecificationService>()
                .AddTransient<IDataSetSpecificationProcessingService, DataSetSpecificationProcessingService>();

            serviceCollection.AddDecryptionClient(this.dependencyBroker.Configuration);
            serviceCollection.Remove(new ServiceDescriptor(typeof(IDownloadProvider), typeof(FtpDownloadProvider)));

            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string defaultFolderPath = Path.Combine(assemblyPath, "temp", "downloads");

            serviceCollection.AddTransient<IDownloadProvider>(_ =>
                new DiskDownloadProvider(new DiskDownloadProviderSettings
                {
                    IncludeSubDirectories = true,
                    LocalRootFolder = defaultFolderPath
                }));

            var serviceProvider = serviceCollection.BuildServiceProvider();
            this.ingestionTrackingService = serviceProvider.GetService<IIngestionTrackingService>();
            this.supplierService = serviceProvider.GetService<ISupplierService>();
            this.dataSetService = serviceProvider.GetService<IDataSetService>();
            this.dataSetSpecificationService = serviceProvider.GetService<IDataSetSpecificationService>();
            this.documentProcessingService = serviceProvider.GetService<IDocumentProcessingService>();
            this.subscriberCredentialOrchestration = serviceProvider.GetService<ISubscriberCredentialOrchestration>();
            this.ingestionTrackingAuditService = serviceProvider.GetService<IIngestionTrackingAuditService>();
            this.dateTimeBroker = serviceProvider.GetService<IDateTimeBroker>();
            this.blobStorageBroker = serviceProvider.GetService<IBlobStorageBroker>();
            this.storageBroker = serviceProvider.GetService<IStorageBroker>();
            this.hashBroker = serviceProvider.GetService<IHashBroker>();
            this.landingConfiguration = serviceProvider.GetRequiredService<LandingConfiguration>();
            this.cryptographyProvider = serviceProvider.GetRequiredService<ICryptographyProvider>();
            decryptionClient = serviceProvider.GetService<IDecryptionClient>();
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static string GetRandomString(int length) =>
          new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static int GetRandomNumber() =>
           new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime().AddDays(7)).GetValue();

        private static string GetRandomMessage() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static SubscriberAgreement CreateRandomSubscriberAgreement(DateTimeOffset dateTimeOffset) =>
           CreateSubscriberAgreementFiller(dateTimeOffset).Create();

        private static Supplier CreateRandomSupplier(DateTimeOffset dateTimeOffset) =>
           CreateSupplierFiller(dateTimeOffset).Create();

        private static Filler<Supplier> CreateSupplierFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<Supplier>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
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

        private static Filler<SubscriberAgreement> CreateSubscriberAgreementFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<SubscriberAgreement>();
            Guid id = new Guid("2b086eb6-4666-45c1-baa8-1cbdda532e5c");
            Guid supplierSharingAgreementGuid = new Guid("6263EBC7-D8CC-4AA9-8849-60DCEDB63974");

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(subscriberAgreement => subscriberAgreement.CreatedBy).Use(user)
                .OnProperty(subscriberAgreement => subscriberAgreement.Id).Use(id)
                .OnProperty(subscriberAgreement => subscriberAgreement.SupplierSharingAgreementGuid)
                    .Use(supplierSharingAgreementGuid)

                .OnProperty(subscriberAgreement => subscriberAgreement.UpdatedBy).Use(user);

            return filler;
        }

        private static IngestionTracking CreateRandomIngestionTracking(
           DateTimeOffset dateTimeOffset,
           Download fileToRetrieve,
           string encryptedFileName,
           string decryptedFileName,
           string encryptedFileSha256Hash,
           Guid supplierId)
        {
            IngestionTracking ingestionTracking = CreateIngestionTrackingFiller(
                dateTimeOffset,
                fileName: fileToRetrieve.Document.FileName,
                encryptedFileName,
                decryptedFileName,
                encryptedFileSha256Hash,
                supplierId)
                    .Create();

            return ingestionTracking;
        }

        private static Filler<IngestionTracking> CreateIngestionTrackingFiller(
            DateTimeOffset dateTimeOffset,
            string fileName,
            string encryptedFileName,
            string decryptedFileName,
            string encryptedFileSha256Hash,
            Guid supplierId)
        {
            string user = "System";
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnProperty(ingestionTracking => ingestionTracking.FileName).Use(fileName)
                .OnProperty(ingestionTracking => ingestionTracking.SupplierId).Use(supplierId)
                .OnProperty(ingestionTracking => ingestionTracking.EncryptedFileName).Use(encryptedFileName)
                .OnProperty(ingestionTracking => ingestionTracking.DecryptedFileName).Use(decryptedFileName)
                .OnProperty(ingestionTracking => ingestionTracking.CreatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.EncryptedFileSha256Hash).Use(encryptedFileSha256Hash)
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
                .OnProperty(subscriberCredential => subscriberCredential.GpgPublicKey)
                .Use("LS0tLS1CRUdJTiBQR1AgUFVCTElDIEtFWSBCTE9DSy0tLS0tCgptRE1FWStKTjl4WUpLd1lCQkFIYVJ3OEJBUWRBZFpNbEhySm9yRDJSaDJlTGFNRDJOUFNUeW9mYzRXVnU2R1FhCndNNWd5dWUwRmt4SVJGTWdQSFJsYzNSQWJHaGtjeTVzYjJOaGJENklrd1FURmdvQU94WWhCQkF4dVR3MnBHQjcKOHBFSWFsVjJadU1ZSW9PbEJRSmo0azMzQWhzREJRc0pDQWNDQWlJQ0JoVUtDUWdMQWdRV0FnTUJBaDRIQWhlQQpBQW9KRUZWMlp1TVlJb09sVmw0QS8xRk8xVUd3V05JcWFuUTBPaDJKbTlQK2YyNVRvWFJ2bnRRRktTYUx2aUJZCkFQNDhCQ3FaVm1pcERaaFlNdVFmUWpyTmJEV2gvUWc1YTlJUG5xeHB4N2JtQUxnNEJHUGlUZmNTQ2lzR0FRUUIKbDFVQkJRRUJCMEFCUXl0b3hoNExMSCtPRi9OdmhxamE1THNtbEhPNS9wTU5rUnlSZ0l2eEZRTUJDQWVJZUFRWQpGZ29BSUJZaEJCQXh1VHcycEdCNzhwRUlhbFYyWnVNWUlvT2xCUUpqNGszM0Foc01BQW9KRUZWMlp1TVlJb09sCk5OZ0JBTEdSWWNFbzRYeXJJZjVaaDhSeWRNd1dqVTJtZjk5M0tZWjFQaVgyLy9hbUFQOVF4WnNac2oxTjJNdDAKTjZveWs3UDJSRExrSTBVRDZiUlJTS0NTVnFIRkJBPT0KPWVIMEoKLS0tLS1FTkQgUEdQIFBVQkxJQyBLRVkgQkxPQ0stLS0tLQo=")
                .OnProperty(subscriberCredential => subscriberCredential.CreatedBy).Use(user)
                .OnProperty(subscriberCredential => subscriberCredential.UpdatedBy).Use(user);

            return filler;
        }

        private static string GetRandomFileName(Guid subscriberAgreementId)
        {
            string filename =
                $"delta" +
                $"_{GetRandomNumber()}" +
                $"_Admin" +
                $"_Location" +
                $"_{DateTime.Now.ToString("yyyyMMddHHmmss")}" +
                $"_{subscriberAgreementId}.csv.gpg";

            return filename;
        }

        private static string CreateRandomFilePath(Guid subscriberAgreementId, string fileName)
        {
            return $"emisnightingale-data-preprod-provider-extracts" +
                $"/IM1" +
                $"/sftp" +
                $"/{subscriberAgreementId}" +
                $"/{DateTime.Now.ToString("yyyyMMdd")}" +
                $"/{fileName}";
        }

        private static string CreateRandomFilePath(Guid? identifier)
        {
            return $"{GetRandomString()}" +
                $"/{GetRandomString()}" +
                $"/{GetRandomString()}" +
                $"/{identifier}" +
                $"/0122235" +
                $"/{GetRandomString()}" +
                $"_{GetRandomNumber()}" +
                $"_{GetRandomString()}" +
                $"_{GetRandomString()}" +
                $"_{GetRandomNumber()}" +
                $"_{identifier}.csv.gpg";
        }

        private static string CreateRandomEncryptedFilePath(
            Guid? storageSubscriberAgreementId,
            Guid? supplierSharingAgreementGuid)
        {
            return $"{"encrypted"}/" +
                    $"{storageSubscriberAgreementId}" +
                    $"/{"0122235"}" +
                    $"/{GetRandomString()}" +
                    $"_{GetRandomNumber()}" +
                    $"_{GetRandomString()}" +
                    $"_{GetRandomString()}" +
                    $"_{GetRandomNumber()}" +
                    $"_{supplierSharingAgreementGuid}.csv.gpg";
        }

        private static string CreateRandomDecryptedFilePath(string dataSetName, Guid? datasetSpecificationId, string fileNameBase, Guid storageSubscriberAgreementId, Guid? supplierSharingAgreementGuid)
        {
            return $"{"decrypted"}/" +
                    $"{dataSetName}" +
                    $"/{datasetSpecificationId}" +
                    $"/{fileNameBase}" +
                    $"/{storageSubscriberAgreementId}" +
                    $"/{"0122235"}" +
                    $"_{GetRandomString()}" +
                    $"_{GetRandomNumber()}" +
                    $"_{GetRandomString()}" +
                    $"_{GetRandomString()}" +
                    $"_{supplierSharingAgreementGuid}.csv.gpg";
        }

        private static void ValidateBlobStorageSettings(BlobStorageSettings blobStorageSettings)
        {
            if (blobStorageSettings == null)
            {
                throw new InvalidConfigurationException("Configuration section 'blobStorage' not defined.");
            }

            Validate(
                (Rule: IsInvalid(blobStorageSettings.AzureBlobServiceUri),
                    Parameter: "blobStorage__azureBlobServiceUri"),

                (Rule: IsInvalid(blobStorageSettings.AzureTenantId),
                    Parameter: "blobStorage__azureTenantId"));
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Configuration value does not exist"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            StringBuilder validationErrors = new StringBuilder();
            validationErrors.AppendLine("Configuration error(s):");
            IDictionary errors = new Dictionary<string, List<string>>();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    validationErrors.AppendLine(
                        $"{parameter} -> Configuration value does not exist or does not meet validation criteria");

                    if (errors.Contains(parameter))
                    {
                        (errors[parameter] as List<string>)?.Add(rule.Message);
                        return;
                    }

                    errors.Add(parameter, new List<string> { rule.Message });
                }
            }

            var invalidConfigurationException = new InvalidConfigurationException(
                message: validationErrors.ToString(),
                data: errors);

            invalidConfigurationException.ThrowIfContainsErrors();
        }
    }
}

// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.Suppliers;
using LHDS.Core.Models.Orchestrations.EmisLandings;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Providers.Downloads;
using LHDS.Core.Providers.Downloads.DiskDownloads;
using LHDS.Core.Providers.Downloads.FtpDownloads;
using LHDS.Core.Services.Foundations.DataSets;
using LHDS.Core.Services.Foundations.DataSetSpecifications;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.IngestionTrackingAudits;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using LHDS.Core.Services.Foundations.Suppliers;
using LHDS.Core.Services.Orchestrations.SubscriberCredentials;
using LHDS.Core.Services.Processings.DataSetSpecifications;
using LHDS.Core.Tests.Acceptance.Brokers.DependencyBrokers;
using LHDS.Core.Tests.Acceptance.Clients.EmisLandings.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.EmisLandings
{
    [Collection(nameof(CoreTestCollection))]
    public partial class LandingTests
    {
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIngestionTrackingService ingestionTrackingService;
        private readonly IDataSetSpecificationService dataSetSpecificationService;
        private readonly ISupplierService supplierService;
        private readonly IDataSetService dataSetService;
        private readonly IDocumentService documentService;
        private readonly IDataSetSpecificationProcessingService dataSetSpecificationProcessingService;
        private readonly ISubscriberCredentialOrchestration subscriberCredentialOrchestration;
        private readonly IEmisLandingClient landingClient;
        private readonly LandingConfiguration landingConfiguration;
        private readonly IIngestionTrackingAuditService ingestionTrackingAuditService;
        private readonly ICompareLogic compareLogic;
        private readonly DependencyBroker dependencyBroker;
        private readonly BlobContainers blobContainers;
        private readonly string dropfolder = "landing";

        public LandingTests(DependencyBroker dependencyBroker)
        {
            this.dependencyBroker = dependencyBroker;
            this.compareLogic = new CompareLogic();
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

            serviceCollection.AddEmisLandingClient(this.dependencyBroker.Configuration);
            serviceCollection.Remove(new ServiceDescriptor(typeof(IDownloadProvider), typeof(FtpDownloadProvider)));

            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string defaultFolderPath = Path.Combine(assemblyPath, "temp", dropfolder);

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
            this.subscriberCredentialOrchestration = serviceProvider.GetService<ISubscriberCredentialOrchestration>();
            this.documentService = serviceProvider.GetService<IDocumentService>();
            this.blobContainers = serviceProvider.GetService<BlobContainers>();

            this.dataSetSpecificationProcessingService =
                serviceProvider.GetService<IDataSetSpecificationProcessingService>();

            this.ingestionTrackingAuditService = serviceProvider.GetService<IIngestionTrackingAuditService>();
            this.landingConfiguration = serviceProvider.GetService<LandingConfiguration>();
            this.dateTimeBroker = serviceProvider.GetService<IDateTimeBroker>();
            landingClient = serviceProvider.GetService<IEmisLandingClient>();
        }

        private void CleanupDownloadFolder()
        {
            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string defaultFolderPath = Path.Combine(assemblyPath, "temp", dropfolder);

            if (!Directory.Exists(defaultFolderPath))
            {
                return;
            }

            string[] files = Directory.GetFiles(defaultFolderPath);
            foreach (string file in files)
            {
                File.Delete(file);
            }

            // Delete all subfolders recursively
            string[] subdirectories = Directory.GetDirectories(defaultFolderPath);
            foreach (string subdirectory in subdirectories)
            {
                Directory.Delete(subdirectory, true);
            }
        }

        private List<DocumentSource> PrepareAndAddFile(
            Guid subscriberAgreementId,
            DataSetSpecification dataSetSpecification,
            bool createFiles,
            int count)
        {
            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string defaultFolderPath = Path.Combine(assemblyPath, "temp", dropfolder);
            List<DocumentSource> randomFiles = new List<DocumentSource>();

            for (int i = 0; i < count; i++)
            {
                string randomFileName = GetRandomFileName(subscriberAgreementId);
                string randomFilePath = CreateRandomFilePath(subscriberAgreementId, randomFileName);
                string filePath = Path.Combine(defaultFolderPath, randomFilePath);
                FileInfo fileInfo = new FileInfo(filePath);

                if (!fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                }

                File.WriteAllText(filePath, GetRandomString());
                var relativeSourcePath = Path.GetRelativePath(defaultFolderPath, filePath).Replace("\\", "/");
                Console.WriteLine($"relativeSourcePath: {relativeSourcePath}");

                var filename = relativeSourcePath.StartsWith('/')
                    ? relativeSourcePath
                    : "/" + relativeSourcePath;

                string[] splitFileName = filename.Split('/');
                string newFileName = $"{subscriberAgreementId}/{splitFileName[5]}/{splitFileName[6]}"; ;

                var encryptedFilePath = $"/{landingConfiguration.EncryptedFolder}/{newFileName}"; ;

                var relativeDecryptedPath =
                    $"/{landingConfiguration.DecryptedFolder}" +
                    $"/{dataSetSpecification.DataSet.DataSetName}" +
                    $"/{dataSetSpecification.OurSpecificationVersion}" +
                    $"/{filename.Split('_')[2]}_{filename.Split('_')[3]}" +
                    $"/{newFileName.Replace(".gpg", "", StringComparison.InvariantCultureIgnoreCase)}";

                DocumentSource documentSource = new DocumentSource
                {
                    FtpPath = relativeSourcePath,
                    EncryptedBlobPath = encryptedFilePath,
                    DecryptedBlobPath = relativeDecryptedPath,
                    FilePath = filePath
                };

                randomFiles.Add(documentSource);
            }

            return randomFiles;
        }


        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static string GetRandomFileName(Guid subscriberAgreementId)
        {
            string filename =
                $"delta{GetRandomString()}" +
                $"_{GetRandomNumber(min: 2, max: 1000)}" +
                $"_Admin" +
                $"_Location" +
                $"_{DateTime.Now.ToString("yyyyMMddHHmmss")}" +
                $"_{subscriberAgreementId}.csv.gpg";

            return filename;
        }

        private static string CreateRandomFilePath(Guid subscriberAgreementId, string fileName)
        {
            return Path.Combine(
                "emisnightingale-data-preprod-provider-extracts",
                "IM1",
                "sftp",
                $"{subscriberAgreementId}",
                $"{DateTime.Now.ToString("yyyyMMdd")}",
                $"{fileName}");
        }

        private static string GetRandomString(int length) =>
               new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static int GetRandomNumber(int min = 2, int max = 10) =>
            new IntRange(min, max).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime().AddDays(7)).GetValue();

        private async ValueTask<List<IngestionTracking>> CreateRandomIngestionTrackings(
            List<DocumentSource> documentSources,
            Guid supplierId)
        {
            List<IngestionTracking> items = new List<IngestionTracking>();

            foreach (var documentSource in documentSources)
            {
                var item = CreateIngestionTrackingFiller(
                    this.dateTimeBroker.GetCurrentDateTimeOffset(),
                    documentSource,
                    supplierId)
                        .Create();

                await this.ingestionTrackingService.AddIngestionTrackingAsync(item);
                items.Add(item);
            }

            return items;
        }

        private static Filler<IngestionTracking> CreateIngestionTrackingFiller(
            DateTimeOffset dateTimeOffset, DocumentSource documentSource, Guid supplierId)
        {
            string user = "System";
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnProperty(ingestionTracking => ingestionTracking.FileName).Use(documentSource.FtpPath)

                .OnProperty(ingestionTracking => ingestionTracking.EncryptedFileName)
                    .Use(documentSource.EncryptedBlobPath)

                .OnProperty(ingestionTracking => ingestionTracking.DecryptedFileName)
                    .Use(documentSource.DecryptedBlobPath)

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

        private Expression<Func<Download, bool>> SameDownloadAs(
            Download expectedDownload)
        {
            return actualDownload =>
                this.compareLogic.Compare(expectedDownload, actualDownload)
                    .AreEqual;
        }
    }
}

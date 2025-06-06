// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSets;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSetSpecifications;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SubscriberAgreements;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SubscriberCredentials;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using LHDS.Core.Models.Foundations.Documents;
using Tynamix.ObjectFiller;
using Xunit;
using Xunit.Abstractions;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.Landings
{
    [Collection(nameof(ApiTestCollection))]
    public partial class EmisLandingsApiTests
    {
        private readonly ApiBroker apiBroker;
        private readonly string encryptedFolder;
        private readonly string decryptedFolder;
        private readonly string dropfolder = "landing";
        private readonly Guid supplierId;
        private readonly Guid dataSetId;
        private readonly Guid dataSetSpecificationId;
        private readonly ITestOutputHelper output;

        public EmisLandingsApiTests(
            ApiBroker apiBroker,
            ITestOutputHelper output)
        {
            this.apiBroker = apiBroker;
            this.supplierId = this.apiBroker.landingConfiguration.LandingSupplierId;
            this.dataSetId = Guid.Parse("6a62313a-7442-462e-b6e8-dec541ddd0ba");
            this.dataSetSpecificationId = Guid.Parse("e8ebce80-e619-40ca-b45f-9c3ac0328143");
            this.encryptedFolder = this.apiBroker.landingConfiguration.EncryptedFolder;
            this.decryptedFolder = this.apiBroker.landingConfiguration.DecryptedFolder;
            this.output = output;
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

            string[] subdirectories = Directory.GetDirectories(defaultFolderPath);
            foreach (string subdirectory in subdirectories)
            {
                Directory.Delete(subdirectory, true);
            }
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static string CreateRandomFilePath(Guid identifier)
        {
            return $"emisnightingale-data-preprod-provider-extracts" +
                $"/IM1" +
                $"/sftp" +
                $"/{identifier}" +
                $"/{DateTime.Now.ToString("yyyyMMdd")}" +
                $"/delta{GetRandomString()}" +
                $"_{GetRandomNumber(min: 2, max: 1000)}" +
                $"_Admin" +
                $"_Location" +
                $"_{DateTime.Now.ToString("yyyyMMddHHmmss")}" +
                $"_{identifier}.csv.gpg";
        }

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
            return $"emisnightingale-data-preprod-provider-extracts" +
                $"/IM1" +
                $"/sftp" +
                $"/{subscriberAgreementId}" +
                $"/{DateTime.Now.ToString("yyyyMMdd")}" +
                $"/{fileName}";
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static int GetRandomNumber(int min = 2, int max = 10) =>
            new IntRange(min, max).GetValue();

        private static string GetRandomString(int length) =>
            new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private async ValueTask<IngestionTracking> PostRandomIngestionTrackingAsync(
            Guid supplierId,
            string fileName,
            string encryptedFilePath,
            string decryptedFilePath)
        {
            IngestionTracking randomIngestionTracking =
                CreateRandomIngestionTracking(supplierId, fileName, encryptedFilePath, decryptedFilePath);

            await this.apiBroker.PostIngestionTrackingAsync(randomIngestionTracking);

            return randomIngestionTracking;
        }

        private static IngestionTracking CreateRandomIngestionTracking(
            Guid supplierId,
            string fileName,
            string encryptedFilePath,
            string decryptedFilePath) =>
            CreateRandomIngestionTrackingFiller(supplierId, fileName, encryptedFilePath, decryptedFilePath).Create();

        private static Filler<IngestionTracking> CreateRandomIngestionTrackingFiller(
            Guid supplierId,
            string fileName,
            string encryptedFilePath,
            string decryptedFilePath)
        {
            string user = GetRandomString(255);
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnProperty(ingestionTracking => ingestionTracking.SupplierId).Use(supplierId)
                .OnProperty(ingestionTracking => ingestionTracking.SubscriberAgreementId).IgnoreIt()
                .OnProperty(ingestionTracking => ingestionTracking.FileName).Use($"{fileName}")

                .OnProperty(ingestionTracking =>
                    ingestionTracking.EncryptedFileName).Use($"/{encryptedFilePath}{fileName}")

                .OnProperty(ingestionTracking =>
                    ingestionTracking.DecryptedFileName).Use($"/{decryptedFilePath}{fileName}")

                .OnProperty(ingestionTracking => ingestionTracking.CreatedDate).Use(now)
                .OnProperty(ingestionTracking => ingestionTracking.CreatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedDate).Use(now)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedBy).Use(user);
            return filler;
        }

        private async ValueTask<Supplier> PostRandomSupplierAsync(Guid supplierId = default)
        {
            if (supplierId == default)
            {
                supplierId = Guid.NewGuid();
            }

            Supplier existingSupplier = (await this.apiBroker.FindSupplierByIdAsync(supplierId)).FirstOrDefault();

            if (existingSupplier != null)
            {
                return existingSupplier;
            }

            Supplier randomSupplier = CreateRandomSupplier(supplierId);
            await this.apiBroker.PostSupplierAsync(randomSupplier);

            return randomSupplier;
        }

        private static Supplier CreateRandomSupplier(Guid supplierId) =>
            CreateRandomSupplierFiller(supplierId).Create();

        private static Filler<Supplier> CreateRandomSupplierFiller(Guid supplierId)
        {
            Guid id = supplierId == default ? Guid.NewGuid() : supplierId;
            string userId = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<Supplier>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnProperty(supplier => supplier.Id).Use(id)
                .OnProperty(supplier => supplier.CreatedDate).Use(now)
                .OnProperty(supplier => supplier.CreatedBy).Use(userId)
                .OnProperty(supplier => supplier.UpdatedDate).Use(now)
                .OnProperty(supplier => supplier.UpdatedBy).Use(userId);

            return filler;
        }

        private async ValueTask<Supplier> PostLandingSupplierAsync(Guid supplierId)
        {
            Supplier landingsSupplier = CreateLandingSupplier(supplierId);
            await this.apiBroker.PostSupplierAsync(landingsSupplier);

            return landingsSupplier;
        }

        private async ValueTask<DataSetSpecification> PostDataSetSpecificationAsync(Guid dataSetSpecificationId, Guid dataSetId)
        {
            var now = DateTimeOffset.UtcNow;

            var dataSetSpecification = new DataSetSpecification
            {
                Id = dataSetSpecificationId,
                DataSetId = dataSetId,
                SupplierSpecificationVersion = "7.0",
                OurSpecificationVersion = "1.0",
                Notes = "This is a test dataset specification",
                IsMultiAuthorPerBatch = true,
                EntityChangeSynchronisation = "",
                DateReleased = new DateTime(year: 2023, month: 1, day: 1, hour: 0, minute: 0, second: 0),
                DateImplemented = new DateTime(year: 2023, month: 1, day: 1, hour: 0, minute: 0, second: 0),
                DateSuperseded = null,
                IsPublished = true,
                PresededById = null,
                SupersededById = null,
                CreatedBy = "System",
                CreatedDate = new DateTime(year: 2023, month: 1, day: 1, hour: 0, minute: 0, second: 0),
                UpdatedBy = "System",
                UpdatedDate = new DateTime(year: 2023, month: 1, day: 1, hour: 0, minute: 0, second: 0),
                ActiveFrom = new DateTime(year: 2023, month: 1, day: 1, hour: 0, minute: 0, second: 0),
                ActiveTo = new DateTime(year: 2123, month: 1, day: 1, hour: 0, minute: 0, second: 0),
                IsActive = true,
            };

            await this.apiBroker.PostDataSetSpecificationAsync(dataSetSpecification);

            return dataSetSpecification;
        }

        private async ValueTask<DataSet> PostDataSetAsync(Guid supplierId, Guid dataSetId)
        {
            var now = DateTimeOffset.UtcNow;

            DataSet dataSet = new DataSet
            {
                Id = dataSetId,
                SupplierId = supplierId,
                DataSetName = "PrimaryCareEMISDEV",
                DataSetAliases = "PrimaryCareEMISDEV",
                DataSetAuthor = "EMISDEV",
                SpecifiedBy = "EMIS",
                IsNationallySpecified = false,
                CollectedBy = "EMIS",
                IsNationallyCollected = false,
                DataSourceType = "PrimaryCareEMISDEV",
                CreatedBy = "System",
                CreatedDate = now,
                UpdatedBy = "System",
                UpdatedDate = now,
                ActiveFrom = now.AddDays(-1),
                ActiveTo = now.AddYears(100),
                IsActive = true,
            };

            await this.apiBroker.PostDataSetAsync(dataSet);

            return dataSet;
        }

        private static DataSet CreateDataSet(Guid supplierId) =>
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
                .OnProperty(dataSet => dataSet.CreatedBy).Use(user)
                .OnProperty(dataSet => dataSet.UpdatedBy).Use(user)
                .OnProperty(dataSet => dataSet.ActiveTo).Use(now.AddDays(GetRandomNumber()));

            return filler;
        }

        private static Supplier CreateLandingSupplier(Guid supplierId) =>
            CreateLandingSupplierFiller(supplierId).Create();

        private static Filler<Supplier> CreateLandingSupplierFiller(Guid supplierId)
        {
            string userId = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<Supplier>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnProperty(supplier => supplier.Id).Use(supplierId)
                .OnProperty(supplier => supplier.CreatedDate).Use(now)
                .OnProperty(supplier => supplier.CreatedBy).Use(userId)
                .OnProperty(supplier => supplier.UpdatedDate).Use(now)
                .OnProperty(supplier => supplier.UpdatedBy).Use(userId);

            return filler;
        }

        private async ValueTask<DataSet> PostRandomActiveDataSetAsync(Guid supplierId)
        {
            DataSet randomDataSet = CreateRandomActiveDataSet(supplierId);
            await this.apiBroker.PostDataSetAsync(randomDataSet);

            return randomDataSet;
        }

        private static DataSet CreateRandomActiveDataSet(Guid supplierId) =>
            CreateActiveDataSetFiller(supplierId).Create();

        private static Filler<DataSet> CreateActiveDataSetFiller(Guid supplierId)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<DataSet>();
            var now = DateTimeOffset.UtcNow;

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(dataSet => dataSet.SupplierId).Use(supplierId)
                .OnProperty(dataSet => dataSet.IsActive).Use(true)
                .OnProperty(dataSet => dataSet.CreatedBy).Use(user)
                .OnProperty(dataSet => dataSet.UpdatedBy).Use(user)
                .OnProperty(dataSet => dataSet.ActiveTo).Use(now.AddDays(GetRandomNumber()));

            return filler;
        }

        private async ValueTask<DataSetSpecification> PostRandomActiveDataSetSpecificationAsync(Guid dataSetId)
        {
            DataSetSpecification randomDataSetSpecification = CreateRandomActiveDataSetSpecification(dataSetId);
            await this.apiBroker.PostDataSetSpecificationAsync(randomDataSetSpecification);

            return randomDataSetSpecification;
        }

        private static DataSetSpecification CreateRandomActiveDataSetSpecification(Guid dataSetId) =>
            CreateActiveDataSetSpecificationFiller(dataSetId).Create();

        private static Filler<DataSetSpecification> CreateActiveDataSetSpecificationFiller(Guid dataSetId)
        {
            string user = GetRandomString(255);
            var filler = new Filler<DataSetSpecification>();
            var now = DateTimeOffset.UtcNow;

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(dataSetSpecification => dataSetSpecification.DataSetId).Use(dataSetId)
                .OnProperty(dataSetSpecification => dataSetSpecification.IsActive).Use(true)

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

        private static Document CreateRandomDocument() =>
            CreateDocumentFiller().Create();

        private static Filler<Document> CreateDocumentFiller()
        {
            var filler = new Filler<Document>();
            string filename = GetRandomString();

            filler.Setup()
                .OnProperty(document => document.FileName).Use(filename);

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
                .OnProperty(subscriberCredential => subscriberCredential.CreatedBy).Use(user)
                .OnProperty(subscriberCredential => subscriberCredential.UpdatedBy).Use(user);

            return filler;
        }

        private async ValueTask<SubscriberCredential> PostRandomSubscriberCredentialAsync(Guid id)
        {
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential(id);
            await this.apiBroker.PostSubscriberCredentialAsync(randomSubscriberCredential);

            return randomSubscriberCredential;
        }

        private async ValueTask<List<SubscriberCredential>> PostRandomSubscriberCredentialsAsync()
        {
            int randomNumber = GetRandomNumber();
            var randomSubscriberCredentials = new List<SubscriberCredential>();

            for (int i = 0; i < randomNumber; i++)
            {
                Guid id = Guid.NewGuid();
                randomSubscriberCredentials.Add(await PostRandomSubscriberCredentialAsync(id));
            }

            return randomSubscriberCredentials;
        }

        public static List<SubscriberCredential> CreatSubscriberCredentialsFromAgreements(
            List<SubscriberAgreement> subscriberAgreements)
        {
            List<SubscriberCredential> subscriberCredentials = new List<SubscriberCredential>();

            foreach (SubscriberAgreement subscriberAgreement in subscriberAgreements)
            {
                SubscriberCredential subscriberCredential =
                    CreateSubscriberCredentialFromAgreement(subscriberAgreement);

                subscriberCredentials.Add(subscriberCredential);
            }

            return subscriberCredentials;
        }

        private static SubscriberCredential CreateSubscriberCredentialFromAgreement(
            SubscriberAgreement subscriberAgreement)
        {
            SubscriberCredential subscriberCredential = new SubscriberCredential
            {
                Id = subscriberAgreement.Id,
                SupplierSharingAgreementShortName = subscriberAgreement.SupplierSharingAgreementShortName,
                SupplierSharingAgreementGuid = subscriberAgreement.SupplierSharingAgreementGuid,
                FtpPublicKey = subscriberAgreement.FtpPublicKey,
                FtpUserName = subscriberAgreement.FtpUserName,
                FtpPassPhrase = null,
                FtpPassword = null,
                FtpPrivateKey = null,
                GpgPublicKey = subscriberAgreement.GpgPublicKey,
                GpgPassPhrase = null,
                GpgPrivateKey = null,
                IsActive = subscriberAgreement.IsActive,
                LastPollEndDate = subscriberAgreement.LastPollEndDate,
                LastPollStartDate = subscriberAgreement.LastPollStartDate,
                CreatedBy = subscriberAgreement.CreatedBy,
                CreatedDate = subscriberAgreement.CreatedDate,
                UpdatedBy = subscriberAgreement.UpdatedBy,
                UpdatedDate = subscriberAgreement.UpdatedDate,
            };

            return subscriberCredential;
        }

        private static SubscriberCredential CreateRandomSubscriberCredential(Guid id) =>
            CreateRandomSubscriberCredentialFiller(id).Create();

        private static Filler<SubscriberCredential> CreateRandomSubscriberCredentialFiller(Guid id)
        {
            string user = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<SubscriberCredential>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(subscriberCredential => subscriberCredential.Id).Use(id)
                .OnProperty(subscriberCredential => subscriberCredential.CreatedDate).Use(now)
                .OnProperty(subscriberCredential => subscriberCredential.CreatedBy).Use(user)
                .OnProperty(subscriberCredential => subscriberCredential.UpdatedDate).Use(now)
                .OnProperty(subscriberCredential => subscriberCredential.UpdatedBy).Use(user);

            return filler;
        }

        private async ValueTask<SubscriberAgreement> PostRandomSubscriberAgreementAsync(Guid id)
        {
            SubscriberAgreement randomSubscriberAgreement = CreateRandomSubscriberAgreement(id);
            await this.apiBroker.PostSubscriberAgreementAsync(randomSubscriberAgreement);

            return randomSubscriberAgreement;
        }

        private async ValueTask<List<SubscriberAgreement>> PostRandomSubscriberAgreementsAsync(
            List<Guid> subscriberAgreementIds)
        {
            var randomSubscriberAgreements = new List<SubscriberAgreement>();

            foreach (Guid subscriberAgreementId in subscriberAgreementIds)
            {
                randomSubscriberAgreements.Add(await PostRandomSubscriberAgreementAsync(subscriberAgreementId));
            }

            return randomSubscriberAgreements;
        }

        public static List<SubscriberAgreement> CreateRandomSubscriberAgreements()
        {
            List<SubscriberAgreement> subscriberAgreements = new List<SubscriberAgreement>();

            for (int i = 0; i < 10; i++)
            {
                Guid id = Guid.NewGuid();
                SubscriberAgreement subscriberAgreement = CreateRandomSubscriberAgreement(id);
                subscriberAgreements.Add(subscriberAgreement);
            }

            return subscriberAgreements;
        }

        private static SubscriberAgreement CreateRandomSubscriberAgreement(Guid id) =>
            CreateRandomSubscriberAgreementFiller(id).Create();

        private static Filler<SubscriberAgreement> CreateRandomSubscriberAgreementFiller(Guid id)
        {
            string user = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<SubscriberAgreement>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(subscriberAgreement => subscriberAgreement.Id).Use(id)
                .OnProperty(subscriberAgreement => subscriberAgreement.CreatedDate).Use(now)
                .OnProperty(subscriberAgreement => subscriberAgreement.CreatedBy).Use(user)
                .OnProperty(subscriberAgreement => subscriberAgreement.UpdatedDate).Use(now)
                .OnProperty(subscriberAgreement => subscriberAgreement.UpdatedBy).Use(user);

            return filler;
        }
    }
}
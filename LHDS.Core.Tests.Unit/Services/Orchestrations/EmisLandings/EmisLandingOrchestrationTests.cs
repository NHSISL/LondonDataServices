// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Files;
using LHDS.Core.Brokers.Hashing;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Models.Foundations.Downloads.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using LHDS.Core.Models.Orchestrations.EmisLandings;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Services.Orchestrations.Downloads;
using LHDS.Core.Services.Orchestrations.EmisLandings;
using LHDS.Core.Services.Processings.DataSetSpecifications;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.Downloads;
using LHDS.Core.Services.Processings.IngestionTrackingAudits;
using LHDS.Core.Services.Processings.IngestionTrackings;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;
using Xunit.Abstractions;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.EmisLandings
{
    public partial class EmisLandingOrchestrationTests
    {
        private readonly ITestOutputHelper output;
        private readonly Mock<IDocumentProcessingService> documentProcessingServiceMock;
        private readonly Mock<IDownloadProcessingService> downloadProcessingServiceMock;
        private readonly Mock<IIngestionTrackingProcessingService> ingestionTrackingProcessingServiceMock;
        private readonly Mock<IIngestionTrackingAuditProcessingService> auditServiceMock;
        private readonly Mock<IDataSetSpecificationProcessingService> dataSetSpecificationProcessingServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<IIdentifierBroker> identifierBrokerMock;
        private readonly Mock<IHashBroker> hashBrokerMock;
        private readonly Mock<IFileBroker> fileBrokerMock;
        private readonly LandingConfiguration landingConfiguration;
        private readonly BlobContainers blobContainers;
        private readonly IEmisLandingOrchestrationService emisLandingOrchestrationService;
        private readonly ICompareLogic compareLogic;

        public EmisLandingOrchestrationTests(ITestOutputHelper output)
        {
            this.output = output;
            documentProcessingServiceMock = new Mock<IDocumentProcessingService>();
            downloadProcessingServiceMock = new Mock<IDownloadProcessingService>();
            ingestionTrackingProcessingServiceMock = new Mock<IIngestionTrackingProcessingService>();
            dataSetSpecificationProcessingServiceMock = new Mock<IDataSetSpecificationProcessingService>();
            auditServiceMock = new Mock<IIngestionTrackingAuditProcessingService>();
            loggingBrokerMock = new Mock<ILoggingBroker>();
            dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            identifierBrokerMock = new Mock<IIdentifierBroker>();
            hashBrokerMock = new Mock<IHashBroker>();
            fileBrokerMock = new Mock<IFileBroker>();
            compareLogic = new CompareLogic();

            landingConfiguration = new LandingConfiguration
            {
                LandingSupplierId = Guid.NewGuid(),
                EncryptedFolder = "encrypted",
                DecryptedFolder = "inbox/landing"
            };

            blobContainers = new BlobContainers
            {
                EmisLanding = "emislanding",
                Versioner = "versioner",
                OptOut = "optout",
                Pds = "pds",
            };

            emisLandingOrchestrationService = new EmisLandingOrchestrationService(
                documentProcessingService: documentProcessingServiceMock.Object,
                downloadProcessingService: downloadProcessingServiceMock.Object,
                ingestionTrackingProcessingService: ingestionTrackingProcessingServiceMock.Object,
                auditService: auditServiceMock.Object,
                dataSetSpecificationProcessingService: dataSetSpecificationProcessingServiceMock.Object,
                blobContainers,
                loggingBroker: loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object,
                identifierBroker: identifierBrokerMock.Object,
                hashBroker: hashBrokerMock.Object,
                fileBroker: fileBrokerMock.Object,
                landingConfiguration: landingConfiguration);
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

        public byte[] CreateRandomData()
        {
            string randomMessage = GetRandomString();
            return Encoding.ASCII.GetBytes(randomMessage);
        }

        private static List<string> GetRandomStrings() =>
            Enumerable.Range(1, GetRandomNumber())
                .Select(i => GetRandomString())
                .ToList();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

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

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static string GetRandomString(int length) =>
            new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static string GetRandomMessage() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static List<IngestionTracking> CreateRandomIngestionTrackings(
            DateTimeOffset dateTimeOffset,
            List<string> fileNames,
            bool isDownloaded,
            int retryCount)
        {
            List<IngestionTracking> items = new List<IngestionTracking>();

            foreach (var fileName in fileNames)
            {
                items.Add(CreateIngestionTrackingFiller(dateTimeOffset, fileName, isDownloaded, retryCount).Create());
            }

            return items;
        }

        private static IngestionTracking CreateRandomIngestionTracking(DateTimeOffset dateTimeOffset) =>
            CreateIngestionTrackingFiller(dateTimeOffset).Create();

        private Expression<Func<IngestionTracking, bool>> SameIngestionTrackingAs(
            IngestionTracking expectedIngestionTracking)
        {
            return actualIngestionTracking =>
                CompareObjects(expectedIngestionTracking, actualIngestionTracking);
        }

        private bool CompareObjects(object expected, object actual)
        {
            return this.compareLogic.Compare(expected, actual)
                    .AreEqual;
        }

        private Expression<Func<Download, bool>> SameDownloadAs(Download expectedDownload)
        {
            return actualDownload =>
                this.compareLogic.Compare(expectedDownload, actualDownload).AreEqual;
        }

        private Expression<Func<Stream, bool>> SameStreamAs(Stream expectedStream)
        {
            return actualStream =>
                this.compareLogic.Compare(expectedStream, actualStream).AreEqual;
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

        private static IQueryable<DataSetSpecification> CreateRandomDataSetSpecifications(DataSet dataSet)
        {
            return CreateDataSetSpecificationFiller(dataSet)
                .Create(count: 1)
                    .AsQueryable();
        }

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

        private Expression<Func<Document, bool>> SameDocumentAs(
            Document expectedDocument)
        {
            return actualDocument =>
                CompareObjects(expectedDocument, actualDocument);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
          actualException => actualException.SameExceptionAs(expectedException);

        public static TheoryData<Xeption> DownloadDependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new DocumentValidationException(
                    message: "Document validation errors occured, please try again",
                    innerException),

                new DocumentDependencyValidationException(
                    message: "Document dependency validation occurred, please try again.",
                    innerException),

                new DownloadValidationException(
                    message: "Download validation errors occurred, please try again.",
                    innerException),

                new DownloadDependencyValidationException(
                    message: "Download dependency validation occurred, please try again.",
                    innerException),

                new IngestionTrackingValidationException(
                    message: "Ingestion tracking validation errors occurred, fix the errors and try again.",
                    innerException),

                new IngestionTrackingDependencyValidationException(
                    message: "Ingestion tracking dependency validation occurred, please try again.",
                    innerException),

                new IngestionTrackingAuditValidationException(
                    message: "Audit validation errors occurred, please try again.",
                    innerException),

                new IngestionTrackingAuditDependencyValidationException(
                    message: "Audit dependency validation occurred, please try again.",
                    innerException)
            };
        }

        public static TheoryData<Xeption> DownloadDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new DocumentDependencyException(
                    message: "Document dependency error occurred, please contact support.",
                    innerException),

                new DocumentServiceException(
                    message: "Document service error occurred, please contact support.",
                    innerException),

                new DownloadDependencyException(
                    message: "Download dependency error occurred, please contact support.",
                    innerException),

                new DownloadServiceException(
                    message: "Download service error occurred, please contact support.",
                    innerException),

                new IngestionTrackingDependencyException(
                    message: "Failed ingestion tracking storage error occurred, please contact support.",
                    innerException),

                new IngestionTrackingServiceException(
                    message: "Ingestion tracking service error occurred, please contact support.",
                    innerException),

                new IngestionTrackingAuditDependencyException(
                    message: "Audit dependency error occurred, please contact support.",
                    innerException),

                new IngestionTrackingAuditServiceException(
                    message: "Audit service error occurred, please contact support.",
                    innerException)
            };
        }

        private static Filler<IngestionTracking> CreateIngestionTrackingFiller(DateTimeOffset dateTimeOffset)
        {
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(ingestionTracking => ingestionTracking.Supplier).IgnoreIt()
                .OnProperty(ingestionTracking => ingestionTracking.IngestionTrackingAudits).IgnoreIt();

            return filler;
        }

        private static Filler<IngestionTracking> CreateIngestionTrackingFiller(
            DateTimeOffset dateTimeOffset,
            string id,
            bool isDownloaded,
            int retryCount)
        {
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnProperty(ingestionTracking => ingestionTracking.FileName).Use(id)
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(ingestionTracking => ingestionTracking.Supplier).IgnoreIt()
                .OnProperty(ingestionTracking => ingestionTracking.IngestionTrackingAudits).IgnoreIt();

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
    }
}
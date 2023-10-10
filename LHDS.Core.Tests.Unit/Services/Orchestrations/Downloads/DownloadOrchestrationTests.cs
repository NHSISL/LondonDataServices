// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Audits.Exceptions;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Models.Foundations.Downloads.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using LHDS.Core.Models.Orchestrations.Downloads;
using LHDS.Core.Services.Foundations.Audits;
using LHDS.Core.Services.Foundations.DataSetSpecifications;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.Downloads;
using LHDS.Core.Services.Foundations.IngestionTrackingAudits;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using LHDS.Core.Services.Orchestrations.Downloads;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;
using Xunit.Abstractions;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Downloads
{
    public partial class DownloadOrchestrationTests
    {
        private readonly ITestOutputHelper output;
        private readonly Mock<IDocumentService> documentServiceMock;
        private readonly Mock<IDownloadService> downloadServiceMock;
        private readonly Mock<IIngestionTrackingService> ingestionTrackingServiceMock;
        private readonly Mock<IIngestionTrackingAuditService> auditServiceMock;
        private readonly Mock<IAuditService> auditServiceMock;
        private readonly Mock<IDataSetSpecificationService> dataSetSpecificationServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<IIdentifierBroker> identifierBrokerMock;
        private readonly LandingConfiguration landingConfiguration;
        private readonly IDownloadOrchestrationService downloadOrchestrationService;
        private readonly ICompareLogic compareLogic;

        public DownloadOrchestrationTests(ITestOutputHelper output)
        {
            this.output = output;
            documentServiceMock = new Mock<IDocumentService>();
            downloadServiceMock = new Mock<IDownloadService>();
            ingestionTrackingServiceMock = new Mock<IIngestionTrackingService>();
            auditServiceMock = new Mock<IAuditService>();
            dataSetSpecificationServiceMock = new Mock<IDataSetSpecificationService>();
            auditServiceMock = new Mock<IIngestionTrackingAuditService>();
            loggingBrokerMock = new Mock<ILoggingBroker>();
            dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            identifierBrokerMock = new Mock<IIdentifierBroker>();
            compareLogic = new CompareLogic();

            landingConfiguration = new LandingConfiguration
            {
                LandingSupplierId = Guid.NewGuid(),
                EncryptedFolder = "encrypted",
                DecryptedFolder = "inbox/landings"
            };

            downloadOrchestrationService = new DownloadOrchestrationService(
                documentService: documentServiceMock.Object,
                downloadService: downloadServiceMock.Object,
                ingestionTrackingService: ingestionTrackingServiceMock.Object,
                auditService: auditServiceMock.Object,
                dataSetSpecificationService: dataSetSpecificationServiceMock.Object,
                loggingBroker: loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object,
                identifierBroker: identifierBrokerMock.Object,
                landingConfiguration: landingConfiguration);
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static List<Document> CreateRandomDocuments()
        {
            return CreateDocumentFiller()
                .Create(count: 1)
                    .ToList();
        }

        private static Document CreateRandomDocument() =>
            CreateDocumentFiller().Create();

        private static Filler<Document> CreateDocumentFiller()
        {
            var filler = new Filler<Document>();
            string filename = GetRandomString(10);

            for (int i = 0; i < 6; i++)
            {
                filename = $"{filename}" + "_" + $"{GetRandomString(10)}";
            }

            filler.Setup()
                .OnProperty(dataSet => dataSet.FileName).Use(filename);

            return filler;
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
            List<Document> documents)
        {
            List<IngestionTracking> items = new List<IngestionTracking>();

            foreach (var document in documents)
            {
                items.Add(CreateIngestionTrackingFiller(dateTimeOffset, document.FileName).Create());
            }

            return items;
        }

        private static IngestionTracking CreateRandomIngestionTracking(DateTimeOffset dateTimeOffset) =>
            CreateIngestionTrackingFiller(dateTimeOffset).Create();

        private Expression<Func<IngestionTracking, bool>> SameIngestionTrackingAs(
            IngestionTracking exprectedIngestionTracking)
        {
            return actualIngestionTracking =>
                this.compareLogic.Compare(exprectedIngestionTracking, actualIngestionTracking)
                    .AreEqual;
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

        private static IQueryable<DataSetSpecification> CreateRandomDataSetSpecifications(Guid dataSetId)
        {
            return CreateDataSetSpecificationFiller(dataSetId)
                .Create(count: 1)
                    .AsQueryable();
        }

        private static Filler<DataSetSpecification> CreateDataSetSpecificationFiller(Guid dataSetId)
        {
            string user = GetRandomString(255);
            var filler = new Filler<DataSetSpecification>();
            var now = DateTimeOffset.UtcNow;

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)

                .OnProperty(dataSetSpecification =>
                    dataSetSpecification.DataSetId).Use(dataSetId)

                .OnProperty(dataSetSpecification =>
                    dataSetSpecification.IsActive).Use(true)

                .OnProperty(dataSetSpecification =>
                    dataSetSpecification.ActiveFrom).Use(now.AddDays(-2))

                .OnProperty(dataSetSpecification =>
                    dataSetSpecification.ActiveTo).Use(now.AddDays(2))

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
                this.compareLogic.Compare(expectedDocument, actualDocument)
                    .AreEqual;
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
          actualException => actualException.SameExceptionAs(expectedException);

        public static TheoryData DownloadDependencyValidationExceptions()
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

        public static TheoryData DownloadDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new DocumentDependencyException(
                    message: "Document dependency error occurred, contact support.",
                    innerException),

                new DocumentServiceException(
                    message: "Document service error occurred, contact support.",
                    innerException),

                new DownloadDependencyException(
                    message: "Download dependency error occurred, contact support.",
                    innerException),

                new DownloadServiceException(
                    message: "Download service error occurred, contact support.",
                    innerException),

                new IngestionTrackingDependencyException(
                    message: "Failed ingestion tracking storage error occurred, contact support.",
                    innerException),

                new IngestionTrackingServiceException(
                    message: "Ingestion tracking service error occurred, contact support.",
                    innerException),

                new IngestionTrackingAuditDependencyException(
                    message: "Audit dependency error occurred, contact support.",
                    innerException),

                new IngestionTrackingAuditServiceException(
                    message: "Audit service error occurred, contact support.",
                    innerException)
            };
        }

        private static Filler<IngestionTracking> CreateIngestionTrackingFiller(DateTimeOffset dateTimeOffset)
        {
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset);

            return filler;
        }

        private static Filler<IngestionTracking> CreateIngestionTrackingFiller(
            DateTimeOffset dateTimeOffset, string id)
        {
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnProperty(ingestionTracking => ingestionTracking.FileName).Use(id)
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset);

            return filler;
        }
    }
}
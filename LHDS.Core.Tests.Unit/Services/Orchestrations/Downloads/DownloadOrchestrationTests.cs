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
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Models.Foundations.Downloads.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using LHDS.Core.Models.Orchestrations.Downloads;
using LHDS.Core.Services.Foundations.Audits;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.Downloads;
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
        private readonly Mock<IAuditService> auditServiceMock;
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
            loggingBrokerMock = new Mock<ILoggingBroker>();
            dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            identifierBrokerMock = new Mock<IIdentifierBroker>();
            compareLogic = new CompareLogic();

            landingConfiguration = new LandingConfiguration
            {
                LandingSupplierId = Guid.NewGuid(),
                EncryptedFolder = "encrypted",
                DecryptedFolder = "decrypted"
            };

            downloadOrchestrationService = new DownloadOrchestrationService(
                documentService: documentServiceMock.Object,
                downloadService: downloadServiceMock.Object,
                ingestionTrackingService: ingestionTrackingServiceMock.Object,
                auditService: auditServiceMock.Object,
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
            filler.Setup();

            return filler;
        }

        private static string GetRandomString() =>
          new MnemonicString().GetValue();

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

                new DownloadValidationException(innerException),

                new DownloadDependencyValidationException(
                    message: "Download dependency validation occurred, please try again.",
                    innerException),

                new IngestionTrackingValidationException(innerException),
                new IngestionTrackingDependencyValidationException(innerException),

                new AuditValidationException(
                    message: "Audit validation errors occurred, please try again.",
                    innerException),

                new AuditDependencyValidationException(
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

                new IngestionTrackingDependencyException(innerException),
                new IngestionTrackingServiceException(innerException),

                new AuditDependencyException(
                    message: "Audit dependency error occurred, contact support.",
                    innerException),

                new AuditServiceException(
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
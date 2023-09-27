// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Audits.Exceptions;
using LHDS.Core.Models.Foundations.Decryptions.Exceptions;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using LHDS.Core.Services.Foundations.Audits;
using LHDS.Core.Services.Foundations.Decryptions;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using LHDS.Core.Services.Orchestrations.Decryptions;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Decryptions
{
    public partial class DecryptionOrchestrationTests
    {
        private readonly Mock<IDocumentService> documentServiceMock;
        private readonly Mock<IDecryptionService> decryptionServiceMock;
        private readonly Mock<IIngestionTrackingService> ingestionTrackingServiceMock;
        private readonly Mock<IAuditService> auditServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly IDecryptionOrchestrationService decryptionOrchestrationService;
        private readonly ICompareLogic compareLogic;

        public DecryptionOrchestrationTests()
        {
            documentServiceMock = new Mock<IDocumentService>();
            decryptionServiceMock = new Mock<IDecryptionService>();
            ingestionTrackingServiceMock = new Mock<IIngestionTrackingService>();
            auditServiceMock = new Mock<IAuditService>();
            loggingBrokerMock = new Mock<ILoggingBroker>();
            dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.compareLogic = new CompareLogic();

            this.decryptionOrchestrationService = new DecryptionOrchestrationService(
                documentService: documentServiceMock.Object,
                decryptionService: decryptionServiceMock.Object,
                ingestionTrackingService: ingestionTrackingServiceMock.Object,
                auditService: auditServiceMock.Object,
                loggingBroker: loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object
                );
        }
        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

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

        private Expression<Func<IngestionTracking, bool>> SameIngestionTrackingAs(
            IngestionTracking exprectedIngestionTracking)
        {
            return actualIngestionTracking =>
                this.compareLogic.Compare(exprectedIngestionTracking, actualIngestionTracking)
                    .AreEqual;
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
          actualException => actualException.SameExceptionAs(expectedException);

        private Expression<Func<Document, bool>> SameDocumentAs(
          Document expectedDocument)
        {
            return actualDocument =>
                this.compareLogic.Compare(expectedDocument, actualDocument)
                    .AreEqual;
        }

        public static TheoryData DecryptionDependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new DocumentValidationException(innerException),
                new DocumentDependencyValidationException(innerException),
                new DecryptionValidationException(innerException),
                new DecryptionDependencyValidationException(innerException),
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

        public static TheoryData DecryptionDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new DocumentDependencyException(innerException),
                new DocumentServiceException(innerException),
                new DecryptionDependencyException(innerException),
                new DecryptionServiceException(innerException),
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

        private static IngestionTracking CreateRandomIngestionTracking() =>
            CreateIngestionTrackingFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static IngestionTracking CreateRandomIngestionTracking(DateTimeOffset dateTimeOffset) =>
            CreateIngestionTrackingFiller(dateTimeOffset).Create();

        private static Filler<IngestionTracking> CreateIngestionTrackingFiller(DateTimeOffset dateTimeOffset)
        {
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset>().Use(dateTimeOffset);

            return filler;
        }
    }
}
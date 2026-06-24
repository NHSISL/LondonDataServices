// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Linq.Expressions;
using FluentAssertions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Hashing;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.Cryptographies.Exceptions;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using LHDS.Core.Models.Orchestrations.EmisLandings;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Services.Foundations.Cryptographies;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.Downloads;
using LHDS.Core.Services.Foundations.IngestionTrackingAudits;
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
        private readonly Mock<IDownloadService> downloadServiceMock;
        private readonly Mock<ICryptographyService> cryptographyServiceMock;
        private readonly Mock<IIngestionTrackingService> ingestionTrackingServiceMock;
        private readonly Mock<IIngestionTrackingAuditService> auditServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<IHashBroker> hashBrokerMock;
        private readonly IDecryptionOrchestrationService decryptionOrchestrationService;
        private readonly ICompareLogic compareLogic;
        private readonly BlobContainers blobContainers;
        private readonly LandingConfiguration landingConfiguration;
        private readonly ITestOutputHelper output;

        public DecryptionOrchestrationTests(ITestOutputHelper output)
        {
            this.output = output;
            documentServiceMock = new Mock<IDocumentService>();
            downloadServiceMock = new Mock<IDownloadService>();
            cryptographyServiceMock = new Mock<ICryptographyService>();
            ingestionTrackingServiceMock = new Mock<IIngestionTrackingService>();
            auditServiceMock = new Mock<IIngestionTrackingAuditService>();
            loggingBrokerMock = new Mock<ILoggingBroker>();
            dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            hashBrokerMock = new Mock<IHashBroker>();
            this.compareLogic = new CompareLogic();

            this.blobContainers = new BlobContainers
            {
                OptOut = "optout"
            };

            this.landingConfiguration = new LandingConfiguration();

            this.decryptionOrchestrationService = new DecryptionOrchestrationService(
                documentService: documentServiceMock.Object,
                downloadService: downloadServiceMock.Object,
                cryptographyService: cryptographyServiceMock.Object,
                ingestionTrackingService: ingestionTrackingServiceMock.Object,
                auditService: auditServiceMock.Object,
                blobContainers,
                loggingBroker: loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object,
                hashBroker: hashBrokerMock.Object,
                landingConfiguration: this.landingConfiguration);
        }

        private Expression<Func<Stream, bool>> SameStreamAs(Stream expectedStream)
        {
            return actualStream =>
                IsSameStream(expectedStream, actualStream);
        }

        private static bool IsSameStream(Stream expectedStream, Stream actualStream)
        {
            byte[] expectedBytes = ReadAllBytesFromStream(expectedStream);
            byte[] actualBytes = ReadAllBytesFromStream(actualStream);

            return new CompareLogic().Compare(expectedBytes, actualBytes).AreEqual;
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

        private Expression<Func<Download, bool>> SameDownloadAs(
            Download expectedDownload)
        {
            return actualDownload =>
                this.compareLogic.Compare(expectedDownload, actualDownload)
                    .AreEqual;
        }

        private static string GetRandomString() =>
          new MnemonicString().GetValue();

        private static string GetRandomString(int length) =>
            new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static string GetRandomMessage() =>
           new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private Expression<Func<IngestionTracking, bool>> SameIngestionTrackingAs(
            IngestionTracking exprectedIngestionTracking)
        {
            return actualIngestionTracking =>
                IsSameIngestionTracking(exprectedIngestionTracking, actualIngestionTracking);
        }

        private bool IsSameIngestionTracking(
            IngestionTracking expectedIngestionTracking,
            IngestionTracking actualIngestionTracking)
        {
            try
            {
                actualIngestionTracking.Should().BeEquivalentTo(expectedIngestionTracking);
            }
            catch (Exception exception)
            {
                output.WriteLine(exception.Message);
            }

            return new CompareLogic().Compare(expectedIngestionTracking, actualIngestionTracking).AreEqual;
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

        public static TheoryData<Xeption> DecryptionDependencyValidationExceptions()
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

                new CryptographyValidationException(
                    message: "Cryptography validation errors occurred, please try again.",
                    innerException),

                new CryptographyDependencyValidationException(
                    message: "Cryptography dependency validation occurred, please try again.",
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

        public static TheoryData<Xeption> DecryptionDependencyExceptions()
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

                new CryptographyDependencyException(
                    message: "Cryptography dependency error occurred, please contact support.",
                    innerException),

                new CryptographyServiceException(
                    message: "Cryptography service error occurred, please contact support.",
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

        private static IngestionTracking CreateRandomIngestionTracking() =>
            CreateIngestionTrackingFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static IngestionTracking CreateRandomIngestionTracking(DateTimeOffset dateTimeOffset) =>
            CreateIngestionTrackingFiller(dateTimeOffset).Create();

        private static Filler<IngestionTracking> CreateIngestionTrackingFiller(DateTimeOffset dateTimeOffset)
        {
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(ingestionTracking => ingestionTracking.Supplier).IgnoreIt()
                .OnProperty(ingestionTracking => ingestionTracking.SubscriberAgreement).IgnoreIt();

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

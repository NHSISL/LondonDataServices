// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.Audits;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Processings.Documents.Exceptions;
using LHDS.Core.Models.Processings.IngestionTrackings.Exceptions;
using LHDS.Core.Models.Processings.SpecificationObjects.Exceptions;
using LHDS.Core.Services.Orchestrations.Ingress;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.IngestionTrackings;
using LHDS.Core.Services.Processings.SpecificationObjects;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;
using Xunit.Abstractions;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Ingress
{
    public partial class IngressOrchestrationTests
    {
        private readonly Mock<IIngestionTrackingProcessingService> ingestionTrackingProcessingServiceMock;
        private readonly Mock<ISpecificationObjectProcessingService> specificationObjectProcessingServiceMock;
        private readonly Mock<IDocumentProcessingService> documentProcessingServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IAuditBroker> auditBrokerMock;
        private readonly BlobContainers blobContainers;
        private readonly IIngressOrchestrationService ingressOrchestrationService;
        private readonly ITestOutputHelper output;

        public IngressOrchestrationTests(ITestOutputHelper output)
        {
            this.output = output;
            this.ingestionTrackingProcessingServiceMock = new Mock<IIngestionTrackingProcessingService>();
            this.specificationObjectProcessingServiceMock = new Mock<ISpecificationObjectProcessingService>();
            this.documentProcessingServiceMock = new Mock<IDocumentProcessingService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.auditBrokerMock = new Mock<IAuditBroker>();

            blobContainers = new BlobContainers
            {
                EmisLanding = "emislanding",
                Versioner = "versioner",
                Ingress = "ingress",
                OptOut = "optout",
                Pds = "pds",
                TppLanding = "tpplanding"
            };

            this.ingressOrchestrationService = new IngressOrchestrationService(
                ingestionTrackingProcessingService: this.ingestionTrackingProcessingServiceMock.Object,
                specificationObjectProcessingService: this.specificationObjectProcessingServiceMock.Object,
                documentProcessingService: this.documentProcessingServiceMock.Object,
                blobContainers: this.blobContainers,
                loggingBroker: this.loggingBrokerMock.Object,
                auditBroker: this.auditBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private Expression<Func<Stream, bool>> SameStreamAs(Stream expectedStream)
        {
            return actualStream =>
                IsSameStream(expectedStream, actualStream);
        }

        private bool IsSameStream(Stream expectedStream, Stream actualStream)
        {
            byte[] expectedBytes = ReadAllBytesFromStream(expectedStream);
            byte[] actualBytes = ReadAllBytesFromStream(actualStream);

            string expectedString = System.Text.Encoding.UTF8.GetString(expectedBytes);
            string actualString = System.Text.Encoding.UTF8.GetString(actualBytes);

            try
            {
                actualString.Should().BeEquivalentTo(expectedString);
            }
            catch (Exception exception)
            {
                output.WriteLine(exception.Message);
            }

            return new CompareLogic().Compare(expectedBytes, actualBytes).AreEqual;
        }

        public static TheoryData<Xeption> IngressDependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new IngestionTrackingProcessingValidationException(
                    message: "Ingestion tracking processing validation errors occurred, please try again.",
                    innerException),

                new IngestionTrackingProcessingDependencyValidationException(
                    message: "Ingestion tracking processing dependency validation error occurred, " +
                        "fix the errors and try again.",
                    innerException),

                new SpecificationObjectProcessingValidationException(
                    message: "Specification object processing validation errors occurred, please try again.",
                    innerException),

                new SpecificationObjectProcessingDependencyValidationException(
                    message: "Specification object processing  dependency validation occurred, please try again.",
                    innerException),

                new DocumentValidationException(
                    message: "Document validation errors occured, please try again",
                    innerException),

                new DocumentDependencyValidationException(
                    message: "Document dependency validation occurred, please try again.",
                    innerException),
            };
        }

        public static TheoryData<Xeption> IngressDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new IngestionTrackingProcessingDependencyException(
                    message: "Ingestion tracking processing dependency error occurred, fix the errors and try again.",
                    innerException),

                new IngestionTrackingProcessingServiceException(
                    message: "Ingestion tracking processing service error occurred, please contact support.",
                    innerException),

                new SpecificationObjectProcessingDependencyException(
                    message: "Specification object processing dependency error occurred, please contact support.",
                    innerException),

                new SpecificationObjectProcessingServiceException(
                    message: "Specification object processing service error occurred, please contact support.",
                    innerException),

                new DocumentProcessingDependencyException(
                    message: "Document processing dependency error occurred, please contact support.",
                    innerException),

                new DocumentProcessingServiceException(
                    message: "Document processing service error occurred, please contact support.",
                    innerException),
            };
        }

        private static byte[] ReadAllBytesFromStream(Stream stream)
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

        private static string CreateRandomDecryptedFilePath() =>
            $"/{GetRandomString()}/{DateTime.UtcNow.ToString("yyyyMMdd_HHmm")}/{GetRandomString()}.csv";

        private static string GetRandomString() =>
            new MnemonicString(wordCount: 1, wordMinLength: 1, wordMaxLength: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static List<string> GetRandomStringList()
        {
            return Enumerable.Range(start: 0, count: GetRandomNumber())
                .Select(number => GetRandomString())
                    .ToList();
        }

        private static IngestionTracking CreateRandomIngestionTracking() =>
            CreateIngestionTrackingFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static Filler<IngestionTracking> CreateIngestionTrackingFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(ingestionTracking => ingestionTracking.CreatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.IngestionTrackingAudits).IgnoreIt()
                .OnProperty(ingestionTracking => ingestionTracking.Supplier).IgnoreIt();

            return filler;
        }
    }
}

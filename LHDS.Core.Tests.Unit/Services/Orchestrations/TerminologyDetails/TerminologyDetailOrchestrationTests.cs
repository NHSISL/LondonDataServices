// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Processings.Documents.Exceptions;
using LHDS.Core.Models.Processings.Ontologies.Exceptions;
using LHDS.Core.Models.Processings.TerminologyArtifacts.Exceptions;
using LHDS.Core.Services.Orchestrations.TerminologyDetails;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.Ontologies;
using LHDS.Core.Services.Processings.TerminologyArtifacts;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.TerminologyDetails
{
    public partial class TerminologyDetailOrchestrationTests
    {
        private readonly Mock<ITerminologyArtifactProcessingService> terminologyArtifactProcessingServiceMock;
        private readonly Mock<IOntologyProcessingService> ontologyProcessingServiceMock;
        private readonly Mock<IDocumentProcessingService> documentProcessingServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly BlobContainers blobContainers;
        private readonly ITerminologyDetailOrchestrationService terminologyDetailOrchestrationService;
        private readonly ICompareLogic compareLogic;

        public TerminologyDetailOrchestrationTests()
        {
            documentProcessingServiceMock = new Mock<IDocumentProcessingService>();
            terminologyArtifactProcessingServiceMock = new Mock<ITerminologyArtifactProcessingService>();
            ontologyProcessingServiceMock = new Mock<IOntologyProcessingService>();
            loggingBrokerMock = new Mock<ILoggingBroker>();
            dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            compareLogic = new CompareLogic();

            blobContainers = new BlobContainers
            {
                EmisLanding = "emislanding",
                Versioner = "versioner",
                OptOut = "optout",
                Pds = "pds",
                Terminology = "terminology",
            };

            terminologyDetailOrchestrationService = new TerminologyDetailOrchestrationService(
                terminologyArtifactProcessingService: terminologyArtifactProcessingServiceMock.Object,
                ontologyProcessingService: ontologyProcessingServiceMock.Object,
                documentProcessingService: documentProcessingServiceMock.Object,
                blobContainers: this.blobContainers,
                loggingBroker: loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object);
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
                filename = $"{filename}_{GetRandomString(10)}";
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

        private Expression<Func<Document, bool>> SameDocumentAs(
            Document expectedDocument)
        {
            return actualDocument =>
                this.compareLogic.Compare(expectedDocument, actualDocument)
                    .AreEqual;
        }

        private Expression<Func<TerminologyArtifact, bool>> SameTerminologyArtifactAs(
            TerminologyArtifact expectedTerminologyArtifact)
        {
            return actualTerminologyArtifact =>
                this.compareLogic.Compare(expectedTerminologyArtifact, actualTerminologyArtifact)
                    .AreEqual;
        }

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static TerminologyArtifact CreateRandomUndownloadedTerminologyArtifact() =>
            CreateUndownloadedTerminologyArtifactFiller().Create();

        private static Filler<TerminologyArtifact> CreateUndownloadedTerminologyArtifactFiller()
        {
            string user = Guid.NewGuid().ToString();
            DateTimeOffset dateTimeOffset = DateTimeOffset.Now;
            var filler = new Filler<TerminologyArtifact>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(terminologyArtifact => terminologyArtifact.IsDownloaded).Use(false)
                .OnProperty(terminologyArtifact => terminologyArtifact.CreatedBy).Use(user)
                .OnProperty(terminologyArtifact => terminologyArtifact.UpdatedBy).Use(user);

            return filler;
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
          actualException => actualException.SameExceptionAs(expectedException);

        public static TheoryData<Xeption> TerminologyDetailOrchestrationDependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new DocumentProcessingValidationException(
                    message: "Document processing validation errors occured, please try again",
                    innerException),

                new DocumentProcessingDependencyValidationException(
                    message: "Document processing dependency validation occurred, please try again.",
                    innerException),

                new TerminologyArtifactProcessingValidationException(
                    message: "Terminology artifact processing validation errors occurred, please try again.",
                    innerException),

                new TerminologyArtifactProcessingDependencyValidationException(
                    message: "Terminology artifact processing dependency validation occurred, please try again.",
                    innerException),

                new OntologyProcessingValidationException(
                    message: "Ontology processing validation errors occurred, fix the errors and try again.",
                    innerException),

                new OntologyProcessingDependencyValidationException(
                    message: "Ontology processing dependency validation occurred, please try again.",
                    innerException),
            };
        }

        public static TheoryData<Xeption> TerminologyDetailOrchestrationDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new DocumentProcessingDependencyException(
                    message: "Document processing dependency error occurred, please contact support.",
                    innerException),

                new DocumentProcessingServiceException(
                    message: "Document processing service error occurred, please contact support.",
                    innerException),

                new TerminologyArtifactProcessingDependencyException(
                    message: "Terminology artifact processing dependency error occurred, please contact support.",
                    innerException),

                new TerminologyArtifactProcessingServiceException(
                    message: "Terminology artifact processing service error occurred, please contact support.",
                    innerException),

                new OntologyProcessingDependencyException(
                    message: "Ontology processing dependency error occurred, please contact support.",
                    innerException),

                new OntologyProcessingServiceException(
                    message: "Ontology processing service error occurred, please contact support.",
                    innerException),
            };
        }
    }
}
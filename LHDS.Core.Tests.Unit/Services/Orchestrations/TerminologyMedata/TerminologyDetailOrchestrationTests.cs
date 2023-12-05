// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.TerminologyMetadata
{
    public partial class TerminologyMedataOrchestrationTests
    {
        private readonly Mock<ITerminologyArtifactProcessingService> terminologyArtifactProcessingServiceMock;
        private readonly Mock<IOntologyProcessingService> ontologyProcessingServiceMock;
        private readonly Mock<IDocumentProcessingService> documentProcessingServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly BlobContainers blobContainers;
        private readonly ITerminologyDetailOrchestrationService terminologyDetailOrchestrationService;
        private readonly ICompareLogic compareLogic;

        public TerminologyMedataOrchestrationTests()
        {
            documentProcessingServiceMock = new Mock<IDocumentProcessingService>();
            terminologyArtifactProcessingServiceMock = new Mock<ITerminologyArtifactProcessingService>();
            ontologyProcessingServiceMock = new Mock<IOntologyProcessingService>();
            loggingBrokerMock = new Mock<ILoggingBroker>();
            dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            compareLogic = new CompareLogic();

            terminologyDetailOrchestrationService = new TerminologyDetailOrchestrationService(
                terminologyArtifactProcessingService: terminologyArtifactProcessingServiceMock.Object,
                ontologyProcessingService: ontologyProcessingServiceMock.Object,
                documentProcessingService: documentProcessingServiceMock.Object,
                blobContainers: this.blobContainers,
                loggingBroker: loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object);

            blobContainers = new BlobContainers
            {
                EmisLanding = "emislanding",
                Versioner = "versioner",
                OptOut = "optout",
                Pds = "pds",
                Terminology = "terminology",
            };
        }

        private static int GetRandomNumber() =>
                new IntRange(min: 2, max: 10).GetValue();

    
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
    }
}
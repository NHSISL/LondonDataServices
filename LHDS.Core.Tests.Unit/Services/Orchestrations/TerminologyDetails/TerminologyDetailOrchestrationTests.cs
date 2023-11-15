// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Services.Orchestrations.TerminologyDetails;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.Ontologies;
using LHDS.Core.Services.Processings.TerminologyArtifacts;
using Moq;
using Tynamix.ObjectFiller;

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

            terminologyDetailOrchestrationService = new TerminologyDetailOrchestrationService(
                terminologyArtifactProcessingService: terminologyArtifactProcessingServiceMock.Object,
                ontologyProcessingService: ontologyProcessingServiceMock.Object,
                documentProcessingService: documentProcessingServiceMock.Object,
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

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static IQueryable<TerminologyArtifact> CreateRandomUndownloadedTerminologyArtifacts()
        {
            return CreateUndownloadedTerminologyArtifactFiller()
                .Create(count: 1)
                    .AsQueryable();
        }

        private static TerminologyArtifact CreateRandomTerminologyArtifact() =>
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
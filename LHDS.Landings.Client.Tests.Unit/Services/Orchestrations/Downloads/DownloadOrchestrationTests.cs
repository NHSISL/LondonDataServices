// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.Landings.Client.Brokers.Loggings;
using LHDS.Landings.Client.Models.Foundations.Documents;
using LHDS.Landings.Client.Services.Foundations.Documents;
using LHDS.Landings.Client.Services.Foundations.Downloads;
using LHDS.Landings.Client.Services.Foundations.IngestionTrackings;
using LHDS.Landings.Client.Services.Orchestrations.Download;
using Moq;
using Tynamix.ObjectFiller;

namespace LHDS.Landings.Client.Tests.Unit.Services.Orchestrations.Downloads
{
    public partial class DownloadOrchestrationTests
    {
        private readonly Mock<IDocumentService> documentServiceMock;
        private readonly Mock<IDownloadService> downloadServiceMock;
        private readonly Mock<IIngestionTrackingService> ingestionTrackingServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IDownloadOrchestrationService downloadOrchestrationService;

        public DownloadOrchestrationTests()
        {
            documentServiceMock = new Mock<IDocumentService>();
            downloadServiceMock = new Mock<IDownloadService>();
            ingestionTrackingServiceMock = new Mock<IIngestionTrackingService>();
            loggingBrokerMock = new Mock<ILoggingBroker>();

            this.downloadOrchestrationService = new DownloadOrchestrationService(
                documentService: documentServiceMock.Object,
                downloadService: downloadServiceMock.Object,
                ingestionTrackingService: ingestionTrackingServiceMock.Object,
                loggingBroker: loggingBrokerMock.Object);
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static List<Document> CreateRandomDocuments()
        {
            return CreateDocumentFiller()
                .Create(count: GetRandomNumber())
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

    }
}

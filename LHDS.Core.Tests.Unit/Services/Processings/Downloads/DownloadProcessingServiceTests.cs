// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Services.Foundations.Downloads;
using LHDS.Core.Services.Processings.Downloads;
using Moq;
using Tynamix.ObjectFiller;

namespace LHDS.Core.Tests.Unit.Services.Processings.Downloads
{
    public partial class DownloadProcessingServiceTests
    {
        private readonly Mock<IDownloadService> downloadServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IDownloadProcessingService downloadProcessingService;

        public DownloadProcessingServiceTests()
        {
            this.downloadServiceMock = new Mock<IDownloadService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.downloadProcessingService = new DownloadProcessingService(
                downloadService: this.downloadServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
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
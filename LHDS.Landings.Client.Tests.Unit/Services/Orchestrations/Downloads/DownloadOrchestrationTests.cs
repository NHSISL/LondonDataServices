// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using LHDS.Landings.Client.Brokers.DateTimes;
using LHDS.Landings.Client.Brokers.Loggings;
using LHDS.Landings.Client.Models.Foundations.Documents;
using LHDS.Landings.Client.Models.Foundations.IngestionTrackings;
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
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly IDownloadOrchestrationService downloadOrchestrationService;

        public DownloadOrchestrationTests()
        {
            documentServiceMock = new Mock<IDocumentService>();
            downloadServiceMock = new Mock<IDownloadService>();
            ingestionTrackingServiceMock = new Mock<IIngestionTrackingService>();
            loggingBrokerMock = new Mock<ILoggingBroker>();
            dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.downloadOrchestrationService = new DownloadOrchestrationService(
                documentService: documentServiceMock.Object,
                downloadService: downloadServiceMock.Object,
                ingestionTrackingService: ingestionTrackingServiceMock.Object,
                loggingBroker: loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object
                );
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

        private static IngestionTracking CreateRandomIngestionTracking(DateTimeOffset dateTimeOffset) =>
            CreateIngestionTrackingFiller(dateTimeOffset).Create();

        private static Filler<IngestionTracking> CreateIngestionTrackingFiller(DateTimeOffset dateTimeOffset)
        {
            var filler = new Filler<IngestionTracking>();
            filler.Setup().OnType<DateTimeOffset>().Use(dateTimeOffset);

            return filler;
        }
    }
}

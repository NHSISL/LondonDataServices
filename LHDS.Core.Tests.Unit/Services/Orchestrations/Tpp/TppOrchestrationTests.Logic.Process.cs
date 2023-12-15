// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Tpp
{
    public partial class TppOrchestrationTests
    {
        [Fact]
        public async Task ShouldProcessDocumentAndUpdateHashIfExistsAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            string randomHash = GetRandomString(64);
            Models.Foundations.Documents.Document randomDocument = CreateRandomDocument();
            randomDocument.SHA256Hash = randomHash;

            List<Models.Foundations.Documents.Document> randomDocuments = CreateRandomDocuments();
            randomDocuments[1].FileName = randomDocument.FileName;
            //wont have same hash as stored randomDocument coming in

            List<IngestionTracking> randomIngestionTrackings =
                CreateRandomIngestionTrackings(randomDateTime, randomDocuments);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackings())
                    .Returns(randomIngestionTrackings.AsQueryable());

            this.hashBrokerMock.Setup(broker =>
                    broker.GenerateSha256Hash(randomDocument.DocumentData))
                        .Returns(randomHash);

            //if(randomDocument.SHA256Hash == )

            // when
            //ValueTask<Guid> = this.tppOrchestrationService.

            // then

        }
    }
}
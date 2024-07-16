// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Decryptions
{

    public partial class DecryptionOrchestrationTests
    {
        [Fact]
        public async Task ShouldGetNextItemToBeDecryptedAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            DateTimeOffset olderThanDateTimeOffset = randomDateTimeOffset.AddMinutes(-15);
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking(randomDateTimeOffset);
            randomIngestionTracking.IsDownloaded = true;
            randomIngestionTracking.Decrypted = false;
            randomIngestionTracking.IsProcessing = false;
            randomIngestionTracking.RetryCount = 0;
            randomIngestionTracking.LastAttempt = olderThanDateTimeOffset;
            IngestionTracking storageIngestionTracking = randomIngestionTracking;
            var updatedIngestionTracking = storageIngestionTracking.DeepClone();
            updatedIngestionTracking.IsProcessing = true;
            updatedIngestionTracking.RetryCount += 1;
            updatedIngestionTracking.LastAttempt = randomDateTimeOffset;
            var outputIngestionTracking = updatedIngestionTracking.DeepClone();

            this.ingestionTrackingServiceMock.Setup(service =>
               service.RetrieveAllIngestionTrackings())
                   .Returns(new List<IngestionTracking> { storageIngestionTracking }.AsQueryable());

            this.ingestionTrackingServiceMock.Setup(service =>
                service.ModifyIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(updatedIngestionTracking))))
                    .ReturnsAsync(outputIngestionTracking);

            // when
            string? actualEncryptedFileName =
                await this.decryptionOrchestrationService.GetNextItemToBeDecrypted();

            // then
            actualEncryptedFileName.Should().Be(storageIngestionTracking.EncryptedFileName);

            this.ingestionTrackingServiceMock.Verify(service =>
              service.RetrieveAllIngestionTrackings(),
                  Times.Once);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.ModifyIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(updatedIngestionTracking))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Exactly(2));

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

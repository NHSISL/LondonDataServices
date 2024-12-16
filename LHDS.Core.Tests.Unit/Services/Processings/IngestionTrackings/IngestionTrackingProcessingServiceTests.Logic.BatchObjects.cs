// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.IngestionTrackings
{
    public partial class IngestionTrackingProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveObjectsInBatchByBatchReferenceAsync()
        {
            // Given
            string batchReference = GetRandomString();

            List<IngestionTracking> randomIngestionTrackings = CreateRandomIngestionTrackings();

            randomIngestionTrackings.ForEach(ingestionTracking =>
            {
                ingestionTracking.Batch = batchReference;
            });

            List<IngestionTracking> storageIngestionTrackings = randomIngestionTrackings;

            List<string> ingestionTrackingObjects = randomIngestionTrackings
                .Select(ingestionTracking => ingestionTracking.ObjectName)
                    .ToList();

            List<string> expectedIngestionTracking = ingestionTrackingObjects.DeepClone();

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackings())
                    .Returns(storageIngestionTrackings.AsQueryable());

            // When
            List<string> actualIngestionTracking =
                await this.ingestionTrackingProcessingService
                    .RetrieveObjectsInBatchByBatchReference(batchReference);

            // Then
            actualIngestionTracking.Should().BeEquivalentTo(expectedIngestionTracking);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackings(),
                    Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

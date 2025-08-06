// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Ingress
{
    public partial class IngressOrchestrationTests
    {
        [Fact]
        public async Task ShouldRollBackIngestionTrackingItemAsync()
        {
            // given
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking();
            IngestionTracking storageIngestionTracking = randomIngestionTracking;
            storageIngestionTracking.IsProcessing = true;

            IQueryable<IngestionTracking> storageIngestionTrackings =
                new List<IngestionTracking> { storageIngestionTracking }.AsQueryable();

            IngestionTracking pendingUpdateIngestionTracking = storageIngestionTracking.DeepClone();
            pendingUpdateIngestionTracking.IsProcessing = false;
            IngestionTracking updatedIngestionTracking = storageIngestionTracking.DeepClone();

            this.ingestionTrackingProcessingServiceMock
                .Setup(service => service.RetrieveAllIngestionTrackingsAsync())
                    .ReturnsAsync(storageIngestionTrackings);

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.ModifyIngestionTrackingAsync(pendingUpdateIngestionTracking))
                    .ReturnsAsync(updatedIngestionTracking);

            // when
            await this.ingressOrchestrationService
                .RollbackIngestionTrackingItemAsync(randomIngestionTracking.EncryptedFileName);

            // then
            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackingsAsync(),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.ModifyIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(pendingUpdateIngestionTracking))),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.specificationObjectProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

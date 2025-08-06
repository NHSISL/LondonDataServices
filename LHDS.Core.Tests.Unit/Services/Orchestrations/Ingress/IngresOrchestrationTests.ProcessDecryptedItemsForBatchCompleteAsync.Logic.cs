// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Services.Orchestrations.Ingress;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Ingress
{
    public partial class IngressOrchestrationTests
    {
        [Fact]
        public async Task ShouldProcessDecryptedItemsForBatchCompleteAsyncLogicAsync()
        {
            // given
            IngestionTracking randomIngestionTrackingOne = CreateRandomIngestionTracking();
            randomIngestionTrackingOne.Decrypted = true;
            randomIngestionTrackingOne.IsBatchComplete = false;

            IngestionTracking randomIngestionTrackingTwo = randomIngestionTrackingOne.DeepClone();
            randomIngestionTrackingTwo.Id = Guid.NewGuid();

            List<IngestionTracking> storageIngestionTrackings = new List<IngestionTracking>
            {
                randomIngestionTrackingOne,
                randomIngestionTrackingTwo
            };

            var ingressOrchestrationServiceMock = new Mock<IngressOrchestrationService>(
                this.ingestionTrackingProcessingServiceMock.Object,
                this.specificationObjectProcessingServiceMock.Object,
                this.documentProcessingServiceMock.Object,
                this.landingConfiguration,
                this.blobContainers,
                this.loggingBrokerMock.Object,
                this.auditBrokerMock.Object)
            {
                CallBase = true
            };

            this.ingestionTrackingProcessingServiceMock
                .SetupSequence(service => service.RetrieveAllIngestionTrackingsAsync())
                    .ReturnsAsync(storageIngestionTrackings.AsQueryable())
                        .ReturnsAsync(new List<IngestionTracking>().AsQueryable());

            var itemId = storageIngestionTrackings.First().Id;

            ingressOrchestrationServiceMock.Setup(service =>
                service.CheckForBatchCompleteAsync(itemId))
                    .Returns(ValueTask.CompletedTask);

            // when
            await ingressOrchestrationServiceMock.Object.ProcessDecryptedItemsForBatchCompleteAsync();

            // then
            this.ingestionTrackingProcessingServiceMock
                .Verify(service => service.RetrieveAllIngestionTrackingsAsync(),
                    Times.Exactly(2));

            ingressOrchestrationServiceMock.Verify(service =>
                service.CheckForBatchCompleteAsync(storageIngestionTrackings.First().Id),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.specificationObjectProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

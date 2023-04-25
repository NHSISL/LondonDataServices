// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.OptOuts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.OptOuts
{
    public partial class OptOutOrchestrationTests
    {
        [Fact]
        public async Task ShouldPushExpiredOptOutsToMeshForRenewalStatusAsync()
        {
            // Given
            DateTimeOffset randomDate = GetRandomDateTimeOffset();
            DateTimeOffset currentDateTime = randomDate;
            bool withHeader = optOutConfiguration.OptOutFileHasHeader;
            bool shouldAddTrailingComma = optOutConfiguration.OptOutFileRequireTrailingComma;
            List<OptOut> randomOptOuts = CreateRandomOptOutsList();
            List<OptOut> outputOptOuts = randomOptOuts;
            var processedOutputString = GetRandomString();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDate);

            this.optOutProcessingServiceMock.Setup(processing =>
                processing.RetrieveAllExpiredOptOutsAsync(optOutConfiguration.ExpiredAfterDays))
                    .ReturnsAsync(outputOptOuts);

            string batchReference = randomDate.ToString("yyyyMMddHHmmss");

            this.csvMapperProcessingServiceMock.Setup(processings =>
                processings.MapObjectToCsvAsync(outputOptOuts, withHeader, shouldAddTrailingComma))
                    .ReturnsAsync(processedOutputString);

            MeshMessage message = new MeshMessage
            {
                StringContent = processedOutputString
            };

            this.meshProcessingServiceMock.Setup(processings =>
                processings.SendMessageAsync(message));

            foreach (var optOut in outputOptOuts)
            {
                optOut.LastSentToMesh = currentDateTime;
                optOut.UpdatedDate = currentDateTime;
                optOut.BatchReference = batchReference;

                this.optOutProcessingServiceMock.Setup(processing =>
                    processing.ModifyOptOutAsync(optOut))
                        .ReturnsAsync(optOut);
            }

            // When
            await this.optOutOrchestrationService.PushExpiredOptOutsToMeshForRenewalAsync();

            // Then

            this.optOutProcessingServiceMock.Verify(processings =>
                processings.RetrieveAllExpiredOptOutsAsync(optOutConfiguration.ExpiredAfterDays),
                    Times.Once);

            this.csvMapperProcessingServiceMock.Verify(processings =>
                processings.MapObjectToCsvAsync(It.IsAny<List<OptOut>>(), withHeader, shouldAddTrailingComma),
                    Times.Once);


            this.meshProcessingServiceMock.Verify(processings =>
                processings.SendMessageAsync(It.Is(SameMessageAs(message))),
                    Times.Once);

            foreach (var optOut in outputOptOuts)
            {
                optOut.LastSentToMesh = currentDateTime;
                optOut.UpdatedDate = currentDateTime;
                optOut.BatchReference = batchReference;

                this.optOutProcessingServiceMock.Verify(processing =>
                    processing.ModifyOptOutAsync(optOut),
                        Times.Once());
            }

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.csvMapperProcessingServiceMock.VerifyNoOtherCalls();
            this.meshProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}

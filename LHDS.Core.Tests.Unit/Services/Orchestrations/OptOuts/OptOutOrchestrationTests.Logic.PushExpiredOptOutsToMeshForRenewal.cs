// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
            //Given
            List<OptOut> randomOptOuts = CreateRandomOptOutsList();
            List<OptOut> outputOptOuts = randomOptOuts;

            var processedOutputString = GetRandomString();

            this.optOutProcessingServiceMock.Setup(processings =>
                   processings.RetrieveAllExpiredOptOutsAsync())
                       .ReturnsAsync(outputOptOuts);

            this.csvMapperProcessingServiceMock.Setup(processings =>
                processings.MapObjectToCsvAsync(outputOptOuts, false))
                    .ReturnsAsync(processedOutputString);

            MeshMessage message = new MeshMessage
            {
                StringContent = processedOutputString
            };

            this.meshProcessingServiceMock.Setup(processings =>
                processings.SendMessageAsync(message));

            //When
            await this.optOutOrchestrationService.PushExpiredOptOutsToMeshForRenewalAsync();

            //Then

            this.optOutProcessingServiceMock.Verify(processings =>
                  processings.RetrieveAllExpiredOptOutsAsync(),
                    Times.Once);

            this.csvMapperProcessingServiceMock.Verify(processings =>
                  processings.MapObjectToCsvAsync(It.IsAny<List<OptOut>>(), false),
                    Times.Once);


            this.meshProcessingServiceMock.Verify(processings =>
                  processings.SendMessageAsync(message),
                    Times.Once);


            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.csvMapperProcessingServiceMock.VerifyNoOtherCalls();
            this.meshProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}

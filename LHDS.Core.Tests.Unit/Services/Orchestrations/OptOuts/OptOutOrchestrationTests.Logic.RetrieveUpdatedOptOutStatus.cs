// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.OptOuts
{
    public partial class OptOutOrchestrationTests
    {
        [Fact]
        public async Task ShouldRetrieveUpdatedMeshOptOutStatusChangesAsync()
        {
            List<string> outputMessageIds = GetRandomMessages(GetRandomNumber());

            // Given
            this.meshProcessingServiceMock.Setup(Processings =>
                Processings.RetrieveMessageIdsFromInboxAsync())
                    .ReturnsAsync(outputMessageIds);

            //Loop over ouytputMessageIds to retrieve by ID to get full message (MeshService Ret and Ack)

            // put all returned items as opt in list
            // 
            // work out delta for opt out by pulling original file and doing a diff on the list
            //
            // Combine opt out and opt in list
            // Update Cache for opt in and opt out list
            // Add Document and put in out folder


            // When
            await this.optOutOrchestrationService.RetrieveUpdatedMeshOptOutStatusChangesAsync();

            // Then
            this.meshProcessingServiceMock.Verify(Processings =>
                Processings.RetrieveMessageIdsFromInboxAsync(),
                    Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.csvMapperProcessingServiceMock.VerifyNoOtherCalls();
            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}

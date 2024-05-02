// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.OptOuts
{
    public partial class OptOutOrchestrationTests
    {
        [Fact]
        public async Task ShouldValidateMailboxAccessAsync()
        {
            // given
            bool storageResult = true;
            bool expectedResult = storageResult;

            meshProcessingServiceMock.Setup(processings =>
               processings.ValidateMailboxAccessAsync())
                   .ReturnsAsync(storageResult);

            // when
            bool actualResult = await this.optOutOrchestrationService.ValidateMailboxAccessAsync();

            //then
            actualResult.Should().Be(expectedResult);

            meshProcessingServiceMock.Verify(Processings =>
                Processings.ValidateMailboxAccessAsync(),
                    Times.Once);

            meshProcessingServiceMock.VerifyNoOtherCalls();
            csvHelperBrokerMock.VerifyNoOtherCalls();
            optOutProcessingServiceMock.VerifyNoOtherCalls();
            documentProcessingServiceMock.VerifyNoOtherCalls();
            dateTimeBrokerMock.VerifyNoOtherCalls();
            identifierBrokerMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
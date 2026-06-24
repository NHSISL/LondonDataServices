// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading;
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
               processings.ValidateMailboxAccessAsync(It.IsAny<CancellationToken>()))
                   .ReturnsAsync(storageResult);

            // when
            bool actualResult = await this.optOutOrchestrationService
                .ValidateMailboxAccessAsync(TestContext.Current.CancellationToken);

            //then
            actualResult.Should().Be(expectedResult);

            meshProcessingServiceMock.Verify(Processings =>
                Processings.ValidateMailboxAccessAsync(It.IsAny<CancellationToken>()),
                    Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
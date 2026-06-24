// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Pds
{
    public partial class PdsOrchestrationTests
    {
        [Fact]
        public async Task ShouldValidateMailboxAccessAsync()
        {
            // given
            bool storageResult = true;
            bool expectedResult = storageResult;

            meshServiceMock.Setup(processings =>
               processings.ValidateMailboxAccessAsync(It.IsAny<CancellationToken>()))
                   .ReturnsAsync(storageResult);

            // when
            bool actualResult = await this.pdsOrchestrationService.ValidateMailboxAccessAsync(
                TestContext.Current.CancellationToken);

            //then
            actualResult.Should().Be(expectedResult);

            meshServiceMock.Verify(Processings =>
                Processings.ValidateMailboxAccessAsync(It.IsAny<CancellationToken>()),
                    Times.Once);

            meshServiceMock.VerifyNoOtherCalls();
            documentServiceMock.VerifyNoOtherCalls();
            dateTimeBrokerMock.VerifyNoOtherCalls();
            identifierBrokerMock.VerifyNoOtherCalls();
            pdsAuditServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

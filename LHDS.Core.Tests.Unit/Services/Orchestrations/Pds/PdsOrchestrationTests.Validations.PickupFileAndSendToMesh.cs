// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Models.Orchestrations.Pds.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Pds
{
    public partial class PdsOrchestrationTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnPickupFileAndSendIfArgsAreInvalidAndLogItAsync(
            string invalidText)
        {
            byte[] pdsFile = null;
            string fileName = invalidText;

            var invalidArgumentPdsException =
               new InvalidArgumentPdsException();

            invalidArgumentPdsException.AddData(
                key: "pdsFile",
                values: "Data is required");

            invalidArgumentPdsException.AddData(
                key: "fileName",
                values: "Text is required");

            var expectedPdsValidationException =
                new PdsValidationException(
                    innerException: invalidArgumentPdsException);

            // when
            ValueTask<PdsAudit> retrievePdsAuditTask =
                this.pdsOrchestrationService.PickupFileAndSendToMesh(pdsFile, fileName);

            PdsValidationException actualPdsValidationException =
                await Assert.ThrowsAsync<PdsValidationException>(retrievePdsAuditTask.AsTask);

            // then
            actualPdsValidationException.Should()
                .BeEquivalentTo(expectedPdsValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPdsValidationException))),
                        Times.Once);

            this.pdsAuditServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}

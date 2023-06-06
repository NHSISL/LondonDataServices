// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Models.Orchestrations.Pds.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Pds
{
    public partial class PdsOrchestrationTests
    {
        [Theory]
        [MemberData(nameof(PdsDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnPickupFileAndSendIfDependencyValidationOccursAndLogItAsync(
           Xeption dependancyValidationException)
        {
            var randomString = GetRandomString();
            var randomBytes = Encoding.ASCII.GetBytes(randomString);
            var randomRecieveFileName = GetRandomString();

            var expectedDependencyException =
                new PdsOrchestrationDependencyValidationException(
                    dependancyValidationException.InnerException as Xeption);

            this.dateTimeBrokerMock.Setup(broker =>
              broker.GetCurrentDateTimeOffset())
                    .Throws(dependancyValidationException);

            //when
            ValueTask<PdsAudit> actualPdsAudit =
                this.pdsOrchestrationService.PickupFileAndSendToMesh(randomBytes, randomRecieveFileName);

            PdsOrchestrationDependencyValidationException actualException =
              await Assert.ThrowsAsync<PdsOrchestrationDependencyValidationException>(
                  actualPdsAudit.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                   Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.pdsAuditServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(PdsDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnPickupFileAndSendIfDependencyExceptionOccursAndLogItAsync(
         Xeption dependancyException)
        {
            // given
            var randomString = GetRandomString();
            var randomBytes = Encoding.ASCII.GetBytes(randomString);
            var randomRecieveFileName = GetRandomString();

            var expectedDependencyException =
                new PdsOrchestrationDependencyException(
                    dependancyException.InnerException as Xeption);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTimeOffset())
                   .Throws(dependancyException);

            // when
            ValueTask<PdsAudit> retrievePdsAudit =
              this.pdsOrchestrationService.PickupFileAndSendToMesh(randomBytes, randomRecieveFileName);

            PdsOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<PdsOrchestrationDependencyException>(retrievePdsAudit.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                 broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                        Times.Once);

            this.pdsAuditServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}

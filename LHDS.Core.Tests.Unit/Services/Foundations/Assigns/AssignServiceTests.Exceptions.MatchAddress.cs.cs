// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AssignAddresses;
using LHDS.Core.Models.Foundations.Assigns.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Assigns
{
    public partial class AssignServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAsync()
        {
            // given
            string someAddress = GetRandomString();

            var serviceException = new Exception();

            var failedAssignAddressServiceException =
                new FailedAssignServiceException(
                    message: "Failed assign service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedAssignAddressServiveException =
                new AssignServiceException(
                    message: "Assign service error occurred, please contact support.",
                    innerException: failedAssignAddressServiceException);

            this.assignBrokerMock.Setup(service =>
                service.MatchAddressAsync(It.IsAny<string>()))
                    .Throws(serviceException);

            // when
            ValueTask<AssignAddress> addAssignAddressTask =
                this.assignService.MatchAddressAsync(someAddress);

            AssignServiceException actualException =
                await Assert.ThrowsAsync<AssignServiceException>(addAssignAddressTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAssignAddressServiveException);

            this.assignBrokerMock.Verify(service =>
                service.MatchAddressAsync(It.IsAny<string>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedAssignAddressServiveException))),
                         Times.Once);

            this.assignBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

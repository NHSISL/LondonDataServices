// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AssignAddresses;
using LHDS.Core.Models.Processings.Assigns.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Assigns
{
    public partial class AssignProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            string someAddress = GetRandomString();

            var expectedAssignAddressProcessingDependencyValidationException =
                new AssignProcessingDependencyValidationException(
                    message: "Assign processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.assignServiceMock.Setup(service =>
                service.MatchAddressAsync(It.IsAny<string>()))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<AssignAddress> matchTask =
                this.assignProcessingService.MatchAddressAsync(someAddress);

            AssignProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<AssignProcessingDependencyValidationException>(
                    matchTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAssignAddressProcessingDependencyValidationException);

            this.assignServiceMock.Verify(service =>
                service.MatchAddressAsync(It.IsAny<string>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedAssignAddressProcessingDependencyValidationException))),
                         Times.Once);

            this.assignServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnAddIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            string someAddress = GetRandomString();

            var expectedAssignAddressProcessingDependencyException =
                new AssignProcessingDependencyException(
                    message: "Assign processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.assignServiceMock.Setup(service =>
                service.MatchAddressAsync(It.IsAny<string>()))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<AssignAddress> matchTask =
                this.assignProcessingService.MatchAddressAsync(someAddress);

            AssignProcessingDependencyException actualException =
                await Assert.ThrowsAsync<AssignProcessingDependencyException>(matchTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAssignAddressProcessingDependencyException);

            this.assignServiceMock.Verify(service =>
                service.MatchAddressAsync(It.IsAny<string>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedAssignAddressProcessingDependencyException))),
                         Times.Once);

            this.assignServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAsync()
        {
            // given
            string someAddress = GetRandomString();

            var serviceException = new Exception();

            var failedAssignAddressProcessingServiceException =
                new FailedAssignProcessingServiceException(
                    message: "Failed assign processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedAssignAddressProcessingServiveException =
                new AssignProcessingServiceException(
                    message: "Assign processing service error occurred, please contact support.",
                    innerException: failedAssignAddressProcessingServiceException);

            this.assignServiceMock.Setup(service =>
                service.MatchAddressAsync(It.IsAny<string>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<AssignAddress> addAssignAddressTask =
                this.assignProcessingService.MatchAddressAsync(someAddress);

            AssignProcessingServiceException actualException =
                await Assert.ThrowsAsync<AssignProcessingServiceException>(addAssignAddressTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAssignAddressProcessingServiveException);

            this.assignServiceMock.Verify(service =>
                service.MatchAddressAsync(It.IsAny<string>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedAssignAddressProcessingServiveException))),
                         Times.Once);

            this.assignServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

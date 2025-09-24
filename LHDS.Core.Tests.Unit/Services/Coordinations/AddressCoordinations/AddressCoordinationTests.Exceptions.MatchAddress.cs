// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Brokers.Storages.StorageQueues;
using LHDS.Core.Models.Coordinations.AddressCoordinations.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.AddressCoordinations
{
    public partial class AddressCoordinationServiceTests
    {
        [Theory]
        [MemberData(nameof(AddressCoordinationDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnMatchAddressIfErrorOccursAndLogItAsync(
            Xeption dependancyValidationException)
        {
            // given
            Payload<Guid> somePayload = CreateRandomPayload();

            var expectedDependencyException =
                new AddressCoordinationDependencyValidationException(
                    message: "Address coordination dependency validation error occurred, please try again.",
                    innerException: dependancyValidationException.InnerException as Xeption);

            this.resolvedAddressOrchestrationServiceMock.Setup(service =>
                service.MatchAddressDataAsync(It.IsAny<Payload<Guid>>()))
                    .ThrowsAsync(dependancyValidationException);

            // when
            ValueTask matchAddressesTask =
                this.addressCoordinationService.MatchAddressDataAsync(payload: somePayload);

            AddressCoordinationDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressCoordinationDependencyValidationException>(matchAddressesTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.resolvedAddressOrchestrationServiceMock.Verify(service =>
                service.MatchAddressDataAsync(It.IsAny<Payload<Guid>>()),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(AddressCoordinationDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnMatchAddressIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            Payload<Guid> somePayload = CreateRandomPayload();

            var expectedDependencyException =
                new AddressCoordinationDependencyException(
                    message: "Address coordination dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.resolvedAddressOrchestrationServiceMock.Setup(service =>
                service.MatchAddressDataAsync(It.IsAny<Payload<Guid>>()))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask matchAddressesTask =
                this.addressCoordinationService.MatchAddressDataAsync(payload: somePayload);

            AddressCoordinationDependencyException actualException =
                await Assert.ThrowsAsync<AddressCoordinationDependencyException>(matchAddressesTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.resolvedAddressOrchestrationServiceMock.Verify(service =>
                service.MatchAddressDataAsync(It.IsAny<Payload<Guid>>()),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.addressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnMatchAddressIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Payload<Guid> somePayload = CreateRandomPayload();
            var serviceException = new Exception();

            var failedAddressCoordinationServiceException =
                new FailedAddressCoordinationServiceException(
                    message: "Failed address coordination service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedAddressCoordinationServiceException =
                new AddressCoordinationServiceException(
                    message: "Address coordination service error occurred, please contact support.",
                    innerException: failedAddressCoordinationServiceException);

            this.resolvedAddressOrchestrationServiceMock.Setup(service =>
                service.MatchAddressDataAsync(It.IsAny<Payload<Guid>>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask matchAddressesTask =
                this.addressCoordinationService.MatchAddressDataAsync(payload: somePayload);

            AddressCoordinationServiceException actualException =
                await Assert.ThrowsAsync<AddressCoordinationServiceException>(matchAddressesTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressCoordinationServiceException);

            this.resolvedAddressOrchestrationServiceMock.Verify(service =>
                service.MatchAddressDataAsync(It.IsAny<Payload<Guid>>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressCoordinationServiceException))),
                        Times.Once);

            this.addressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
        }
    }
}
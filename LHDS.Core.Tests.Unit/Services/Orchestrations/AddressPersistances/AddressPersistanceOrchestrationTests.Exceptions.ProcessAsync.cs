// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Orchestrations.AddressPersistances.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressPersistances
{
    public partial class AddressPersistanceOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(AddressPersistanceDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnAddressPersistanceIfDependencyValidationOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            List<Address> randomAddresses = CreateRandomAddresses(GetRandomNumber()).ToList();
            Address address = randomAddresses[0];

            var expectedDependencyException =
                new AddressPersistanceOrchestrationDependencyValidationException(
                    message: "Address persistance orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.addressProcessingServiceMock.Setup(service =>
              service.ModifyOrAddAddressAsync(address))
                  .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<List<Address>> processTask =
                this.addressPersistanceOrchestrationService.PersistAddressAsync(randomAddresses);

            AddressPersistanceOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressPersistanceOrchestrationDependencyValidationException>(
                    processTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.addressProcessingServiceMock.Verify(service =>
             service.ModifyOrAddAddressAsync(It.IsAny<Address>()),
                 Times.Never);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(AddressPersistanceDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnAddressPersistanceIfDependencyExceptionOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            List<Address> randomAddresses = CreateRandomAddresses(1).ToList();
            Address address = randomAddresses[0];

            var expectedDependencyException =
                new AddressPersistanceOrchestrationDependencyException(
                    message: "Address persistance orchestration dependency error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.addressProcessingServiceMock.Setup(service =>
                service.ModifyOrAddAddressAsync(address))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<List<Address>> processTask =
                this.addressPersistanceOrchestrationService.PersistAddressAsync(randomAddresses);

            AddressPersistanceOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<AddressPersistanceOrchestrationDependencyException>(
                    processTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.addressProcessingServiceMock.Verify(service =>
             service.ModifyOrAddAddressAsync(It.IsAny<Address>()),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddressPersistanceIfServiceErrorOccursAndLogItAsync()
        {
            //Given
            List<Address> randomAddresses = CreateRandomAddresses(1).ToList();
            Address address = randomAddresses[0];
            var serviceException = new Exception();

            var failedAddressPersistanceOrchestrationServiceException =
                new FailedAddressPersistanceOrchestrationServiceException(
                    message: "Failed address persistance orchestration service error occurred, please contact support",
                    innerException: serviceException);

            var expectedAddressPersistanceOrchestrationServiceException =
                new AddressPersistanceOrchestrationServiceException(
                    message: "Address persistance orchestration service error occurred, contact support.",
                    innerException: failedAddressPersistanceOrchestrationServiceException);

            this.addressProcessingServiceMock.Setup(processing =>
                processing.ModifyOrAddAddressAsync(address))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<List<Address>> processTask =
                this.addressPersistanceOrchestrationService.PersistAddressAsync(randomAddresses);

            AddressPersistanceOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<AddressPersistanceOrchestrationServiceException>(
                    processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressPersistanceOrchestrationServiceException);

            this.addressProcessingServiceMock.Verify(service =>
             service.ModifyOrAddAddressAsync(It.IsAny<Address>()),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedAddressPersistanceOrchestrationServiceException))),
                       Times.Once);

            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
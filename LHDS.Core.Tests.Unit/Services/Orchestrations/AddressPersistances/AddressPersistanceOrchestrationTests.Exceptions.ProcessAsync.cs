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
using LHDS.Core.Services.Orchestrations.AddressPersistances;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressPersistances
{
    public partial class AddressPersistanceOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(AddressPersistenceDependencyValidationExceptions))]
        public async Task ShouldThrowAggregateDependencyValidationOnAddressPersistenceIfDependencyValidationOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            List<Address> randomAddresses = CreateRandomAddresses(1).ToList();
            List<Exception> exceptions = new List<Exception>();

            foreach (Address address in randomAddresses)
            {
                this.addressProcessingServiceMock.Setup(processing =>
                    processing.ModifyOrAddAddressAsync(It.IsAny<Address>()))
                        .ThrowsAsync(dependencyValidationException);

                var addressPersistanceOrchestrationDependencyValidationException =
                    new AddressPersistenceOrchestrationDependencyValidationException(
                        message: "Address persistence orchestration dependency validation error occurred, " +
                            "please try again.",
                        innerException: dependencyValidationException.InnerException as Xeption);

                exceptions.Add(addressPersistanceOrchestrationDependencyValidationException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to add or modify {exceptions.Count} address(es)",
                    exceptions);

            var failedAddressPersistanceOrchestrationServiceException =
                 new FailedAddressPersistenceOrchestrationServiceException(
                     message: "Failed address persistence aggregate processing service error occurred, " +
                     "contact support.",
                     innerException: aggregateException);

            var expectedAddressPersistanceOrchestrationServiceException =
                new AddressPersistenceOrchestrationServiceException(
                    message: "Address persistence orchestration service error occurred, contact support.",
                    innerException: failedAddressPersistanceOrchestrationServiceException);

            // when
            ValueTask<List<Address>> processTask =
                this.addressPersistanceOrchestrationService.PersistAddressAsync(randomAddresses);

            AddressPersistenceOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<AddressPersistenceOrchestrationServiceException>(
                    processTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedAddressPersistanceOrchestrationServiceException);

            this.addressProcessingServiceMock.Verify(service =>
             service.ModifyOrAddAddressAsync(It.IsAny<Address>()),
                  Times.Exactly(randomAddresses.Count));

            var addressPersistanceDependencyValidationException =
                new AddressPersistenceOrchestrationDependencyValidationException(
                    message: "Address persistence orchestration dependency validation error occurred, " +
                    "please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     addressPersistanceDependencyValidationException))),
                         Times.Exactly(randomAddresses.Count));

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedAddressPersistanceOrchestrationServiceException))),
                       Times.Once);

            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        //[Theory]
        //[MemberData(nameof(AddressPersistanceDependencyExceptions))]
        //public async Task ShouldThrowDependencyExceptionOnAddressPersistanceIfDependencyExceptionOccursAndLogItAsync(
        //    Xeption dependencyException)
        //{
        //    // given
        //    List<Address> randomAddresses = CreateRandomAddresses(1).ToList();
        //    Address address = randomAddresses[0];

        //    var expectedDependencyException =
        //        new AddressPersistanceOrchestrationDependencyException(
        //            message: "Address persistance orchestration dependency error occurred, " +
        //                "fix the errors and try again.",
        //            innerException: dependencyException.InnerException as Xeption);

        //    this.addressProcessingServiceMock.Setup(service =>
        //        service.ModifyOrAddAddressAsync(address))
        //            .ThrowsAsync(dependencyException);

        //    // when
        //    ValueTask<List<Address>> processTask =
        //        this.addressPersistanceOrchestrationService.PersistAddressAsync(randomAddresses);

        //    AddressPersistanceOrchestrationDependencyException actualException =
        //        await Assert.ThrowsAsync<AddressPersistanceOrchestrationDependencyException>(
        //            processTask.AsTask);

        //    // then
        //    actualException.Should()
        //         .BeEquivalentTo(expectedDependencyException);

        //    this.addressProcessingServiceMock.Verify(service =>
        //     service.ModifyOrAddAddressAsync(It.IsAny<Address>()),
        //         Times.Once);

        //    this.loggingBrokerMock.Verify(broker =>
        //       broker.LogError(It.Is(SameExceptionAs(
        //           expectedDependencyException))),
        //               Times.Once);

        //    this.addressProcessingServiceMock.VerifyNoOtherCalls();
        //    this.loggingBrokerMock.VerifyNoOtherCalls();
        //}

        //[Fact]
        //public async Task ShouldThrowServiceExceptionOnAddressPersistanceIfServiceErrorOccursAndLogItAsync()
        //{
        //    //Given
        //    List<Address> randomAddresses = CreateRandomAddresses(1).ToList();
        //    Address address = randomAddresses[0];
        //    var serviceException = new Exception();

        //    var failedAddressPersistanceOrchestrationServiceException =
        //        new FailedAddressPersistanceOrchestrationServiceException(
        //            message: "Failed address persistance orchestration service error occurred, please contact support",
        //            innerException: serviceException);

        //    var expectedAddressPersistanceOrchestrationServiceException =
        //        new AddressPersistanceOrchestrationServiceException(
        //            message: "Address persistance orchestration service error occurred, contact support.",
        //            innerException: failedAddressPersistanceOrchestrationServiceException);

        //    this.addressProcessingServiceMock.Setup(processing =>
        //        processing.ModifyOrAddAddressAsync(address))
        //            .ThrowsAsync(serviceException);

        //    // when
        //    ValueTask<List<Address>> processTask =
        //        this.addressPersistanceOrchestrationService.PersistAddressAsync(randomAddresses);

        //    AddressPersistanceOrchestrationServiceException actualException =
        //        await Assert.ThrowsAsync<AddressPersistanceOrchestrationServiceException>(
        //            processTask.AsTask);

        //    // then
        //    actualException.Should().BeEquivalentTo(expectedAddressPersistanceOrchestrationServiceException);

        //    this.addressProcessingServiceMock.Verify(service =>
        //     service.ModifyOrAddAddressAsync(It.IsAny<Address>()),
        //         Times.Once);

        //    this.loggingBrokerMock.Verify(broker =>
        //       broker.LogError(It.Is(SameExceptionAs(
        //           expectedAddressPersistanceOrchestrationServiceException))),
        //               Times.Once);

        //    this.addressProcessingServiceMock.VerifyNoOtherCalls();
        //    this.loggingBrokerMock.VerifyNoOtherCalls();
        //}

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddressPersistIfServiceErrorOccursAsync()
        {
            // given
            var mock = new Mock<AddressPersistanceOrchestrationService>(
                addressProcessingServiceMock.Object,
                loggingBrokerMock.Object,
                dateTimeBrokerMock.Object)
            { CallBase = true };

            List<Address> randomAddresses = CreateRandomAddresses(1).ToList();
            Address address = randomAddresses[0];
            string exceptionMessage = GetRandomString();
            var serviceException = new Exception(exceptionMessage);

            mock.Setup(x => x.ValidateAddressListOrchestrationOnProcess(randomAddresses))
                .Throws(serviceException);

            AddressPersistanceOrchestrationService addressPersistanceOrchestrationService = mock.Object;

            var failedAddressPersistanceOrchestrationServiceException =
                new FailedAddressPersistenceOrchestrationServiceException(
                    message: "Failed address persistence orchestration service error occurred, contact support.",
                    innerException: serviceException);

            var expectedAddressPersistanceOrchestrationServiceException =
                new AddressPersistenceOrchestrationServiceException(
                    message: "Address persistence orchestration service error occurred, contact support.",
                    innerException: failedAddressPersistanceOrchestrationServiceException);

            // when
            ValueTask<List<Address>> processTask =
                addressPersistanceOrchestrationService.PersistAddressAsync(randomAddresses);

            AddressPersistenceOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<AddressPersistenceOrchestrationServiceException>(
                    processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressPersistanceOrchestrationServiceException);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedAddressPersistanceOrchestrationServiceException))),
                       Times.Once);

            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
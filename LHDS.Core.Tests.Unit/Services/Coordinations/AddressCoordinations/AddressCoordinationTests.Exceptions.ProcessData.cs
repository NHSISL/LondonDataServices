// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Coordinations.AddressCoordinations.Exceptions;
using LHDS.Core.Models.Foundations.Addresses;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.AddressCoordinations
{
    public partial class AddressCoordinationServiceTests
    {
        [Theory]
        [MemberData(nameof(AddressCoordinationDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnProcessDataIfDependencyValidationOccursAndLogItAsync(
            Xeption dependancyValidationException)
        {
            // given
            string someFilename = GetRandomString();
            byte[] randomData = Encoding.UTF8.GetBytes(GetRandomString());

            var expectedDependencyException =
                new AddressCoordinationDependencyValidationException(
                    message: "Address coordination dependency validation error occurred, please try again.",
                    innerException: dependancyValidationException.InnerException as Xeption);

            this.addressExtractionOrchestrationServiceMock.Setup(service =>
                service.BulkAddAddressesAsync(randomData, someFilename))
                    .ThrowsAsync(dependancyValidationException);

            // when
            ValueTask<List<Address>> processDataTask =
                this.addressCoordinationService.LoadAddressDataAsync(randomData, someFilename);

            AddressCoordinationDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressCoordinationDependencyValidationException>(processDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.addressExtractionOrchestrationServiceMock.Verify(service =>
             service.BulkAddAddressesAsync(randomData, someFilename),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.addressExtractionOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressPersistanceOrchestrationServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(AddressCoordinationDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnProcessDataIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            string someFilename = GetRandomString();
            byte[] randomData = Encoding.UTF8.GetBytes(GetRandomString());

            var expectedDependencyException =
                new AddressCoordinationDependencyException(
                    message: "Address coordination dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.addressExtractionOrchestrationServiceMock.Setup(service =>
                service.BulkAddAddressesAsync(randomData, someFilename))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<List<Address>> processDataTask =
                this.addressCoordinationService.LoadAddressDataAsync(randomData, someFilename);

            AddressCoordinationDependencyException actualException =
                await Assert.ThrowsAsync<AddressCoordinationDependencyException>(processDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.addressExtractionOrchestrationServiceMock.Verify(service =>
             service.BulkAddAddressesAsync(randomData, someFilename),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.addressExtractionOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressPersistanceOrchestrationServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnProcessIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string someFilename = GetRandomString();
            byte[] randomData = Encoding.UTF8.GetBytes(GetRandomString());
            var serviceException = new Exception();

            var failedAddressCoordinationServiceException =
                new FailedAddressCoordinationServiceException(
                    message: "Failed address coordination service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedAddressCoordinationServiceException =
                new AddressCoordinationServiceException(
                    message: "Address coordination service error occurred, please contact support.",
                    innerException: failedAddressCoordinationServiceException);

            this.addressExtractionOrchestrationServiceMock.Setup(service =>
                service.BulkAddAddressesAsync(randomData, someFilename))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<List<Address>> processDataTask = this.addressCoordinationService
                .LoadAddressDataAsync(randomData, someFilename);

            AddressCoordinationServiceException actualException =
                await Assert.ThrowsAsync<AddressCoordinationServiceException>(processDataTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressCoordinationServiceException);

            this.addressExtractionOrchestrationServiceMock.Verify(service =>
                service.BulkAddAddressesAsync(randomData, someFilename),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressCoordinationServiceException))),
                        Times.Once);

            this.addressExtractionOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressPersistanceOrchestrationServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
        }
    }
}
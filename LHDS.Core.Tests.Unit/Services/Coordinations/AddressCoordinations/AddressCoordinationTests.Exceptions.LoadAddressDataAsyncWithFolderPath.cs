// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
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
        public async Task ShouldThrowDependencyValidationOnLoadAddressDataIfDependencyValidationOccursAndLogItAsyncWithFolderPath(
            Xeption dependancyValidationException)
        {
            // given
            string someFolderPath = GetRandomString();

            var expectedDependencyException =
                new AddressCoordinationDependencyValidationException(
                    message: "Address coordination dependency validation error occurred, please try again.",
                    innerException: dependancyValidationException.InnerException as Xeption);

            this.addressOrchestrationServiceMock.Setup(service =>
                service.BulkAddAddressesAsync(someFolderPath))
                    .ThrowsAsync(dependancyValidationException);

            // when
            ValueTask loadAddressDataTask =
                this.addressCoordinationService.LoadAddressDataAsync(someFolderPath);

            AddressCoordinationDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressCoordinationDependencyValidationException>(loadAddressDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.addressOrchestrationServiceMock.Verify(service =>
             service.BulkAddAddressesAsync(someFolderPath),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.addressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(AddressCoordinationDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnLoadAddressDataIfDependencyErrorOccursAndLogItAsyncWithFolderPath(
            Xeption dependencyException)
        {
            // given
            string someFolderPath = GetRandomString();

            var expectedDependencyException =
                new AddressCoordinationDependencyException(
                    message: "Address coordination dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.addressOrchestrationServiceMock.Setup(service =>
                service.BulkAddAddressesAsync(someFolderPath))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask loadAddressDataTask =
                this.addressCoordinationService.LoadAddressDataAsync(someFolderPath);

            AddressCoordinationDependencyException actualException =
                await Assert.ThrowsAsync<AddressCoordinationDependencyException>(loadAddressDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.addressOrchestrationServiceMock.Verify(service =>
             service.BulkAddAddressesAsync(someFolderPath),
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
        public async Task ShouldThrowServiceExceptionOnLoadAddressDataIfServiceErrorOccursAndLogItAsyncWithFolderPath()
        {
            // given
            string someFolderPath = GetRandomString();
            var serviceException = new Exception();

            var failedAddressCoordinationServiceException =
                new FailedAddressCoordinationServiceException(
                    message: "Failed address coordination service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedAddressCoordinationServiceException =
                new AddressCoordinationServiceException(
                    message: "Address coordination service error occurred, please contact support.",
                    innerException: failedAddressCoordinationServiceException);

            this.addressOrchestrationServiceMock.Setup(service =>
                service.BulkAddAddressesAsync(someFolderPath))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask loadAddressDataTask = this.addressCoordinationService
                .LoadAddressDataAsync(someFolderPath);

            AddressCoordinationServiceException actualException =
                await Assert.ThrowsAsync<AddressCoordinationServiceException>(loadAddressDataTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressCoordinationServiceException);

            this.addressOrchestrationServiceMock.Verify(service =>
                service.BulkAddAddressesAsync(someFolderPath),
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
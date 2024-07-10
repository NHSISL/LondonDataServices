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
        public async Task ShouldThrowDependencyValidationOnUploadIfDependencyValidationOccursAndLogItAsync(
            Xeption dependancyValidationException)
        {
            // given
            var expectedDependencyException =
                new AddressCoordinationDependencyValidationException(
                    message: "Address coordination dependency validation error occurred, please try again.",
                    innerException: dependancyValidationException.InnerException as Xeption);

            this.resolvedAddressOrchestrationServiceMock.Setup(service =>
                service.ExportResolvedAddressesAsync())
                    .ThrowsAsync(dependancyValidationException);

            // when
            ValueTask<Guid?> uploadAddressesTask =
                this.addressCoordinationService.ExportResolvedAddressesAsync();

            AddressCoordinationDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressCoordinationDependencyValidationException>(uploadAddressesTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.resolvedAddressOrchestrationServiceMock.Verify(service =>
                service.ExportResolvedAddressesAsync(),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(AddressCoordinationDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnUploadAddressesDataIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var expectedDependencyException =
                new AddressCoordinationDependencyException(
                    message: "Address coordination dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.resolvedAddressOrchestrationServiceMock.Setup(service =>
                service.ExportResolvedAddressesAsync())
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<Guid?> uploadAddressesTask =
                this.addressCoordinationService.ExportResolvedAddressesAsync();

            AddressCoordinationDependencyException actualException =
                await Assert.ThrowsAsync<AddressCoordinationDependencyException>(uploadAddressesTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.resolvedAddressOrchestrationServiceMock.Verify(service =>
                service.ExportResolvedAddressesAsync(),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnUploadIfServiceErrorOccursAndLogItAsync()
        {
            // given
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
                service.ExportResolvedAddressesAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Guid?> uploadAddressesTask =
                this.addressCoordinationService.ExportResolvedAddressesAsync();

            AddressCoordinationServiceException actualException =
                await Assert.ThrowsAsync<AddressCoordinationServiceException>(uploadAddressesTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressCoordinationServiceException);

            this.resolvedAddressOrchestrationServiceMock.Verify(service =>
                service.ExportResolvedAddressesAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressCoordinationServiceException))),
                        Times.Once);

            this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressOrchestrationServiceMock.VerifyNoOtherCalls();
        }
    }
}
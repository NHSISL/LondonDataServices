// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Text;
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
        public async Task ShouldThrowDependencyValidationOnLoadAddressDataIfDependencyValidationOccursAndLogItAsync(
            Xeption dependancyValidationException)
        {
            // given
            string someFilename = GetRandomString();
            byte[] randomData = Encoding.UTF8.GetBytes(GetRandomString());
            Stream someStream = new MemoryStream(randomData);

            var expectedDependencyException =
                new AddressCoordinationDependencyValidationException(
                    message: "Address coordination dependency validation error occurred, please try again.",
                    innerException: dependancyValidationException.InnerException as Xeption);

            this.addressOrchestrationServiceMock.Setup(service =>
                service.BulkAddAddressesAsync(someStream, someFilename))
                    .ThrowsAsync(dependancyValidationException);

            // when
            ValueTask loadAddressDataTask =
                this.addressCoordinationService.LoadAddressDataAsync(someStream, someFilename);

            AddressCoordinationDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressCoordinationDependencyValidationException>(loadAddressDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.addressOrchestrationServiceMock.Verify(service =>
             service.BulkAddAddressesAsync(someStream, someFilename),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.addressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(AddressCoordinationDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnLoadAddressDataIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            string someFilename = GetRandomString();
            byte[] randomData = Encoding.UTF8.GetBytes(GetRandomString());
            Stream someStream = new MemoryStream(randomData);

            var expectedDependencyException =
                new AddressCoordinationDependencyException(
                    message: "Address coordination dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.addressOrchestrationServiceMock.Setup(service =>
                service.BulkAddAddressesAsync(someStream, someFilename))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask loadAddressDataTask =
                this.addressCoordinationService.LoadAddressDataAsync(someStream, someFilename);

            AddressCoordinationDependencyException actualException =
                await Assert.ThrowsAsync<AddressCoordinationDependencyException>(loadAddressDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.addressOrchestrationServiceMock.Verify(service =>
             service.BulkAddAddressesAsync(someStream, someFilename),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.addressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnLoadAddressDataIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string someFilename = GetRandomString();
            byte[] randomData = Encoding.UTF8.GetBytes(GetRandomString());
            Stream someStream = new MemoryStream(randomData);
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
                service.BulkAddAddressesAsync(someStream, someFilename))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask loadAddressDataTask = this.addressCoordinationService
                .LoadAddressDataAsync(someStream, someFilename);

            AddressCoordinationServiceException actualException =
                await Assert.ThrowsAsync<AddressCoordinationServiceException>(loadAddressDataTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressCoordinationServiceException);

            this.addressOrchestrationServiceMock.Verify(service =>
                service.BulkAddAddressesAsync(someStream, someFilename),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressCoordinationServiceException))),
                        Times.Once);

            this.addressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
        }
    }
}
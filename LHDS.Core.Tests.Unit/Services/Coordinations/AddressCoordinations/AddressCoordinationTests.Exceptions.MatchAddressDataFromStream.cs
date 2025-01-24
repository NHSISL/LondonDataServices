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
        public async Task ShouldThrowDependencyValidationOnMatchAddressDataFromStreamIfErrorOccursAndLogItAsync(
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

            this.resolvedAddressOrchestrationServiceMock.Setup(service =>
                service.UploadAddressesToResolveAsync(It.IsAny<Stream>(), It.IsAny<string>()))
                    .ThrowsAsync(dependancyValidationException);

            // when
            ValueTask matchAddressDataTask = this.addressCoordinationService
                .LoadAddressesToResolveAsync(someStream, someFilename);

            AddressCoordinationDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressCoordinationDependencyValidationException>(matchAddressDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.resolvedAddressOrchestrationServiceMock.Verify(service =>
             service.UploadAddressesToResolveAsync(It.IsAny<Stream>(), It.IsAny<string>()),
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
        public async Task ShouldThrowDependencyExceptionOnMatchAddressDataFromStreamIfErrorOccursAndLogItAsync(
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

            this.resolvedAddressOrchestrationServiceMock.Setup(service =>
                service.UploadAddressesToResolveAsync(It.IsAny<Stream>(), It.IsAny<string>()))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask matchAddressDataTask = this.addressCoordinationService
                .LoadAddressesToResolveAsync(someStream, someFilename);

            AddressCoordinationDependencyException actualException =
                await Assert.ThrowsAsync<AddressCoordinationDependencyException>(matchAddressDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.resolvedAddressOrchestrationServiceMock.Verify(service =>
             service.UploadAddressesToResolveAsync(It.IsAny<Stream>(), It.IsAny<string>()),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnMatchAddressDataFromStreamIfServiceErrorOccursAndLogItAsync()
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

            this.resolvedAddressOrchestrationServiceMock.Setup(service =>
                service.UploadAddressesToResolveAsync(It.IsAny<Stream>(), It.IsAny<string>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask matchAddressDataTask = this.addressCoordinationService
                .LoadAddressesToResolveAsync(someStream, someFilename);

            AddressCoordinationServiceException actualException =
                await Assert.ThrowsAsync<AddressCoordinationServiceException>(matchAddressDataTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressCoordinationServiceException);

            this.resolvedAddressOrchestrationServiceMock.Verify(service =>
                service.UploadAddressesToResolveAsync(It.IsAny<Stream>(), It.IsAny<string>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressCoordinationServiceException))),
                        Times.Once);

            this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressOrchestrationServiceMock.VerifyNoOtherCalls();
        }
    }
}
// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.Addresses.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Addresses
{
    public partial class AddressOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(AddressOrchestrationDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnAddressIfDependencyValidationOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            string someFilename = GetRandomString();
            byte[] inputData = Encoding.UTF8.GetBytes(someFilename);
            Stream inputStream = new MemoryStream(inputData);

            var expectedDependencyException =
                new AddressOrchestrationDependencyValidationException(
                    message: "Address orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.fileBrokerMock.Setup(service =>
                service.GetTempPath())
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask processDataTask = this.addressOrchestrationService
                .BulkAddAddressesAsync(input: inputStream, fileName: someFilename);

            AddressOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressOrchestrationDependencyValidationException>(
                    processDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.fileBrokerMock.Verify(service =>
                service.GetTempPath(),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.assignProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(AddressDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnAddressIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            string someFilename = GetRandomString();
            byte[] inputData = Encoding.UTF8.GetBytes(someFilename);
            Stream inputStream = new MemoryStream(inputData);

            var expectedDependencyException =
                new AddressOrchestrationDependencyException(
                    message: "Address orchestration dependency error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.fileBrokerMock.Setup(service =>
                service.GetTempPath())
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask processDataTask = this.addressOrchestrationService
                .BulkAddAddressesAsync(input: inputStream, fileName: someFilename);

            AddressOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<AddressOrchestrationDependencyException>(
                    processDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.fileBrokerMock.Verify(service =>
                service.GetTempPath(),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.assignProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddressIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string someFilename = GetRandomString();
            byte[] inputData = Encoding.UTF8.GetBytes(someFilename);
            Stream inputStream = new MemoryStream(inputData);
            var serviceException = new Exception();

            var failedAddressPersistanceOrchestrationServiceException =
                new FailedAddressOrchestrationServiceException(
                    message: "Failed address orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedAddressOrchestrationServiceException =
                new AddressOrchestrationServiceException(
                    message: "Address orchestration service error occurred, please contact support.",
                    innerException: failedAddressPersistanceOrchestrationServiceException);

            this.fileBrokerMock.Setup(service =>
                service.GetTempPath())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask processDataTask = this.addressOrchestrationService
                .BulkAddAddressesAsync(input: inputStream, fileName: someFilename);

            AddressOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<AddressOrchestrationServiceException>(
                    processDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedAddressOrchestrationServiceException);

            this.fileBrokerMock.Verify(service =>
                service.GetTempPath(),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedAddressOrchestrationServiceException))),
                       Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.assignProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}


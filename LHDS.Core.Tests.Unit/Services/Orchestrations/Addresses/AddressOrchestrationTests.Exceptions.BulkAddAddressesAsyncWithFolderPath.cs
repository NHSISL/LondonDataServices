// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.Addresses.Exceptions;
using LHDS.Core.Services.Orchestrations.Addresses;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Addresses
{
    public partial class AddressOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(AddressOrchestrationDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnAddressIfDependencyValidationOccursAndLogItAsyncWithFolderPath(
            Xeption dependencyValidationException)
        {
            // given
            var addressOrchestrationServiceMock = new Mock<AddressOrchestrationService>
               (this.addressProcessingServiceMock.Object,
                this.assignProcessingServiceMock.Object,
                this.fileBrokerMock.Object,
                this.csvHelperBrokerMock.Object,
                this.dateTimeBrokerMock.Object,
                this.auditBrokerMock.Object,
                this.loggingBrokerMock.Object,
                this.identifierBrokerMock.Object)
            { CallBase = true };

            string someFolderPath = GetRandomString();
                        var expectedDependencyException =
                new AddressOrchestrationDependencyValidationException(
                    message: "Address orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            addressOrchestrationServiceMock.Setup(service =>
                service.ReadCsvDataAndBulkAddAddressesAsync(someFolderPath))
                    .ThrowsAsync(dependencyValidationException);

            AddressOrchestrationService service = addressOrchestrationServiceMock.Object;

            // when
            ValueTask processDataTask = service.BulkAddAddressesAsync(someFolderPath);

            AddressOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressOrchestrationDependencyValidationException>(
                    processDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            addressOrchestrationServiceMock.Verify(service =>
                service.ReadCsvDataAndBulkAddAddressesAsync(someFolderPath),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
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
        public async Task ShouldThrowDependencyExceptionOnAddressIfDependencyErrorOccursAndLogItAsyncWithFolderPath(
            Xeption dependencyException)
        {
            // given
            var addressOrchestrationServiceMock = new Mock<AddressOrchestrationService>
               (this.addressProcessingServiceMock.Object,
                this.assignProcessingServiceMock.Object,
                this.fileBrokerMock.Object,
                this.csvHelperBrokerMock.Object,
                this.dateTimeBrokerMock.Object,
                this.auditBrokerMock.Object,
                this.loggingBrokerMock.Object,
                this.identifierBrokerMock.Object)
            { CallBase = true };

            string someFolderPath = GetRandomString();
                        var expectedDependencyException =
                new AddressOrchestrationDependencyException(
                    message: "Address orchestration dependency error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyException.InnerException as Xeption);

            addressOrchestrationServiceMock.Setup(service =>
               service.ReadCsvDataAndBulkAddAddressesAsync(someFolderPath))
                   .ThrowsAsync(dependencyException);

            AddressOrchestrationService service = addressOrchestrationServiceMock.Object;

            // when
            ValueTask processDataTask = service.BulkAddAddressesAsync(someFolderPath);

            AddressOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<AddressOrchestrationDependencyException>(
                    processDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            addressOrchestrationServiceMock.Verify(service =>
                service.ReadCsvDataAndBulkAddAddressesAsync(someFolderPath),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
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
        public async Task ShouldThrowServiceExceptionOnAddressIfServiceErrorOccursAndLogItAsyncWithFolderPath()
        {
            // given
            var addressOrchestrationServiceMock = new Mock<AddressOrchestrationService>
               (this.addressProcessingServiceMock.Object,
                this.assignProcessingServiceMock.Object,
                this.fileBrokerMock.Object,
                this.csvHelperBrokerMock.Object,
                this.dateTimeBrokerMock.Object,
                this.auditBrokerMock.Object,
                this.loggingBrokerMock.Object,
                this.identifierBrokerMock.Object)
            { CallBase = true };

            string someFolderPath = GetRandomString();
                        var serviceException = new Exception();

            var failedAddressPersistanceOrchestrationServiceException =
                new FailedAddressOrchestrationServiceException(
                    message: "Failed address orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedAddressOrchestrationServiceException =
                new AddressOrchestrationServiceException(
                    message: "Address orchestration service error occurred, please contact support.",
                    innerException: failedAddressPersistanceOrchestrationServiceException);

            addressOrchestrationServiceMock.Setup(service =>
                service.ReadCsvDataAndBulkAddAddressesAsync(someFolderPath))
                    .ThrowsAsync(serviceException);

            AddressOrchestrationService service = addressOrchestrationServiceMock.Object;

            // when
            ValueTask processDataTask = service.BulkAddAddressesAsync(someFolderPath);

            AddressOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<AddressOrchestrationServiceException>(
                    processDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedAddressOrchestrationServiceException);

            addressOrchestrationServiceMock.Verify(service =>
                service.ReadCsvDataAndBulkAddAddressesAsync(someFolderPath),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
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

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddressIfAggregateExceptionOccursAndLogItAsyncWithFolderPath()
        {
            // given
            var addressOrchestrationServiceMock = new Mock<AddressOrchestrationService>
               (this.addressProcessingServiceMock.Object,
                this.assignProcessingServiceMock.Object,
                this.fileBrokerMock.Object,
                this.csvHelperBrokerMock.Object,
                this.dateTimeBrokerMock.Object,
                this.auditBrokerMock.Object,
                this.loggingBrokerMock.Object,
                this.identifierBrokerMock.Object)
            { CallBase = true };

            string someFolderPath = GetRandomString();
                        var aggregateException = new AggregateException();

            var failedAddressPersistanceOrchestrationServiceException =
                new FailedAddressOrchestrationServiceException(
                    message: "Failed address aggregate orchestration service error occurred, please contact support.",
                    innerException: aggregateException);

            var expectedAddressOrchestrationServiceException =
                new AddressOrchestrationServiceException(
                    message: "Address orchestration service error occurred, please contact support.",
                    innerException: failedAddressPersistanceOrchestrationServiceException);

            addressOrchestrationServiceMock.Setup(service =>
                service.ReadCsvDataAndBulkAddAddressesAsync(someFolderPath))
                    .ThrowsAsync(aggregateException);

            AddressOrchestrationService service = addressOrchestrationServiceMock.Object;

            // when
            ValueTask processDataTask = service.BulkAddAddressesAsync(someFolderPath);

            AddressOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<AddressOrchestrationServiceException>(
                    processDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedAddressOrchestrationServiceException);

            addressOrchestrationServiceMock.Verify(service =>
               service.ReadCsvDataAndBulkAddAddressesAsync(someFolderPath),
                   Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
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


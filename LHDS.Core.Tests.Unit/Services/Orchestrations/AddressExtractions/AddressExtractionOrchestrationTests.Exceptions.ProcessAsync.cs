// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressExtractionAudits;
using LHDS.Core.Models.Orchestrations.AddressExtractions.Exceptions;
using LHDS.Core.Models.Orchestrations.AddressPersistances.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressExtractions
{
    public partial class AddressExctractioneOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(AddressExtractionOrchestrationDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnAddressExtractionIfDependencyValidationOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            string inputFilePath = @"c:\temp\TestNestedZip.zip";
            byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);

            var expectedDependencyException =
                new AddressExtractionOrchestrationDependencyValidationException(
                    message: "Address extraction orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.addressParserServiceMock.Setup(service =>
                service.ProcessCsvAsync(It.IsAny<byte[]>()))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<List<Address>> processDataTask =
                this.addressExtractionOrchestrationService.ProcessDataAsync(inputData);

            AddressExtractionOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressExtractionOrchestrationDependencyValidationException>(
                    processDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.addressParserServiceMock.Verify(service =>
             service.ProcessCsvAsync(It.IsAny<byte[]>()),
                 Times.Once);

            this.addressExtractionAuditServiceMock.Verify(service =>
             service.AddAddressExtractionAuditAsync(It.IsAny<AddressExtractionAudit>()),
                 Times.Never);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.addressParserServiceMock.VerifyNoOtherCalls();
            this.addressExtractionAuditServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(AddressExtractionDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnAddressExtractionIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            string inputFilePath = @"c:\temp\TestNestedZip.zip";
            byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);

            var expectedDependencyException =
                new AddressExtractionOrchestrationDependencyException(
                    message: "Address extraction orchestration dependency error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.addressParserServiceMock.Setup(service =>
                service.ProcessCsvAsync(It.IsAny<byte[]>()))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<List<Address>> processDataTask =
                this.addressExtractionOrchestrationService.ProcessDataAsync(inputData);

            AddressExtractionOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<AddressExtractionOrchestrationDependencyException>(
                    processDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.addressParserServiceMock.Verify(service =>
             service.ProcessCsvAsync(It.IsAny<byte[]>()),
                 Times.Once);

            this.addressExtractionAuditServiceMock.Verify(service =>
             service.AddAddressExtractionAuditAsync(It.IsAny<AddressExtractionAudit>()),
                 Times.Never);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.addressParserServiceMock.VerifyNoOtherCalls();
            this.addressExtractionAuditServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        public async Task ShouldThrowServiceExceptionOnAddressExtractionIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string inputFilePath = @"c:\temp\TestNestedZip.zip";
            byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);
            var serviceException = new Exception();

            var failedAddressPersistanceOrchestrationServiceException =
                new FailedAddressExtractionOrchestrationServiceException(
                    message: "Failed address extraction orchestration service occurred, please contact support",
                    innerException: serviceException);

            var expectedAddressExtractionOrchestrationServiceException =
                new AddressExtractionOrchestrationServiceException(
                    message: "Address extraction orchestration service error occurred, contact support.",
                    innerException: failedAddressPersistanceOrchestrationServiceException);

            this.addressParserServiceMock.Setup(service =>
                service.ProcessCsvAsync(It.IsAny<byte[]>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<List<Address>> processDataTask =
                this.addressExtractionOrchestrationService.ProcessDataAsync(inputData);

            AddressExtractionOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<AddressExtractionOrchestrationServiceException>(
                    processDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedAddressExtractionOrchestrationServiceException);

            this.addressParserServiceMock.Verify(service =>
             service.ProcessCsvAsync(It.IsAny<byte[]>()),
                 Times.Once);

            this.addressExtractionAuditServiceMock.Verify(service =>
             service.AddAddressExtractionAuditAsync(It.IsAny<AddressExtractionAudit>()),
                 Times.Never);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedAddressExtractionOrchestrationServiceException))),
                       Times.Once);

            this.addressParserServiceMock.VerifyNoOtherCalls();
            this.addressExtractionAuditServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
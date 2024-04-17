// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressExtractionAudits;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Orchestrations.AddressExtractions.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressExtractions
{
    public partial class AddressExctractionOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(AddressExtractionOrchestrationDependencyValidationExceptions))]
        public async Task
            ShouldThrowDependencyValidationOnResolvedAddressExtractionIfDependencyValidationOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            string assembly = Assembly.GetExecutingAssembly().Location;

            string inputFilePath = Path.Combine(Path.GetDirectoryName(assembly),
                @"Resources/Services/Orchestrations/AddressExtractions/ShouldProcessZipFileWithOnlyCsvAddressesData.zip");

            byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);

            var expectedDependencyException =
                new AddressExtractionOrchestrationDependencyValidationException(
                    message: "Address extraction orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.resolvedAddressParserServiceMock.Setup(service =>
                service.ProcessCsvAsync(It.IsAny<byte[]>()))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<List<ResolvedAddress>> processDataTask =
                this.addressExtractionOrchestrationService.ProcessResolvedAddressesAsync(inputData);

            AddressExtractionOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressExtractionOrchestrationDependencyValidationException>(
                    processDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.resolvedAddressParserServiceMock.Verify(service =>
             service.ProcessCsvAsync(It.IsAny<byte[]>()),
                 Times.Once);

            this.addressExtractionAuditServiceMock.Verify(service =>
             service.AddAddressExtractionAuditAsync(It.IsAny<AddressExtractionAudit>()),
                 Times.Never);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.resolvedAddressParserServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressNormalisationServiceMock.VerifyNoOtherCalls();
            this.addressParserServiceMock.VerifyNoOtherCalls();
            this.addressExtractionAuditServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(AddressExtractionDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnResolvedAddressExtractionIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            string assembly = Assembly.GetExecutingAssembly().Location;

            string inputFilePath = Path.Combine(Path.GetDirectoryName(assembly),
                @"Resources/Services/Orchestrations/AddressExtractions/ShouldProcessZipFileWithOnlyCsvAddressesData.zip");

            byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);

            var expectedDependencyException =
                new AddressExtractionOrchestrationDependencyException(
                    message: "Address extraction orchestration dependency error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.resolvedAddressParserServiceMock.Setup(service =>
                service.ProcessCsvAsync(It.IsAny<byte[]>()))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<List<ResolvedAddress>> processDataTask =
                this.addressExtractionOrchestrationService.ProcessResolvedAddressesAsync(inputData);

            AddressExtractionOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<AddressExtractionOrchestrationDependencyException>(
                    processDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.resolvedAddressParserServiceMock.Verify(service =>
             service.ProcessCsvAsync(It.IsAny<byte[]>()),
                 Times.Once);

            this.addressExtractionAuditServiceMock.Verify(service =>
             service.AddAddressExtractionAuditAsync(It.IsAny<AddressExtractionAudit>()),
                 Times.Never);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.resolvedAddressParserServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressNormalisationServiceMock.VerifyNoOtherCalls();
            this.addressParserServiceMock.VerifyNoOtherCalls();
            this.addressExtractionAuditServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnResolvedAddressExtractionIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string assembly = Assembly.GetExecutingAssembly().Location;

            string inputFilePath = Path.Combine(Path.GetDirectoryName(assembly),
                @"Resources/Services/Orchestrations/AddressExtractions/ShouldProcessZipFileWithOnlyCsvAddressesData.zip");

            byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);
            var serviceException = new Exception();

            var failedAddressPersistanceOrchestrationServiceException =
                new FailedAddressExtractionOrchestrationServiceException(
                    message: "Failed address extraction orchestration service error occurred, please contact support",
                    innerException: serviceException);

            var expectedAddressExtractionOrchestrationServiceException =
                new AddressExtractionOrchestrationServiceException(
                    message: "Address extraction orchestration service error occurred, contact support.",
                    innerException: failedAddressPersistanceOrchestrationServiceException);

            this.resolvedAddressParserServiceMock.Setup(service =>
                service.ProcessCsvAsync(It.IsAny<byte[]>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<List<ResolvedAddress>> processDataTask =
                this.addressExtractionOrchestrationService.ProcessResolvedAddressesAsync(inputData);

            AddressExtractionOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<AddressExtractionOrchestrationServiceException>(
                    processDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedAddressExtractionOrchestrationServiceException);

            this.resolvedAddressParserServiceMock.Verify(service =>
             service.ProcessCsvAsync(It.IsAny<byte[]>()),
                 Times.Once);

            this.addressExtractionAuditServiceMock.Verify(service =>
             service.AddAddressExtractionAuditAsync(It.IsAny<AddressExtractionAudit>()),
                 Times.Never);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedAddressExtractionOrchestrationServiceException))),
                       Times.Once);

            this.resolvedAddressParserServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressNormalisationServiceMock.VerifyNoOtherCalls();
            this.addressParserServiceMock.VerifyNoOtherCalls();
            this.addressExtractionAuditServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
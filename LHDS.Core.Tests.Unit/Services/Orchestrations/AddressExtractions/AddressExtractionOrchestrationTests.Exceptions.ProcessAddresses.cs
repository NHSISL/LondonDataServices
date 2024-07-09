// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Orchestrations.AddressExtractions.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressExtractions
{
    public partial class AddressExtractionOrchestrationServiceTests
    {
        [Theory(Skip = "No longer needed will refacor out")]
        [MemberData(nameof(AddressExtractionOrchestrationDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnAddressExtractionIfDependencyValidationOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            string someFilename = GetRandomString();
            string assembly = Assembly.GetExecutingAssembly().Location;

            string inputFilePath = Path.Combine(Path.GetDirectoryName(assembly),
                @"Resources/Services/Orchestrations/AddressExtractions/ShouldProcessZipFileWithOnlyCsvAddressesData.zip");

            byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);

            var expectedDependencyException =
                new AddressExtractionOrchestrationDependencyValidationException(
                    message: "Address extraction orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.csvHelperBrokerMock.Setup(service =>
                service.MapCsvToObjectAsync<Address>(
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>()))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<List<Address>> processDataTask =
                this.addressExtractionOrchestrationService.BulkAddAddressesAsync(inputData, someFilename);

            AddressExtractionOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressExtractionOrchestrationDependencyValidationException>(
                    processDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.csvHelperBrokerMock.Verify(service =>
                service.MapCsvToObjectAsync<Address>(
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.addressNormalisationProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Theory(Skip = "No longer needed will refacor out")]
        [MemberData(nameof(AddressExtractionDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnAddressExtractionIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            string someFilename = GetRandomString();
            string assembly = Assembly.GetExecutingAssembly().Location;

            string inputFilePath = Path.Combine(Path.GetDirectoryName(assembly),
                @"Resources/Services/Orchestrations/AddressExtractions/ShouldProcessZipFileWithOnlyCsvAddressesData.zip");

            byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);

            var expectedDependencyException =
                new AddressExtractionOrchestrationDependencyException(
                    message: "Address extraction orchestration dependency error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.csvHelperBrokerMock.Setup(service =>
                service.MapCsvToObjectAsync<Address>(
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>()))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<List<Address>> processDataTask =
                this.addressExtractionOrchestrationService.BulkAddAddressesAsync(inputData, someFilename);

            AddressExtractionOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<AddressExtractionOrchestrationDependencyException>(
                    processDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.csvHelperBrokerMock.Verify(service =>
                service.MapCsvToObjectAsync<Address>(
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.addressNormalisationProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Fact(Skip = "no longer needed will refactor out")]
        public async Task ShouldThrowServiceExceptionOnAddressExtractionIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string someFilename = GetRandomString();
            string assembly = Assembly.GetExecutingAssembly().Location;

            string inputFilePath = Path.Combine(Path.GetDirectoryName(assembly),
                @"Resources/Services/Orchestrations/AddressExtractions/ShouldProcessZipFileWithOnlyCsvAddressesData.zip");

            byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);
            var serviceException = new Exception();

            var failedAddressPersistanceOrchestrationServiceException =
                new FailedAddressExtractionOrchestrationServiceException(
                    message: "Failed address extraction orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedAddressExtractionOrchestrationServiceException =
                new AddressExtractionOrchestrationServiceException(
                    message: "Address extraction orchestration service error occurred, please contact support.",
                    innerException: failedAddressPersistanceOrchestrationServiceException);

            this.csvHelperBrokerMock.Setup(service =>
                service.MapCsvToObjectAsync<Address>(
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<List<Address>> processDataTask =
                this.addressExtractionOrchestrationService.BulkAddAddressesAsync(inputData, someFilename);

            AddressExtractionOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<AddressExtractionOrchestrationServiceException>(
                    processDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedAddressExtractionOrchestrationServiceException);

            this.csvHelperBrokerMock.Verify(service =>
                service.MapCsvToObjectAsync<Address>(
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedAddressExtractionOrchestrationServiceException))),
                       Times.Once);

            this.addressNormalisationProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}
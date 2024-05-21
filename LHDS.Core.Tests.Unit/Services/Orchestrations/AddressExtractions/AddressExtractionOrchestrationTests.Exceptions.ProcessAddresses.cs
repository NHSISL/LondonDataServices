// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Extensions.Addresses;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Orchestrations.AddressExtractions.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressExtractions
{
    public partial class AddressExtractionOrchestrationServiceTests
    {
        [Theory(Skip = "WIP: Hassan to complete")]
        [MemberData(nameof(AddressExtractionOrchestrationDependencyValidationExceptions))]
        public async Task
            ShouldThrowAggregateDependencyValidationExceptionOnProcessAddressesIfErrorsInLoopAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // Given
            string someFilename = GetRandomString();
            byte[] randomData = Encoding.ASCII.GetBytes(GetRandomString());
            List<Address> randomAddresses = CreateRandomAddresses().ToList();
            List<Exception> exceptions = new List<Exception>();

            this.csvHelperBrokerMock.Setup(service =>
                service.MapCsvToObjectAsync<Address>(
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>()))
                    .ReturnsAsync(randomAddresses);

            foreach (Address address in randomAddresses)
            {
                string addressString = address.GetFormattedAddress();

                this.addressNormalisationProcessingServiceMock.Setup(service =>
                    service.GetNormalisedAddress(addressString))
                        .ThrowsAsync(dependencyValidationException);

                var addressExtractionOrchestrationDependencyValidationException =
                    new AddressExtractionOrchestrationDependencyValidationException(
                        message: "Address extraction orchestration dependency validation error occurred, " +
                        "please try again.",
                        innerException: dependencyValidationException.InnerException as Xeption);

                exceptions.Add(addressExtractionOrchestrationDependencyValidationException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to normalise address for {exceptions.Count} addresses",
                    exceptions);

            var failedAddressExtractionOrchestrationServiceException =
                new FailedAddressExtractionOrchestrationServiceException(
                    message: "Failed address extraction aggregate orchestration service error occurred, " +
                    "please contact support.",
                    innerException: aggregateException);

            var expectedAddressExtractionOrchestrationServiceException =
                new AddressExtractionOrchestrationServiceException(
                    message: "Address extraction orchestration service error occurred, please contact support.",
                    innerException: failedAddressExtractionOrchestrationServiceException);

            // When
            ValueTask<List<Address>> processAddressTask =
                this.addressExtractionOrchestrationService.ProcessAddressesAsync(randomData, someFilename);

            AddressExtractionOrchestrationServiceException actualAddressExtractionOrchestrationServiceException =
                await Assert.ThrowsAsync<AddressExtractionOrchestrationServiceException>(async () =>
                    await processAddressTask);

            // Then
            actualAddressExtractionOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedAddressExtractionOrchestrationServiceException);

            this.csvHelperBrokerMock.Verify(service =>
                service.MapCsvToObjectAsync<Address>(
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>()),
                        Times.Once);

            foreach (Address address in randomAddresses)
            {
                string addressString = address.GetFormattedAddress();

                this.addressNormalisationProcessingServiceMock.Verify(service =>
                    service.GetNormalisedAddress(addressString),
                        Times.Once);
            }

            var addressExtractionOrchestrationDependencyValidationLoggingException =
                new AddressExtractionOrchestrationDependencyValidationException(
                    message: "Address extraction orchestration dependency validation error occurred, " +
                    "fix the errors and try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    addressExtractionOrchestrationDependencyValidationLoggingException))),
                        Times.Exactly(randomAddresses.Count));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    actualAddressExtractionOrchestrationServiceException))),
                        Times.Once);

            this.addressNormalisationProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Theory(Skip = "WIP: Hassan to complete")]
        [MemberData(nameof(AddressExtractionDependencyExceptions))]
        public async Task
            ShouldThrowAggregateDependencyExceptionOnProcessAddressesIfErrorsInLoopAndLogItAsync(
            Xeption dependencyException)
        {
            // Given
            string someFilename = GetRandomString();
            byte[] randomData = Encoding.ASCII.GetBytes(GetRandomString());
            List<Address> randomAddresses = CreateRandomAddresses().ToList();
            List<Exception> exceptions = new List<Exception>();

            this.csvHelperBrokerMock.Setup(service =>
                service.MapCsvToObjectAsync<Address>(
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>()))
                    .ReturnsAsync(randomAddresses);

            foreach (Address address in randomAddresses)
            {
                string addressString = address.GetFormattedAddress();

                this.addressNormalisationProcessingServiceMock.Setup(service =>
                    service.GetNormalisedAddress(addressString))
                        .ThrowsAsync(dependencyException);

                var addressExtractionOrchestrationDependencyException =
                    new AddressExtractionOrchestrationDependencyException(
                        message: "Address extraction orchestration dependency error occurred, " +
                        "please try again.",
                        innerException: dependencyException.InnerException as Xeption);

                exceptions.Add(addressExtractionOrchestrationDependencyException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to normalise address for {exceptions.Count} addresses",
                    exceptions);

            var failedAddressExtractionOrchestrationServiceException =
                new FailedAddressExtractionOrchestrationServiceException(
                    message: "Failed address extraction aggregate orchestration service error occurred, " +
                    "please contact support.",
                    innerException: aggregateException);

            var expectedAddressExtractionOrchestrationServiceException =
                new AddressExtractionOrchestrationServiceException(
                    message: "Address extraction orchestration service error occurred, please contact support.",
                    innerException: failedAddressExtractionOrchestrationServiceException);

            // When
            ValueTask<List<Address>> processAddressTask =
                this.addressExtractionOrchestrationService.ProcessAddressesAsync(randomData, someFilename);

            AddressExtractionOrchestrationServiceException actualAddressExtractionOrchestrationServiceException =
                await Assert.ThrowsAsync<AddressExtractionOrchestrationServiceException>(async () =>
                    await processAddressTask);

            // Then
            actualAddressExtractionOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedAddressExtractionOrchestrationServiceException);

            this.csvHelperBrokerMock.Verify(service =>
                service.MapCsvToObjectAsync<Address>(
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>()),
                        Times.Once);

            foreach (Address address in randomAddresses)
            {
                string addressString = address.GetFormattedAddress();

                this.addressNormalisationProcessingServiceMock.Verify(service =>
                    service.GetNormalisedAddress(addressString),
                        Times.Once);
            }

            var addressExtractionOrchestrationDependencyLoggingException =
                new AddressExtractionOrchestrationDependencyException(
                    message: "Address extraction orchestration dependency error occurred, " +
                    "fix the errors and try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    addressExtractionOrchestrationDependencyLoggingException))),
                        Times.Exactly(randomAddresses.Count));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    actualAddressExtractionOrchestrationServiceException))),
                        Times.Once);

            this.addressNormalisationProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Fact(Skip = "WIP: Hassan to complete")]
        public async Task ShouldThrowAggregateServiceExceptionOnProcessAddressIfErrorsInLoopAndLogItAsync()
        {
            // Given
            string someFilename = GetRandomString();
            byte[] randomData = Encoding.ASCII.GetBytes(GetRandomString());
            var serviceException = new Exception();
            List<Address> randomAddresses = CreateRandomAddresses().ToList();
            List<Exception> exceptions = new List<Exception>();

            this.csvHelperBrokerMock.Setup(service =>
                service.MapCsvToObjectAsync<Address>(
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>()))
                    .ReturnsAsync(randomAddresses);

            var innerFailedAddressExtractionOrchestrationServiceException =
                new FailedAddressExtractionOrchestrationServiceException(
                    message: "Failed address extraction orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var innerAddressExtractionOrchestrationServiceException =
                new AddressExtractionOrchestrationServiceException(
                    message: "Address extraction orchestration service error occurred, please contact support.",
                    innerException: innerFailedAddressExtractionOrchestrationServiceException);

            foreach (Address address in randomAddresses)
            {
                string stringAddress = address.GetFormattedAddress();

                this.addressNormalisationProcessingServiceMock.Setup(service =>
                    service.GetNormalisedAddress(stringAddress))
                        .ThrowsAsync(serviceException);

                exceptions.Add(innerAddressExtractionOrchestrationServiceException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to normalise address for {exceptions.Count} addresses",
                    exceptions);

            var failedAddressExtractionOrchestrationServiceException =
                new FailedAddressExtractionOrchestrationServiceException(
                    message: "Failed address extraction aggregate orchestration service error occurred, " +
                        "please contact support.",
                    innerException: aggregateException);

            var expectedAddressExtractionOrchestrationServiceException =
                new AddressExtractionOrchestrationServiceException(
                    message: "Address extraction orchestration service error occurred, please contact support.",
                    innerException: failedAddressExtractionOrchestrationServiceException);

            // When
            ValueTask<List<Address>> processAddressTask =
                this.addressExtractionOrchestrationService.ProcessAddressesAsync(randomData, someFilename);

            AddressExtractionOrchestrationServiceException actualAddressExtractionOrchestrationServiceException =
                await Assert.ThrowsAsync<AddressExtractionOrchestrationServiceException>(async () =>
                    await processAddressTask);

            // Then
            actualAddressExtractionOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedAddressExtractionOrchestrationServiceException);

            this.csvHelperBrokerMock.Verify(service =>
                service.MapCsvToObjectAsync<Address>(
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>()),
                        Times.Once);

            foreach (Address address in randomAddresses)
            {
                string addressString = address.GetFormattedAddress();

                this.addressNormalisationProcessingServiceMock.Verify(service =>
                    service.GetNormalisedAddress(addressString),
                        Times.Once);
            }

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    innerAddressExtractionOrchestrationServiceException))),
                        Times.Exactly(randomAddresses.Count));

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

        [Theory]
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
                this.addressExtractionOrchestrationService.ProcessAddressesAsync(inputData, someFilename);

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

        [Theory]
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
                this.addressExtractionOrchestrationService.ProcessAddressesAsync(inputData, someFilename);

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

        [Fact]
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
                this.addressExtractionOrchestrationService.ProcessAddressesAsync(inputData, someFilename);

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
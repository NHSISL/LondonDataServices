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
using LHDS.Core.Models.Coordinations.EmisLandings.Exceptions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Orchestrations.AddressExtractions.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressExtractions
{
    public partial class AddressExtractionOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(AddressExtractionOrchestrationDependencyValidationExceptions))]
        public async Task 
            ShouldThrowAggregateDependencyValidationExceptionOnProcessAddressesIfErrorsInLoopAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // Given
            byte[] randomData = Encoding.ASCII.GetBytes(GetRandomString());
            List<Address> randomAddresses = CreateRandomAddresses().ToList();
            List<Exception> exceptions = new List<Exception>();

            this.addressParserServiceMock.Setup(service =>
                service.ProcessCsvAsync(randomData))
                    .ReturnsAsync(randomAddresses);

            foreach (Address address in randomAddresses)
            {
                string addressString = address.GetFormattedAddress();

                this.addressNormalisationServiceMock.Setup(service =>
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
                    message: "Failed address extraction aggregate orchestration service occurred, " +
                    "please contact support.",
                    innerException: aggregateException);

            var expectedAddressExtractionOrchestrationServiceException =
                new AddressExtractionOrchestrationServiceException(
                    message: "Address extraction orchestration service error occurred, contact support.",
                    innerException: failedAddressExtractionOrchestrationServiceException);

            // When
            ValueTask<List<Address>> processAddressTask = 
                this.addressExtractionOrchestrationService.ProcessAddressesAsync(randomData);

            AddressExtractionOrchestrationServiceException actualAddressExtractionOrchestrationServiceException =
                await Assert.ThrowsAsync<AddressExtractionOrchestrationServiceException>(async () =>
                    await processAddressTask);

            // Then
            actualAddressExtractionOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedAddressExtractionOrchestrationServiceException);

            this.addressParserServiceMock.Verify(service =>
                service.ProcessCsvAsync(randomData),
                    Times.Once);

            foreach (Address address in randomAddresses)
            {
                string addressString = address.GetFormattedAddress();
                
                this.addressNormalisationServiceMock.Verify(service =>
                    service.GetNormalisedAddress(addressString),
                        Times.Once);
            }

            var addressExtractionOrchestrationDependencyValidationLoggingException =
                new AddressExtractionOrchestrationDependencyValidationException(
                    message: "Address extraction orchestration dependency validation error occurred, " +
                    "please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    addressExtractionOrchestrationDependencyValidationLoggingException))),
                        Times.Exactly(randomAddresses.Count));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    actualAddressExtractionOrchestrationServiceException))),
                        Times.Once);

            this.addressParserServiceMock.VerifyNoOtherCalls();
            this.addressNormalisationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(AddressExtractionDependencyExceptions))]
        public async Task
            ShouldThrowAggregateDependencyExceptionOnProcessAddressesIfErrorsInLoopAndLogItAsync(
            Xeption dependencyException)
        {
            // Given
            byte[] randomData = Encoding.ASCII.GetBytes(GetRandomString());
            List<Address> randomAddresses = CreateRandomAddresses().ToList();
            List<Exception> exceptions = new List<Exception>();

            this.addressParserServiceMock.Setup(service =>
                service.ProcessCsvAsync(randomData))
                    .ReturnsAsync(randomAddresses);

            foreach (Address address in randomAddresses)
            {
                string addressString = address.GetFormattedAddress();

                this.addressNormalisationServiceMock.Setup(service =>
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
                    message: "Failed address extraction aggregate orchestration service occurred, " +
                    "please contact support.",
                    innerException: aggregateException);

            var expectedAddressExtractionOrchestrationServiceException =
                new AddressExtractionOrchestrationServiceException(
                    message: "Address extraction orchestration service error occurred, contact support.",
                    innerException: failedAddressExtractionOrchestrationServiceException);

            // When
            ValueTask<List<Address>> processAddressTask =
                this.addressExtractionOrchestrationService.ProcessAddressesAsync(randomData);

            AddressExtractionOrchestrationServiceException actualAddressExtractionOrchestrationServiceException =
                await Assert.ThrowsAsync<AddressExtractionOrchestrationServiceException>(async () =>
                    await processAddressTask);

            // Then
            actualAddressExtractionOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedAddressExtractionOrchestrationServiceException);

            this.addressParserServiceMock.Verify(service =>
                service.ProcessCsvAsync(randomData),
                    Times.Once);

            foreach (Address address in randomAddresses)
            {
                string addressString = address.GetFormattedAddress();

                this.addressNormalisationServiceMock.Verify(service =>
                    service.GetNormalisedAddress(addressString),
                        Times.Once);
            }

            var addressExtractionOrchestrationDependencyLoggingException =
                new AddressExtractionOrchestrationDependencyException(
                    message: "Address extraction orchestration dependency error occurred, " +
                    "please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    addressExtractionOrchestrationDependencyLoggingException))),
                        Times.Exactly(randomAddresses.Count));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    actualAddressExtractionOrchestrationServiceException))),
                        Times.Once);

            this.addressParserServiceMock.VerifyNoOtherCalls();
            this.addressNormalisationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowAggregateServiceExceptionOnProcessAddressIfErrorsInLoopAndLogItAsync()
        {
            // Given
            byte[] randomData = Encoding.ASCII.GetBytes(GetRandomString());
            var serviceException = new Exception();
            List<Address> randomAddresses = CreateRandomAddresses().ToList();
            List<Exception> exceptions = new List<Exception>();

            this.addressParserServiceMock.Setup(service =>
                service.ProcessCsvAsync(randomData))
                    .ReturnsAsync(randomAddresses);

            var innerFailedAddressExtractionOrchestrationServiceException =
                new FailedAddressExtractionOrchestrationServiceException(
                    message: "Failed address extraction orchestration service occurred, please contact support.",
                    innerException: serviceException);

            var innerAddressExtractionOrchestrationServiceException =
                new AddressExtractionOrchestrationServiceException(
                    message: "Address extraction orchestration service error occurred, contact support.",
                    innerException: innerFailedAddressExtractionOrchestrationServiceException);

            foreach (Address address in randomAddresses)
            {
                string stringAddress = address.GetFormattedAddress();

                this.addressNormalisationServiceMock.Setup(service =>
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
                    message: "Failed address extraction aggregate orchestration service occurred, " +
                    "please contact support.",
                    innerException: aggregateException);

            var expectedAddressExtractionOrchestrationServiceException =
                new AddressExtractionOrchestrationServiceException(
                    message: "Address extraction orchestration service error occurred, contact support.",
                    innerException: failedAddressExtractionOrchestrationServiceException);

            // When
            ValueTask<List<Address>> processAddressTask =
                this.addressExtractionOrchestrationService.ProcessAddressesAsync(randomData);

            AddressExtractionOrchestrationServiceException actualAddressExtractionOrchestrationServiceException =
                await Assert.ThrowsAsync<AddressExtractionOrchestrationServiceException>(async () =>
                    await processAddressTask);

            // Then
            actualAddressExtractionOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedAddressExtractionOrchestrationServiceException);

            this.addressParserServiceMock.Verify(service =>
                service.ProcessCsvAsync(randomData),
                    Times.Once);

            foreach (Address address in randomAddresses)
            {
                this.addressNormalisationServiceMock.Verify(service =>
                    service.GetNormalisedAddress(It.IsAny<string>()),
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

            this.addressParserServiceMock.VerifyNoOtherCalls();
            this.addressNormalisationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(AddressExtractionOrchestrationDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnAddressExtractionIfDependencyValidationOccursAndLogItAsync(
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

            this.addressParserServiceMock.Setup(service =>
                service.ProcessCsvAsync(It.IsAny<byte[]>()))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<List<Address>> processDataTask =
                this.addressExtractionOrchestrationService.ProcessAddressesAsync(inputData);

            AddressExtractionOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressExtractionOrchestrationDependencyValidationException>(
                    processDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.addressParserServiceMock.Verify(service =>
             service.ProcessCsvAsync(It.IsAny<byte[]>()),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.addressParserServiceMock.VerifyNoOtherCalls();
            this.addressNormalisationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(AddressExtractionDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnAddressExtractionIfDependencyErrorOccursAndLogItAsync(
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

            this.addressParserServiceMock.Setup(service =>
                service.ProcessCsvAsync(It.IsAny<byte[]>()))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<List<Address>> processDataTask =
                this.addressExtractionOrchestrationService.ProcessAddressesAsync(inputData);

            AddressExtractionOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<AddressExtractionOrchestrationDependencyException>(
                    processDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.addressParserServiceMock.Verify(service =>
             service.ProcessCsvAsync(It.IsAny<byte[]>()),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.addressParserServiceMock.VerifyNoOtherCalls();
            this.addressNormalisationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddressExtractionIfServiceErrorOccursAndLogItAsync()
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

            this.addressParserServiceMock.Setup(service =>
                service.ProcessCsvAsync(It.IsAny<byte[]>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<List<Address>> processDataTask =
                this.addressExtractionOrchestrationService.ProcessAddressesAsync(inputData);

            AddressExtractionOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<AddressExtractionOrchestrationServiceException>(
                    processDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedAddressExtractionOrchestrationServiceException);

            this.addressParserServiceMock.Verify(service =>
             service.ProcessCsvAsync(It.IsAny<byte[]>()),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedAddressExtractionOrchestrationServiceException))),
                       Times.Once);

            this.addressParserServiceMock.VerifyNoOtherCalls();
            this.addressNormalisationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
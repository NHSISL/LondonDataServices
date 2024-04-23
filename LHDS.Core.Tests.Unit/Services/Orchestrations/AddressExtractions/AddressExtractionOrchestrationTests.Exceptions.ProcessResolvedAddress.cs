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
using LHDS.Core.Models.Foundations.AddressExtractionAudits;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
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
            ShouldThrowAggregateDependencyValidationExceptionOnProcessResolvedAddressesIfErrorsInLoopAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // Given
            byte[] randomData = Encoding.ASCII.GetBytes(GetRandomString());
            string randomFilename = GetRandomString();
            List<ResolvedAddress> randomResolvedAddresses = CreateRandomResolvedAddresses().ToList();
            List<Exception> exceptions = new List<Exception>();

            this.resolvedAddressParserServiceMock.Setup(service =>
                service.ProcessCsvAsync(randomData, randomFilename))
                    .ReturnsAsync(randomResolvedAddresses);

            foreach (ResolvedAddress resolvedAddress in randomResolvedAddresses)
            {
                string addressString = resolvedAddress.UnstructuredPostalAddress;

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
                    $"Unable to normalise address for {exceptions.Count} resolved addresses",
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
            ValueTask<List<ResolvedAddress>> processResolvedAddressTask =
                this.addressExtractionOrchestrationService.ProcessResolvedAddressesAsync(randomData, randomFilename);

            AddressExtractionOrchestrationServiceException actualAddressExtractionOrchestrationServiceException =
                await Assert.ThrowsAsync<AddressExtractionOrchestrationServiceException>(async () =>
                    await processResolvedAddressTask);

            // Then
            actualAddressExtractionOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedAddressExtractionOrchestrationServiceException);

            this.resolvedAddressParserServiceMock.Verify(service =>
                service.ProcessCsvAsync(randomData, randomFilename),
                    Times.Once);

            foreach (ResolvedAddress resolvedAddress in randomResolvedAddresses)
            {
                string addressString = resolvedAddress.UnstructuredPostalAddress;

                this.addressNormalisationServiceMock.Verify(service =>
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
                        Times.Exactly(randomResolvedAddresses.Count));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    actualAddressExtractionOrchestrationServiceException))),
                        Times.Once);

            this.resolvedAddressParserServiceMock.VerifyNoOtherCalls();
            this.addressNormalisationServiceMock.VerifyNoOtherCalls();
            this.addressParserServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(AddressExtractionDependencyExceptions))]
        public async Task
            ShouldThrowAggregateDependencyExceptionOnProcessResolvedAddressesIfErrorsInLoopAndLogItAsync(
            Xeption dependencyException)
        {
            // Given
            byte[] randomData = Encoding.ASCII.GetBytes(GetRandomString());
            string randomFilename = GetRandomString();
            List<ResolvedAddress> randomResolvedAddresses = CreateRandomResolvedAddresses().ToList();
            List<Exception> exceptions = new List<Exception>();

            this.resolvedAddressParserServiceMock.Setup(service =>
                service.ProcessCsvAsync(randomData, randomFilename))
                    .ReturnsAsync(randomResolvedAddresses);

            foreach (ResolvedAddress resolvedAddress in randomResolvedAddresses)
            {
                string addressString = resolvedAddress.UnstructuredPostalAddress;

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
                    $"Unable to normalise address for {exceptions.Count} resolved addresses",
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
            ValueTask<List<ResolvedAddress>> processResolvedAddressTask =
                this.addressExtractionOrchestrationService.ProcessResolvedAddressesAsync(randomData, randomFilename);

            AddressExtractionOrchestrationServiceException actualAddressExtractionOrchestrationServiceException =
                await Assert.ThrowsAsync<AddressExtractionOrchestrationServiceException>(async () =>
                    await processResolvedAddressTask);

            // Then
            actualAddressExtractionOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedAddressExtractionOrchestrationServiceException);

            this.resolvedAddressParserServiceMock.Verify(service =>
                service.ProcessCsvAsync(randomData, randomFilename),
                    Times.Once);

            foreach (ResolvedAddress resolvedAddress in randomResolvedAddresses)
            {
                string addressString = resolvedAddress.UnstructuredPostalAddress;

                this.addressNormalisationServiceMock.Verify(service =>
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
                        Times.Exactly(randomResolvedAddresses.Count));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    actualAddressExtractionOrchestrationServiceException))),
                        Times.Once);

            this.resolvedAddressParserServiceMock.VerifyNoOtherCalls();
            this.addressNormalisationServiceMock.VerifyNoOtherCalls();
            this.addressParserServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowAggregateServiceExceptionOnProcessResolvedAddressesIfErrorsInLoopAndLogItAsync()
        {
            // Given
            byte[] randomData = Encoding.ASCII.GetBytes(GetRandomString());
            string randomFilename = GetRandomString();
            var serviceException = new Exception();
            List<ResolvedAddress> randomResolvedAddresses = CreateRandomResolvedAddresses().ToList();
            List<Exception> exceptions = new List<Exception>();

            this.resolvedAddressParserServiceMock.Setup(service =>
                service.ProcessCsvAsync(randomData, randomFilename))
                    .ReturnsAsync(randomResolvedAddresses);

            var innerFailedAddressExtractionOrchestrationServiceException =
                new FailedAddressExtractionOrchestrationServiceException(
                    message: "Failed address extraction orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var innerAddressExtractionOrchestrationServiceException =
                new AddressExtractionOrchestrationServiceException(
                    message: "Address extraction orchestration service error occurred, contact support.",
                    innerException: innerFailedAddressExtractionOrchestrationServiceException);

            foreach (ResolvedAddress resolvedAddress in randomResolvedAddresses)
            {
                string stringAddress = resolvedAddress.UnstructuredPostalAddress;

                this.addressNormalisationServiceMock.Setup(service =>
                    service.GetNormalisedAddress(stringAddress))
                        .ThrowsAsync(serviceException);

                exceptions.Add(innerAddressExtractionOrchestrationServiceException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to normalise address for {exceptions.Count} resolved addresses",
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
            ValueTask<List<ResolvedAddress>> processResolvedAddressTask =
                this.addressExtractionOrchestrationService.ProcessResolvedAddressesAsync(randomData, randomFilename);

            AddressExtractionOrchestrationServiceException actualAddressExtractionOrchestrationServiceException =
                await Assert.ThrowsAsync<AddressExtractionOrchestrationServiceException>(async () =>
                    await processResolvedAddressTask);

            // Then
            actualAddressExtractionOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedAddressExtractionOrchestrationServiceException);

            this.resolvedAddressParserServiceMock.Verify(service =>
                service.ProcessCsvAsync(randomData, randomFilename),
                    Times.Once);

            foreach (ResolvedAddress resolvedAddress in randomResolvedAddresses)
            {
                string addressString = resolvedAddress.UnstructuredPostalAddress;

                this.addressNormalisationServiceMock.Verify(service =>
                    service.GetNormalisedAddress(addressString),
                        Times.Once);
            }

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    innerAddressExtractionOrchestrationServiceException))),
                        Times.Exactly(randomResolvedAddresses.Count));

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
        public async Task
            ShouldThrowDependencyValidationOnResolvedAddressExtractionIfDependencyValidationOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            string assembly = Assembly.GetExecutingAssembly().Location;

            string inputFilePath = Path.Combine(Path.GetDirectoryName(assembly),
                @"Resources/Services/Orchestrations/AddressExtractions/ShouldProcessZipFileWithOnlyCsvAddressesData.zip");

            byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);
            string randomFilename = GetRandomString();

            var expectedDependencyException =
                new AddressExtractionOrchestrationDependencyValidationException(
                    message: "Address extraction orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.resolvedAddressParserServiceMock.Setup(service =>
                service.ProcessCsvAsync(It.IsAny<byte[]>(), It.IsAny<string>()))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<List<ResolvedAddress>> processDataTask =
                this.addressExtractionOrchestrationService.ProcessResolvedAddressesAsync(inputData, randomFilename);

            AddressExtractionOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressExtractionOrchestrationDependencyValidationException>(
                    processDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.resolvedAddressParserServiceMock.Verify(service =>
             service.ProcessCsvAsync(It.IsAny<byte[]>(), It.IsAny<string>()),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.resolvedAddressParserServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressNormalisationServiceMock.VerifyNoOtherCalls();
            this.addressParserServiceMock.VerifyNoOtherCalls();
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
            string randomFilename = GetRandomString();

            var expectedDependencyException =
                new AddressExtractionOrchestrationDependencyException(
                    message: "Address extraction orchestration dependency error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.resolvedAddressParserServiceMock.Setup(service =>
                service.ProcessCsvAsync(It.IsAny<byte[]>(), It.IsAny<string>()))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<List<ResolvedAddress>> processDataTask =
                this.addressExtractionOrchestrationService.ProcessResolvedAddressesAsync(inputData, randomFilename);

            AddressExtractionOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<AddressExtractionOrchestrationDependencyException>(
                    processDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.resolvedAddressParserServiceMock.Verify(service =>
             service.ProcessCsvAsync(It.IsAny<byte[]>(), It.IsAny<string>()),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.resolvedAddressParserServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressNormalisationServiceMock.VerifyNoOtherCalls();
            this.addressParserServiceMock.VerifyNoOtherCalls();
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
            string randomFilename = GetRandomString();
            var serviceException = new Exception();

            var failedAddressPersistanceOrchestrationServiceException =
                new FailedAddressExtractionOrchestrationServiceException(
                    message: "Failed address extraction orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedAddressExtractionOrchestrationServiceException =
                new AddressExtractionOrchestrationServiceException(
                    message: "Address extraction orchestration service error occurred, contact support.",
                    innerException: failedAddressPersistanceOrchestrationServiceException);

            this.resolvedAddressParserServiceMock.Setup(service =>
                service.ProcessCsvAsync(It.IsAny<byte[]>(), It.IsAny<string>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<List<ResolvedAddress>> processDataTask =
                this.addressExtractionOrchestrationService.ProcessResolvedAddressesAsync(inputData, randomFilename);

            AddressExtractionOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<AddressExtractionOrchestrationServiceException>(
                    processDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedAddressExtractionOrchestrationServiceException);

            this.resolvedAddressParserServiceMock.Verify(service =>
             service.ProcessCsvAsync(It.IsAny<byte[]>(), It.IsAny<string>()),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedAddressExtractionOrchestrationServiceException))),
                       Times.Once);

            this.resolvedAddressParserServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressNormalisationServiceMock.VerifyNoOtherCalls();
            this.addressParserServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
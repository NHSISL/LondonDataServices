// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using LHDS.Core.Models.Orchestrations.AddressNormalisations.Exceptions;
using LHDS.Core.Models.Processings.AddressNormalisations.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressNormalisations
{
    public partial class AddressNormalisationOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task
        ShouldThrowDependencyValidationExceptionOnGetNormalisedAddressIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var randomAddress = GetRandomString();
            string inputAddress = randomAddress;

            var expectedAddressNormalisationOrchestrationDependencyValidationException =
                new AddressNormalisationOrchestrationDependencyValidationException(
                    message: "Address normalisation orchestration dependency validation occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.addressParserProcessingServiceMock.Setup(processing =>
                processing.ProcessCsvAsync(It.IsAny<string>()))
                    .Throws(dependencyValidationException);

            // when
            ValueTask<List<AddressNormalisation>> actualAddressesTask =
                this.addressNormalisationOrchestrationService.ProcessDataAsync(inputAddress);

            AddressNormalisationOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressNormalisationOrchestrationDependencyValidationException>(
                   actualAddressesTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(
                expectedAddressNormalisationOrchestrationDependencyValidationException);

            this.addressParserProcessingServiceMock.Verify(processing =>
                processing.ProcessCsvAsync(It.IsAny<string>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedAddressNormalisationOrchestrationDependencyValidationException))),
                         Times.Once);

            this.addressParserProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyOnGetNormalisedAddressIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var randomAddress = GetRandomString();
            string inputAddress = randomAddress;
            var randomMessage = GetRandomString();

            var expectedAddressNormalisationOrchestrationDependencyException =
                new AddressNormalisationOrchestrationDependencyException(
                    message: "Address normalisation orchestration dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.addressParserProcessingServiceMock.Setup(processing =>
               processing.ProcessCsvAsync(It.IsAny<string>()))
                   .Throws(dependencyException);

            // when
            ValueTask<List<AddressNormalisation>> actualAddressesTask =
                this.addressNormalisationOrchestrationService.ProcessDataAsync(inputAddress);

            AddressNormalisationOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<AddressNormalisationOrchestrationDependencyException>(
                    actualAddressesTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressNormalisationOrchestrationDependencyException);

            this.addressParserProcessingServiceMock.Verify(processing =>
                processing.ProcessCsvAsync(It.IsAny<string>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedAddressNormalisationOrchestrationDependencyException))),
                         Times.Once);

            this.addressParserProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetNormalisedAddressIfServiceErrorOccursAsync()
        {
            // given
            var randomAddress = GetRandomString();
            string inputAddress = randomAddress;
            var serviceException = new Exception();

            var failedAddressNormalisationOrchestrationServiceException =
                new FailedAddressNormalisationOrchestrationServiceException(
                    message: "Failed address normalisation orchestration service error occurred, contact support.",
                    innerException: serviceException);

            var expectedAddressNormalisationOrchestrationServiveException =
                new AddressNormalisationOrchestrationServiceException(
                    message: "Address normalisation orchestration service error occurred, contact support.",
                    innerException: failedAddressNormalisationOrchestrationServiceException);

            this.addressParserProcessingServiceMock.Setup(processing =>
               processing.ProcessCsvAsync(It.IsAny<string>()))
                    .Throws(serviceException);

            // when;
            ValueTask<List<AddressNormalisation>> actualAddressesTask =
               this.addressNormalisationOrchestrationService.ProcessDataAsync(inputAddress);

            AddressNormalisationOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<AddressNormalisationOrchestrationServiceException>(
                    actualAddressesTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressNormalisationOrchestrationServiveException);

            this.addressParserProcessingServiceMock.Verify(processing =>
                 processing.ProcessCsvAsync(It.IsAny<string>()),
                     Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedAddressNormalisationOrchestrationServiveException))),
                         Times.Once);

            this.addressParserProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

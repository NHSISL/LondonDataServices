// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Orchestrations.AddressPersistances.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressPersistances
{
    public partial class AddressPersistanceOrchestrationServiceTests
    {
        [Theory(Skip = "no longer used, will refactor out")]
        [MemberData(nameof(AddressPersistenceDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnMatchAndPersistIfDependencyValidationErrorOccursAndLogItAsync(
           Xeption dependencyValidationException)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            ResolvedAddress randomResolvedAddress = CreateRandomResolvedAddress(randomDateTimeOffset);

            var expectedAddressPersistenceOrchestrationDependencyValidationException =
                new AddressPersistenceOrchestrationDependencyValidationException(
                    message: "Address persistence orchestration dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.addressMatcherProcessingServiceMock.Setup(processing =>
                processing.ExtractPostCode(It.IsAny<string>()))
                    .Throws(dependencyValidationException);

            // when
            ValueTask<ResolvedAddress> PersistTask = this.addressPersistanceOrchestrationService
                    .MatchAndPersistResolvedAddressAsync(randomResolvedAddress);

            AddressPersistenceOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressPersistenceOrchestrationDependencyValidationException>(
                    PersistTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressPersistenceOrchestrationDependencyValidationException);

            this.addressMatcherProcessingServiceMock.Verify(processing =>
                processing.ExtractPostCode(It.IsAny<string>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedAddressPersistenceOrchestrationDependencyValidationException))),
                         Times.Once);

            this.addressMatcherProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory(Skip = "no longer used, will refactor out")]
        [MemberData(nameof(AddressPersistenceDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnMatchAndPersistIfDependencyValidationErrorOccursAndLogItAsync(
           Xeption dependencyException)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            ResolvedAddress randomResolvedAddress = CreateRandomResolvedAddress(randomDateTimeOffset);

            var expectedAddressPersistenceOrchestrationDependencyException =
                new AddressPersistenceOrchestrationDependencyException(
                    message: "Address persistence orchestration dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.addressMatcherProcessingServiceMock.Setup(processing =>
                processing.ExtractPostCode(It.IsAny<string>()))
                    .Throws(dependencyException);

            // when
            ValueTask<ResolvedAddress> PersistTask = this.addressPersistanceOrchestrationService
                    .MatchAndPersistResolvedAddressAsync(randomResolvedAddress);

            AddressPersistenceOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<AddressPersistenceOrchestrationDependencyException>(
                    PersistTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressPersistenceOrchestrationDependencyException);

            this.addressMatcherProcessingServiceMock.Verify(processing =>
                processing.ExtractPostCode(It.IsAny<string>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedAddressPersistenceOrchestrationDependencyException))),
                         Times.Once);

            this.addressMatcherProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact(Skip = "no longer used, will refactor out")]
        public async Task ShouldThrowServiceExceptionOnMatchAndPersistIfDependencyValidationErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            ResolvedAddress randomResolvedAddress = CreateRandomResolvedAddress(randomDateTimeOffset);
            var serviceException = new Exception();

            var failedAddressPersistenceOrchestrationServiceException =
                new FailedAddressPersistenceOrchestrationServiceException(
                    message: "Failed address persistence orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedAddressPersistenceOrchestrationServiceException =
                new AddressPersistenceOrchestrationServiceException(
                    message: "Address persistence orchestration service error occurred, please contact support.",
                    innerException: failedAddressPersistenceOrchestrationServiceException);

            this.addressMatcherProcessingServiceMock.Setup(processing =>
                processing.ExtractPostCode(It.IsAny<string>()))
                    .Throws(serviceException);

            // when
            ValueTask<ResolvedAddress> PersistTask = this.addressPersistanceOrchestrationService
                    .MatchAndPersistResolvedAddressAsync(randomResolvedAddress);

            AddressPersistenceOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<AddressPersistenceOrchestrationServiceException>(
                    PersistTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressPersistenceOrchestrationServiceException);

            this.addressMatcherProcessingServiceMock.Verify(processing =>
                processing.ExtractPostCode(It.IsAny<string>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedAddressPersistenceOrchestrationServiceException))),
                         Times.Once);

            this.addressMatcherProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
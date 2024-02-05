// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using LHDS.Core.Models.Orchestrations.AddressPersistances.Exceptions;
using LHDS.Core.Models.Orchestrations.AddressResolvings.Exceptions;
using Moq;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressResolvings
{
    public partial class AddressResolvingOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(AddressResolvingDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnAddressResolvingIfDependencyValidationOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            AddressNormalisation randomAddress = CreateRandomAddressNormalisation();

            var expectedDependencyException =
                new AddressResolvingOrchestrationDependencyValidationException(
                    message: "Normalised address resolving orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.resolvedAddressProcessingServiceMock.Setup(service =>
                service.IsExactMatchForResolvedAddressAsync(randomAddress.PostalAddress))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<AddressNormalisation> processTask =
                this.addressResolvingOrchestrationService.ResolvedAddressAsync(randomAddress);

            AddressResolvingOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressResolvingOrchestrationDependencyValidationException>(
                    processTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.IsExactMatchForResolvedAddressAsync(randomAddress.PostalAddress),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            addressProcessingServiceMock.VerifyNoOtherCalls();
            addressMatcherProcessingServiceMock.VerifyNoOtherCalls();
            resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
            dateTimeBrokerMock.VerifyNoOtherCalls();
            serializationBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(AddressResolvingDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnAddressResolvingIfDependencyExceptionOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            AddressNormalisation randomAddress = CreateRandomAddressNormalisation();

            var expectedDependencyException =
                new AddressResolvingOrchestrationDependencyException(
                    message: "Address resolving orchestration dependency error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.resolvedAddressProcessingServiceMock.Setup(service =>
                service.IsExactMatchForResolvedAddressAsync(randomAddress.PostalAddress))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<AddressNormalisation> processTask =
                this.addressResolvingOrchestrationService.ResolvedAddressAsync(randomAddress);

            AddressResolvingOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<AddressResolvingOrchestrationDependencyException>(
                    processTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.IsExactMatchForResolvedAddressAsync(randomAddress.PostalAddress),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            addressProcessingServiceMock.VerifyNoOtherCalls();
            addressMatcherProcessingServiceMock.VerifyNoOtherCalls();
            resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
            dateTimeBrokerMock.VerifyNoOtherCalls();
            serializationBrokerMock.VerifyNoOtherCalls();
        }
    }
}
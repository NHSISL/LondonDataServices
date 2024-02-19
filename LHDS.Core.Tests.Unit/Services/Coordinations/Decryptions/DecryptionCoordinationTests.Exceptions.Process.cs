// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Coordinations.Decryptions.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.Decryptions
{
    public partial class DecryptionCoordinationServiceTests
    {

        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnProcessIfErrorsAndLogItAsync(
            Xeption dependancyValidationException)
        {
            // Given
            Guid SubscriberCredentialId = Guid.NewGuid();
            string filePath = GetRandomString();

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RetrieveSubscriberCredentialByIdAsync(SubscriberCredentialId))
                    .ThrowsAsync(dependancyValidationException);

            var expectedDecryptionCoordinationDependencyValidationException =
                new DecryptionCoordinationDependencyValidationException(
                    message: "Decryption coordination dependency validation error occurred, please try again.",
                    innerException: dependancyValidationException.InnerException as Xeption);

            // When
            ValueTask<string> processDataTask = this.decryptionCoordinationService.DecryptAsync(filePath);

            DecryptionCoordinationDependencyValidationException
                actualDecryptionCoordinationDependencyValidationException =
                    await Assert.ThrowsAsync<DecryptionCoordinationDependencyValidationException>(async () =>
                        await processDataTask);

            // Then
            actualDecryptionCoordinationDependencyValidationException.Should()
                .BeEquivalentTo(expectedDecryptionCoordinationDependencyValidationException);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RetrieveSubscriberCredentialByIdAsync(SubscriberCredentialId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(IsSameExceptionAs(
                     expectedDecryptionCoordinationDependencyValidationException))),
                         Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.decryptionOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}


// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Coordinations.Decryptions.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.Decryptions
{
    public partial class DecryptionCoordinationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnProcessDecryptedItemsForBatchCompleteAndLogItAsync()
        {
            // given
            Guid invalidSupplierId = default;

            var invalidArgumentDecryptionCoordinationException =
                new InvalidArgumentDecryptionCoordinationException(
                    message: "Invalid decryption coordination argument, please correct the errors and try again.");

            var expectedDecryptionCoordinationValidationException =
                new DecryptionCoordinationValidationException(
                    message: "Decryption coordination validation error occurred, please try again.",
                    innerException: invalidArgumentDecryptionCoordinationException);

            // when
            ValueTask processTask =
                this.decryptionCoordinationService.ProcessDecryptedItemsForBatchCompleteAsync(invalidSupplierId);

            DecryptionCoordinationValidationException actualDecryptionCoordinationValidationException =
                await Assert.ThrowsAsync<DecryptionCoordinationValidationException>(
                    processTask.AsTask);

            // then
            actualDecryptionCoordinationValidationException.Should()
                .BeEquivalentTo(expectedDecryptionCoordinationValidationException);

            this.ingressOrchestrationServiceMock.Verify(service =>
                service.ProcessDecryptedItemsForBatchCompleteAsync(invalidSupplierId),
                    Times.Never());

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(IsSameExceptionAs(
                     invalidArgumentDecryptionCoordinationException))),
                         Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.decryptionOrchestrationServiceMock.VerifyNoOtherCalls();
            this.ingressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
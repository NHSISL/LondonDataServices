// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.CryptographicKeys.Exceptions;
using LHDS.Core.Models.Foundations.Cryptographies.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Cryptographies
{
    public partial class CryptographyServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnDecryptIfDataIsNullAndLogItAsync()
        {
            // given
            byte[] nullData = null;
            SubscriberCredential someSubscriberCredential = CreateRandomSubscriberCredential();

            var nullDecryptionException =
                new NullDataCryptographyException(message: "Data is null.");

            var expectedDecryptionValidationException =
                new CryptographyValidationException(
                    message: "Cryptography validation errors occurred, please try again.",
                    innerException: nullDecryptionException);

            // when
            Task<byte[]> decryptTask =
                this.cryptographyService.DecryptAsync(data: nullData, subscriberCredential: someSubscriberCredential);

            CryptographyValidationException actualDecryptionValidationException =
                await Assert.ThrowsAsync<CryptographyValidationException>(async () =>
                    await decryptTask);

            // then
            actualDecryptionValidationException.Should()
                .BeEquivalentTo(expectedDecryptionValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDecryptionValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.cryptographyBroker.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnDecryptIfSubscriberCredentialIsNullAndLogItAsync()
        {
            // given
            byte[] someData = CreateRandomData();
            SubscriberCredential nullSubscriberCredential = null;

            var nullSubscriberCredentialCryptographyException =
                new NullSubscriberCredentialCryptographyException(message: "Subscriber credential is null.");

            var expectedDecryptionValidationException =
                new CryptographyValidationException(
                    message: "Cryptography validation errors occurred, please try again.",
                    innerException: nullSubscriberCredentialCryptographyException);

            // when
            Task<byte[]> decryptTask = this.cryptographyService.DecryptAsync(
                data: someData,
                subscriberCredential: nullSubscriberCredential);

            CryptographyValidationException actualDecryptionValidationException =
                await Assert.ThrowsAsync<CryptographyValidationException>(async () =>
                    await decryptTask);

            // then
            actualDecryptionValidationException.Should()
                .BeEquivalentTo(expectedDecryptionValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDecryptionValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.cryptographyBroker.VerifyNoOtherCalls();
        }
    }
}

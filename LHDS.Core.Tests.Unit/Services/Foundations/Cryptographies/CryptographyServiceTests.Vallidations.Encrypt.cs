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
        public async Task ShouldThrowValidationExceptionOnEncryptIfDataIsNullAndLogItAsync()
        {
            // given
            byte[] nullData = null;
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();

            var nullEncryptionException =
                new NullDataCryptographyException(message: "Data is null.");

            var expectedEncryptionValidationException =
                new CryptographyValidationException(
                    message: "Cryptography validation errors occurred, please try again.",
                    innerException: nullEncryptionException);

            // when
            Task<byte[]> decryptTask = this.cryptographyService.EncryptAsync(
                data: nullData,
                subscriberCredential: randomSubscriberCredential);

            CryptographyValidationException actualEncryptionValidationException =
                await Assert.ThrowsAsync<CryptographyValidationException>(async () =>
                    await decryptTask);

            // then
            actualEncryptionValidationException.Should()
                .BeEquivalentTo(expectedEncryptionValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedEncryptionValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.cryptographyBroker.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnEncryptIfSubscriberCredentialIsNullAndLogItAsync()
        {
            // given
            byte[] randomData = CreateRandomData();
            SubscriberCredential nullSubscriberCredential = null;

            var nullSubscriberCredentialCryptographyException =
                new NullSubscriberCredentialCryptographyException(message: "Subscriber credential is null.");

            var expectedEncryptionValidationException =
                new CryptographyValidationException(
                    message: "Cryptography validation errors occurred, please try again.",
                    innerException: nullSubscriberCredentialCryptographyException);

            // when
            Task<byte[]> decryptTask = this.cryptographyService.EncryptAsync(
                data: randomData,
                subscriberCredential: nullSubscriberCredential);

            CryptographyValidationException actualEncryptionValidationException =
                await Assert.ThrowsAsync<CryptographyValidationException>(async () =>
                    await decryptTask);

            // then
            actualEncryptionValidationException.Should()
                .BeEquivalentTo(expectedEncryptionValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedEncryptionValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.cryptographyBroker.VerifyNoOtherCalls();
        }
    }
}

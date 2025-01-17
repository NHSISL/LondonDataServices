// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
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
            Stream invalidInputStream = null;
            Stream invalidOutputStream = null;
            SubscriberCredential nullSubscriberCredential = null;

            var invalidArgumentCryptographyException =
                new InvalidArgumentCryptographyException(
                    message: "Invalid cryptography arguments. Please correct the errors and try again.");

            invalidArgumentCryptographyException.AddData(
                key: "input",
                values: "Stream is required");

            invalidArgumentCryptographyException.AddData(
                key: "output",
                values: "Stream is required");

            invalidArgumentCryptographyException.AddData(
                key: "subscriberCredential",
                values: "SubscriberCredential is required");

            var expectedEncryptionValidationException =
                new CryptographyValidationException(
                    message: "Cryptography validation errors occurred, please try again.",
                    innerException: invalidArgumentCryptographyException);

            // when
            ValueTask encryptTask = this.cryptographyService.EncryptAsync(
                input: invalidInputStream,
                output: invalidOutputStream,
                subscriberCredential: nullSubscriberCredential);

            CryptographyValidationException actualEncryptionValidationException =
                await Assert.ThrowsAsync<CryptographyValidationException>(
                    encryptTask.AsTask);

            // then
            actualEncryptionValidationException.Should()
                .BeEquivalentTo(expectedEncryptionValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedEncryptionValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.cryptographyBroker.VerifyNoOtherCalls();
        }
    }
}

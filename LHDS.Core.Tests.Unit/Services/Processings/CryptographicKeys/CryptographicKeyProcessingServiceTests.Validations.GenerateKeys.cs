// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.CryptographicKeys.Exceptions;
using LHDS.Core.Models.Processings.CryptographicKeys.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.CryptographicKeys
{
    public partial class CryptographicKeyProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnGenerateKeysIfSubscriberCredentialIsNullAndLogItAsync()
        {
            //given
            SubscriberCredential nullSubscriberCredential = null;

            var nullSubscriberCredentialProcessingException =
                new NullSubscriberCredentialCryptographicKeyProcessingException(
                    message: "Null subscriber credential processing exception, " +
                        "please correct the errors and try again.");

            var expectedSubscriberCredentialValidationProcessingException =
                new CryptographicKeyValidationProcessingException(
                    message: "Cryptography key processing validation error occurred, please try again.",
                    innerException: nullSubscriberCredentialProcessingException);

            // when
            ValueTask<SubscriberCredential> generateKeysTask =
                this.cryptographyKeyProcessingService.GenerateKeysAsync(nullSubscriberCredential);

            CryptographicKeyValidationProcessingException actualException =
                await Assert.ThrowsAsync<CryptographicKeyValidationProcessingException>(
                    generateKeysTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedSubscriberCredentialValidationProcessingException);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedSubscriberCredentialValidationProcessingException))),
                       Times.Once);

            this.cryptographyKeyServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

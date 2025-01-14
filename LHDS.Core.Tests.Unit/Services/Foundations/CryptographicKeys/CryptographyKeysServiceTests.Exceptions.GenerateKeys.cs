// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.CryptographicKeys;
using LHDS.Core.Models.Foundations.CryptographicKeys.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.CryptographicKeys
{
    public partial class CryptographyKeyServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnGenerateIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string cryptographyType = GetRandomString();
            string publicKeyComment = GetRandomString();

            var serviceException = new Exception();

            var failedCryptographyKeyServiceException =
                new FailedCryptographyKeyServiceException(
                    message: "Failed cryptography key service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedCryptographyKeyServiceException =
                new CryptographyKeyServiceException(
                    message: "Cryptography key service error occurred, please contact support.",
                    innerException: failedCryptographyKeyServiceException);

            this.cryptographyKeyBrokerMock.Setup(broker =>
               broker.CryptographyType)
                   .Returns(cryptographyType);

            this.cryptographyKeyBrokerMock.Setup(broker =>
                broker.GenerateKeysAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                        .ThrowsAsync(serviceException);

            // when
            ValueTask<CryptographicKey> CryptographicKeyTask =
                this.cryptographyKeyService.GenerateKeysAsync(
                cryptographyType: cryptographyType,
                comment: publicKeyComment);

            CryptographyKeyServiceException actualCryptographyKeyServiceException =
                await Assert.ThrowsAsync<CryptographyKeyServiceException>(
                    CryptographicKeyTask.AsTask);

            // then
            actualCryptographyKeyServiceException.Should()
                .BeEquivalentTo(expectedCryptographyKeyServiceException);

            this.cryptographyKeyBrokerMock.Verify(broker =>
                broker.CryptographyType,
                    Times.Once);

            this.cryptographyKeyBrokerMock.Verify(broker =>
                broker.GenerateKeysAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                        Times.Once);

            loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedCryptographyKeyServiceException))),
                        Times.Once);

            this.cryptographyKeyBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

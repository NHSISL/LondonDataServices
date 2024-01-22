// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Cryptographies.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Cryptographies
{
    public partial class CryptographyServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnEncryptfServiceErrorOccursAndLogItAsync()
        {
            // given
            byte[] someId = CreateRandomData();
            var serviceException = new Exception();

            var failedDecryptionServiceException =
                new FailedCryptographyServiceException(
                    message: "Failed cryptography service occurred, please contact support",
                    innerException: serviceException);

            var expectedDecryptionServiceException =
                new CryptographyServiceException(
                    message: "Cryptography service error occurred, contact support.",
                    innerException: failedDecryptionServiceException);

            this.cryptographyBroker.Setup(broker =>
                broker.EncryptAsync(It.IsAny<byte[]>()))
                    .ThrowsAsync(serviceException);

            // when
            Task<byte[]> decryptTask =
                this.cryptographyService.EncryptAsync(someId);

            CryptographyServiceException actualDecryptionServiceException =
                await Assert.ThrowsAsync<CryptographyServiceException>(async () =>
                    await decryptTask);

            // then
            actualDecryptionServiceException.Should()
                .BeEquivalentTo(expectedDecryptionServiceException);

            this.cryptographyBroker.Verify(broker =>
                broker.EncryptAsync(It.IsAny<byte[]>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDecryptionServiceException))),
                        Times.Once);

            this.cryptographyBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

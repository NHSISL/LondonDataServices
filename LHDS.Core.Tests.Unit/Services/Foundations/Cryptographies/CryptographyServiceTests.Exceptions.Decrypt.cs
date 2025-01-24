// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public async Task ShouldThrowServiceExceptionOnDecryptIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Stream someInputStream = new MemoryStream(CreateRandomData());
            Stream someOutputStream = new MemoryStream();
            SubscriberCredential someSubscriberCredential = CreateRandomSubscriberCredential();
            var serviceException = new Exception();

            var failedDecryptionServiceException =
                new FailedCryptographyServiceException(
                    message: "Failed cryptography service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedDecryptionServiceException =
                new CryptographyServiceException(
                    message: "Cryptography service error occurred, please contact support.",
                    innerException: failedDecryptionServiceException);

            this.cryptographyBroker.Setup(broker =>
                broker.DecryptAsync(It.IsAny<Stream>(), It.IsAny<Stream>(), It.IsAny<SubscriberCredential>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask decryptTask = this.cryptographyService.DecryptAsync(
                input: someInputStream,
                output: someOutputStream,
                subscriberCredential: someSubscriberCredential);

            CryptographyServiceException actualDecryptionServiceException =
                await Assert.ThrowsAsync<CryptographyServiceException>(
                    decryptTask.AsTask);

            // then
            actualDecryptionServiceException.Should()
                .BeEquivalentTo(expectedDecryptionServiceException);

            this.cryptographyBroker.Verify(broker =>
                broker.DecryptAsync(It.IsAny<Stream>(), It.IsAny<Stream>(), It.IsAny<SubscriberCredential>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDecryptionServiceException))),
                        Times.Once);

            this.cryptographyBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

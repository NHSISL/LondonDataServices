// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Decryptions.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Decryptions
{
    public partial class DecryptionServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAndLogItAsync()
        {
            // given
            byte[] someId = CreateRandomDecryption();
            var serviceException = new Exception();

            var failedDecryptionServiceException =
                new FailedDecryptionServiceException(
                    message: "Failed decryption service occurred, please contact support", 
                    innerException: serviceException);

            var expectedDecryptionServiceException =
                new DecryptionServiceException(
                    message: "Decryption service error occurred, contact support.",
                    innerException: failedDecryptionServiceException);

            this.decryptionBrokerMock.Setup(broker =>
                broker.DecryptAsync(It.IsAny<byte[]>()))
                    .ThrowsAsync(serviceException);

            // when
            Task<byte[]> decryptTask =
                this.decryptionService.DecryptAsync(someId);

            DecryptionServiceException actualDecryptionServiceException =
                await Assert.ThrowsAsync<DecryptionServiceException>(async () =>
                    await decryptTask);

            // then
            actualDecryptionServiceException.Should()
                .BeEquivalentTo(expectedDecryptionServiceException);

            this.decryptionBrokerMock.Verify(broker =>
                broker.DecryptAsync(It.IsAny<byte[]>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDecryptionServiceException))),
                        Times.Once);

            this.decryptionBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}

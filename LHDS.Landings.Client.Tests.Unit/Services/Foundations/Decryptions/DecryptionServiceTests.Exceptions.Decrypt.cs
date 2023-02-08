// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Landings.Client.Models.Foundations.Decryptions.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.Decryptions
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
                new FailedDecryptionServiceException(serviceException);

            var expectedDecryptionServiceException =
                new DecryptionServiceException(failedDecryptionServiceException);

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

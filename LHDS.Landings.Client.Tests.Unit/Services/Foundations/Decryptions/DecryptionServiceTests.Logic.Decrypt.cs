// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.Decryptions
{
    public partial class DecryptionServiceTests
    {
        [Fact]
        public async Task ShouldDecryptAsync()
        {
            // given
            byte[] randomDecryption = CreateRandomDecryption();
            byte[] inputDecryption = randomDecryption;


            // When
            await this.decryptionService.DecryptAsync(randomDecryption);

            // Then
            this.decryptionBrokerMock.Verify(broker =>
                broker.DecryptAsync(inputDecryption),
                Times.Once);

            this.decryptionBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Cryptographies
{
    public partial class CryptographyServiceTests
    {
        [Fact]
        public async Task ShouldDecryptAsync()
        {
            // given
            byte[] randomEncryptedData = CreateRandomData();
            byte[] inputEncryptedData = randomEncryptedData;
            byte[] randomDecryptedData = CreateRandomData();
            byte[] outputDecryptedData = randomDecryptedData;
            byte[] expectedDecryptedData = outputDecryptedData.DeepClone();

            this.cryptographyBroker.Setup(broker =>
                broker.DecryptAsync(inputEncryptedData))
                    .ReturnsAsync(outputDecryptedData);

            // When
            byte[] actualDecryptedData = await this.cryptographyService.DecryptAsync(inputEncryptedData);

            // Then
            actualDecryptedData.Should().BeEquivalentTo(expectedDecryptedData);

            this.cryptographyBroker.Verify(broker =>
                broker.DecryptAsync(inputEncryptedData),
                Times.Once);

            this.cryptographyBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

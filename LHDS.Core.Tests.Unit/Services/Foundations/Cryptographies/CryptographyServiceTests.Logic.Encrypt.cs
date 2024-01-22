// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        public async Task ShouldEncryptAsync()
        {
            // given
            byte[] randomDecryptedData = CreateRandomData();
            byte[] inputDecryptedData = randomDecryptedData;

            byte[] randomEncryptedData = CreateRandomData();
            byte[] outputEncryptedData = randomEncryptedData;
            byte[] expectedEncryptedData = outputEncryptedData.DeepClone();

            this.cryptographyBroker.Setup(broker =>
                broker.EncryptAsync(inputDecryptedData))
                    .ReturnsAsync(outputEncryptedData);

            // When
            byte[] actualEncryptedData = await this.cryptographyService.EncryptAsync(inputDecryptedData);

            // Then
            actualEncryptedData.Should().BeEquivalentTo(expectedEncryptedData);

            this.cryptographyBroker.Verify(broker =>
                broker.EncryptAsync(inputDecryptedData),
                Times.Once);

            this.cryptographyBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

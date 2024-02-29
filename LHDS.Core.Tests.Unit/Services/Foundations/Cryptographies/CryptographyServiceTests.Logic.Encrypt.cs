// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Processings.SubscriberCredentials;
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
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            byte[] randomEncryptedData = CreateRandomData();
            byte[] outputEncryptedData = randomEncryptedData;
            byte[] expectedEncryptedData = outputEncryptedData.DeepClone();

            this.cryptographyBroker.Setup(broker =>
                broker.EncryptAsync(inputDecryptedData, inputSubscriberCredential))
                    .ReturnsAsync(outputEncryptedData);

            // When
            byte[] actualEncryptedData = await this.cryptographyService
                .EncryptAsync(data: inputDecryptedData, subscriberCredential: inputSubscriberCredential);

            // Then
            actualEncryptedData.Should().BeEquivalentTo(expectedEncryptedData);

            this.cryptographyBroker.Verify(broker =>
                broker.EncryptAsync(inputDecryptedData, inputSubscriberCredential),
                Times.Once);

            this.cryptographyBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
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
            byte[] randomEncryptedData = CreateRandomData();
            byte[] inputDecryptedData = randomDecryptedData;
            Stream returnedEncryptedstream = new MemoryStream(randomEncryptedData);
            Stream inputDecryptedStream = new MemoryStream(inputDecryptedData);
            Stream outputEncryptedStream = new MemoryStream();

            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            byte[] expectedEncryptedData = randomEncryptedData.DeepClone();

            this.cryptographyBroker
                .Setup(broker => broker.EncryptAsync(
                    inputDecryptedStream,
                    outputEncryptedStream,
                    inputSubscriberCredential))
                .Callback<Stream, Stream, SubscriberCredential>((input, output, subscriber) =>
                    {
                        returnedEncryptedstream.CopyTo(output);
                    })
                .Returns(ValueTask.CompletedTask);

            // When
            await this.cryptographyService.EncryptAsync(
                input: inputDecryptedStream,
                output: outputEncryptedStream,
                subscriberCredential: inputSubscriberCredential);

            // Then
            byte[] actualEncryptedData = ReadAllBytesFromStream(outputEncryptedStream);
            actualEncryptedData.Should().BeEquivalentTo(expectedEncryptedData);

            this.cryptographyBroker.Verify(broker =>
                broker.EncryptAsync(
                    inputDecryptedStream,
                    It.IsAny<Stream>(),
                    inputSubscriberCredential),
                Times.Once);

            this.cryptographyBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

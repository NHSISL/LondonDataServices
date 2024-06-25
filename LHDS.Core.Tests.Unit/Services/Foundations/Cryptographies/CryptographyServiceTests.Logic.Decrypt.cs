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
        public async Task ShouldDecryptAsync()
        {
            // given
            byte[] randomEncryptedData = CreateRandomData();
            byte[] inputEncryptedData = randomEncryptedData;
            Stream inputEncryptedStream = new MemoryStream(inputEncryptedData);
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            byte[] randomDecryptedData = CreateRandomData();
            byte[] outputDecryptedData = randomDecryptedData;
            Stream returnedDecryptedStream = new MemoryStream(outputDecryptedData);
            Stream outputDecryptedStream = new MemoryStream();
            Stream initialOutputDecryptedStream = outputDecryptedStream.DeepClone();
            byte[] expectedDecryptedData = outputDecryptedData.DeepClone();

            this.cryptographyBroker
                .Setup(broker => broker.DecryptAsync(
                    inputEncryptedStream,
                    outputDecryptedStream,
                    inputSubscriberCredential))
                .Callback<Stream, Stream, SubscriberCredential>((inputStream, outputStream, subscriberCredential) =>
                    {
                        returnedDecryptedStream.CopyTo(outputStream);
                    })
                .Returns(ValueTask.CompletedTask);

            // When
            await this.cryptographyService.DecryptAsync(
                input: inputEncryptedStream,
                output: outputDecryptedStream,
                subscriberCredential: inputSubscriberCredential);

            // Then
            byte[] actualDecryptedData = ReadAllBytesFromStream(outputDecryptedStream);
            actualDecryptedData.Should().BeEquivalentTo(expectedDecryptedData);

            this.cryptographyBroker.Verify(broker =>
                broker.DecryptAsync(
                    inputEncryptedStream,
                    It.IsAny<Stream>(),
                    inputSubscriberCredential),
                        Times.Once);

            this.cryptographyBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

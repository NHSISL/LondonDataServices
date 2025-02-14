// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.OptOuts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.OptOuts
{
    public partial class OptOutTests
    {
        [Fact]
        public async Task ShouldRetrieveOptOutStatusAsyncAsync()
        {
            //Given
            Guid identifier = Guid.NewGuid();
            DateTimeOffset currentDateTimeOffset = DateTimeOffset.UtcNow;
            string timestamp = currentDateTimeOffset.ToString("yyyyMMddHHmmss");
            List<OptOutIdentifier> optOutIdentifiers = CreateRandomOptOutIdentifiersList();
            bool hasHeaderRecord = optOutConfiguration.OptOutFileHasHeader;
            bool shouldAddTrailingComma = optOutConfiguration.OptOutFileRequireTrailingComma;
            string csvData = GenerateCsv(optOutIdentifiers, hasHeaderRecord, shouldAddTrailingComma);
            byte[] optOutFile = Encoding.ASCII.GetBytes(csvData);
            Stream optOutStream = new MemoryStream(optOutFile);
            string fileName = GetRandomString();
            Stream stream = new MemoryStream(optOutFile);
            string expectedString = $"/out/{fileName}_{timestamp}_Response.csv";

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(currentDateTimeOffset);

            //When
            var actualString = await this.optOutClient.RetrieveOptOutStatusAsync(input: optOutStream, fileName);

            //Then
            actualString.Should().Be(expectedString);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.InsertFileAsync(It.IsAny<Stream>(), expectedString, It.IsAny<string>()),
                    Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}

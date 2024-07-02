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
            List<OptOutIdentifier> optOutIdentifiers = CreateRandomOptOutIdentifiersList();
            bool hasHeaderRecord = optOutConfiguration.OptOutFileHasHeader;
            Dictionary<string, int> fieldMappings = null;
            bool shouldAddTrailingComma = optOutConfiguration.OptOutFileRequireTrailingComma;
            string csvData = GenerateCsv(optOutIdentifiers, hasHeaderRecord, shouldAddTrailingComma);

            byte[] optOutFile = Encoding.ASCII.GetBytes(csvData);
            string fileName = GetRandomString();

            Stream stream = new MemoryStream(optOutFile);
            string expectedString = $"/out/{fileName}_Response.csv";

            //When
            var actualString = await this.optOutClient.RetrieveOptOutStatusAsync(stream, fileName);

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

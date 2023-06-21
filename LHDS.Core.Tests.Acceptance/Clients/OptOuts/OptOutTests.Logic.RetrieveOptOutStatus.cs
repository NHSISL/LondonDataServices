// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Clients;
using LHDS.Core.Models.Foundations.OptOuts;
using Moq;
using Xunit;
using Xunit.Sdk;

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
            bool shouldAddTrailingComma = optOutConfiguration.OptOutFileRequireTrailingComma;

            string csvData = await this.csvMapperBroker
                .MapObjectToCsvAsync(optOutIdentifiers, hasHeaderRecord, shouldAddTrailingComma);

            byte[] optOutFile = Encoding.ASCII.GetBytes(csvData);
            string fileName = GetRandomString();

            Stream stream = new MemoryStream(optOutFile);
            string expectedString = $"/out/{fileName}_Response.csv";

            //When
            var actualString = await this.optOutClient.RetrieveOptOutStatusAsync(optOutFile, fileName);

            //Then
            actualString.Should().Be(expectedString);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.InsertFileAsync(expectedString, It.IsAny<Stream>()),
                    Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}

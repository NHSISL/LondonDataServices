// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.OptOuts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.OptOuts
{
    public partial class OptOutTests
    {
        [Fact]
        public async Task ShouldRetrieveOptOutStatusAsync()
        {
            //Given
            Guid identifier = Guid.NewGuid();
            DateTimeOffset currentDateTimeOffset = DateTimeOffset.UtcNow;
            string timestamp = currentDateTimeOffset.ToString("yyyyMMddHHmmss");

            List<OptOut> optOutsStatusUnknown = CreateRandomOptOutsList(
                GetRandomNumber(),
                currentDateTimeOffset,
                timestamp,
                "Unknown");

            List<OptOut> optOutsStatusOptIn = CreateRandomOptOutsList(
                GetRandomNumber(),
                currentDateTimeOffset,
                timestamp,
                "Opt-In");

            List<OptOut> optOutsStatusOptOut = CreateRandomOptOutsList(
                GetRandomNumber(),
                currentDateTimeOffset,
                timestamp,
                "Opt-Out");

            List<OptOut> existingOptOuts = [.. optOutsStatusOptIn, .. optOutsStatusOptOut];
            List<OptOut> inputOptOuts = [.. existingOptOuts, .. optOutsStatusUnknown];
            List<OptOut> expectedOptOuts = inputOptOuts.DeepClone();

            var expectedStatusList = expectedOptOuts.Select(optOut =>
                new { optOut.UniqueReference, optOut.NhsNumber, optOut.Status }).ToList();

            bool hasHeaderRecord = optOutConfiguration.OptOutFileHasHeader;
            bool shouldAddTrailingComma = optOutConfiguration.OptOutFileRequireTrailingComma;
            string csvData = GenerateCsv(inputOptOuts, hasHeaderRecord, shouldAddTrailingComma);
            byte[] optOutFile = Encoding.ASCII.GetBytes(csvData);
            Stream optOutStream = new MemoryStream(optOutFile);
            string fileName = GetRandomString();
            Stream stream = new MemoryStream(optOutFile);

            foreach (OptOut optOut in existingOptOuts)
            {
                await this.optOutService.AddOptOutAsync(optOut);
            }

            //When
            await this.optOutClient
                .RetrieveOptOutStatusAsync(input: optOutStream, fileName, TestContext.Current.CancellationToken);

            //Then
            IQueryable<OptOut> actualOptOuts = await this.optOutService.RetrieveAllOptOutsAsync();
            List<OptOut> actualOptOutsList = actualOptOuts.ToList();

            var actualStatusList = actualOptOutsList.Select(optOut =>
                new { optOut.UniqueReference, optOut.NhsNumber, optOut.Status }).ToList();

            actualStatusList.Should().Contain(expectedStatusList);

            foreach (OptOut optOut in actualOptOutsList)
            {
                await this.optOutService.RemoveOptOutByIdAsync(optOut.Id);
            }

            this.blobStorageBrokerMock.Verify(broker =>
                broker.InsertFileAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>()),
                    Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}

// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.OptOuts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.CsvMappers
{
    public partial class CsvMapperTests
    {
        [Fact]
        public async Task ShouldMapCsvToObjectAsync()
        {
            // given
            string randomCsvFormattedOptOutData = GetRandomString();
            string inputCsvFormattedOptOutData = randomCsvFormattedOptOutData;
            List<OptOut> randomOptouts = CreateRandomOptOuts();
            List<OptOut> expectedOptOuts = randomOptouts;
            bool withHeaderRecord = true;

            this.csvMapperBrokerMock.Setup(broker =>
                broker.MapCsvToObjectAsync<OptOut>(inputCsvFormattedOptOutData, withHeaderRecord))
                    .ReturnsAsync(expectedOptOuts);

            // when
            List<OptOut> actualOptOuts = await this.csvMapperService.MapCsvToObjectAsync<OptOut>(
                data: inputCsvFormattedOptOutData,
                hasHeaderRecord: withHeaderRecord);

            // then
            actualOptOuts.Should().BeEquivalentTo(expectedOptOuts);

            this.csvMapperBrokerMock.Verify(broker =>
                broker.MapCsvToObjectAsync<OptOut>(inputCsvFormattedOptOutData, withHeaderRecord),
                    Times.Once());

            this.csvMapperBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

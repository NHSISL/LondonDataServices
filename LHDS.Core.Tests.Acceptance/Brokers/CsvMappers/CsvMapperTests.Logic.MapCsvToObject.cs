// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Tests.Acceptance.Brokers.CsvMappers.Models;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Brokers.CsvMappers
{
    public partial class CsvMapperTests
    {
        [Fact]
        public async Task ShouldMapCsvWithTrailingCommaToObjectAsync()
        {
            // given
            List<OptOutCsv> randomList = CreateRandomOptOuts();
            List<OptOutCsv> expectedList = randomList;
            string randomNhsNumbers = CreateRandomNhsNumberCsvListWithTrailingComma(randomList);

            // when
            List<OptOutCsv> actualList = await this.csvMapperBroker
                .MapCsvToObjectAsync<OptOutCsv>(data: randomNhsNumbers, hasHeaderRecord: false);

            // then
            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Fact]
        public async Task ShouldMapCsvToObjectAsync()
        {
            // given
            List<OptOutCsv> randomList = CreateRandomOptOuts();
            List<OptOutCsv> expectedList = randomList;
            string randomNhsNumbers = CreateRandomNhsNumberCsvList(randomList);

            // when
            List<OptOutCsv> actualList = await this.csvMapperBroker
                .MapCsvToObjectAsync<OptOutCsv>(data: randomNhsNumbers, hasHeaderRecord: false);

            // then
            actualList.Should().BeEquivalentTo(expectedList);
        }

    }
}

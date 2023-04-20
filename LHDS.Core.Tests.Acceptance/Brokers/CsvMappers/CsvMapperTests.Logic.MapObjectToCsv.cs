// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
        public async Task ShouldMapObjectToCsvAsync()
        {
            // given
            List<OptOutCsv> randomList = CreateRandomOptOuts();
            List<OptOutCsv> inputList = randomList;
            string randomNhsNumbers = CreateRandomNhsNumberCsvList(randomList);
            string expectedList = randomNhsNumbers;
            bool hasTrailingComma = false;

            // when
            string actualList = await this.csvMapperBroker
                .MapObjectToCsvAsync<OptOutCsv>(
                    @object: inputList,
                    addHeaderRecord: false,
                    shouldAddTrailingComma: hasTrailingComma);

            // then
            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Fact]
        public async Task ShouldMapObjectToCsvWithTrailingCommsAsync()
        {
            // given
            List<OptOutCsv> randomList = CreateRandomOptOuts();
            List<OptOutCsv> inputList = randomList;
            string randomNhsNumbers = CreateRandomNhsNumberCsvListWithTrailingComma(randomList);
            string expectedList = randomNhsNumbers;
            bool hasTrailingComma = true;

            // when
            string actualList = await this.csvMapperBroker
                .MapObjectToCsvAsync<OptOutCsv>(
                    @object: inputList,
                    addHeaderRecord: false,
                    shouldAddTrailingComma: hasTrailingComma);

            // then
            actualList.Should().BeEquivalentTo(expectedList);
        }
    }
}

// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Foundations.CsvHelpers
{
    public partial class CsvHelperServiceTests
    {
        [Fact]
        public async Task ShouldMapCsvToObjectAsync()
        {
            // given
            string randomCsv = GetRandomString();
            string inputCsv = randomCsv;
            Dictionary<string, int> emptyDict = new Dictionary<string, int>();
            dynamic expectedResult = new ExpandoObject();
            expectedResult.test = "test";
            List<dynamic> expectedResults = new List<dynamic> { expectedResult };

            this.csvHelperBrokerMock.Setup(broker =>
                broker.MapCsvToObjectAsync<dynamic>(inputCsv,true,emptyDict))
                    .ReturnsAsync(expectedResults);

            // when
            List<dynamic> actualResult = 
                await this.csvHelperService.MapCsvToObjectAsync<dynamic>(inputCsv, true, emptyDict);

            // then
            actualResult.Should().BeEquivalentTo(expectedResults);

            this.csvHelperBrokerMock.Verify(broker =>
                broker.MapCsvToObjectAsync<dynamic>(inputCsv, true, emptyDict),
                    Times.Once);

            this.csvHelperBrokerMock.VerifyNoOtherCalls();
        }
    }
}

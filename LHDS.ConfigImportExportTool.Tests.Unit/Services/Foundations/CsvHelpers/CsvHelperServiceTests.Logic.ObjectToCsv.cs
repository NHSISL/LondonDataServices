// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Foundations.CsvHelpers
{
    public partial class CsvHelperServiceTests
    {
        [Fact]
        public async Task ShouldMapObjectToCsvAsync()
        {
            // given
            string randomCsv = GetRandomString();
            string expectedResult = randomCsv;
            Dictionary<string, int> emptyDict = new Dictionary<string, int>();
            dynamic inputObject = new ExpandoObject();
            inputObject.test = "test";
            List<dynamic> inputObjectList = new List<dynamic> { inputObject };

            this.csvHelperBrokerMock.Setup(broker =>
                broker.MapObjectToCsvAsync<dynamic>(inputObjectList, true, emptyDict, false))
                    .ReturnsAsync(expectedResult);

            // when
            string actualResult =
                await this.csvHelperService.MapObjectToCsvAsync<dynamic>(inputObjectList, true, emptyDict, false);

            // then
            actualResult.Should().BeEquivalentTo(expectedResult);

            this.csvHelperBrokerMock.Verify(broker =>
                broker.MapObjectToCsvAsync<dynamic>(inputObjectList, true, emptyDict, false),
                    Times.Once);

            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

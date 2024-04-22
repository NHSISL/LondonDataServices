// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.OptOuts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.CsvMappers
{
    public partial class CsvMapperTests
    {
        [Fact]
        public async Task ShouldMapObjectToCsvAsync()
        {
            // given
            List<OptOut> randomOptouts = CreateRandomOptOuts();
            List<OptOut> inputOptOuts = randomOptouts;
            string randomCsvFormattedOptOutData = GetRandomString();
            string expectedCsvFormattedOptOutData = randomCsvFormattedOptOutData;
            bool withHeaderRecord = true;
            bool shouldAddTrailingComma = true;

            this.csvMapperServiceMock.Setup(service =>
                service.MapObjectToCsvAsync<OptOut>(inputOptOuts, withHeaderRecord, shouldAddTrailingComma))
                    .ReturnsAsync(expectedCsvFormattedOptOutData);

            // when
            string actualCsvFormattedOptOutData = await this.csvMapperProcessingService.MapObjectToCsvAsync<OptOut>(
                @object: inputOptOuts,
                addHeaderRecord: withHeaderRecord,
                shouldAddTrailingComma);

            // then
            actualCsvFormattedOptOutData.Should().BeEquivalentTo(expectedCsvFormattedOptOutData);

            this.csvMapperServiceMock.Verify(service =>
                service.MapObjectToCsvAsync<OptOut>(inputOptOuts, withHeaderRecord, shouldAddTrailingComma),
                    Times.Once());

            this.csvMapperServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

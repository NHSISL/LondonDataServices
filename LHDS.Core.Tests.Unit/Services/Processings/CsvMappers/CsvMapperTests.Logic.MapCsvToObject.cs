// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
        public async Task ShouldMapCsvToObjectAsync()
        {
            // given
            string randomString = GetRandomString();
            string inputString = randomString;
            byte[] randomBytes = System.Text.Encoding.UTF8.GetBytes(inputString);
            byte[] inputBytes = randomBytes;

            List<OptOut> randomOptouts = CreateRandomOptOuts();
            List<OptOut> expectedOptOuts = randomOptouts;
            bool withHeaderRecord = true;

            this.csvMapperServiceMock.Setup(service =>
                service.MapCsvToObjectAsync<OptOut>(inputString, withHeaderRecord))
                    .ReturnsAsync(expectedOptOuts);

            // when
            List<OptOut> actualOptOuts = await this.csvMapperProcessingService.MapCsvToObjectAsync<OptOut>(
                data: inputString,
                hasHeaderRecord: withHeaderRecord);

            // then
            actualOptOuts.Should().BeEquivalentTo(expectedOptOuts);

            this.csvMapperServiceMock.Verify(service =>
                service.MapCsvToObjectAsync<OptOut>(inputString, withHeaderRecord),
                    Times.Once());

            this.csvMapperServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

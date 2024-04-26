// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Tests.Unit.Models.Foundations.CsvMappers;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.CsvMappers
{
    public partial class CsvMapperTests
    {
        [Theory]
        [InlineData(true, false)]
        [InlineData(false, false)]
        [InlineData(true, true)]
        [InlineData(false, true)]
        public async Task ShouldMapObjectToCsvWithoutFieldMappingsAsync(
            bool withHeader,
            bool withTrailingComma)
        {
            // given
            int count = GetRandomNumber();
            List<Car> randomCars = CreateRandomCars();

            string randomCsvFormattedcars = GetCsvRepresentationOfCar(
                cars: randomCars,
                hasHeaderRow: withHeader,
                shouldAddTrailingComma: withTrailingComma);

            string expectedCsvFormattedCars = randomCsvFormattedcars.DeepClone();

            List<Car> inputCars = randomCars;
            Dictionary<string, int> fieldMappings = null;

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = withHeader,
                MissingFieldFound = null
            };

            using StringWriter stringWriter = new StringWriter();
            using CsvWriter writer = new CsvWriter(stringWriter, config);

            this.csvMapperBrokerMock.Setup(broker =>
                broker.CreateCsvWriter(stringWriter, withHeader))
                    .Returns(writer);

            // when
            string actualCsvFormattedCars = await this.csvMapperService.MapObjectToCsvAsync<Car>(
                @object: inputCars,
                hasHeaderRecord: withHeader,
                fieldMappings,
                shouldAddTrailingComma: withTrailingComma);

            // then
            actualCsvFormattedCars.Should().BeEquivalentTo(expectedCsvFormattedCars);

            this.csvMapperBrokerMock.Verify(broker =>
                broker.CreateCsvWriter(stringWriter, withHeader),
                    Times.Once());

            this.csvMapperBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

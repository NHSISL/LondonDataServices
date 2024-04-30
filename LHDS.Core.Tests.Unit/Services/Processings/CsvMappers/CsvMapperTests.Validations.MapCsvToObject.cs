// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.CsvMappers.Exceptions;
using LHDS.Core.Models.Foundations.OptOuts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.CsvMappers
{
    public partial class CsvMapperTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnMapCsvToObjectIfInputsIsInvalidAndLogItAsync(
            string invalidString)
        {
            // given
            string inputString = invalidString;

            List<OptOut> randomOptouts = CreateRandomOptOuts();
            List<OptOut> expectedOptOuts = randomOptouts;
            bool withHeaderRecord = true;
            Dictionary<string, int> fieldMappings = null;

            this.csvMapperServiceMock.Setup(service =>
                service.MapCsvToObjectAsync<OptOut>(inputString, withHeaderRecord, fieldMappings))
                    .ReturnsAsync(expectedOptOuts);

            var invalidCsvMapperArgumentsException = new InvalidCsvMapperArgumentsException(
                message: "Invalid CSV mapper arguments. Please fix the errors and try again.");

            invalidCsvMapperArgumentsException.AddData(
                key: "Data",
                values: "Text is required");

            var expectedCsvMapperValidationException =
                new CsvMapperValidationException(
                    message: "CSV mapper validation errors occurred, fix the errors and try again.",
                    innerException: invalidCsvMapperArgumentsException);

            // when
            ValueTask<List<OptOut>> mapCsvToObjectTask = this.csvMapperProcessingService.MapCsvToObjectAsync<OptOut>(
                data: invalidString,
                hasHeaderRecord: withHeaderRecord,
                fieldMappings);

            CsvMapperValidationException actualCsvMapperValidationException =
                await Assert.ThrowsAsync<CsvMapperValidationException>(mapCsvToObjectTask.AsTask);

            // then
            actualCsvMapperValidationException.Should().BeEquivalentTo(expectedCsvMapperValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCsvMapperValidationException))),
                        Times.Once);

            this.csvMapperServiceMock.Verify(service =>
                service.MapCsvToObjectAsync<OptOut>(
                    inputString,
                    withHeaderRecord,
                    fieldMappings),
                        Times.Never());

            this.csvMapperServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

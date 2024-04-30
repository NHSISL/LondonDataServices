// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.CsvMappers.Exceptions;
using LHDS.Core.Tests.Unit.Models.Foundations.CsvMappers;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.CsvMappers
{
    public partial class CsvMapperTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnMapCsvToObjectIfInputsIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            string randomCsvFormattedOptOutData = invalidText;
            string inputCsvFormattedOptOutData = randomCsvFormattedOptOutData;
            bool withHeaderRecord = true;

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
            ValueTask<List<Car>> mapCsvToObjectTask = this.csvMapperService.MapCsvToObjectAsync<Car>(
                data: inputCsvFormattedOptOutData,
                hasHeaderRecord: withHeaderRecord);

            CsvMapperValidationException actualCsvMapperValidationException =
                await Assert.ThrowsAsync<CsvMapperValidationException>(mapCsvToObjectTask.AsTask);

            // then
            actualCsvMapperValidationException.Should().BeEquivalentTo(expectedCsvMapperValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCsvMapperValidationException))),
                        Times.Once);

            this.csvMapperBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

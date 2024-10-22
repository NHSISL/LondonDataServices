// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Foundations.CsvHelpers.Exceptions;
using Moq;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Foundations.CsvHelpers
{
    public partial class CsvHelperServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnMapCsvToObjectIfDataIsInvalidAsync(string invalidData)
        {
            // given
            var invalidArgumentCsvHelperException =
                new InvalidArgumentCsvHelperException(
                    message: "Invalid csv helper argument(s), please correct the errors and try again.");

            invalidArgumentCsvHelperException.AddData(
                key: "data",
                values: "Text is required");

            var expectedCsvHelperValidationException =
                new CsvHelperValidationException(
                    message: "Csv helper validation error occurred, fix the errors and try again.",
                    innerException: invalidArgumentCsvHelperException);

            // when
            ValueTask<List<dynamic>> mapCsvToObjectTask =
                this.csvHelperService.MapCsvToObjectAsync<dynamic>(
                    invalidData, 
                    It.IsAny<bool>(), 
                    It.IsAny<Dictionary<string, int>>());

            CsvHelperValidationException actualException =
                await Assert.ThrowsAsync<CsvHelperValidationException>(mapCsvToObjectTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedCsvHelperValidationException);

            this.csvHelperBrokerMock.Verify(broker =>
                broker.MapCsvToObjectAsync<dynamic>(
                    invalidData,
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>()),
                        Times.Never);

            this.csvHelperBrokerMock.VerifyNoOtherCalls();
        }
    }
}

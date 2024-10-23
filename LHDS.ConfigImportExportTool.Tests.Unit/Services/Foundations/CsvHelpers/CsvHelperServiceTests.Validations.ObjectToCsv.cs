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
        [Fact]
        public async Task ShouldThrowValidationExceptionOnMapObjectToCsvIfObjectListIsNullAndLogItAsync()
        {
            // given
            List<dynamic> nullObjectList = null;

            var invalidArgumentCsvHelperException =
                new InvalidArgumentCsvHelperException(
                    message: "Invalid csv helper argument(s), please correct the errors and try again.");

            var expectedCsvHelperValidationException =
                new CsvHelperValidationException(
                    message: "Csv helper validation error occurred, fix the errors and try again.",
                    innerException: invalidArgumentCsvHelperException);

            // when
            ValueTask<string> mapObjectToCsvTask =
                this.csvHelperService.MapObjectToCsvAsync<dynamic>(
                    nullObjectList,
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>(),
                    It.IsAny<bool>());

            CsvHelperValidationException actualCsvHelperServiceException =
                await Assert.ThrowsAsync<CsvHelperValidationException>(mapObjectToCsvTask.AsTask);

            // then
            actualCsvHelperServiceException.Should()
                .BeEquivalentTo(expectedCsvHelperValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedCsvHelperValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
        }

    }
}

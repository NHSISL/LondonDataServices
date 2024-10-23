// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public async Task ShouldThrowServiceExceptionOnMapCsvToObjectIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string someData = GetRandomString();
            var serviceException = new Exception();

            var failedCsvHelperServiceException =
                new FailedCsvHelperServiceException(
                    message: "Failed csv helper service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedCsvHelperServiceException =
                new CsvHelperServiceException(
                    message: "Csv helper service error occurred, please contact support.",
                    innerException: failedCsvHelperServiceException);

            this.csvHelperBrokerMock.Setup(broker =>
                broker.MapCsvToObjectAsync<dynamic>(
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>()))
                        .ThrowsAsync(serviceException);

            // when
            ValueTask<List<dynamic>> mapCsvToObjectTask =
                this.csvHelperService.MapCsvToObjectAsync<dynamic>(
                    someData,
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>());

            CsvHelperServiceException actualCsvHelperServiceException =
                await Assert.ThrowsAsync<CsvHelperServiceException>(mapCsvToObjectTask.AsTask);

            // then
            actualCsvHelperServiceException.Should()
                .BeEquivalentTo(expectedCsvHelperServiceException);

            this.csvHelperBrokerMock.Verify(broker =>
                broker.MapCsvToObjectAsync<dynamic>(
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedCsvHelperServiceException))),
                        Times.Once);

            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
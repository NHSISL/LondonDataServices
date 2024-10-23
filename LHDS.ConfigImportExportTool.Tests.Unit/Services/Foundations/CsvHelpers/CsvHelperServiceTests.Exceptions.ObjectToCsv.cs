// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Dynamic;
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
        public async Task ShouldThrowServiceExceptionOnMapObjectToCsvIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();
            dynamic inputObject = new ExpandoObject();
            inputObject.test = "test";
            List<dynamic> inputObjectList = new List<dynamic> { inputObject };

            var failedCsvHelperServiceException =
                new FailedCsvHelperServiceException(
                    message: "Failed csv helper service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedCsvHelperServiceException =
                new CsvHelperServiceException(
                    message: "Csv helper service error occurred, please contact support.",
                    innerException: failedCsvHelperServiceException);

            this.csvHelperBrokerMock.Setup(broker =>
                broker.MapObjectToCsvAsync<dynamic>(
                    inputObjectList,
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>(),
                    It.IsAny<bool>()))
                        .ThrowsAsync(serviceException);

            // when
            ValueTask<string> mapObjectToCsvTask =
                this.csvHelperService.MapObjectToCsvAsync<dynamic>(
                    inputObjectList,
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>(),
                    It.IsAny<bool>());

            CsvHelperServiceException actualCsvHelperServiceException =
                await Assert.ThrowsAsync<CsvHelperServiceException>(mapObjectToCsvTask.AsTask);

            // then
            actualCsvHelperServiceException.Should()
                .BeEquivalentTo(expectedCsvHelperServiceException);

            this.csvHelperBrokerMock.Verify(broker =>
                broker.MapObjectToCsvAsync<dynamic>(
                    inputObjectList,
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>(),
                    It.IsAny<bool>()),
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
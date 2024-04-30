// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.CsvMappers.Exceptions;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Tests.Unit.Models.Foundations.CsvMappers;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.CsvMappers
{
    public partial class CsvMapperTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnMapCsvToObjectIfServiceErrorOccursAndLogItAsync()
        {
            // given
            int count = GetRandomNumber();
            List<Car> randomCars = CreateRandomCars();
            bool hasHeaderRow = true;
            bool shouldAddTrailingComma = true;

            string inputCsvFormattedOptOutData =
                GetCsvRepresentationOfCar(cars: randomCars, hasHeaderRow, shouldAddTrailingComma);

            Dictionary<string, int> fieldMappings = null;
            var serviceException = new Exception();

            var failedCsvMapperServiceException =
                new FailedCsvMapperServiceException(
                    message: "Failed CSV mapper service error occurred, contact support.",
                    innerException: serviceException);

            var expectedCsvMapperServiceException =
                new CsvMapperServiceException(
                    message: "CSV mapper service error occurred, contact support.",
                    innerException: failedCsvMapperServiceException);

            this.csvMapperBrokerMock.Setup(broker =>
                broker.CreateCsvReader(It.IsAny<StringReader>(), It.IsAny<bool>()))
                    .Throws(serviceException);

            // when
            ValueTask<List<OptOut>> mapCsvToObjectTask = this.csvMapperService.MapCsvToObjectAsync<OptOut>(
                data: inputCsvFormattedOptOutData,
                hasHeaderRow,
                fieldMappings);

            CsvMapperServiceException actualCsvMapperServiceException =
                await Assert.ThrowsAsync<CsvMapperServiceException>(mapCsvToObjectTask.AsTask);

            // then
            actualCsvMapperServiceException.Should().BeEquivalentTo(expectedCsvMapperServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCsvMapperServiceException))),
                        Times.Once);

            this.csvMapperBrokerMock.Verify(broker =>
                broker.CreateCsvReader(It.IsAny<StringReader>(), It.IsAny<bool>()),
                    Times.Once());

            this.csvMapperBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
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
        [Fact]

        public async Task ShouldThrowServiceExceptionOnMapObjectToCsvIfServiceErrorOccursAndLogItAsync()
        {
            // given
            int count = GetRandomNumber();
            List<Car> randomCars = CreateRandomCars();
            bool withHeaderRecord = true;
            Dictionary<string, int> fieldMappings = null;
            bool shouldAddTrailingComma = true;
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
                broker.CreateCsvWriter(It.IsAny<StringWriter>(), It.IsAny<bool>()))
                    .Throws(serviceException);

            // when
            ValueTask<string> mapCsvToObjectTask = this.csvMapperService.MapObjectToCsvAsync<Car>(
                @object: randomCars,
                hasHeaderRecord: withHeaderRecord,
                fieldMappings,
                shouldAddTrailingComma);

            CsvMapperServiceException actualCsvMapperServiceException =
                await Assert.ThrowsAsync<CsvMapperServiceException>(mapCsvToObjectTask.AsTask);

            // then
            actualCsvMapperServiceException.Should().BeEquivalentTo(expectedCsvMapperServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCsvMapperServiceException))),
                        Times.Once);

            this.csvMapperBrokerMock.Verify(broker =>
                broker.CreateCsvWriter(It.IsAny<StringWriter>(), It.IsAny<bool>()),
                        Times.Once());

            this.csvMapperBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

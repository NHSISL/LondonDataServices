// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.CsvMappers.Exceptions;
using LHDS.Core.Models.Foundations.OptOuts;
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
            string randomCsvFormattedOptOutData = GetRandomString();
            string inputCsvFormattedOptOutData = randomCsvFormattedOptOutData;
            bool withHeaderRecord = true;
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
                broker.MapCsvToObjectAsync<OptOut>(inputCsvFormattedOptOutData, withHeaderRecord))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<List<OptOut>> mapCsvToObjectTask = this.csvMapperService.MapCsvToObjectAsync<OptOut>(
                data: inputCsvFormattedOptOutData,
                hasHeaderRecord: withHeaderRecord);

            CsvMapperServiceException actualCsvMapperServiceException =
                await Assert.ThrowsAsync<CsvMapperServiceException>(mapCsvToObjectTask.AsTask);

            // then
            actualCsvMapperServiceException.Should().BeEquivalentTo(expectedCsvMapperServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCsvMapperServiceException))),
                        Times.Once);

            this.csvMapperBrokerMock.Verify(broker =>
                broker.MapCsvToObjectAsync<OptOut>(inputCsvFormattedOptOutData, withHeaderRecord),
                    Times.Once());

            this.csvMapperBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

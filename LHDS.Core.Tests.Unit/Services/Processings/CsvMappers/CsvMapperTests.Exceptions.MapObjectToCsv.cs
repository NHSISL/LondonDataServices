// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
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
        [Fact]
        public async Task ShouldThrowServiceExceptionOnMapObjectToCsvIfServiceErrorOccursAndLogItAsync()
        {
            // given
            List<OptOut> randomOptOuts = CreateRandomOptOuts();
            List<OptOut> inputOptOuts = randomOptOuts;
            bool withHeaderRecord = true;
            var serviceException = new Exception();

            var failedCsvMapperServiceException =
                new FailedCsvMapperServiceException(serviceException);

            var expectedCsvMapperServiceException =
                new CsvMapperServiceException(failedCsvMapperServiceException);

            this.csvMapperServiceMock.Setup(service =>
                service.MapObjectToCsvAsync<OptOut>(inputOptOuts, withHeaderRecord))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<string> mapCsvToObjectTask = this.csvMapperProcessingService.MapObjectToCsvAsync<OptOut>(
                @object: inputOptOuts,
                addHeaderRecord: withHeaderRecord);

            CsvMapperServiceException actualCsvMapperServiceException =
                await Assert.ThrowsAsync<CsvMapperServiceException>(mapCsvToObjectTask.AsTask);

            // then
            actualCsvMapperServiceException.Should().BeEquivalentTo(expectedCsvMapperServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCsvMapperServiceException))),
                        Times.Once);

            this.csvMapperServiceMock.Verify(service =>
                service.MapObjectToCsvAsync<OptOut>(inputOptOuts, withHeaderRecord),
                    Times.Once());

            this.csvMapperServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

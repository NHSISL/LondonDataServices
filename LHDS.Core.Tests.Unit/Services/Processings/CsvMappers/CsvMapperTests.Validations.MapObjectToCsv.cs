// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
        public async Task ShouldThrowValidationExceptionOnMapObjectToCsvIfInputsIsInvalidAndLogItAsync()
        {
            // given
            List<OptOut> nullOptOuts = null;
            string randomString = GetRandomString();
            string outputString = randomString;
            bool withHeaderRecord = true;

            this.csvMapperServiceMock.Setup(service =>
                service.MapObjectToCsvAsync<OptOut>(nullOptOuts, withHeaderRecord))
                    .ReturnsAsync(outputString);

            var invalidCsvMapperArgumentsException = new InvalidCsvMapperArgumentsException();

            invalidCsvMapperArgumentsException.AddData(
                key: "Object",
                values: "Object is required");

            var expectedCsvMapperValidationException =
                new CsvMapperValidationException(innerException: invalidCsvMapperArgumentsException);

            // when
            ValueTask<string> mapObjectToCsvTask = this.csvMapperProcessingService.MapObjectToCsvAsync<OptOut>(
                @object: nullOptOuts,
                addHeaderRecord: withHeaderRecord);

            CsvMapperValidationException actualCsvMapperValidationException =
                await Assert.ThrowsAsync<CsvMapperValidationException>(mapObjectToCsvTask.AsTask);

            // then
            actualCsvMapperValidationException.Should().BeEquivalentTo(expectedCsvMapperValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCsvMapperValidationException))),
                        Times.Once);

            this.csvMapperServiceMock.Verify(service =>
                service.MapObjectToCsvAsync<OptOut>(nullOptOuts, withHeaderRecord),
                    Times.Never());

            this.csvMapperServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

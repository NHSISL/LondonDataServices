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

namespace LHDS.Core.Tests.Unit.Services.Foundations.CsvMappers
{
    public partial class CsvMapperTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnMapObjectToCsvIfInputsIsInvalidAndLogItAsync()
        {
            // given
            List<OptOut> nullOptOuts = null;
            string randomCsvFormattedOptOutData = GetRandomString();
            string expectedCsvFormattedOptOutData = randomCsvFormattedOptOutData;
            bool withHeaderRecord = true;
            bool shouldAddTrailingComma = true;

            this.csvMapperBrokerMock.Setup(broker =>
                broker.MapObjectToCsvAsync<OptOut>(nullOptOuts, withHeaderRecord, shouldAddTrailingComma))
                    .ReturnsAsync(expectedCsvFormattedOptOutData);

            var invalidCsvMapperArgumentsException = new InvalidCsvMapperArgumentsException();

            invalidCsvMapperArgumentsException.AddData(
                key: "Object",
                values: "Object is required");

            var expectedCsvMapperValidationException =
                new CsvMapperValidationException(innerException: invalidCsvMapperArgumentsException);

            // when
            ValueTask<string> mapObjectToCsvTask = this.csvMapperService.MapObjectToCsvAsync<OptOut>(
                @object: nullOptOuts,
                addHeaderRecord: withHeaderRecord,
                shouldAddTrailingComma);

            CsvMapperValidationException actualCsvMapperValidationException =
                await Assert.ThrowsAsync<CsvMapperValidationException>(mapObjectToCsvTask.AsTask);

            // then
            actualCsvMapperValidationException.Should().BeEquivalentTo(expectedCsvMapperValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCsvMapperValidationException))),
                        Times.Once);

            this.csvMapperBrokerMock.Verify(broker =>
                broker.MapObjectToCsvAsync<OptOut>(nullOptOuts, withHeaderRecord, shouldAddTrailingComma),
                    Times.Never());

            this.csvMapperBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

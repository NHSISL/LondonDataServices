// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
            Dictionary<string, int> fieldMappings = null;
            bool shouldAddTrailingComma = true;

            this.csvMapperServiceMock.Setup(service =>
                service.MapObjectToCsvAsync<OptOut>(
                    nullOptOuts,
                    withHeaderRecord,
                    fieldMappings,
                    shouldAddTrailingComma))
                        .ReturnsAsync(outputString);

            var invalidCsvMapperArgumentsException = new InvalidCsvMapperArgumentsException(
                message: "Invalid CSV mapper arguments. Please fix the errors and try again.");

            invalidCsvMapperArgumentsException.AddData(
                key: "Object",
                values: "Object is required");

            var expectedCsvMapperValidationException =
                new CsvMapperValidationException(
                    message: "CSV mapper validation errors occurred, fix the errors and try again.",
                    innerException: invalidCsvMapperArgumentsException);

            // when
            ValueTask<string> mapObjectToCsvTask = this.csvMapperProcessingService.MapObjectToCsvAsync<OptOut>(
                @object: nullOptOuts,
                addHeaderRecord: withHeaderRecord,
                fieldMappings,
                shouldAddTrailingComma);

            CsvMapperValidationException actualCsvMapperValidationException =
                await Assert.ThrowsAsync<CsvMapperValidationException>(mapObjectToCsvTask.AsTask);

            // then
            actualCsvMapperValidationException.Should().BeEquivalentTo(expectedCsvMapperValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCsvMapperValidationException))),
                        Times.Once);

            this.csvMapperServiceMock.Verify(service =>
                service.MapObjectToCsvAsync<OptOut>(
                    nullOptOuts,
                    withHeaderRecord,
                    fieldMappings,
                    shouldAddTrailingComma),
                        Times.Never());

            this.csvMapperServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

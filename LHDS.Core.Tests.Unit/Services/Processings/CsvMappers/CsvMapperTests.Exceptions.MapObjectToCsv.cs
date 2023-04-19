// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.CsvMappers.Exceptions;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Processings.CsvMapper.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.CsvMappers
{
    public partial class CsvMapperTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnMapObjectToCsvIfDependencyValidationOccursAndLogItAsync(
            Xeption dependancyValidationException)
        {
            // given
            List<OptOut> randomOptOuts = CreateRandomOptOuts();
            List<OptOut> inputOptOuts = randomOptOuts;
            bool withHeaderRecord = true;

            var expectedCsvMapperProcessingDependencyValidationException =
                new CsvMapperProcessingDependencyValidationException(
                    dependancyValidationException.InnerException as Xeption);

            this.csvMapperServiceMock.Setup(service =>
                service.MapCsvToObjectAsync<OptOut>(It.IsAny<string>(), It.IsAny<bool>()))
                    .ThrowsAsync(dependancyValidationException);

            // when
            ValueTask<string> mapCsvToObjectTask = this.csvMapperProcessingService.MapObjectToCsvAsync<OptOut>(
                @object: inputOptOuts,
                addHeaderRecord: withHeaderRecord);

            CsvMapperProcessingDependencyValidationException actualCsvMapperProcessingDependencyValidationException =
                await Assert.ThrowsAsync<CsvMapperProcessingDependencyValidationException>(mapCsvToObjectTask.AsTask);

            // then
            actualCsvMapperProcessingDependencyValidationException.Should()
                .BeEquivalentTo(expectedCsvMapperProcessingDependencyValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCsvMapperProcessingDependencyValidationException))),
                        Times.Once);

            this.csvMapperServiceMock.Verify(service =>
                service.MapCsvToObjectAsync<OptOut>(It.IsAny<string>(), It.IsAny<bool>()),
                    Times.Once());

            this.csvMapperServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnMapObjectToCsvIfDependencyExceptionOccursAndLogItAsync(
           Xeption dependancyException)
        {
            // given
            List<OptOut> randomOptOuts = CreateRandomOptOuts();
            List<OptOut> inputOptOuts = randomOptOuts;
            bool withHeaderRecord = true;

            var expectedCsvMapperProcessingDependencyException =
                new CsvMapperProcessingDependencyException(
                    dependancyException.InnerException as Xeption);

            this.csvMapperServiceMock.Setup(service =>
                service.MapCsvToObjectAsync<OptOut>(It.IsAny<string>(), It.IsAny<bool>()))
                    .ThrowsAsync(dependancyException);

            // when
            ValueTask<string> mapCsvToObjectTask = this.csvMapperProcessingService.MapObjectToCsvAsync<OptOut>(
                @object: inputOptOuts,
                addHeaderRecord: withHeaderRecord);

            CsvMapperProcessingDependencyException actualCsvMapperProcessingDependencyException =
                await Assert.ThrowsAsync<CsvMapperProcessingDependencyException>(mapCsvToObjectTask.AsTask);

            // then
            actualCsvMapperProcessingDependencyException.Should().BeEquivalentTo(expectedCsvMapperProcessingDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCsvMapperProcessingDependencyException))),
                        Times.Once);

            this.csvMapperServiceMock.Verify(service =>
                service.MapCsvToObjectAsync<OptOut>(It.IsAny<string>(), It.IsAny<bool>()),
                    Times.Once());

            this.csvMapperServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

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

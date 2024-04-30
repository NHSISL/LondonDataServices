// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.CsvMappers.Exceptions;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Processings.CsvMappers.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.CsvMappers
{
    public partial class CsvMapperTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnMapCsvToObjectIfDependencyValidationOccursAndLogItAsync(
            Xeption dependancyValidationException)
        {
            // given
            string randomString = GetRandomString();
            string inputString = randomString;
            bool withHeaderRecord = true;
            Dictionary<string, int> fieldMappings = null;

            var expectedCsvMapperProcessingDependencyValidationException =
                new CsvMapperProcessingDependencyValidationException(
                    message: "Csv Mapper processing dependency validation occurred, please try again.",
                    dependancyValidationException.InnerException as Xeption);

            this.csvMapperServiceMock.Setup(service =>
                service.MapCsvToObjectAsync<OptOut>(inputString, withHeaderRecord, fieldMappings))
                    .ThrowsAsync(dependancyValidationException);

            // when
            ValueTask<List<OptOut>> mapCsvToObjectTask = this.csvMapperProcessingService.MapCsvToObjectAsync<OptOut>(
                data: inputString,
                hasHeaderRecord: withHeaderRecord);

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
                service.MapCsvToObjectAsync<OptOut>(inputString, withHeaderRecord, fieldMappings),
                    Times.Once());

            this.csvMapperServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnMapCsvToObjectIfDependencyExceptionOccursAndLogItAsync(
           Xeption dependancyException)
        {
            // given
            string randomString = GetRandomString();
            string inputString = randomString;
            bool withHeaderRecord = true;
            Dictionary<string, int> fieldMappings = null;

            var expectedCsvMapperProcessingDependencyException =
                new CsvMapperProcessingDependencyException(
                    message: "Csv Mapper processing dependency validation occurred, please try again.",
                    dependancyException.InnerException as Xeption);

            this.csvMapperServiceMock.Setup(service =>
                service.MapCsvToObjectAsync<OptOut>(inputString, withHeaderRecord, fieldMappings))
                    .ThrowsAsync(dependancyException);

            // when
            ValueTask<List<OptOut>> mapCsvToObjectTask = this.csvMapperProcessingService.MapCsvToObjectAsync<OptOut>(
                data: inputString,
                hasHeaderRecord: withHeaderRecord);

            CsvMapperProcessingDependencyException actualCsvMapperProcessingDependencyException =
                await Assert.ThrowsAsync<CsvMapperProcessingDependencyException>(mapCsvToObjectTask.AsTask);

            // then
            actualCsvMapperProcessingDependencyException.Should()
                .BeEquivalentTo(expectedCsvMapperProcessingDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCsvMapperProcessingDependencyException))),
                        Times.Once);

            this.csvMapperServiceMock.Verify(service =>
                service.MapCsvToObjectAsync<OptOut>(inputString, withHeaderRecord, fieldMappings),
                    Times.Once());

            this.csvMapperServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnMapCsvToObjectIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string randomString = GetRandomString();
            string inputString = randomString;
            byte[] randomBytes = System.Text.Encoding.UTF8.GetBytes(inputString);
            byte[] inputBytes = randomBytes;
            bool withHeaderRecord = true;
            Dictionary<string, int> fieldMappings = null;
            var serviceException = new Exception();

            var failedCsvMapperServiceException =
                new FailedCsvMapperServiceException(
                    message: "Failed CSV mapper service error occurred, contact support.",
                    innerException: serviceException);

            var expectedCsvMapperServiceException =
                new CsvMapperProcessingServiceException(
                    message: "Csv Mapper processing service error occurred, contact support.",
                    failedCsvMapperServiceException);

            this.csvMapperServiceMock.Setup(service =>
                service.MapCsvToObjectAsync<OptOut>(inputString, withHeaderRecord, fieldMappings))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<List<OptOut>> mapCsvToObjectTask = this.csvMapperProcessingService.MapCsvToObjectAsync<OptOut>(
                data: inputString,
                hasHeaderRecord: withHeaderRecord);

            CsvMapperProcessingServiceException actualCsvMapperServiceException =
                await Assert.ThrowsAsync<CsvMapperProcessingServiceException>(mapCsvToObjectTask.AsTask);

            // then
            actualCsvMapperServiceException.Should().BeEquivalentTo(expectedCsvMapperServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCsvMapperServiceException))),
                        Times.Once);

            this.csvMapperServiceMock.Verify(service =>
                service.MapCsvToObjectAsync<OptOut>(inputString, withHeaderRecord, fieldMappings),
                    Times.Once());

            this.csvMapperServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

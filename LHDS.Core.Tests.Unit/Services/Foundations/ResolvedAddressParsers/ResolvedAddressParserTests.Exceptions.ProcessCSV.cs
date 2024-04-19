// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddressParsers.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ResolvedAddressParsers
{
    public partial class ResolvedAddressParserTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnProcessByteIfServiceErrorOccursAndLogItAsync()
        {
            // given
            byte[] someData = Encoding.GetEncoding("UTF-8").GetBytes(GetRandomString());
            string someFileName = GetRandomString();
            var serviceException = new Exception();

            var failedResolvedAddressParserServiceException =
                new FailedResolvedAddressParserServiceException(
                    message: "Failed address parser service occurred, please contact support",
                    innerException: serviceException);

            var expectedResolvedAddressParserServiceException =
                new ResolvedAddressParserServiceException(
                    message: "ResolvedAddress parser service error occurred, contact support.",
                    innerException: failedResolvedAddressParserServiceException);

            this.csvMapperBrokerMock.Setup(broker =>
                broker.MapCsvToListArrayAsync(It.IsAny<string>(), It.IsAny<bool>()))
                    .Throws(serviceException);

            // when
            ValueTask<List<ResolvedAddress>> processCSVTask =
                addressParserService.ProcessCsvAsync(someData, someFileName);

            ResolvedAddressParserServiceException actualResolvedAddressParserServiceException =
                await Assert.ThrowsAsync<ResolvedAddressParserServiceException>(async () =>
                    await processCSVTask);

            // then
            actualResolvedAddressParserServiceException.Should()
                .BeEquivalentTo(expectedResolvedAddressParserServiceException);

            this.csvMapperBrokerMock.Verify(broker =>
                broker.MapCsvToListArrayAsync(It.IsAny<string>(), It.IsAny<bool>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedResolvedAddressParserServiceException))),
                        Times.Once);

            this.csvMapperBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnProcessStringIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string someData = GetRandomString();
            string someFileName = GetRandomString();
            var serviceException = new Exception();

            var failedResolvedAddressParserServiceException =
                new FailedResolvedAddressParserServiceException(
                    message: "Failed address parser service occurred, please contact support",
                    innerException: serviceException);

            var expectedResolvedAddressParserServiceException =
                new ResolvedAddressParserServiceException(
                    message: "ResolvedAddress parser service error occurred, contact support.",
                    innerException: failedResolvedAddressParserServiceException);

            this.csvMapperBrokerMock.Setup(broker =>
                broker.MapCsvToListArrayAsync(It.IsAny<string>(), It.IsAny<bool>()))
                    .Throws(serviceException);

            // when
            ValueTask<List<ResolvedAddress>> processCSVTask =
                addressParserService.ProcessCsvAsync(someData, someFileName);

            ResolvedAddressParserServiceException actualResolvedAddressParserServiceException =
                await Assert.ThrowsAsync<ResolvedAddressParserServiceException>(async () =>
                    await processCSVTask);

            // then
            actualResolvedAddressParserServiceException.Should()
                .BeEquivalentTo(expectedResolvedAddressParserServiceException);

            this.csvMapperBrokerMock.Verify(broker =>
                broker.MapCsvToListArrayAsync(It.IsAny<string>(), It.IsAny<bool>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedResolvedAddressParserServiceException))),
                        Times.Once);

            this.csvMapperBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowFailedToParseExceptionOnProcessByteIfServiceErrorOccursAndLogItAsync()
        {
            // given
            byte[] someData = Encoding.GetEncoding("UTF-8").GetBytes(GetRandomString());
            string someFileName = GetRandomString();
            var someException = new Exception();

            InvalidCsvItemResolvedAddressParserException invalidCsvItemResolvedAddressParserException =
                new InvalidCsvItemResolvedAddressParserException(
                    message: "Error processing record at row 1: Something went wrong.");

            AggregateException aggregateException = new AggregateException(
                message: $"Unable to process 1 records from {someFileName}",
                    innerExceptions: new List<Exception> { invalidCsvItemResolvedAddressParserException });

            FailedToParseResolvedAddressParserException failedToParseResolvedAddressParserException =
                new FailedToParseResolvedAddressParserException(
                    message: $"Unable to fully parse resolved addresses from file {someFileName}",
                    innerException: aggregateException);

            var expectedResolvedAddressParserValidationException =
                new ResolvedAddressParserValidationException(
                    message: "ResolvedAddress parser validation errors occurred, please try again.",
                    innerException: failedToParseResolvedAddressParserException);

            this.csvMapperBrokerMock.Setup(broker =>
                broker.MapCsvToListArrayAsync(It.IsAny<string>(), It.IsAny<bool>()))
                    .Throws(failedToParseResolvedAddressParserException);

            // when
            ValueTask<List<ResolvedAddress>> processCSVTask =
                addressParserService.ProcessCsvAsync(someData, someFileName);

            ResolvedAddressParserValidationException actualResolvedAddressParserValidationException =
                await Assert.ThrowsAsync<ResolvedAddressParserValidationException>(async () =>
                    await processCSVTask);

            // then
            actualResolvedAddressParserValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressParserValidationException);

            this.csvMapperBrokerMock.Verify(broker =>
                broker.MapCsvToListArrayAsync(It.IsAny<string>(), It.IsAny<bool>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedResolvedAddressParserValidationException))),
                        Times.Once);

            this.csvMapperBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowFailedToParseExceptionOnProcessStringIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string someData = GetRandomString();
            string someFileName = GetRandomString();
            var someException = new Exception();

            InvalidCsvItemResolvedAddressParserException invalidCsvItemResolvedAddressParserException =
                new InvalidCsvItemResolvedAddressParserException(
                    message: "Error processing record at row 1: Something went wrong.");

            AggregateException aggregateException = new AggregateException(
                message: $"Unable to process 1 records from {someFileName}",
                    innerExceptions: new List<Exception> { invalidCsvItemResolvedAddressParserException });

            FailedToParseResolvedAddressParserException failedToParseResolvedAddressParserException =
                new FailedToParseResolvedAddressParserException(
                    message: $"Unable to fully parse resolved addresses from file {someFileName}",
                    innerException: aggregateException);

            var expectedResolvedAddressParserValidationException =
                new ResolvedAddressParserValidationException(
                    message: "ResolvedAddress parser validation errors occurred, please try again.",
                    innerException: failedToParseResolvedAddressParserException);

            this.csvMapperBrokerMock.Setup(broker =>
                broker.MapCsvToListArrayAsync(It.IsAny<string>(), It.IsAny<bool>()))
                    .Throws(failedToParseResolvedAddressParserException);

            // when
            ValueTask<List<ResolvedAddress>> processCSVTask =
                addressParserService.ProcessCsvAsync(someData, someFileName);

            ResolvedAddressParserValidationException actualResolvedAddressParserValidationException =
                await Assert.ThrowsAsync<ResolvedAddressParserValidationException>(async () =>
                    await processCSVTask);

            // then
            actualResolvedAddressParserValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressParserValidationException);

            this.csvMapperBrokerMock.Verify(broker =>
                broker.MapCsvToListArrayAsync(It.IsAny<string>(), It.IsAny<bool>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedResolvedAddressParserValidationException))),
                        Times.Once);

            this.csvMapperBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

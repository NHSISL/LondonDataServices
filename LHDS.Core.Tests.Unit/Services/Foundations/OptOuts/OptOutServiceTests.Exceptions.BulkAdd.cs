// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Foundations.OptOuts.Exceptions;
using LHDS.Core.Services.Foundations.OptOuts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OptOuts
{
    public partial class OptOutServiceTests
    {
        [Fact]
        public async Task
            ShouldThrowAggregateServiceExceptionOnBulkAddIfErrorOccursAndLogItAsync()
        {
            // given
            int randomCount = GetRandomNumber();
            string someFileName = GetRandomString();
            List<OptOut> someOptOuts = CreateRandomOptOuts(
                count: randomCount,
                dateTimeOffset: GetRandomDateTimeOffset(),
                userId: GetRandomString());

            Exception exception = new Exception("Some exception");

            var aggregateException =
                new AggregateException(
                    message: $"Unable to process optOuts in 1 of the"
                        + $" batch(es) from {someFileName}",
                    innerExceptions: exception);

            var failedOptOutServiceException =
                new FailedOptOutServiceException(
                    message: "Failed aggregate optOut service error"
                        + " occurred, please contact support.",
                    innerException: aggregateException);

            var expectedOptOutServiceException =
                new OptOutServiceException(
                    message: "OptOut service error occurred,"
                        + " please contact support.",
                    innerException: failedOptOutServiceException);

            var optOutServiceMock = new Mock<OptOutService>(
                this.storageBrokerMock.Object,
                this.dateTimeBrokerMock.Object,
                this.securityAuditBrokerMock.Object,
                this.identifierBrokerMock.Object,
                this.loggingBrokerMock.Object,
                this.auditBrokerMock.Object)
            {
                CallBase = true
            };

            optOutServiceMock.Setup(service =>
                service.BulkAddOrModifyBatchAsync(
                    It.IsAny<List<OptOut>>(),
                    It.IsAny<string>(),
                    It.IsAny<int>()))
                        .ThrowsAsync(aggregateException);

            // when
            ValueTask bulkAddOptOutTask = optOutServiceMock.Object
                .BulkAddOptOutsAsync(
                    optOuts: someOptOuts,
                    fileName: someFileName);

            OptOutServiceException actualOptOutServiceException =
                await Assert.ThrowsAsync<OptOutServiceException>(
                    bulkAddOptOutTask.AsTask);

            // then
            actualOptOutServiceException.Should()
                .BeEquivalentTo(expectedOptOutServiceException);

            optOutServiceMock.Verify(service =>
                service.BulkAddOrModifyBatchAsync(
                    It.IsAny<List<OptOut>>(),
                    It.IsAny<string>(),
                    It.IsAny<int>()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowServiceExceptionOnBulkAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var optOutServiceMock = new Mock<OptOutService>(
                this.storageBrokerMock.Object,
                this.dateTimeBrokerMock.Object,
                this.securityAuditBrokerMock.Object,
                this.identifierBrokerMock.Object,
                this.loggingBrokerMock.Object,
                this.auditBrokerMock.Object)
            {
                CallBase = true
            };

            optOutServiceMock.Setup(service =>
                service.BulkAddOrModifyBatchAsync(
                    It.IsAny<List<OptOut>>(),
                    It.IsAny<string>(),
                    It.IsAny<int>()))
                        .ThrowsAsync(serviceException);

            int randomCount = GetRandomNumber();
            string someFileName = GetRandomString();

            List<OptOut> someOptOuts = CreateRandomOptOuts(
                count: randomCount,
                dateTimeOffset: GetRandomDateTimeOffset(),
                userId: GetRandomString());

            var failedOptOutServiceException =
                new FailedOptOutServiceException(
                    message: "Failed optOut service error occurred,"
                        + " please contact support.",
                    innerException: serviceException);

            var expectedOptOutServiceException =
                new OptOutServiceException(
                    message: "OptOut service error occurred,"
                        + " please contact support.",
                    innerException: failedOptOutServiceException);

            // when
            ValueTask addOptOutTask = optOutServiceMock.Object
                .BulkAddOptOutsAsync(
                    optOuts: someOptOuts,
                    fileName: someFileName);

            OptOutServiceException actualOptOutServiceException =
                await Assert.ThrowsAsync<OptOutServiceException>(
                    addOptOutTask.AsTask);

            // then
            actualOptOutServiceException.Should()
                .BeEquivalentTo(expectedOptOutServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

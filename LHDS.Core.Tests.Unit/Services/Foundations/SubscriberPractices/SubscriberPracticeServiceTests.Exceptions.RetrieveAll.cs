// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.SubscriberPractices;
using LHDS.Core.Models.Foundations.SubscriberPractices.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SubscriberPractices
{
    public partial class SubscriberPracticeServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedSubscriberPracticeStorageException =
                new FailedSubscriberPracticeStorageException(
                    message: "Failed subscriberPractice storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedSubscriberPracticeDependencyException =
                new SubscriberPracticeDependencyException(
                    message: "SubscriberPractice dependency error occurred, please contact support.",
                    innerException: failedSubscriberPracticeStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllSubscriberPracticesAsync())
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<IQueryable<SubscriberPractice>> retrieveAllSubscriberPracticesTask =
                this.subscriberPracticeService.RetrieveAllSubscriberPracticesAsync();

            SubscriberPracticeDependencyException actualSubscriberPracticeDependencyException =
                await Assert.ThrowsAsync<SubscriberPracticeDependencyException>(
                    testCode: retrieveAllSubscriberPracticesTask.AsTask);

            // then
            actualSubscriberPracticeDependencyException.Should()
                .BeEquivalentTo(expectedSubscriberPracticeDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSubscriberPracticesAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedSubscriberPracticeDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string exceptionMessage = GetRandomString();
            var serviceException = new Exception(exceptionMessage);

            var failedSubscriberPracticeServiceException =
                new FailedSubscriberPracticeServiceException(
                    message: "Failed subscriberPractice service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedSubscriberPracticeServiceException =
                new SubscriberPracticeServiceException(
                    message: "SubscriberPractice service error occurred, please contact support.",
                    innerException: failedSubscriberPracticeServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllSubscriberPracticesAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<IQueryable<SubscriberPractice>> retrieveAllSubscriberPracticesTask =
                this.subscriberPracticeService.RetrieveAllSubscriberPracticesAsync();

            SubscriberPracticeServiceException actualSubscriberPracticeServiceException =
                await Assert.ThrowsAsync<SubscriberPracticeServiceException>(
                    testCode: retrieveAllSubscriberPracticesTask.AsTask);

            // then
            actualSubscriberPracticeServiceException.Should()
                .BeEquivalentTo(expectedSubscriberPracticeServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSubscriberPracticesAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberPracticeServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
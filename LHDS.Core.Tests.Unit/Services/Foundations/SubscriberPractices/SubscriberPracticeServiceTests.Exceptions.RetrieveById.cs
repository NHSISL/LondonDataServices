// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
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
                broker.SelectSubscriberPracticeByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<SubscriberPractice> retrieveSubscriberPracticeByIdTask =
                this.subscriberPracticeService.RetrieveSubscriberPracticeByIdAsync(someId);

            SubscriberPracticeDependencyException actualSubscriberPracticeDependencyException =
                await Assert.ThrowsAsync<SubscriberPracticeDependencyException>(
                    retrieveSubscriberPracticeByIdTask.AsTask);

            // then
            actualSubscriberPracticeDependencyException.Should()
                .BeEquivalentTo(expectedSubscriberPracticeDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberPracticeByIdAsync(It.IsAny<Guid>()),
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
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedSubscriberPracticeServiceException =
                new FailedSubscriberPracticeServiceException(
                    message: "Failed subscriberPractice service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedSubscriberPracticeServiceException =
                new SubscriberPracticeServiceException(
                    message: "SubscriberPractice service error occurred, please contact support.",
                    innerException: failedSubscriberPracticeServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSubscriberPracticeByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<SubscriberPractice> retrieveSubscriberPracticeByIdTask =
                this.subscriberPracticeService.RetrieveSubscriberPracticeByIdAsync(someId);

            SubscriberPracticeServiceException actualSubscriberPracticeServiceException =
                await Assert.ThrowsAsync<SubscriberPracticeServiceException>(
                    retrieveSubscriberPracticeByIdTask.AsTask);

            // then
            actualSubscriberPracticeServiceException.Should()
                .BeEquivalentTo(expectedSubscriberPracticeServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberPracticeByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedSubscriberPracticeServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
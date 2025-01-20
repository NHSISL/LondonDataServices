// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SubscriberAgreements
{
    public partial class SubscriberAgreementServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedSubscriberAgreementStorageException =
                new FailedSubscriberAgreementStorageException(
                    message: "Failed subscriberAgreement storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedSubscriberAgreementDependencyException =
                new SubscriberAgreementDependencyException(
                    message: "SubscriberAgreement dependency error occurred, please contact support.",
                    innerException: failedSubscriberAgreementStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllSubscriberAgreementsAsync())
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<IQueryable<SubscriberAgreement>> retrieveAllSubscriberAgreementsTask =
                this.subscriberAgreementService.RetrieveAllSubscriberAgreementsAsync();

            SubscriberAgreementDependencyException actualSubscriberAgreementDependencyException =
                await Assert.ThrowsAsync<SubscriberAgreementDependencyException>(
                    testCode: retrieveAllSubscriberAgreementsTask.AsTask);

            // then
            actualSubscriberAgreementDependencyException.Should()
                .BeEquivalentTo(expectedSubscriberAgreementDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSubscriberAgreementsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementDependencyException))),
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

            var failedSubscriberAgreementServiceException =
                new FailedSubscriberAgreementServiceException(
                    message: "Failed subscriberAgreement service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedSubscriberAgreementServiceException =
                new SubscriberAgreementServiceException(
                    message: "SubscriberAgreement service error occurred, please contact support.",
                    innerException: failedSubscriberAgreementServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllSubscriberAgreementsAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<IQueryable<SubscriberAgreement>> retrieveAllSubscriberAgreementsTask =
                this.subscriberAgreementService.RetrieveAllSubscriberAgreementsAsync();

            SubscriberAgreementServiceException actualSubscriberAgreementServiceException =
                await Assert.ThrowsAsync<SubscriberAgreementServiceException>(
                    testCode: retrieveAllSubscriberAgreementsTask.AsTask);

            // then
            actualSubscriberAgreementServiceException.Should()
                .BeEquivalentTo(expectedSubscriberAgreementServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSubscriberAgreementsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
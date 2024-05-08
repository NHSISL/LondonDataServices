// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using FluentAssertions;
using LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SubscriberAgreements
{
    public partial class SubscriberAgreementServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
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
                broker.SelectAllSubscriberAgreements())
                    .Throws(sqlException);

            // when
            Action retrieveAllSubscriberAgreementsAction = () =>
                this.subscriberAgreementService.RetrieveAllSubscriberAgreements();

            SubscriberAgreementDependencyException actualSubscriberAgreementDependencyException =
                Assert.Throws<SubscriberAgreementDependencyException>(retrieveAllSubscriberAgreementsAction);

            // then
            actualSubscriberAgreementDependencyException.Should()
                .BeEquivalentTo(expectedSubscriberAgreementDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSubscriberAgreements(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
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
                broker.SelectAllSubscriberAgreements())
                    .Throws(serviceException);

            // when
            Action retrieveAllSubscriberAgreementsAction = () =>
                this.subscriberAgreementService.RetrieveAllSubscriberAgreements();

            SubscriberAgreementServiceException actualSubscriberAgreementServiceException =
                Assert.Throws<SubscriberAgreementServiceException>(retrieveAllSubscriberAgreementsAction);

            // then
            actualSubscriberAgreementServiceException.Should()
                .BeEquivalentTo(expectedSubscriberAgreementServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSubscriberAgreements(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
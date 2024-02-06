using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions;
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
                    message: "Failed subscriberAgreement storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedSubscriberAgreementDependencyException =
                new SubscriberAgreementDependencyException(
                    message: "SubscriberAgreement dependency error occurred, contact support.",
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
    }
}
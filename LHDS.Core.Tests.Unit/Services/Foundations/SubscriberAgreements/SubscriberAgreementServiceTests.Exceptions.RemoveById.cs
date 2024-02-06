using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SubscriberAgreements
{
    public partial class SubscriberAgreementServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
        {
            // given
            SubscriberAgreement randomSubscriberAgreement = CreateRandomSubscriberAgreement();
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
                broker.SelectSubscriberAgreementByIdAsync(randomSubscriberAgreement.Id))
                    .Throws(sqlException);

            // when
            ValueTask<SubscriberAgreement> addSubscriberAgreementTask =
                this.subscriberAgreementService.RemoveSubscriberAgreementByIdAsync(randomSubscriberAgreement.Id);

            SubscriberAgreementDependencyException actualSubscriberAgreementDependencyException =
                await Assert.ThrowsAsync<SubscriberAgreementDependencyException>(
                    addSubscriberAgreementTask.AsTask);

            // then
            actualSubscriberAgreementDependencyException.Should()
                .BeEquivalentTo(expectedSubscriberAgreementDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberAgreementByIdAsync(randomSubscriberAgreement.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
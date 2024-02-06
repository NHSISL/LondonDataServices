// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SubscriberAgreements
{
    public partial class SubscriberAgreementServiceTests
    {
        [Fact]
        public async Task
            ShouldThrowValidationExceptionOnRetrieveByRetrieveBySupplierSharingAgreementGuidIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidRetrieveBySupplierSharingAgreementGuid = Guid.Empty;

            var invalidSubscriberAgreementException =
                new InvalidSubscriberAgreementException(
                    message: "Invalid subscriberAgreement. Please correct the errors and try again.");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.SupplierSharingAgreementGuid),
                values: "Id is required");

            var expectedSubscriberAgreementValidationException =
                new SubscriberAgreementValidationException(
                    message: "SubscriberAgreement validation errors occurred, please try again.",
                    innerException: invalidSubscriberAgreementException);

            // when
            ValueTask<SubscriberAgreement> retrieveSubscriberAgreementBySupplierSharingAgreementGuidTask =
                this.subscriberAgreementService.RetrieveSubscriberAgreementBySupplierSharingAgreementGuidAsync(
                    invalidRetrieveBySupplierSharingAgreementGuid);

            SubscriberAgreementValidationException actualSubscriberAgreementValidationException =
                await Assert.ThrowsAsync<SubscriberAgreementValidationException>(
                    retrieveSubscriberAgreementBySupplierSharingAgreementGuidTask.AsTask);

            // then
            actualSubscriberAgreementValidationException.Should()
                .BeEquivalentTo(expectedSubscriberAgreementValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberAgreementBySupplierSharingAgreementGuidAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowNotFoundExceptionOnRetrieveBySupplierSharingAgreementGuidIfSubscriberAgreementIsNotFoundAndLogItAsync()
        {
            //given
            Guid someSupplierSharingAgreementGuidId = Guid.NewGuid();
            SubscriberAgreement noSubscriberAgreement = null;

            var notFoundSubscriberAgreementException =
                new NotFoundSubscriberAgreementException(someSupplierSharingAgreementGuidId);

            var expectedSubscriberAgreementValidationException =
                new SubscriberAgreementValidationException(
                    message: "SubscriberAgreement validation errors occurred, please try again.",
                    innerException: notFoundSubscriberAgreementException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSubscriberAgreementBySupplierSharingAgreementGuidAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noSubscriberAgreement);

            //when
            ValueTask<SubscriberAgreement> retrieveSubscriberAgreementByIdTask =
                this.subscriberAgreementService.RetrieveSubscriberAgreementBySupplierSharingAgreementGuidAsync(
                    someSupplierSharingAgreementGuidId);

            SubscriberAgreementValidationException actualSubscriberAgreementValidationException =
                await Assert.ThrowsAsync<SubscriberAgreementValidationException>(
                    retrieveSubscriberAgreementByIdTask.AsTask);

            //then
            actualSubscriberAgreementValidationException.Should().BeEquivalentTo(
                expectedSubscriberAgreementValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberAgreementBySupplierSharingAgreementGuidAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.IngestionTrackingAudits
{
    public partial class IngestionTrackingAuditTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidIngestionTrackingAuditId = Guid.Empty;

            var invalidIngestionTrackingAuditException =
                new InvalidIngestionTrackingAuditException(
                    message: "Invalid IngestionTrackingAudit. Please correct the errors and try again.");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.Id),
                values: "Id is required");

            var expectedIngestionTrackingAuditValidationException =
                new IngestionTrackingAuditValidationException(
                    message: "IngestionTrackingAudit validation errors occurred, please try again.",
                    innerException: invalidIngestionTrackingAuditException);

            // when
            ValueTask<IngestionTrackingAudit> retrieveIngestionTrackingAuditByIdTask =
                this.ingestionTrackingAuditService.RetrieveIngestionTrackingAuditByIdAsync(
                    invalidIngestionTrackingAuditId);

            IngestionTrackingAuditValidationException actualAuditValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditValidationException>(
                    retrieveIngestionTrackingAuditByIdTask.AsTask);

            // then
            actualAuditValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfAuditIsNotFoundAndLogItAsync()
        {
            //given
            Guid someIngestionTrackingAuditId = Guid.NewGuid();
            IngestionTrackingAudit noIngestionTrackingAudit = null;

            var notFoundAuditException =
                new NotFoundIngestionTrackingAuditException(someIngestionTrackingAuditId);

            var expectedIngestionTrackingAuditValidationException =
                new IngestionTrackingAuditValidationException(
                    message: "IngestionTrackingAudit validation errors occurred, please try again.",
                    notFoundAuditException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noIngestionTrackingAudit);

            //when
            ValueTask<IngestionTrackingAudit> retrieveIngestionTrackingAuditByIdTask =
                this.ingestionTrackingAuditService.RetrieveIngestionTrackingAuditByIdAsync(
                    someIngestionTrackingAuditId);

            IngestionTrackingAuditValidationException actualIngestionTrackingAuditValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditValidationException>(
                    retrieveIngestionTrackingAuditByIdTask.AsTask);

            //then
            actualIngestionTrackingAuditValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
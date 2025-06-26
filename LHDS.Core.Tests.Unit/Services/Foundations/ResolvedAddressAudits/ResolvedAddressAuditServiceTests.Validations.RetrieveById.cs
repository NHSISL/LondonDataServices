// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ResolvedAddressAudits.Exceptions;
using LHDS.Core.Models.Foundations.ResolvedAddressesAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ResolvedAddressAudits
{
    public partial class ResolvedAddressAuditServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidResolvedAddressAuditId = Guid.Empty;

            var invalidResolvedAddressAuditException =
                new InvalidResolvedAddressAuditException(message: "Invalid resolvedAddressAudit. Please correct the errors and try again.");

            invalidResolvedAddressAuditException.AddData(
                key: nameof(ResolvedAddressAudit.Id),
                values: "Id is required");

            var expectedResolvedAddressAuditValidationException =
                new ResolvedAddressAuditValidationException(
                    message: "ResolvedAddressAudit validation errors occurred, please try again.",
                    innerException: invalidResolvedAddressAuditException);

            // when
            ValueTask<ResolvedAddressAudit> retrieveResolvedAddressAuditByIdTask =
                this.resolvedAddressAuditService.RetrieveResolvedAddressAuditByIdAsync(invalidResolvedAddressAuditId);

            ResolvedAddressAuditValidationException actualResolvedAddressAuditValidationException =
                await Assert.ThrowsAsync<ResolvedAddressAuditValidationException>(
                    retrieveResolvedAddressAuditByIdTask.AsTask);

            // then
            actualResolvedAddressAuditValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfResolvedAddressAuditIsNotFoundAndLogItAsync()
        {
            //given
            Guid someResolvedAddressAuditId = Guid.NewGuid();
            ResolvedAddressAudit noResolvedAddressAudit = null;

            var notFoundResolvedAddressAuditException =
                new NotFoundResolvedAddressAuditException(someResolvedAddressAuditId);

            var expectedResolvedAddressAuditValidationException =
                new ResolvedAddressAuditValidationException(
                    message: "ResolvedAddressAudit validation errors occurred, please try again.",
                    innerException: notFoundResolvedAddressAuditException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noResolvedAddressAudit);

            //when
            ValueTask<ResolvedAddressAudit> retrieveResolvedAddressAuditByIdTask =
                this.resolvedAddressAuditService.RetrieveResolvedAddressAuditByIdAsync(someResolvedAddressAuditId);

            ResolvedAddressAuditValidationException actualResolvedAddressAuditValidationException =
                await Assert.ThrowsAsync<ResolvedAddressAuditValidationException>(
                    retrieveResolvedAddressAuditByIdTask.AsTask);

            //then
            actualResolvedAddressAuditValidationException.Should().BeEquivalentTo(expectedResolvedAddressAuditValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
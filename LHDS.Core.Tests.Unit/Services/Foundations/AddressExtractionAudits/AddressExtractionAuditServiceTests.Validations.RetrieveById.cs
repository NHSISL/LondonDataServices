using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.AddressExtractionAudits;
using LHDS.Core.Models.Foundations.AddressExtractionAudits.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressExtractionAudits
{
    public partial class AddressExtractionAuditServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidAddressExtractionAuditId = Guid.Empty;

            var invalidAddressExtractionAuditException = 
                new InvalidAddressExtractionAuditException(
                    message: "Invalid addressExtractionAudit. Please correct the errors and try again.");

            invalidAddressExtractionAuditException.AddData(
                key: nameof(AddressExtractionAudit.Id),
                values: "Id is required");

            var expectedAddressExtractionAuditValidationException =
                new AddressExtractionAuditValidationException(
                    message: "AddressExtractionAudit validation errors occurred, please try again.",
                    innerException: invalidAddressExtractionAuditException);

            // when
            ValueTask<AddressExtractionAudit> retrieveAddressExtractionAuditByIdTask =
                this.addressExtractionAuditService.RetrieveAddressExtractionAuditByIdAsync(invalidAddressExtractionAuditId);

            AddressExtractionAuditValidationException actualAddressExtractionAuditValidationException =
                await Assert.ThrowsAsync<AddressExtractionAuditValidationException>(
                    retrieveAddressExtractionAuditByIdTask.AsTask);

            // then
            actualAddressExtractionAuditValidationException.Should()
                .BeEquivalentTo(expectedAddressExtractionAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressExtractionAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressExtractionAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfAddressExtractionAuditIsNotFoundAndLogItAsync()
        {
            //given
            Guid someAddressExtractionAuditId = Guid.NewGuid();
            AddressExtractionAudit noAddressExtractionAudit = null;

            var notFoundAddressExtractionAuditException =
                new NotFoundAddressExtractionAuditException(someAddressExtractionAuditId);

            var expectedAddressExtractionAuditValidationException =
                new AddressExtractionAuditValidationException(
                    message: "AddressExtractionAudit validation errors occurred, please try again.",
                    innerException: notFoundAddressExtractionAuditException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressExtractionAuditByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noAddressExtractionAudit);

            //when
            ValueTask<AddressExtractionAudit> retrieveAddressExtractionAuditByIdTask =
                this.addressExtractionAuditService.RetrieveAddressExtractionAuditByIdAsync(someAddressExtractionAuditId);

            AddressExtractionAuditValidationException actualAddressExtractionAuditValidationException =
                await Assert.ThrowsAsync<AddressExtractionAuditValidationException>(
                    retrieveAddressExtractionAuditByIdTask.AsTask);

            //then
            actualAddressExtractionAuditValidationException.Should().BeEquivalentTo(expectedAddressExtractionAuditValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressExtractionAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressExtractionAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
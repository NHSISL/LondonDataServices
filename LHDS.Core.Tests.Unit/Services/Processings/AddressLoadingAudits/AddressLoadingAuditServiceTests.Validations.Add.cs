// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using LHDS.Core.Models.Foundations.AddressLoadingAudits.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.AddressLoadingAudits
{
    public partial class AddressLoadingAuditProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfAddressLoadingAuditIsNullAndLogItAsync()
        {
            // given
            AddressLoadingAudit nullAddressLoadingAudit = null;

            var nullAddressLoadingAuditException =
                new NullAddressLoadingAuditException(message: "Address loading audit is null.");

            var expectedAddressLoadingAuditValidationException =
                new AddressLoadingAuditValidationException(
                    message: "Address loading audit validation errors occurred, please try again.",
                    innerException: nullAddressLoadingAuditException);

            // when
            ValueTask<AddressLoadingAudit> addAddressLoadingAuditProcessingTask =
                this.addressLoadingAuditProcessingService.AddAddressLoadingAuditAsync(nullAddressLoadingAudit);

            AddressLoadingAuditValidationException actualAddressLoadingAuditValidationException =
                await Assert.ThrowsAsync<AddressLoadingAuditValidationException>(() =>
                    addAddressLoadingAuditProcessingTask.AsTask());

            // then
            actualAddressLoadingAuditValidationException.Should()
                .BeEquivalentTo(expectedAddressLoadingAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressLoadingAuditValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
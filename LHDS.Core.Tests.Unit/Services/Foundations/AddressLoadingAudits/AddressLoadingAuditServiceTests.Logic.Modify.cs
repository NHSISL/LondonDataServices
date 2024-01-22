// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressLoadingAudits
{
    public partial class AddressLoadingAuditServiceTests
    {
        [Fact]
        public async Task ShouldModifyAddressLoadingAuditAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            AddressLoadingAudit randomAddressLoadingAudit = CreateRandomModifyAddressLoadingAudit(randomDateTimeOffset);
            AddressLoadingAudit inputAddressLoadingAudit = randomAddressLoadingAudit;
            AddressLoadingAudit storageAddressLoadingAudit = inputAddressLoadingAudit.DeepClone();
            storageAddressLoadingAudit.UpdatedDate = randomAddressLoadingAudit.CreatedDate;
            AddressLoadingAudit updatedAddressLoadingAudit = inputAddressLoadingAudit;
            AddressLoadingAudit expectedAddressLoadingAudit = updatedAddressLoadingAudit.DeepClone();
            Guid addressLoadingAuditId = inputAddressLoadingAudit.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(addressLoadingAuditId))
                    .ReturnsAsync(storageAddressLoadingAudit);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateAddressLoadingAuditAsync(inputAddressLoadingAudit))
                    .ReturnsAsync(updatedAddressLoadingAudit);

            // when
            AddressLoadingAudit actualAddressLoadingAudit =
                await this.addressLoadingAuditService.ModifyAddressLoadingAuditAsync(inputAddressLoadingAudit);

            // then
            actualAddressLoadingAudit.Should().BeEquivalentTo(expectedAddressLoadingAudit);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(inputAddressLoadingAudit.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAddressLoadingAuditAsync(inputAddressLoadingAudit),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
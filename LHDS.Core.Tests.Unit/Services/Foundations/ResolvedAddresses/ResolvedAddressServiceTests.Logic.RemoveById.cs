// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ResolvedAddresses
{
    public partial class ResolvedAddressServiceTests
    {
        [Fact]
        public async Task ShouldRemoveResolvedAddressByIdAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomEntraUserId = GetRandomStringWithLengthOf(50);

            ResolvedAddress randomResolvedAddress = 
                CreateRandomResolvedAddress(randomDateTimeOffset, randomEntraUserId);

            Guid inputResolvedAddressId = randomResolvedAddress.Id;
            ResolvedAddress storageResolvedAddress = randomResolvedAddress;
            ResolvedAddress ingestionTrackingWithDeleteAuditApplied = storageResolvedAddress.DeepClone();
            ingestionTrackingWithDeleteAuditApplied.UpdatedBy = randomEntraUserId.ToString();
            ingestionTrackingWithDeleteAuditApplied.UpdatedDate = randomDateTimeOffset;
            ResolvedAddress updatedResolvedAddress = storageResolvedAddress;
            ResolvedAddress deletedResolvedAddress = updatedResolvedAddress;
            ResolvedAddress expectedResolvedAddress = deletedResolvedAddress.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectResolvedAddressByIdAsync(inputResolvedAddressId))
                    .ReturnsAsync(storageResolvedAddress);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateResolvedAddressAsync(randomResolvedAddress))
                    .ReturnsAsync(updatedResolvedAddress);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteResolvedAddressAsync(updatedResolvedAddress))
                    .ReturnsAsync(deletedResolvedAddress);

            // when
            ResolvedAddress actualResolvedAddress = await this.resolvedAddressService
                .RemoveResolvedAddressByIdAsync(inputResolvedAddressId);

            // then
            actualResolvedAddress.Should().BeEquivalentTo(expectedResolvedAddress);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressByIdAsync(inputResolvedAddressId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Exactly(2));

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateResolvedAddressAsync(randomResolvedAddress),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteResolvedAddressAsync(updatedResolvedAddress),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ResolvedAddresses
{
    public partial class ResolvedAddressServiceTests
    {
        [Fact]
        public async Task ShouldModifyResolvedAddressAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomEntraUserId = GetRandomStringWithLengthOf(50);

            ResolvedAddress randomResolvedAddress = 
                CreateRandomModifyResolvedAddress(randomDateTimeOffset, randomEntraUserId);

            ResolvedAddress inputResolvedAddress = randomResolvedAddress;
            ResolvedAddress storageResolvedAddress = inputResolvedAddress.DeepClone();
            storageResolvedAddress.UpdatedDate = randomResolvedAddress.CreatedDate;
            ResolvedAddress updatedResolvedAddress = inputResolvedAddress;
            ResolvedAddress expectedResolvedAddress = updatedResolvedAddress.DeepClone();
            Guid resolvedAddressId = inputResolvedAddress.Id;

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(inputResolvedAddress))
                    .ReturnsAsync(inputResolvedAddress);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
                    inputResolvedAddress,
                    storageResolvedAddress))
                        .ReturnsAsync(inputResolvedAddress);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectResolvedAddressByIdAsync(resolvedAddressId))
                    .ReturnsAsync(storageResolvedAddress);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateResolvedAddressAsync(inputResolvedAddress))
                    .ReturnsAsync(updatedResolvedAddress);

            // when
            ResolvedAddress actualResolvedAddress = 
                await this.resolvedAddressService.ModifyResolvedAddressAsync(inputResolvedAddress);

            // then
            actualResolvedAddress.Should().BeEquivalentTo(expectedResolvedAddress);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(inputResolvedAddress),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
                    inputResolvedAddress,
                    storageResolvedAddress),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressByIdAsync(inputResolvedAddress.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateResolvedAddressAsync(inputResolvedAddress),
                    Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
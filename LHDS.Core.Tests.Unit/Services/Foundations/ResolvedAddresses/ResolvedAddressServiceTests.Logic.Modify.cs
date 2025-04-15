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
        public async Task ShouldModifyResolvedAddressAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser entraUser = CreateRandomEntraUser();

            ResolvedAddress randomResolvedAddress = 
                CreateRandomModifyResolvedAddress(randomDateTimeOffset, entraUser.EntraUserId);

            ResolvedAddress inputResolvedAddress = randomResolvedAddress;
            ResolvedAddress storageResolvedAddress = inputResolvedAddress.DeepClone();
            storageResolvedAddress.UpdatedDate = randomResolvedAddress.CreatedDate;
            ResolvedAddress updatedResolvedAddress = inputResolvedAddress;
            ResolvedAddress expectedResolvedAddress = updatedResolvedAddress.DeepClone();
            Guid resolvedAddressId = inputResolvedAddress.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(entraUser);

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

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressByIdAsync(inputResolvedAddress.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateResolvedAddressAsync(inputResolvedAddress),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
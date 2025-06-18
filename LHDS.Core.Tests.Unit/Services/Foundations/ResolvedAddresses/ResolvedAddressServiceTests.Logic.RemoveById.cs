// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
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
            Guid randomId = Guid.NewGuid();
            Guid inputResolvedAddressId = randomId;
            ResolvedAddress randomResolvedAddress = CreateRandomResolvedAddress();
            ResolvedAddress storageResolvedAddress = randomResolvedAddress;
            ResolvedAddress expectedInputResolvedAddress = storageResolvedAddress;
            ResolvedAddress deletedResolvedAddress = expectedInputResolvedAddress;
            ResolvedAddress expectedResolvedAddress = deletedResolvedAddress.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectResolvedAddressByIdAsync(inputResolvedAddressId))
                    .ReturnsAsync(storageResolvedAddress);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteResolvedAddressAsync(expectedInputResolvedAddress))
                    .ReturnsAsync(deletedResolvedAddress);

            // when
            ResolvedAddress actualResolvedAddress = await this.resolvedAddressService
                .RemoveResolvedAddressByIdAsync(inputResolvedAddressId);

            // then
            actualResolvedAddress.Should().BeEquivalentTo(expectedResolvedAddress);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressByIdAsync(inputResolvedAddressId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteResolvedAddressAsync(expectedInputResolvedAddress),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
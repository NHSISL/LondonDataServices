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
        public async Task ShouldAddResolvedAddressAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomEntraUserId = GetRandomStringWithLengthOf(50);

            ResolvedAddress randomResolvedAddress =
                CreateRandomResolvedAddress(randomDateTimeOffset, randomEntraUserId);

            ResolvedAddress inputResolvedAddress = randomResolvedAddress;
            ResolvedAddress storageResolvedAddress = inputResolvedAddress;
            ResolvedAddress expectedResolvedAddress = storageResolvedAddress.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertResolvedAddressAsync(inputResolvedAddress))
                    .ReturnsAsync(storageResolvedAddress);

            // when
            ResolvedAddress actualResolvedAddress = await this.resolvedAddressService
                .AddResolvedAddressAsync(inputResolvedAddress);

            // then
            actualResolvedAddress.Should().BeEquivalentTo(expectedResolvedAddress);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(2));

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Exactly(2));

            this.storageBrokerMock.Verify(broker =>
                broker.InsertResolvedAddressAsync(inputResolvedAddress),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
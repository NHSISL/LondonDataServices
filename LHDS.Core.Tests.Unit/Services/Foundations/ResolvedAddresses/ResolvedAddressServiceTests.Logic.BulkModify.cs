// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ResolvedAddresses
{
    public partial class ResolvedAddressServiceTests
    {
        [Fact]
        public async Task ShouldBulkModifyResolvedAddressesAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            Guid randomIdentifier = Guid.NewGuid();
            DateTimeOffset updatedDate = randomDateTimeOffset;
            string randomFileName = GetRandomString();

            List<ResolvedAddress> randomResolvedAddresses = new List<ResolvedAddress>
                {
                    CreateRandomModifyResolvedAddress(randomDateTimeOffset),
                    CreateRandomModifyResolvedAddress(randomDateTimeOffset)
                };

            List<ResolvedAddress> inputResolvedAddresses = randomResolvedAddresses;
            List<ResolvedAddress> validatedResolvedAddresses = inputResolvedAddresses.DeepClone();

            validatedResolvedAddresses.ForEach(address =>
            {
                address.UpdatedDate = randomDateTimeOffset;
            });

            List<ResolvedAddress> updatedResolvedAddresses = new List<ResolvedAddress> { validatedResolvedAddresses.Last() };

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllResolvedAddresses())
                    .Returns(new List<ResolvedAddress> { randomResolvedAddresses.Last() }.AsQueryable());

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            await this.resolvedAddressService
                .BulkModifyResolvedAddressesAsync(inputResolvedAddresses);

            // then
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(randomResolvedAddresses.Count * 2));

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllResolvedAddresses(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.BulkUpdateResolvedAddressesAsync(It.Is(SameResolvedAddressesAs(updatedResolvedAddresses))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
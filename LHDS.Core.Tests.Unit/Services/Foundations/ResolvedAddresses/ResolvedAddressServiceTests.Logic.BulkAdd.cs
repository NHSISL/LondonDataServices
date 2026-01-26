// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task ShouldBulkAddResolvedAddressesAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomEntraUserId = GetRandomStringWithLengthOf(50);
            Guid randomIdentifier = Guid.NewGuid();
            string randomFileName = GetRandomString();
            string inputFileName = randomFileName;

            List<ResolvedAddress> randomResolvedAddresses = new List<ResolvedAddress>
                {
                    CreateRandomResolvedAddress(randomDateTimeOffset, randomEntraUserId),
                    CreateRandomResolvedAddress(randomDateTimeOffset, randomEntraUserId)
                };

            List<ResolvedAddress> inputResolvedAddresses = randomResolvedAddresses;
            List<ResolvedAddress> validatedResolvedAddresses = inputResolvedAddresses.DeepClone();

            validatedResolvedAddresses.ForEach(address =>
            {
                address.Id = randomIdentifier;
                address.CreatedBy = randomEntraUserId;
                address.CreatedDate = randomDateTimeOffset;
                address.UpdatedBy = randomEntraUserId;
                address.UpdatedDate = randomDateTimeOffset;
            });

            List<ResolvedAddress> newResolvedAddresses = 
                new List<ResolvedAddress> { validatedResolvedAddresses.First() };

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllResolvedAddressesAsync())
                    .ReturnsAsync(new List<ResolvedAddress> { randomResolvedAddresses.Last() }.AsQueryable());

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ReturnsAsync(randomIdentifier);

            // when
            await this.resolvedAddressService
                .BulkAddResolvedAddressesAsync(inputResolvedAddresses, inputFileName);

            // then
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(randomResolvedAddresses.Count * 2));

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Exactly(randomResolvedAddresses.Count * 2));

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Exactly(randomResolvedAddresses.Count)); 

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllResolvedAddressesAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.BulkInsertResolvedAddressesAsync(It.Is(SameResolvedAddressesAs(newResolvedAddresses))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
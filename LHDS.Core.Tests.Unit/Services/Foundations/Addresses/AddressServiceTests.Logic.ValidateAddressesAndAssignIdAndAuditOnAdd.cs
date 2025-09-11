// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Services.Foundations.Addresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Addresses
{
    public partial class AddressServiceTests
    {
        [Fact]
        public async Task ShouldValidateAddressesAndAssignIdAndAuditOnAddAsync()
        {
            // Given
            List<Address> randomAddresses = CreateRandomAddresses();
            List<Address> inputAddresses = randomAddresses;
            string inputFilename = GetRandomString();
            string randomEntraUserId = GetRandomStringWithLengthOf(50);
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Guid randomId = Guid.NewGuid();
            List<Address> outputAddresses = inputAddresses.DeepClone();

            foreach (Address address in outputAddresses)
            {
                address.Id = randomId;
                address.CreatedBy = randomEntraUserId;
                address.CreatedDate = randomDateTimeOffset;
                address.UpdatedBy = randomEntraUserId;
                address.UpdatedDate = randomDateTimeOffset;
            }

            List<Address> expectedAddresses = outputAddresses.DeepClone();

            var addressServiceMock = new Mock<AddressService>(
                this.storageBrokerMock.Object,
                this.dateTimeBrokerMock.Object,
                this.securityAuditBrokerMock.Object,
                this.identifierBrokerMock.Object,
                this.loggingBrokerMock.Object,
                this.auditBrokerMock.Object)
            {
                CallBase = true
            };

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ReturnsAsync(randomId);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetCurrentUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            // When
            List<Address> actualAddresses = await addressServiceMock.Object
                .ValidateAddressesAndAssignIdAndAuditOnAddAsync(inputAddresses, inputFilename);

            // Then
            actualAddresses.Should().BeEquivalentTo(expectedAddresses);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(inputAddresses.Count * 2));

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Exactly(inputAddresses.Count));

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetCurrentUserIdAsync(),
                    Times.Exactly(inputAddresses.Count * 2));

            addressServiceMock.Verify(service =>
                service.ValidateAddressesAndAssignIdAndAuditOnAddAsync(inputAddresses, inputFilename),
                    Times.Once);

            addressServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
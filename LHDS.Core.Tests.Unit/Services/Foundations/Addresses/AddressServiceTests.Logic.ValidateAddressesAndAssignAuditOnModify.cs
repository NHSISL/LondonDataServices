// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Services.Foundations.Addresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Addresses
{
    public partial class AddressServiceTests
    {
        [Fact]
        public async Task ShouldValidateAddressesAndAssignAuditOnModifyAsync()
        {
            // Given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomEntraUserId = GetRandomStringWithLengthOf(50);
            int randomCount = GetRandomNumber();

            List<Address> randomAddresses =
                CreateRandomAddresses(
                    count: randomCount,
                    dateTimeOffset: randomDateTimeOffset.AddMinutes(-10),
                    userId: randomEntraUserId);

            List<Address> inputAddresses = randomAddresses;
            string inputFilename = GetRandomString();
            Guid randomId = Guid.NewGuid();
            List<Address> outputAddresses = inputAddresses.DeepClone();

            foreach (Address address in outputAddresses)
            {
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

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            // When
            List<Address> actualAddresses =
                await addressServiceMock.Object.ValidateAddressesAndAssignAuditOnModifyAsync(
                    inputAddresses, inputFilename);

            // Then
            actualAddresses.Should().BeEquivalentTo(expectedAddresses);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(randomCount * 2));

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Exactly(randomCount * 2));

            addressServiceMock.Verify(service =>
                service.ValidateAddressesAndAssignAuditOnModifyAsync(inputAddresses, inputFilename),
                    Times.Once);

            addressServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
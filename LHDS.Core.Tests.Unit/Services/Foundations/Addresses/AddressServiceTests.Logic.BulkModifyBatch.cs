// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task ShouldBulkBatchModifyAddressesAsync()
        {
            // given
            int randomBatchSize = GetRandomNumber();
            int inputBatchSize = randomBatchSize;
            EntraUser randomEntraUser = CreateRandomEntraUser();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomFileName = GetRandomString();
            string inputFileName = randomFileName;

            List<Address> randomAddresses = CreateRandomAddresses(
                count: inputBatchSize,
                dateTimeOffset: randomDateTimeOffset,
                userId: randomEntraUser.EntraUserId);

            List<Address> inputAddresses = randomAddresses;

            var addressServiceMock = new Mock<AddressService>(
                this.storageBrokerMock.Object,
                this.dateTimeBrokerMock.Object,
                this.securityBrokerMock.Object,
                this.identifierBrokerMock.Object,
                this.loggingBrokerMock.Object,
                this.auditBrokerMock.Object)
            {
                CallBase = true
            };

            addressServiceMock.Setup(service =>
                service.BulkAddOrModifyBatch(inputAddresses, inputFileName, 10000))
                    .Returns(ValueTask.CompletedTask);

            // when
            await addressServiceMock.Object
                .BulkAddOrModifyBatch(inputAddresses, inputFileName, inputBatchSize);

            // then
            addressServiceMock.Verify(service =>
                service.BulkAddOrModifyBatch(inputAddresses, inputFileName, 10000),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
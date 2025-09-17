// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.Addresses.Exceptions;
using LHDS.Core.Services.Foundations.Addresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Addresses
{
    public partial class AddressServiceTests
    {
        [Fact]
        public async Task ShouldThrowAggregateServiceExceptionOnBulkModifyIfErrorOccursAndLogItAsync()
        {
            // given
            int randomCount = GetRandomNumber();
            string randomFileName = GetRandomString();
            string inputFileName = randomFileName;
            List<Address> someAddresses = CreateRandomAddresses(count: randomCount);
            List<Address> inputAddresses = someAddresses;
            string someFileName = GetRandomString();
            Exception exception = new Exception("Some exception");

            var aggregateException =
                new AggregateException(
                    message: $"Unable to process addresses in 1 of the batch(es) from {someFileName}",
                    innerExceptions: exception);

            var failedAddressServiceException =
                new FailedAddressServiceException(
                    message: "Failed aggregate address service error occurred, please contact support.",
                    innerException: aggregateException);

            var expectedAddressServiceException =
                new AddressServiceException(
                    message: "Address service error occurred, please contact support.",
                    innerException: failedAddressServiceException);

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

            addressServiceMock.Setup(service =>
                service.BulkAddOrModifyBatchAsync(It.IsAny<List<Address>>(), It.IsAny<string>(), It.IsAny<int>()))
                    .ThrowsAsync(aggregateException);

            // when
            ValueTask bulkModifyAddressTask = addressServiceMock.Object
                .BulkModifyAddressesAsync(addresses: someAddresses, fileName: someFileName);

            AddressServiceException actualAddressDependencyException =
                await Assert.ThrowsAsync<AddressServiceException>(
                    bulkModifyAddressTask.AsTask);

            // then
            actualAddressDependencyException.Should()
                .BeEquivalentTo(expectedAddressServiceException);

            addressServiceMock.Verify(service =>
                service.BulkAddOrModifyBatchAsync(It.IsAny<List<Address>>(), It.IsAny<string>(), It.IsAny<int>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnBulkModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

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

            addressServiceMock.Setup(service =>
                service.BulkAddOrModifyBatchAsync(It.IsAny<List<Address>>(), It.IsAny<string>(), It.IsAny<int>()))
                    .ThrowsAsync(serviceException);

            int randomCount = GetRandomNumber();
            string someFileName = GetRandomString();
            List<Address> someAddresses = CreateRandomAddresses(count: randomCount);

            var failedAddressServiceException =
                new FailedAddressServiceException(
                    message: "Failed address service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedAddressServiceException =
                new AddressServiceException(
                    message: "Address service error occurred, please contact support.",
                    innerException: failedAddressServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask addAddressTask = addressServiceMock.Object
                .BulkModifyAddressesAsync(addresses: someAddresses, fileName: someFileName);

            AddressServiceException actualAddressServiceException =
                await Assert.ThrowsAsync<AddressServiceException>(
                    addAddressTask.AsTask);

            // then
            actualAddressServiceException.Should()
                .BeEquivalentTo(expectedAddressServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
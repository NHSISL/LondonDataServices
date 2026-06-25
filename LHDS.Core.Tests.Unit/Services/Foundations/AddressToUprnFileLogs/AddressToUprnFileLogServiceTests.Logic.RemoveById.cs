// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.AddressToUprnFileLogs;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressToUprnFileLogs
{
    public partial class AddressToUprnFileLogServiceTests
    {
        [Fact]
        public async Task ShouldRemoveAddressToUprnFileLogByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputAddressToUprnFileLogId = randomId;
            AddressToUprnFileLog randomAddressToUprnFileLog = CreateRandomAddressToUprnFileLog();
            AddressToUprnFileLog storageAddressToUprnFileLog = randomAddressToUprnFileLog;
            AddressToUprnFileLog expectedInputAddressToUprnFileLog = storageAddressToUprnFileLog;
            AddressToUprnFileLog deletedAddressToUprnFileLog = expectedInputAddressToUprnFileLog;
            AddressToUprnFileLog expectedAddressToUprnFileLog = deletedAddressToUprnFileLog.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressToUprnFileLogByIdAsync(inputAddressToUprnFileLogId))
                    .ReturnsAsync(storageAddressToUprnFileLog);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteAddressToUprnFileLogAsync(expectedInputAddressToUprnFileLog))
                    .ReturnsAsync(deletedAddressToUprnFileLog);

            // when
            AddressToUprnFileLog actualAddressToUprnFileLog =
                await this.addressToUprnFileLogService
                    .RemoveAddressToUprnFileLogByIdAsync(inputAddressToUprnFileLogId);

            // then
            actualAddressToUprnFileLog.Should().BeEquivalentTo(expectedAddressToUprnFileLog);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressToUprnFileLogByIdAsync(inputAddressToUprnFileLogId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAddressToUprnFileLogAsync(expectedInputAddressToUprnFileLog),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}

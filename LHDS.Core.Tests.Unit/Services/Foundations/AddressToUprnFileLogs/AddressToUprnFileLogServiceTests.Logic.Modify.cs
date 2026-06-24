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
        public async Task ShouldModifyAddressToUprnFileLogAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            AddressToUprnFileLog randomAddressToUprnFileLog =
                CreateRandomModifyAddressToUprnFileLog(randomDateTimeOffset, randomUserId);

            AddressToUprnFileLog inputAddressToUprnFileLog = randomAddressToUprnFileLog;
            AddressToUprnFileLog storageAddressToUprnFileLog = inputAddressToUprnFileLog.DeepClone();
            storageAddressToUprnFileLog.UpdatedWhen = randomAddressToUprnFileLog.CreatedWhen;
            AddressToUprnFileLog updatedAddressToUprnFileLog = inputAddressToUprnFileLog;
            AddressToUprnFileLog expectedAddressToUprnFileLog = updatedAddressToUprnFileLog.DeepClone();

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(inputAddressToUprnFileLog))
                    .ReturnsAsync(inputAddressToUprnFileLog);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressToUprnFileLogByIdAsync(inputAddressToUprnFileLog.Id))
                    .ReturnsAsync(storageAddressToUprnFileLog);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateAddressToUprnFileLogAsync(inputAddressToUprnFileLog))
                    .ReturnsAsync(updatedAddressToUprnFileLog);

            // when
            AddressToUprnFileLog actualAddressToUprnFileLog =
                await this.addressToUprnFileLogService.ModifyAddressToUprnFileLogAsync(inputAddressToUprnFileLog);

            // then
            actualAddressToUprnFileLog.Should().BeEquivalentTo(expectedAddressToUprnFileLog);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(inputAddressToUprnFileLog),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressToUprnFileLogByIdAsync(inputAddressToUprnFileLog.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAddressToUprnFileLogAsync(inputAddressToUprnFileLog),
                    Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

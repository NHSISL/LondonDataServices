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
        public async Task ShouldAddAddressToUprnFileLogAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);
            AddressToUprnFileLog randomAddressToUprnFileLog =
                CreateRandomAddressToUprnFileLog(randomDateTimeOffset, randomUserId);

            AddressToUprnFileLog inputAddressToUprnFileLog = randomAddressToUprnFileLog;
            AddressToUprnFileLog storageAddressToUprnFileLog = inputAddressToUprnFileLog;
            AddressToUprnFileLog expectedAddressToUprnFileLog = storageAddressToUprnFileLog.DeepClone();

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(inputAddressToUprnFileLog))
                    .ReturnsAsync(storageAddressToUprnFileLog);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAddressToUprnFileLogAsync(inputAddressToUprnFileLog))
                    .ReturnsAsync(storageAddressToUprnFileLog);

            // when
            AddressToUprnFileLog actualAddressToUprnFileLog =
                await this.addressToUprnFileLogService.AddAddressToUprnFileLogAsync(inputAddressToUprnFileLog);

            // then
            actualAddressToUprnFileLog.Should().BeEquivalentTo(expectedAddressToUprnFileLog);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(inputAddressToUprnFileLog),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressToUprnFileLogAsync(inputAddressToUprnFileLog),
                    Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

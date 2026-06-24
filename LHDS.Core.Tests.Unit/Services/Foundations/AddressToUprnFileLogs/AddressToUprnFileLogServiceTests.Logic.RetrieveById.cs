// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        public async Task ShouldRetrieveAddressToUprnFileLogByIdAsync()
        {
            // given
            AddressToUprnFileLog randomAddressToUprnFileLog = CreateRandomAddressToUprnFileLog();
            AddressToUprnFileLog inputAddressToUprnFileLog = randomAddressToUprnFileLog;
            AddressToUprnFileLog storageAddressToUprnFileLog = randomAddressToUprnFileLog;
            AddressToUprnFileLog expectedAddressToUprnFileLog = storageAddressToUprnFileLog.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressToUprnFileLogByIdAsync(inputAddressToUprnFileLog.Id))
                    .ReturnsAsync(storageAddressToUprnFileLog);

            // when
            AddressToUprnFileLog actualAddressToUprnFileLog =
                await this.addressToUprnFileLogService
                    .RetrieveAddressToUprnFileLogByIdAsync(inputAddressToUprnFileLog.Id);

            // then
            actualAddressToUprnFileLog.Should().BeEquivalentTo(expectedAddressToUprnFileLog);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressToUprnFileLogByIdAsync(inputAddressToUprnFileLog.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

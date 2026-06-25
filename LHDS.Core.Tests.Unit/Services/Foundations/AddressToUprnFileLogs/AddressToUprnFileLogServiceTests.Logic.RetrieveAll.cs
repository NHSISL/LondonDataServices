// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressToUprnFileLogs;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressToUprnFileLogs
{
    public partial class AddressToUprnFileLogServiceTests
    {
        [Fact]
        public async Task ShouldReturnAddressToUprnFileLogs()
        {
            // given
            List<AddressToUprnFileLog> randomAddressToUprnFileLogs = CreateRandomAddressToUprnFileLogs();
            List<AddressToUprnFileLog> storageAddressToUprnFileLogs = randomAddressToUprnFileLogs;
            List<AddressToUprnFileLog> expectedAddressToUprnFileLogs = storageAddressToUprnFileLogs;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAddressToUprnFileLogsAsync())
                    .ReturnsAsync(storageAddressToUprnFileLogs.AsQueryable());

            // when
            IQueryable<AddressToUprnFileLog> actualAddressToUprnFileLogs =
                await this.addressToUprnFileLogService.RetrieveAllAddressToUprnFileLogsAsync();

            // then
            actualAddressToUprnFileLogs.Should().BeEquivalentTo(expectedAddressToUprnFileLogs);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAddressToUprnFileLogsAsync(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}

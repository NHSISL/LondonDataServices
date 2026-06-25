// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;

using LHDS.Core.Models.Foundations.AddressToUprnFileLogs;

using Xunit;

namespace LHDS.Core.Tests.Integration.Addresses
{
    public partial class AddressToUprnTests
    {
        [ReleaseCandidateFact]
        public async Task ShouldMatchAddressToUprnAsync()
        {
            // given
            string randomUnstructuredAddress = "10 Downing Street Westminster London SW1A 2AA";
            string randomFileName = $"{GetRandomString()}.csv";
            Guid randomCorrelationId = Guid.NewGuid();

            using Stream inputStream = CreateAddressCsvStream(randomUnstructuredAddress);

            // when
            await this.addressToUprnClient.MatchAddressToUprnAsync(
                data: inputStream,
                filename: randomFileName,
                correlationId: randomCorrelationId);

            AddressToUprnFileLog maybeAddressToUprnFileLog =
                await this.RetrieveAddressToUprnFileLogByFileNameAsync(randomFileName);

            // then
            Assert.NotNull(maybeAddressToUprnFileLog);
            Assert.Equal(randomFileName, maybeAddressToUprnFileLog.FileName);

            await this.addressToUprnFileLogService
                .RemoveAddressToUprnFileLogByIdAsync(maybeAddressToUprnFileLog.Id);
        }
    }
}

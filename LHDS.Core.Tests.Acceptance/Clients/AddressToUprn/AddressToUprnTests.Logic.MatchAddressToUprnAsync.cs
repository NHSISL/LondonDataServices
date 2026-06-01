// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressToUprnFileLogs;
using LHDS.Core.Models.Foundations.AssignAddresses;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.AddressToUprn
{
    public partial class AddressToUprnTests
    {
        [Fact(Skip = "Acceptance test — requires live infrastructure and valid credentials.")]
        public async Task ShouldMatchAddressToUprnAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUnstructuredAddress = GetRandomString();
            string randomFileName = $"{GetRandomString()}.csv";
            Guid randomCorrelationId = GetRandomGuid();
            Stream inputStream = CreateAddressCsvStream(randomUnstructuredAddress);
            AssignAddress randomAssignAddress = CreateRandomAssignAddress(randomDateTimeOffset);

            this.wireMockServer
                .Given(
                    Request
                        .Create()
                        .UsingGet()
                        .WithPath("/api/getinfo")
                        .WithParam("adrec", randomUnstructuredAddress))
                .RespondWith(
                    Response
                        .Create()
                        .WithStatusCode(HttpStatusCode.OK)
                        .WithBodyAsJson(randomAssignAddress));

            // when
            await this.addressToUprnClient.MatchAddressToUprnAsync(
                data: inputStream,
                filename: randomFileName,
                correlationId: randomCorrelationId,
                cancellationToken: TestContext.Current.CancellationToken);

            // then
            IQueryable<AddressToUprnFileLog> allFileLogs =
                await this.addressToUprnFileLogService.RetrieveAllAddressToUprnFileLogsAsync();

            AddressToUprnFileLog actualFileLog =
                allFileLogs.FirstOrDefault(log => log.FileName == randomFileName);

            actualFileLog.Should().NotBeNull();
            actualFileLog.FileName.Should().Be(randomFileName);
            actualFileLog.EntryCount.Should().Be(1);
            actualFileLog.SuccessStatus.Should().NotBe(SuccessStatus.Failed);

            // cleanup
            await this.addressToUprnFileLogService
                .RemoveAddressToUprnFileLogByIdAsync(actualFileLog.Id);
        }
    }
}

// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Services.Foundations.OptOuts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OptOuts
{
    public partial class OptOutServiceTests
    {
        [Fact]
        public async Task ShouldBulkModifyOptOutsAsync()
        {
            // given
            int randomCount = GetRandomNumber();
            EntraUser randomEntraUser = CreateRandomEntraUser();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomFileName = GetRandomString();
            string inputFileName = randomFileName;

            List<OptOut> randomOptOuts = CreateRandomOptOuts(
                count: randomCount,
                randomDateTimeOffset,
                userId: randomEntraUser.EntraUserId);

            List<OptOut> inputOptOuts = randomOptOuts;

            var optOutServiceMock = new Mock<OptOutService>(
                this.storageBrokerMock.Object,
                this.dateTimeBrokerMock.Object,
                this.securityAuditBrokerMock.Object,
                this.identifierBrokerMock.Object,
                this.loggingBrokerMock.Object,
                this.auditBrokerMock.Object)
            {
                CallBase = true
            };

            optOutServiceMock.Setup(service =>
                service.BulkAddOrModifyBatchAsync(
                    inputOptOuts, inputFileName, 10000))
                        .Returns(ValueTask.CompletedTask);

            // when
            await optOutServiceMock.Object
                .BulkModifyOptOutsAsync(inputOptOuts, inputFileName);

            // then
            optOutServiceMock.Verify(service =>
                service.BulkAddOrModifyBatchAsync(
                    inputOptOuts, inputFileName, 10000),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

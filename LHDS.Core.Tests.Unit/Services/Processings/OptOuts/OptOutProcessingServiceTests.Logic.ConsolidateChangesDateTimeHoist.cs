// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.OptOuts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.OptOuts
{
    public partial class OptOutProcessingServiceTests
    {
        [Fact]
        public async Task
            ShouldCallGetCurrentDateTimeOffsetOnlyOnceAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            List<OptOut> randomConsentList =
                CreateRandomOptOutList("Opt-In").ToList();

            List<OptOut> randomNonConsentList =
                CreateRandomOptOutList("Opt-Out").ToList();

            List<OptOut> currentOptOutList = new List<OptOut>();
            currentOptOutList.AddRange(randomConsentList);
            currentOptOutList.AddRange(randomNonConsentList);
            List<OptOut> currentOptOutInputList = currentOptOutList.DeepClone();

            List<string> consentedNhsNumbers = new List<string>
            {
                randomNonConsentList.First().NhsNumber
            };

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.optOutServiceMock.Setup(service =>
                service.ModifyOptOutAsync(It.IsAny<OptOut>()))
                    .ReturnsAsync((OptOut o) => o);

            // when
            await this.optOutProcessingService
                .ConsolidateOptOutChangesAndReturnChangesOnly(
                    currentOptOutInputList,
                    consentedIdentifiers: consentedNhsNumbers);

            // then
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}

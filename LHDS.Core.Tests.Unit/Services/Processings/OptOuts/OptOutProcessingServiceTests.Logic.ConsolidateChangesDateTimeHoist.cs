// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            List<OptOut> consentedList = currentOptOutList
                .Where(optOut => consentedNhsNumbers.Contains(optOut.NhsNumber))
                    .ToList();

            List<OptOut> nonConsentedList = currentOptOutList
                .Except(consentedList).ToList();

            List<OptOut> allModifiedOptOuts = new List<OptOut>();
            allModifiedOptOuts.AddRange(consentedList);
            allModifiedOptOuts.AddRange(nonConsentedList);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

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

            foreach (var item in consentedList)
            {
                if (item == null)
                {
                    continue;
                }

                item.CacheTime = randomDateTimeOffset;
                item.LastSentToMesh = randomDateTimeOffset;
                item.Status = "Opt-In";
            }

            foreach (var item in nonConsentedList)
            {
                item.CacheTime = randomDateTimeOffset;
                item.LastSentToMesh = randomDateTimeOffset;
                item.Status = "Opt-Out";
            }

            this.optOutServiceMock.Verify(service =>
                service.BulkModifyOptOutsAsync(
                    It.Is<List<OptOut>>(list =>
                        this.compareLogic.Compare(list, allModifiedOptOuts).AreEqual),
                    "ConsolidateOptOutChanges"),
                        Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

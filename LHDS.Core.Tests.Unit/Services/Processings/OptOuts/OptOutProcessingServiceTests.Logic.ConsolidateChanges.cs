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
        public async Task ShouldConsolidateOptOutChangesAndReturnChangesOnlyAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            int randomNumber = GetRandomNumber();
            string randomBatchReference = GetRandomString();
            List<OptOut> randomConsentList = CreateRandomOptOutList("Opt-In").ToList();
            List<OptOut> randomNonConsentList = CreateRandomOptOutList("Opt-Out").ToList();
            List<OptOut> randomUnknownList = CreateRandomOptOutList("Unknown").ToList();
            List<OptOut> currentOptOutList = new List<OptOut>();
            currentOptOutList.AddRange(randomConsentList);
            currentOptOutList.AddRange(randomNonConsentList);
            currentOptOutList.AddRange(randomUnknownList);
            List<OptOut> currentOptOutInputList = currentOptOutList.DeepClone();
            List<string> consentedNhsNumbers = new List<string> { randomNonConsentList.FirstOrDefault().NhsNumber };

            List<OptOut> consentedList = currentOptOutList
                .Where(optOut => consentedNhsNumbers.Contains(optOut.NhsNumber))
                    .ToList();

            List<OptOut> nonConsentedList = currentOptOutList
                .Except(consentedList).ToList();

            List<OptOut> delta = new List<OptOut>();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            foreach (var item in consentedList)
            {
                if (item == null)
                {
                    continue;
                }

                if (item.Status != "Opt-In")
                {
                    delta.Add(item);
                }

                item.UpdatedDate = randomDateTimeOffset;
                item.CacheTime = randomDateTimeOffset;
                item.LastSentToMesh = randomDateTimeOffset;
                item.Status = "Opt-In";
            }

            foreach (var item in nonConsentedList)
            {
                if (item.Status != "Opt-Out")
                {
                    delta.Add(item);
                }

                item.UpdatedDate = randomDateTimeOffset;
                item.CacheTime = randomDateTimeOffset;
                item.LastSentToMesh = randomDateTimeOffset;
                item.Status = "Opt-Out";
            }

            List<OptOut> expectedOptOutList = delta.DeepClone();

            // when
            List<OptOut> actualOptOutList = await this.optOutProcessingService
                .ConsolidateOptOutChangesAndReturnChangesOnly(
                currentOptOutInputList,
                consentedIdentifiers: consentedNhsNumbers);

            // then
            actualOptOutList.Should().BeEquivalentTo(expectedOptOutList);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(currentOptOutInputList.Count));

            foreach (var item in consentedList)
            {
                if (item.Status != "Opt-In")
                {
                    delta.Add(item);
                }

                item.UpdatedDate = randomDateTimeOffset;
                item.CacheTime = randomDateTimeOffset;
                item.LastSentToMesh = randomDateTimeOffset;
                item.Status = "Opt-In";

                this.optOutServiceMock.Verify(service =>
                   service.ModifyOptOutAsync(It.Is(SameOptOutAs(item))),
                       Times.Once);
            }

            foreach (var item in nonConsentedList)
            {
                if (item.Status != "Opt-Out")
                {
                    delta.Add(item);
                }

                item.UpdatedDate = randomDateTimeOffset;
                item.CacheTime = randomDateTimeOffset;
                item.LastSentToMesh = randomDateTimeOffset;
                item.Status = "Opt-Out";

                this.optOutServiceMock.Verify(service =>
                   service.ModifyOptOutAsync(It.Is(SameOptOutAs(item))),
                       Times.Once);
            }

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnEmptyOptOutListOnConsolidateChangesOnlyAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            int randomNumber = GetRandomNumber();
            string randomBatchReference = GetRandomString();
            List<OptOut> randomConsentList = CreateRandomOptOutList("Opt-In").ToList();
            List<OptOut> randomNonConsentList = CreateRandomOptOutList("Opt-Out").ToList();
            List<OptOut> currentOptOutList = new List<OptOut>();
            currentOptOutList.AddRange(randomConsentList);
            currentOptOutList.AddRange(randomNonConsentList);
            List<OptOut> currentOptOutInputList = currentOptOutList.DeepClone();
            List<string> consentedNhsNumbers = randomConsentList.Select(optOut => optOut.NhsNumber).ToList();

            List<OptOut> consentedList = currentOptOutList
                .Where(optOut => consentedNhsNumbers.Contains(optOut.NhsNumber))
                    .ToList();

            List<OptOut> nonConsentedList = currentOptOutList
                .Except(consentedList).ToList();

            List<OptOut> delta = new List<OptOut>();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            foreach (var item in consentedList)
            {
                item.UpdatedDate = randomDateTimeOffset;
                item.CacheTime = randomDateTimeOffset;
                item.LastSentToMesh = randomDateTimeOffset;
                item.Status = "Opt-In";
            }

            foreach (var item in nonConsentedList)
            {
                item.UpdatedDate = randomDateTimeOffset;
                item.CacheTime = randomDateTimeOffset;
                item.LastSentToMesh = randomDateTimeOffset;
                item.Status = "Opt-Out";
            }

            List<OptOut> expectedOptOutList = delta.DeepClone();

            // when
            List<OptOut> actualOptOutList = await this.optOutProcessingService
                .ConsolidateOptOutChangesAndReturnChangesOnly(
                currentOptOutInputList,
                consentedIdentifiers: consentedNhsNumbers);

            // then
            actualOptOutList.Should().BeEquivalentTo(expectedOptOutList);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(currentOptOutInputList.Count));

            foreach (var item in consentedList)
            {
                item.UpdatedDate = randomDateTimeOffset;
                item.CacheTime = randomDateTimeOffset;
                item.LastSentToMesh = randomDateTimeOffset;
                item.Status = "Opt-In";

                this.optOutServiceMock.Verify(service =>
                   service.ModifyOptOutAsync(It.Is(SameOptOutAs(item))),
                       Times.Once);
            }

            foreach (var item in nonConsentedList)
            {
                item.UpdatedDate = randomDateTimeOffset;
                item.CacheTime = randomDateTimeOffset;
                item.LastSentToMesh = randomDateTimeOffset;
                item.Status = "Opt-Out";

                this.optOutServiceMock.Verify(service =>
                   service.ModifyOptOutAsync(It.Is(SameOptOutAs(item))),
                       Times.Once);
            }

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
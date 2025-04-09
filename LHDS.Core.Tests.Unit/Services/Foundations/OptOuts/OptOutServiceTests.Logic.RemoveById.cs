// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.OptOuts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OptOuts
{
    public partial class OptOutServiceTests
    {
        [Fact]
        public async Task ShouldRemoveOptOutByIdAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            OptOut randomOptOut = 
                CreateRandomOptOut(randomDateTimeOffset, randomEntraUser.EntraUserId);

            Guid inputOptOutId = randomOptOut.Id;
            OptOut storageOptOut = randomOptOut;
            OptOut ingestionTrackingWithDeleteAuditApplied = storageOptOut.DeepClone();
            ingestionTrackingWithDeleteAuditApplied.UpdatedBy = randomEntraUser.EntraUserId.ToString();
            ingestionTrackingWithDeleteAuditApplied.UpdatedDate = randomDateTimeOffset;
            OptOut updatedOptOut = storageOptOut;
            OptOut deletedOptOut = updatedOptOut;
            OptOut expectedOptOut = deletedOptOut.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOptOutByIdAsync(inputOptOutId))
                    .ReturnsAsync(storageOptOut);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateOptOutAsync(randomOptOut))
                    .ReturnsAsync(updatedOptOut);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteOptOutAsync(updatedOptOut))
                    .ReturnsAsync(deletedOptOut);

            // when
            OptOut actualOptOut = await this.optOutService
                .RemoveOptOutByIdAsync(inputOptOutId);

            // then
            actualOptOut.Should().BeEquivalentTo(expectedOptOut);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOptOutByIdAsync(inputOptOutId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2));

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOptOutAsync(randomOptOut),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteOptOutAsync(updatedOptOut),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
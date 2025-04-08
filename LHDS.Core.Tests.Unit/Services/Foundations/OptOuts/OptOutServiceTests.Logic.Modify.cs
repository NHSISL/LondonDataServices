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
        public async Task ShouldModifyOptOutAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            OptOut randomOptOut =
                CreateRandomModifyOptOut(randomDateTimeOffset, randomEntraUser.EntraUserId);

            OptOut inputOptOut = randomOptOut;
            OptOut storageOptOut = inputOptOut.DeepClone();
            storageOptOut.UpdatedDate = randomOptOut.CreatedDate;
            OptOut updatedOptOut = inputOptOut;
            OptOut expectedOptOut = updatedOptOut.DeepClone();
            Guid optOutId = inputOptOut.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOptOutByIdAsync(optOutId))
                    .ReturnsAsync(storageOptOut);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateOptOutAsync(inputOptOut))
                    .ReturnsAsync(updatedOptOut);

            // when
            OptOut actualOptOut =
                await this.optOutService.ModifyOptOutAsync(inputOptOut);

            // then
            actualOptOut.Should().BeEquivalentTo(expectedOptOut);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(2));

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2));

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOptOutByIdAsync(inputOptOut.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOptOutAsync(inputOptOut),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
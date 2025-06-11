// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
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
            Guid randomId = Guid.NewGuid();
            Guid inputOptOutId = randomId;
            OptOut randomOptOut = CreateRandomOptOut();
            OptOut storageOptOut = randomOptOut;
            OptOut expectedInputOptOut = storageOptOut;
            OptOut deletedOptOut = expectedInputOptOut;
            OptOut expectedOptOut = deletedOptOut.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOptOutByIdAsync(inputOptOutId))
                    .ReturnsAsync(storageOptOut);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteOptOutAsync(expectedInputOptOut))
                    .ReturnsAsync(deletedOptOut);

            // when
            OptOut actualOptOut = await this.optOutService
                .RemoveOptOutByIdAsync(inputOptOutId);

            // then
            actualOptOut.Should().BeEquivalentTo(expectedOptOut);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOptOutByIdAsync(inputOptOutId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteOptOutAsync(expectedInputOptOut),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
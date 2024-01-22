// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
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
        public async Task ShouldRemoveOptOutProcessingAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputOptOutId = randomId;
            OptOut randomOptOut = CreateRandomOptOut();
            OptOut deletedOptOut = randomOptOut;
            OptOut expectedOptOut = deletedOptOut.DeepClone();

            this.optOutServiceMock.Setup(service =>
                service.RemoveOptOutByIdAsync(inputOptOutId))
                    .ReturnsAsync(deletedOptOut);

            // when
            OptOut actualOptOut =
                await this.optOutProcessingService.RemoveOptOutByIdAsync(inputOptOutId);

            // then
            actualOptOut.Should().BeEquivalentTo(expectedOptOut);

            this.optOutServiceMock.Verify(service =>
                service.RemoveOptOutByIdAsync(inputOptOutId),
                    Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
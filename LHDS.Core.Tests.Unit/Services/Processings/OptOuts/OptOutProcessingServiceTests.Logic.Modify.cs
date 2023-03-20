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
        public async Task ShouldModifyOptOutProcessingAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            OptOut randomOptOut = CreateRandomOptOut(randomDateTimeOffset);
            OptOut inputOptOut = randomOptOut;
            OptOut updatedOptOut = inputOptOut;
            OptOut expectedOptOut = updatedOptOut.DeepClone();

            this.optOutServiceMock.Setup(service =>
                service.ModifyOptOutAsync(inputOptOut))
                    .ReturnsAsync(inputOptOut);

            // when
            OptOut actualOptOut =
                await this.optOutProcessingService.ModifyOptOutAsync(inputOptOut);

            // then
            actualOptOut.Should().BeEquivalentTo(expectedOptOut);

            this.optOutServiceMock.Verify(service =>
                service.ModifyOptOutAsync(inputOptOut),
                    Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
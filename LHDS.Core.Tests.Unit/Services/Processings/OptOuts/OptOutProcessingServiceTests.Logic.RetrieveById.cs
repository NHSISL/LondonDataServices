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

namespace LHDS.Core.Tests.Unit.Services.Processings.OptOuts
{
    public partial class OptOutProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveOptOutProcessingByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputOptOutId = randomId;
            OptOut randomOptOut = CreateRandomOptOut();
            OptOut retrievedOptOut = randomOptOut;
            OptOut expectedOptOut = retrievedOptOut.DeepClone();

            this.optOutServiceMock.Setup(service =>
                service.RetrieveOptOutByIdAsync(inputOptOutId))
                    .ReturnsAsync(retrievedOptOut);

            // when
            OptOut actualOptOut =
                await this.optOutProcessingService.RetrieveOptOutByIdAsync(inputOptOutId);

            // then
            actualOptOut.Should().BeEquivalentTo(expectedOptOut);

            this.optOutServiceMock.Verify(service =>
                service.RetrieveOptOutByIdAsync(inputOptOutId),
                    Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
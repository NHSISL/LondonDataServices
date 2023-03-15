// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.OptOuts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.OptOuts
{
    public partial class OptOutProcessingServiceTests
    {
        [Fact]
        public async Task ShouldAddOptOutProcessingAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            OptOut randomOptOut = CreateRandomOptOut(randomDateTimeOffset);
            OptOut inputOptOut = randomOptOut;

            // when
            await this.optOutProcessingService.RetrieveOrAddOptOutAsync(inputOptOut);

            // then
            this.optOutServiceMock.Verify(service =>
                service.AddOptOutAsync(inputOptOut),
                    Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldNotAddOptOutWhenExistingOptOutIsFoundAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            OptOut existingOptOut = CreateRandomOptOut(randomDateTimeOffset);
            OptOut existingOptOutFound = existingOptOut;

            this.optOutServiceMock.Setup(service => 
                service.RetrieveOptOutByIdAsync(existingOptOut.Id))
                    .ReturnsAsync(existingOptOutFound);

            // when
            OptOut retrievedOptOut = await this.optOutProcessingService.RetrieveOrAddOptOutAsync(existingOptOut);

            // then
            retrievedOptOut.Should().BeEquivalentTo(existingOptOutFound);

            this.optOutServiceMock.Verify(service => 
                service.RetrieveOptOutByIdAsync(existingOptOut.Id),
                    Times.Once);

            this.optOutServiceMock.Verify(service => 
                service.AddOptOutAsync(It.IsAny<OptOut>()), 
                    Times.Never);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
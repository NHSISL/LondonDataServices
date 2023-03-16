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
        public async Task ShouldModifyOptOutProcessingAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            OptOut randomOptOut = CreateRandomOptOut(randomDateTimeOffset);
            OptOut inputOptOut = randomOptOut;

            // when
            await this.optOutProcessingService.ModifyOptOutAsync(inputOptOut);

            // then
            this.optOutServiceMock.Verify(service =>
                service.ModifyOptOutAsync(inputOptOut),
                    Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
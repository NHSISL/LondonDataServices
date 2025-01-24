// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public async Task ShouldAddOptOutIfDoesntExistAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            OptOut randomOptOut = CreateRandomOptOut(randomDateTimeOffset);
            OptOut inputOptOut = randomOptOut;
            OptOut createdOptOut = randomOptOut.DeepClone();
            OptOut updatedOptOut = inputOptOut.DeepClone();
            OptOut expectedOptOut = updatedOptOut.DeepClone();
            IQueryable<OptOut> allOptOuts = CreateRandomOptOuts();

            this.optOutServiceMock.Setup(service =>
                service.RetrieveAllOptOutsAsync())
                    .ReturnsAsync(allOptOuts);

            this.optOutServiceMock.Setup(service =>
               service.AddOptOutAsync(inputOptOut))
                   .ReturnsAsync(createdOptOut);

            // when
            OptOut actualOptOut =
                await this.optOutProcessingService.AddOrModifyOptOutAsync(inputOptOut);

            // then
            actualOptOut.Should().BeEquivalentTo(expectedOptOut);

            this.optOutServiceMock.Verify(service =>
                service.RetrieveAllOptOutsAsync(),
                    Times.Once);

            this.optOutServiceMock.Verify(service =>
                service.AddOptOutAsync(inputOptOut),
                    Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldModifyOptOutProcessingAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            IQueryable<OptOut> allOptOuts = CreateRandomOptOuts();
            OptOut randomOptOut = SelectRandomOptOut(allOptOuts);
            OptOut inputOptOut = randomOptOut;
            OptOut updatedOptOut = inputOptOut.DeepClone();
            OptOut expectedOptOut = updatedOptOut.DeepClone();

            this.optOutServiceMock.Setup(service =>
                service.RetrieveAllOptOutsAsync())
                    .ReturnsAsync(allOptOuts);

            this.optOutServiceMock.Setup(service =>
                service.ModifyOptOutAsync(inputOptOut))
                    .ReturnsAsync(updatedOptOut);

            // when
            OptOut actualOptOut =
                await this.optOutProcessingService.AddOrModifyOptOutAsync(inputOptOut);

            // then
            actualOptOut.Should().BeEquivalentTo(expectedOptOut);

            this.optOutServiceMock.Verify(service =>
               service.RetrieveAllOptOutsAsync(),
                   Times.Once);

            this.optOutServiceMock.Verify(service =>
                service.ModifyOptOutAsync(inputOptOut),
                    Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
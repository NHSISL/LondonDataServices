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
        public async Task ShouldAddOptOutProcessingAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            OptOut randomOptOut = CreateRandomOptOut(randomDateTimeOffset);
            OptOut inputOptOut = randomOptOut;
            OptOut storageOptOut = inputOptOut.DeepClone();
            List<OptOut> optOutlist = CreateRandomOptOuts(string.Empty).ToList();

            this.optOutServiceMock.Setup(service =>
                service.RetrieveAllOptOutsAsync())
                    .ReturnsAsync(optOutlist.AsQueryable);

            this.optOutServiceMock.Setup(service =>
                service.AddOptOutAsync(inputOptOut))
                    .ReturnsAsync(storageOptOut);

            // when
            await this.optOutProcessingService.RetrieveOrAddOptOutAsync(inputOptOut);

            // then
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
        public async Task ShouldNotAddOptOutWhenExistingOptOutIsFoundAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            OptOut existingOptOut = CreateRandomOptOut(randomDateTimeOffset);
            OptOut existingOptOutFound = existingOptOut;
            OptOut storageOptOut = existingOptOut.DeepClone();
            List<OptOut> optOutlist = CreateRandomOptOuts(string.Empty).ToList();
            optOutlist.Add(storageOptOut);

            this.optOutServiceMock.Setup(processings =>
                processings.RetrieveAllOptOutsAsync())
                    .ReturnsAsync(optOutlist.AsQueryable);

            // when
            OptOut retrievedOptOut = await this.optOutProcessingService.RetrieveOrAddOptOutAsync(existingOptOut);

            // then
            retrievedOptOut.Should().BeEquivalentTo(existingOptOutFound);

            this.optOutServiceMock.Verify(service =>
                service.RetrieveAllOptOutsAsync(),
                    Times.Once);

            this.optOutServiceMock.Verify(service =>
                service.AddOptOutAsync(It.IsAny<OptOut>()),
                    Times.Never);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
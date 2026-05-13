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
        public async Task
            ShouldNotOverwriteCacheTimeWhenInputIsDefaultDateTimeOffsetAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            IQueryable<OptOut> allOptOuts = CreateRandomOptOuts();
            OptOut existingOptOut = SelectRandomOptOut(allOptOuts);

            OptOut inputOptOut = existingOptOut.DeepClone();
            inputOptOut.CacheTime = default;
            inputOptOut.LastSentToMesh = default;
            inputOptOut.UpdatedDate = default;

            DateTimeOffset expectedCacheTime = existingOptOut.CacheTime;
            DateTimeOffset expectedLastSentToMesh = existingOptOut.LastSentToMesh;
            DateTimeOffset expectedUpdatedDate = existingOptOut.UpdatedDate;

            this.optOutServiceMock.Setup(service =>
                service.RetrieveAllOptOutsAsync())
                    .ReturnsAsync(allOptOuts);

            this.optOutServiceMock.Setup(service =>
                service.ModifyOptOutAsync(It.IsAny<OptOut>()))
                    .ReturnsAsync((OptOut o) => o);

            // when
            OptOut actualOptOut =
                await this.optOutProcessingService
                    .AddOrModifyOptOutAsync(inputOptOut);

            // then
            actualOptOut.CacheTime.Should().Be(expectedCacheTime);
            actualOptOut.LastSentToMesh.Should().Be(expectedLastSentToMesh);
            actualOptOut.UpdatedDate.Should().Be(expectedUpdatedDate);

            this.optOutServiceMock.Verify(service =>
                service.RetrieveAllOptOutsAsync(),
                    Times.Once);

            this.optOutServiceMock.Verify(service =>
                service.ModifyOptOutAsync(It.IsAny<OptOut>()),
                    Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

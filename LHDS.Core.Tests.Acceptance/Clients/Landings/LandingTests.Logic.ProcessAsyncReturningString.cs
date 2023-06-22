// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Orchestrations.OptOuts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Landings
{
    public partial class LandingTests
    {
        [Fact]
        public async Task ShouldReturnEmptyStringForNullIngestionTrackingOnProcessAsync()
        {
            //Given
            string fileName = GetRandomString();

            //When
            var actualString = await this.landingClient.ProcessAsync(fileName);

            //Then
            actualString.Should().Be(null);

            IngestionTracking ingestionTracking = this.ingestionTrackingService.RetrieveAllIngestionTrackings()
                    .FirstOrDefault(ingestionTracking => ingestionTracking.DecryptedFileName == actualString);

            ingestionTracking.Should().NotBeNull();
            await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(ingestionTracking.Id);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.downloadBrokerMock.VerifyNoOtherCalls();
        }
    }
}

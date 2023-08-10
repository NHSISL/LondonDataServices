// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Audits;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using LHDS.Core.Models.Foundations.OptOuts;
using RESTFulSense.Exceptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.OptOuts
{
    public partial class OptOutsApiTests
    {
        [Fact]
        public async Task ShouldPostOptOutAsync()
        {
            // Given
            OptOut randomOptOut = CreateRandomOptOut();
            OptOut inputOptOut = randomOptOut;
            OptOut expectedOptOut = inputOptOut;

            // When
            OptOut actualOptOut =
                await this.apiBroker.PostOptOutAsync(inputOptOut);

            // Then
            actualOptOut.Should().BeEquivalentTo(expectedOptOut);

            // Cleanup
            //await this.apiBroker.DeleteOptOutByIdAsync(actualOptOut.Id);
        }

        [Fact]
        public async Task ShouldGetOptOutByNhsNumberAsync()
        {
            // Given
            OptOut randomOptOut = await PostRandomOptOutAsync();
            OptOut expectedOptOut = randomOptOut;

            // When
            OptOut actualOptOut =
                await this.apiBroker.GetOptOutByNhsNumberAsync(randomOptOut.NhsNumber);

            // Then
            actualOptOut.Should().BeEquivalentTo(expectedOptOut);

            // Cleanup
            //await this.apiBroker.DeleteOptOutByIdAsync(actualOptOut.Id);
        }

        [Fact]
        public async Task ShouldPutOptOutAsync()
        {
            // given
            OptOut randomOptOut = await PostRandomOptOutAsync();
            OptOut modifiedOptOut = UpdateOptOutWithRandomValues(randomOptOut);

            // when
            await this.apiBroker.PutOptOutAsync(modifiedOptOut);
            OptOut actualOptOut = await this.apiBroker.GetOptOutByNhsNumberAsync(randomOptOut.NhsNumber);

            // then
            actualOptOut.Should().BeEquivalentTo(modifiedOptOut);
            //await this.apiBroker.DeleteOptOutByIdAsync(actualOptOut.Id);
        }
    }
}
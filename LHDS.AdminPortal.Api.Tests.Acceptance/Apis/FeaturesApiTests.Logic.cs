// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.Features
{
    public partial class FeaturesApiTests
    {
        [Fact]
        public async Task ShouldGetFeatureAsync()
        {
            // Given
            var currentFeatures = this.apiBroker.configuration.GetSection("Features").Get<List<string>>();

            // When
            string[] actualFeatures = await this.apiBroker.GetFeaturesAsync();

            // Then
            actualFeatures.Should().BeEquivalentTo(currentFeatures);
        }
    }
}
// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.Features
{
    public partial class FeaturesApiTests
    {
        [Fact]
        public async Task ShouldGetFeatureAsync()
        {
            // Given
            string currentFeatures = this.apiBroker.configuration.GetSection("Features").ToString();

            // When
            string[] actualFeatures = await this.apiBroker.GetFeaturesAsync();

            // Then
            actualFeatures.Should().BeEquivalentTo(currentFeatures);
        }
    }
}
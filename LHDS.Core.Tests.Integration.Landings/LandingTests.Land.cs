// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;

namespace LHDS.Core.Tests.Integration.Landings
{
    public partial class LandingTests
    {
        [ReleaseCandidateFact]
        public async Task ShouldLandFilesAsync()
        {
            try
            {
                //TODO:  Setup files to land
                List<string> files = await landingClient.ProcessAsync();
                files.Should().NotBeNull();
                //TODO: files.Count.Should().BeGreaterThan(setupFiles.Count);
            }
            catch (Exception ex)
            {
                loggingBroker.LogError(ex);
                Console.WriteLine(ex.Message);
            }
        }
    }
}

// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.IO;
using LHDS.Core.Models.Orchestrations.Downloads;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public class LandingConfigurationFixture
    {
        public IOptions<LandingConfiguration> LandingConfigOptions { get; }

        public LandingConfigurationFixture()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();

            var landingConfig = configuration.GetSection("landingSettings").Get<LandingConfiguration>();
            LandingConfigOptions = Options.Create(landingConfig);
        }
    }
}

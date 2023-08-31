// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Audits;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using Microsoft.AspNetCore.Mvc;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string LandingsRelativeUrl = "api/landings";

        public async ValueTask<ActionResult<IngestionTracking>> GetLandingDocumentByFileNameAsync(string fileName) =>
            await this.apiFactoryClient.GetContentAsync<ActionResult<IngestionTracking>>($"{LandingsRelativeUrl}{fileName}");
    }
}

// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using LHDS.Core.Clients;
using LHDS.Core.Services.Orchestrations.Pds;
using Tynamix.ObjectFiller;

namespace LHDS.Core.Tests.Acceptance.Clients
{
    public partial class PdsTests
    {
        private readonly IPdsClient pdsClient;
        private readonly IPdsOrchestrationService pdsOrchestrationService;
        //private readonly WireMockServer wireMockServer;

        public PdsTests()
        {
            this.pdsClient = new PdsClient(pdsOrchestrationService);
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();
    }
}

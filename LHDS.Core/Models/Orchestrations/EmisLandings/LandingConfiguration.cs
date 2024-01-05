// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;

namespace LHDS.Core.Models.Orchestrations.EmisLandings
{
    public class LandingConfiguration
    {
        public Guid LandingSupplierId { get; set; }
        public string EncryptedFolder { get; set; }
        public string DecryptedFolder { get; set; }
    }
}

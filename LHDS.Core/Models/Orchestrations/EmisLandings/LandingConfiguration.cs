// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;

namespace LHDS.Core.Models.Orchestrations.EmisLandings
{
    public class LandingConfiguration
    {
        public Guid LandingSupplierId { get; set; }
        public string EncryptedFolder { get; set; }
        public string DecryptedFolder { get; set; }
        public string KeyVaultUrl { get; set; }
        public string BatchDownloadedFile { get; set; } = "_BatchDownloaded.txt";
        public string BatchReadyFile { get; set; } = "_BatchReady.txt";
    }
}
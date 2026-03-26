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
        public string BatchDownloadedFile { get; set; } = "LDSBatchDownloaded.txt";
        public string BatchReadyFile { get; set; } = "LDSBatchReady.txt";
        public int LastSeenMinutes { get; set; } = 60;
        public string FileNameIncludePattern { get; set; }
        public string FileNameExcludePattern { get; set; }
        public int RelandIntervalInMinutes { get; set; } = 60;
    }
}
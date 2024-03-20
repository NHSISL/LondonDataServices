// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.Core.Tests.Acceptance.Clients.EmisLandings.Models
{
    public class DocumentSource
    {
        public string FtpPath { get; set; }
        public string EncryptedBlobPath { get; set; }
        public string DecryptedBlobPath { get; set; }
        public string FilePath { get; set; }
    }
}

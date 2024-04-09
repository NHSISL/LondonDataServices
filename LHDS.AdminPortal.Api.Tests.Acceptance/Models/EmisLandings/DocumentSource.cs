// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Models.EmisLandings
{
    public class DocumentSource
    {
        public string FtpPath { get; set; }
        public string EncryptedBlobPath { get; set; }
        public string DecryptedBlobPath { get; set; }
        public string FilePath { get; set; }
    }
}

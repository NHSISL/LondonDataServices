// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Models.EmisLandings
{
    public class DocumentSource
    {
        public string FtpPath { get; set; } = string.Empty;
        public string EncryptedBlobPath { get; set; } = string.Empty;
        public string DecryptedBlobPath { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
    }
}

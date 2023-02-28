// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings
{
    public class IngestionTracking
    {
        public string Id { get; set; }
        public string Source { get; set; }
        public string EncryptedFileName { get; set; }
        public string DecryptedFileName { get; set; }
        public bool Decrypted { get; set; }
        public DateTimeOffset LastSeen { get; set; }
        public bool FileDeleted { get; set; }
        public int RecordCount { get; set; }
        public int EncryptedFileSize { get; set; }
        public int DecryptedFileSize { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}

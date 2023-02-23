// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using LHDS.Core.Models.Foundations.Audits;

namespace LHDS.Core.Models.Foundations.IngestionTrackings
{
    public class IngestionTracking
    {
        public string Id { get; set; }
        public string Source { get; set; }
        public string EncryptedFileName { get; set; }
        public string DecryptedFileName { get; set; }
        public bool Decrypted { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset LastSeen { get; set; }
        public bool FileDeleted { get; set; }
        public int RecordCount { get; set; }
        public int EncryptedFileSize { get; set; }
        public int DecryptedFileSize { get; set; }

        public List<Audit> Audits { get; set; }
    }
}

// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;

namespace LHDS.Core.Models.Foundations.IngestionTrackings
{
    public class IngestionTracking
    {
        public string Id { get; set; }
        public string EncryptedFileName { get; set; }
        public string DecryptedFileName { get; set; }
        public bool Decrypted { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset LastSeen { get; set; }
        public bool fileDeleted { get; set; }
        public int fileCount { get; set; }
        public int fileSize { get; set; }
    }
}

// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;

namespace LHDS.Landings.Client.Models.Foundations.IngestionTrackings
{
    public class IngestionTracking
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public bool Decrypted { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}

// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;

namespace LHDS.Core.Models.Foundations.Batches
{
    public class Batch
    {
        public Guid Id { get; set; }
        public string BusinessKey { get; set; } = string.Empty;
        public string SourceSystem { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? ErrorMessage { get; set; }
        public DateTimeOffset StartDateTime { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? EndDateTime { get; set; }
    }
}

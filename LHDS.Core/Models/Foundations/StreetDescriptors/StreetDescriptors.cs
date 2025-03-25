// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Bases;

namespace LHDS.Core.Models.Foundations.StreetDescriptors
{
    public class StreetDescriptor : IKey, IAudit
    {
        public Guid Id { get; set; }
        public string? USRN { get; set; }
        public string? StreetDescription { get; set; }
        public string? Locality { get; set; }
        public string? Town { get; set; }
        public string? AdminisatrativeArea { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public DateTimeOffset? LastUpdatedDate { get; set; }
        public DateTimeOffset? EntryDate { get; set; }
        public bool IsProcessing { get; set; }
        public bool IsSynced { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}

// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using LHDS.Core.Models.Bases;
using LHDS.Core.Models.Foundations.Audits;
using LHDS.Core.Models.Foundations.Suppliers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LHDS.Core.Models.Foundations.IngestionTrackings
{
    public class IngestionTracking : IKey, IAudit
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public Guid SupplierId { get; set; }
        public string EncryptedFileName { get; set; } = string.Empty;
        public string DecryptedFileName { get; set; } = string.Empty;
        public bool Decrypted { get; set; }
        public DateTimeOffset LastSeen { get; set; }
        public bool FileDeleted { get; set; }
        public int RecordCount { get; set; }
        public int EncryptedFileSize { get; set; }
        public int DecryptedFileSize { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTimeOffset UpdatedDate { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;

        [BindNever]
        public Supplier? Supplier { get; set; } = null!;

        [BindNever]
        public List<Audit> Audits { get; set; } = new List<Audit>();
    }
}

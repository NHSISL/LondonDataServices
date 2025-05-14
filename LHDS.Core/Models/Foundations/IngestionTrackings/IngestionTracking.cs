// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using LHDS.Core.Models.Bases;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.Suppliers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LHDS.Core.Models.Foundations.IngestionTrackings
{
    public class IngestionTracking : IKey, IAudit
    {
        public Guid Id { get; set; }
        public Guid SupplierId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string? SourceFolderPath { get; set; } = string.Empty;
        public string? BatchReadyFolderPath { get; set; } = string.Empty;
        public string? Batch { get; set; } = string.Empty;
        public bool IsBatchComplete { get; set; } = false;
        public string? ObjectName { get; set; } = string.Empty;
        public Guid DataSetSpecificationId { get; set; }
        public string EncryptedFileName { get; set; } = string.Empty;
        public string DecryptedFileName { get; set; } = string.Empty;
        public bool Decrypted { get; set; }
        public bool IsDownloaded { get; set; } = false;
        public bool IsProcessing { get; set; } = false;
        public int RetryCount { get; set; } = 0;
        public DateTimeOffset LastAttempt { get; set; }
        public DateTimeOffset LastSeen { get; set; }
        public bool FileDeleted { get; set; }
        public long RecordCount { get; set; }
        public long EncryptedFileSize { get; set; }
        public string EncryptedFileSha256Hash { get; set; } = string.Empty;
        public long DecryptedFileSize { get; set; }
        public string DecryptedFileSha256Hash { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTimeOffset UpdatedDate { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;

        [BindNever]
        public Supplier? Supplier { get; set; } = null!;

        [BindNever]
        public List<IngestionTrackingAudit> IngestionTrackingAudits { get; set; } = new List<IngestionTrackingAudit>();
    }
}

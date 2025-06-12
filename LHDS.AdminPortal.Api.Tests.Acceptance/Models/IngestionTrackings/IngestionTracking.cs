// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings
{
    public class IngestionTracking
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public Guid SupplierId { get; set; }
        public string EncryptedFileName { get; set; } = string.Empty;
        public string DecryptedFileName { get; set; } = string.Empty;
        public bool Decrypted { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTimeOffset LastSeen { get; set; }
        public Guid? SubscriberAgreementId { get; set; }

        public bool FileDeleted { get; set; }
        public int EncryptedFileSize { get; set; }
        public int DecryptedFileSize { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTimeOffset UpdatedDate { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTimeOffset CreatedDate { get; set; }
    }
}

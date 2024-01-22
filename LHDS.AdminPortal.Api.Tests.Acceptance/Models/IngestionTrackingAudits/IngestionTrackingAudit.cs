// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackingAudits
{
    public class IngestionTrackingAudit
    {
        public Guid Id { get; set; }
        public Guid IngestionTrackingId { get; set; }
        public string Message { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTimeOffset UpdatedDate { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTimeOffset CreatedDate { get; set; }
    }
}

// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LHDS.AdminPortal.Web.Tests.Acceptance.Models.Audits
{
    public class Audit
    {
        public Guid Id { get; set; }
        public Guid IngestionTrackingId { get; set; }
        public string Message { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTimeOffset UpdatedDate { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTimeOffset CreatedDate { get; set; }
    }
}

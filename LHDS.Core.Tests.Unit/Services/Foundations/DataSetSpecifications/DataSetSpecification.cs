// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSetSpecifications
{
    public class DataSetSpecification
    {
        public Guid Id { get; set; }
        public Guid DataSetId { get; set; }
        public string SupplierSpecificationVersion { get; set; } = string.Empty;
        public string OurSpecificationVersion { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public bool IsMultiAuthorPerBatch { get; set; }
        public string EntityChangeSynchronisation { get; set; } = string.Empty;

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTimeOffset? DateReleased { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTimeOffset? DateImplemented { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTimeOffset? DateSuperseded { get; set; }

        public Guid? SupersededById { get; set; }
        public Guid? PresededById { get; set; }
        public bool IsPublished { get; set; }
        public bool IsActive { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTimeOffset ActiveFrom { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTimeOffset ActiveTo { get; set; }

        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTimeOffset UpdatedDate { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTimeOffset CreatedDate { get; set; }
    }
}

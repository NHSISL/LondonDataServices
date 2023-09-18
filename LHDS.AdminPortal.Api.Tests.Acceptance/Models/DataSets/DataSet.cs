// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSets
{
    public class DataSet
    {
        public Guid Id { get; set; }
        public string DataSetName { get; set; } = string.Empty;
        public string DataSetAliases { get; set; } = string.Empty;
        public string DataSetSupplier { get; set; } = string.Empty;
        public string DataSetAuthor { get; set; } = string.Empty;
        public string SpecifiedBy { get; set; } = string.Empty;
        public bool IsNationallySpecified { get; set; }
        public string CollectedBy { get; set; } = string.Empty;
        public bool IsNationallyCollected { get; set; }
        public string DataSourceType { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTimeOffset? ActiveFrom { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTimeOffset? ActiveTo { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTimeOffset UpdatedDate { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTimeOffset CreatedDate { get; set; }
    }
}

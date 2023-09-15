// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers
{
    public class Supplier
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FriendlyName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string LandingManualTriggerUrl { get; set; } = string.Empty;
        public string DecryptionManualTriggerUrl { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTimeOffset UpdatedDate { get; set; }

        List<IngestionTracking> IngestionTrackings { get; set; } = new List<IngestionTracking>();
    }
}

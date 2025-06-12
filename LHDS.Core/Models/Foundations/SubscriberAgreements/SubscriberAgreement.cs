// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using LHDS.Core.Models.Bases;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LHDS.Core.Models.Foundations.SubscriberAgreements
{
    public class SubscriberAgreement : IKey, IAudit
    {
        public Guid Id { get; set; }
        public string SupplierSharingAgreementShortName { get; set; }
        public Guid? SupplierSharingAgreementGuid { get; set; }
        public string? FtpUserName { get; set; }
        public string? FtpPublicKey { get; set; }
        public string? GpgPublicKey { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset? LastPollStartDate { get; set; }
        public DateTimeOffset? LastPollEndDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        [BindNever]
        public List<IngestionTracking> IngestionTrackings { get; set; } = new List<IngestionTracking>();
    }
}

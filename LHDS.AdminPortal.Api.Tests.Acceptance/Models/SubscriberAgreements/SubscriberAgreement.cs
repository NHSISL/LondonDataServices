// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Models.SubscriberAgreements
{
    public class SubscriberAgreement
    {
        public Guid Id { get; set; }
        public string SupplierSharingAgreementShortName { get; set; } = string.Empty;
        public Guid SupplierSharingAgreementGuid { get; set; }
        public string FtpUserName { get; set; }
        public string FtpPublicKey { get; set; }
        public string GpgPublicKey { get; set; }
        public bool IsActive { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTimeOffset? LastPollStartDate { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTimeOffset? LastPollEndDate { get; set; }

        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTimeOffset UpdatedDate { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTimeOffset CreatedDate { get; set; }
    }
}

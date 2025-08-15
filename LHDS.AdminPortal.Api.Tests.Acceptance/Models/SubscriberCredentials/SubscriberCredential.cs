// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Models.SubscriberCredentials
{
    public class SubscriberCredential
    {
        public Guid Id { get; set; }
        public Guid SupplierId { get; set; }
        public string SupplierSharingAgreementShortName { get; set; }
        public Guid SupplierSharingAgreementGuid { get; set; }
        public string FtpUserName { get; set; }
        public string FtpPassword { get; set; }
        public string FtpPassPhrase { get; set; }
        public string FtpPrivateKey { get; set; }
        public string FtpPublicKey { get; set; }
        public string GpgPassPhrase { get; set; }
        public string GpgPrivateKey { get; set; }
        public string GpgPublicKey { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset LastPollStartDate { get; set; }
        public DateTimeOffset LastPollEndDate { get; set; }

        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}

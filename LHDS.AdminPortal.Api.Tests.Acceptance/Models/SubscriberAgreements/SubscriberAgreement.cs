using System;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Models.SubscriberAgreements
{
    public class SubscriberAgreement
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
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}

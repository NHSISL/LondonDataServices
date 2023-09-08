// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Models.SpecificationObjects
{
    public class SpecificationObject
    {
        public Guid Id { get; set; }
        public Guid DataSetSpecificationId { get; set; }
        public string SupplierObjectName { get; set; } = string.Empty;
        public string OurObjectName { get; set; } = string.Empty;
        public string ObjectDescription { get; set; } = string.Empty;
        public string InterchangeProtocol { get; set; } = string.Empty;
        public bool IsPushedToUs { get; set; }
        public bool IsPulledByUs { get; set; }
        public string DeletionHandling { get; set; } = string.Empty;
        public bool IsSubmissionHeaderObject { get; set; }
        public bool IsTransactionLog { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}

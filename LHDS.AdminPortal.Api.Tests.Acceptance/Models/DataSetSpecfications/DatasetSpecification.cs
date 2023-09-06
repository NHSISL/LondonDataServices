// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSetSpecifications
{
    public class DataSetSpecification
    {
        public Guid Id { get; set; }
        public Guid DataSetId { get; set; }
        public string SupplierSpecificationVersion { get; set; }
        public string OurSpecificationVersion { get; set; }
        public string Notes { get; set; }
        public bool IsMultiSender { get; set; }
        public string EntityChangeSynchronisation { get; set; }
        public DateTimeOffset? DateReleased { get; set; }
        public DateTimeOffset? DateImplemented { get; set; }
        public DateTimeOffset? DateSuperseded { get; set; }
        public string SupersededBy { get; set; }
        public string PresededBy { get; set; }
        public bool IsPublished { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset ActiveFrom { get; set; }
        public DateTimeOffset ActiveTo { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}

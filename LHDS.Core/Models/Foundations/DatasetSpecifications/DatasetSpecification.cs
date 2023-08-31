// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using LHDS.Core.Models.Bases;
using LHDS.Core.Models.Foundations.DataSetObjects;
using LHDS.Core.Models.Foundations.DataSets;

namespace LHDS.Core.Models.Foundations.DataSetSpecifications
{
    public class DataSetSpecification : IKey, IAudit
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

        public DataSet DataSet { get; set; }
        public List<DataSetObject> DataSetObjects { get; set; } = new List<DataSetObject>();
    }
}

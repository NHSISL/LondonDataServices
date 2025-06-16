// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using LHDS.Core.Models.Bases;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LHDS.Core.Models.Foundations.DataSetSpecifications
{
    public class DataSetSpecification : IKey, IAudit
    {
        public Guid Id { get; set; }
        public Guid DataSetId { get; set; }
        public string SupplierSpecificationVersion { get; set; } = string.Empty;
        public string OurSpecificationVersion { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public bool IsMultiAuthorPerBatch { get; set; }
        public string EntityChangeSynchronisation { get; set; } = string.Empty;
        public DateTimeOffset? DateReleased { get; set; }
        public DateTimeOffset? DateImplemented { get; set; }
        public DateTimeOffset? DateSuperseded { get; set; }
        public Guid? SupersededById { get; set; }
        public Guid? PresededById { get; set; }
        public bool IsPublished { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset? ActiveFrom { get; set; }
        public DateTimeOffset? ActiveTo { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        [BindNever]
        public DataSet? DataSet { get; set; } = null!;

        [BindNever]
        public List<SpecificationObject> SpecificationObjects { get; set; } = new List<SpecificationObject>();

        [BindNever]
        public List<DataSetSpecification> SupersededBy { get; set; } = new List<DataSetSpecification>();

        [BindNever]
        public List<DataSetSpecification> PresededBy { get; set; } = new List<DataSetSpecification>();
    }
}

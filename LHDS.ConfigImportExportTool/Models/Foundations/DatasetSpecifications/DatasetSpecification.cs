// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using LHDS.ConfigImportExportTool.Models.Bases;
using LHDS.ConfigImportExportTool.Models.Foundations.Datasets;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;

namespace LHDS.ConfigImportExportTool.Models.Foundations.DatasetSpecifications
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
        public DateTimeOffset ActiveFrom { get; set; }
        public DateTimeOffset ActiveTo { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DataSet DataSet { get; set; } = null!;
        public List<SpecificationObject> SpecificationObjects { get; set; } = new List<SpecificationObject>();
        public List<DataSetSpecification> SupersededBy { get; set; } = new List<DataSetSpecification>();
        public List<DataSetSpecification> PresededBy { get; set; } = new List<DataSetSpecification>();
    }
}

// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Bases;
using LHDS.ConfigImportExportTool.Models.Foundations.DatasetSpecifications;
using LHDS.ConfigImportExportTool.Models.Foundations.Suppliers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LHDS.ConfigImportExportTool.Models.Foundations.Datasets
{
    public class DataSet : IKey, IAudit
    {
        public Guid Id { get; set; }
        public Guid SupplierId { get; set; }
        public string DataSetName { get; set; } = string.Empty;
        public string DataSetAliases { get; set; } = string.Empty;
        public string DataSetAuthor { get; set; } = string.Empty;
        public string SpecifiedBy { get; set; } = string.Empty;
        public bool IsNationallySpecified { get; set; }
        public string CollectedBy { get; set; } = string.Empty;
        public bool IsNationallyCollected { get; set; }
        public string DataSourceType { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTimeOffset? ActiveFrom { get; set; }
        public DateTimeOffset? ActiveTo { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public Supplier? Supplier { get; set; } = null;
        public List<DataSetSpecification> DataSetSpecifications { get; set; } = new List<DataSetSpecification>();
    }
}

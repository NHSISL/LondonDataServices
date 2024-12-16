// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using LHDS.ConfigImportExportTool.Models.Bases;
using LHDS.ConfigImportExportTool.Models.Foundations.Datasets;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LHDS.ConfigImportExportTool.Models.Foundations.Suppliers
{
    public class Supplier : IKey, IAudit
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FriendlyName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool canRelandIngestionTracking { get; set; }
        public bool CanDecryptIngestionTracking { get; set; }
        public bool CanDownloadIngestionTracking { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTimeOffset UpdatedDate { get; set; }

        [BindNever]
        public List<DataSet> DataSets { get; set; } = new List<DataSet>();
    }
}

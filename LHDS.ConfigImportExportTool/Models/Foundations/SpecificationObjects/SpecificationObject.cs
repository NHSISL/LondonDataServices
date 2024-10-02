// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using LHDS.ConfigImportExportTool.Models.Bases;
using LHDS.ConfigImportExportTool.Models.Foundations.DataSetSpecifications;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects
{
    public class SpecificationObject : IKey, IAudit
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

        [BindNever]
        public DataSetSpecification? DataSetSpecification { get; set; } = null!;

        [BindNever]
        public List<ObjectColumn> ObjectColumns { get; set; } = new List<ObjectColumn>();

    }
}

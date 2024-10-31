// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LHDS.ConfigImportExportTool.Models.Bases.SchemaConfigs
{
    public class SchemaConfig
    {
        [BindNever]
        public List<SpecificationObject> SpecificationObjects { get; set; } = new List<SpecificationObject>();
        [BindNever]
        public List<ObjectColumn> ObjectColumns { get; set; } = new List<ObjectColumn>();
    }
}

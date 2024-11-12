// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;

namespace LHDS.ConfigImportExportTool.Services.Orchestrations.ReadSchema
{
    internal interface IReadSchemaOrchestrationService
    {
        ValueTask<List<SpecificationObject>> ReadFile(string path);
        ValueTask WriteFile(List<ObjectColumn> data, string path);
    }
}

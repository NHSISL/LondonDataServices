// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;

namespace LHDS.ConfigImportExportTool.Services.Orchestrations.ReadSchema
{
    internal interface IReadSchemaOrchestrationService
    {
        ValueTask<List<ObjectColumn>> ReadFile(string path);
        ValueTask WriteFile(List<ObjectColumn> data, string path);
    }
}

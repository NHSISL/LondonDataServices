// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;

namespace LHDS.ConfigImportExportTool.Services.Orchestrations.SchemaConfig
{
    internal interface ISchemaConfigOrchestrationService
    {
        ValueTask Import(List<ObjectColumn> objectColumnList);
    }
}

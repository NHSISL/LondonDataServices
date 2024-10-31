// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Bases.SchemaConfigs;

namespace LHDS.ConfigImportExportTool.Services.Orchestrations.SchemaConfigs
{
    internal interface ISchemaConfigOrchestrationService
    {
        ValueTask Import(SchemaConfig schemaConfig, string dataSetName, string version);
        ValueTask Export(SchemaConfig schemaConfig, string dataSetName, string version);
    }
}

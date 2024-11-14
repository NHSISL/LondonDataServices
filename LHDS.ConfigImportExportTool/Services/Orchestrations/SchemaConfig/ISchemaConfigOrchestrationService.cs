// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;

namespace LHDS.ConfigImportExportTool.Services.Orchestrations.SchemaConfigs
{
    internal interface ISchemaConfigOrchestrationService
    {
        ValueTask Import(List<SpecificationObject> SpecificationObject, string dataSetName, string version);
        ValueTask Export(List<SpecificationObject> SpecificationObject, string dataSetName, string version);
    }
}

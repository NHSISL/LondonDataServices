// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;

namespace LHDS.ConfigImportExportTool.Services.Orchestrations.SchemaConfigs
{
    internal interface ISchemaConfigOrchestrationService
    {
        ValueTask Import(List<SpecificationObject> specificationObject, string dataSetName, string version);
        ValueTask Export(List<SpecificationObject> specificationObject, string dataSetName, string version);
    }
}

// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;

namespace LHDS.ConfigImportExportTool.Services.Orchestrations.SchemaConfigs
{
    internal interface ISchemaConfigOrchestrationService
    {
        ValueTask Import(List<SpecificationObject> SpecificationObject, string dataSetName, string version);
        ValueTask<List<SpecificationObject>> Export(string dataSetName, string version);
    }
}

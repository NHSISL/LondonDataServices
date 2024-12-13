// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;

namespace LHDS.ConfigImportExportTool.Services.Orchestrations.ReadSchemas
{
    internal interface IReadSchemaOrchestrationService
    {
        ValueTask<List<SpecificationObject>> ReadFile(string path);
        ValueTask WriteFile(List<SpecificationObject> data, string path);
    }
}

// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;

namespace LHDS.ConfigImportExportTool.Services.Foundations.Files
{
    internal interface IReadSchemaOrchestrationService
    {
        ValueTask<List<ObjectColumn>> ProcessSchemaFile(string path);
    }
}

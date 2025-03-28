// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;

namespace LHDS.ConfigImportExportTool.Services.Coordinations.ImportExports
{
    internal interface IImportExportCoordinationService
    {
        ValueTask Import(string dataSetName, string version, string filePath);
        ValueTask Export(string dataSetName, string version, string filePath);
    }
}

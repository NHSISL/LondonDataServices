// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;

namespace LHDS.ConfigImportExportTool.Clients.ImportExports
{
    internal interface IImportExportClient
    {
        public ValueTask Import(string dataSetName, string version, string filePath);
        public ValueTask Export(string dataSetName, string version, string filePath);
    }
}

// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.ConfigImportExportTool.Clients
{
    internal interface IImportExportClient
    {
        public ValueTask Import(string dataSetName, string version, string filePath);
    }
}

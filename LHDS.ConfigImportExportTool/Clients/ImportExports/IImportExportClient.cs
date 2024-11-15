// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.ConfigImportExportTool.Clients.ImportExports
{
    public interface IImportExportClient
    {
        public ValueTask Import(string dataSetName, string version, string filePath);
    }
}

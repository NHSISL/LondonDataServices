// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Brokers.Storages.Sql;

namespace LHDS.ConfigImportExportTool.Clients.ImportExports
{
    public interface IImportExportClient
    {
        public ValueTask Import(string dataSetName, string version, string filePath);
    }
}

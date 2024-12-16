// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;

namespace LHDS.ConfigImportExportTool.Services.Processings.ObjectColumns
{
    internal interface IObjectColumnProcessingService
    {
        ValueTask<ObjectColumn> ReadOrInsertObjectColumnAsync(ObjectColumn objectColumn);
    }
}

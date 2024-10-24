// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;

namespace LHDS.ConfigImportExportTool.Services.Processings.ObjectColumns
{
    public interface IObjectColumnProcessingService
    {
        ValueTask<List<ObjectColumn>> ReadOrInsertObjectColumnAsync(ObjectColumn objectColumn);
    }
}

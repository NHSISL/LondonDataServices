// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Services.Foundations.ObjectColumns;

namespace LHDS.ConfigImportExportTool.Services.Processings.ObjectColumns
{
    internal class ObjectColumnProcessingService : IObjectColumnProcessingService
    {
        private readonly IObjectColumnService objectColumnService;

        public ObjectColumnProcessingService(IObjectColumnService objectColumnService)
        {
            this.objectColumnService = objectColumnService;
        }

        public async ValueTask<ObjectColumn> ReadOrInsertObjectColumnAsync(ObjectColumn objectColumn) =>
            throw new NotImplementedException();
    }
}

// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Brokers.CsvHelpers;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Services.Foundations.Files;

namespace LHDS.ConfigImportExportTool.Services.Orchestrations.ReadSchema
{
    internal partial class ReadSchemaOrchestrationService : IReadSchemaOrchestrationService
    {
        private readonly IFileService fileService;
        private readonly ICsvHelperBroker csvHelperBroker;

        public ReadSchemaOrchestrationService(IFileService fileService, ICsvHelperBroker csvHelperBroker)
        {
            this.fileService = fileService;
            this.csvHelperBroker = csvHelperBroker;
        }

        public ValueTask<IQueryable<ObjectColumn>> ProcessSchemaFile(string path) =>
            throw new NotImplementedException();
    }
}

// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Runtime.InteropServices;
using System.Text;
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

        public async ValueTask<List<ObjectColumn>> ProcessSchemaFile(string path)
        {
            byte[] csvData = await this.fileService.ReadFromFileAsync(path);
            string csvString = ASCIIEncoding.UTF8.GetString(csvData);

            return await this.csvHelperBroker.MapCsvToObjectAsync<ObjectColumn>(csvString, true);
        }
    }
}

// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Text;
using LHDS.ConfigImportExportTool.Brokers.Loggings;
using LHDS.ConfigImportExportTool.Models.Bases.;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Models.Orchestrations.ReadSchema;
using LHDS.ConfigImportExportTool.Services.Foundations.CsvHelpers;
using LHDS.ConfigImportExportTool.Services.Foundations.Files;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LHDS.ConfigImportExportTool.Services.Orchestrations.ReadSchema
{
    internal partial class ReadSchemaOrchestrationService : IReadSchemaOrchestrationService
    {
        private readonly IFileService fileService;
        private readonly ICsvHelperService csvHelperService;
        private readonly ILoggingBroker loggingBroker;

        public ReadSchemaOrchestrationService(
            IFileService fileService, 
            ICsvHelperService csvHelperService,
            ILoggingBroker loggingBroker)
        {
            this.fileService = fileService;
            this.csvHelperService = csvHelperService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<List<SpecificationObject>> ReadFile(string path) =>
            TryCatch(async () =>
            {
                ValidateProcessSchemaFileArguments(path);
                byte[] csvData = await this.fileService.ReadFromFileAsync(path);
                string csvString = ASCIIEncoding.UTF8.GetString(csvData);

                List<CannonicalSchemaItem> canonicalSchemaObjects = await this.csvHelperService
                    .MapCsvToObjectAsync<CannonicalSchemaItem>(csvString, true);

                List<SpecificationObject> specificationObjects = new List<SpecificationObject>();

                foreach (var canonicalSchemaObject in canonicalSchemaObjects)
                {
                    var newSpecificationObjects = new SpecificationObject
                    {
                        SupplierObjectName = canonicalSchemaObject.TableName,
                    };

                    var newObjectColumn = new ObjectColumn
                    {
                        

                               
                    };

                    newSpecificationObjects.ObjectColumns.Add(newObjectColumn);
                }
            });

        public async ValueTask WriteFile(List<ObjectColumn> data, string path) =>
            throw new NotImplementedException();
    }
}

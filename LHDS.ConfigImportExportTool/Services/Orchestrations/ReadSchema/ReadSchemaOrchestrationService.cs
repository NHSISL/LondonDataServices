// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Text;
using LHDS.ConfigImportExportTool.Brokers.Loggings;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Models.Orchestrations.ReadSchema;
using LHDS.ConfigImportExportTool.Services.Foundations.CsvHelpers;
using LHDS.ConfigImportExportTool.Services.Foundations.Files;

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
                var groupedSchemaObjects = canonicalSchemaObjects.GroupBy(cso => cso.TableName).ToList();

                foreach (var group in groupedSchemaObjects)
                {
                    var newSpecificationObjects = new SpecificationObject
                    {
                        SupplierObjectName = group.Key,
                        ObjectDescription = group.First().TableDescription,
                    };

                    foreach (var canonicalSchemaObject in group)
                    {
                        var newObjectColumn = new ObjectColumn
                        {
                            SupplierColumnName = canonicalSchemaObject.ColumnName,
                            ColumnDescription = canonicalSchemaObject.ColumnDescription,
                            SqlDataType = canonicalSchemaObject.ColumnDataType,
                            Length = canonicalSchemaObject.ColumnLength,
                            OrdinalPosition = canonicalSchemaObject.ColumnOrdinal,
                            ForeignKeyTableName = canonicalSchemaObject.LinkedTable,
                            ForeignKeyColumnName = canonicalSchemaObject.LinkedColumn,
                        };

                        newSpecificationObjects.ObjectColumns.Add(newObjectColumn);
                    }

                    specificationObjects.Add(newSpecificationObjects);
                }

                return specificationObjects;
            });

        public async ValueTask WriteFile(List<SpecificationObject> data, string path) =>
            throw new NotImplementedException();
    }
}

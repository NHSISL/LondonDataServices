// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
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
                    };

                    foreach (var canonicalSchemaObject in group)
                    {
                        var newObjectColumn = new ObjectColumn
                        {
                            SupplierColumnName = canonicalSchemaObject.ColumnName,
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

        public ValueTask WriteFile(List<SpecificationObject> specificationObjects, string path) =>
            TryCatch(async () =>
            {
                ValidateWriteSchemaFileArguments(specificationObjects, path);
                List<CannonicalSchemaItem> mappedCannonicalSchemaItems = new List<CannonicalSchemaItem>();

                foreach (SpecificationObject specificationObject in specificationObjects)
                {
                    foreach (ObjectColumn objectColumn in specificationObject.ObjectColumns)
                    {
                        CannonicalSchemaItem cannonicalSchemaItem = new CannonicalSchemaItem
                        {
                            TableName = specificationObject.SupplierObjectName,
                            TableDescription = specificationObject.ObjectDescription,
                            ColumnName = objectColumn.SupplierColumnName,
                            ColumnDataType = objectColumn.SqlDataType,
                            ColumnDescription = objectColumn.ColumnDescription,
                            ColumnLength = objectColumn.Length,
                            ColumnOrdinal = objectColumn.OrdinalPosition,
                            LinkedTable = objectColumn.ForeignKeyTableName,
                            LinkedColumn = objectColumn.ForeignKeyColumnName,
                        };

                        mappedCannonicalSchemaItems.Add(cannonicalSchemaItem);
                    }
                }

                string csvString = await this.csvHelperService.MapObjectToCsvAsync(mappedCannonicalSchemaItems, true);

                await this.fileService.WriteToFileAsync(path, csvString);
            });
    }
}

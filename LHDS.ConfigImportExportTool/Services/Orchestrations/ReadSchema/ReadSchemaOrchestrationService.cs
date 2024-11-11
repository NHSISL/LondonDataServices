// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Text;
using LHDS.ConfigImportExportTool.Brokers.Loggings;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
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

        public ValueTask<List<SpecificationObject>> ReadFile(string path, Guid dataSetSpecificationId) =>
            TryCatch(async () =>
            {
                ValidateProcessSchemaFileArguments(path);
                byte[] csvData = await this.fileService.ReadFromFileAsync(path);
                string csvString = ASCIIEncoding.UTF8.GetString(csvData);

                List<CanonicalSchemaObject> canonicalSchemaObjects = await this.csvHelperService
                    .MapCsvToObjectAsync<CanonicalSchemaObject>(csvString, true);


                List<SpecificationObject> specificationObjects = new List<SpecificationObject>();

                foreach (var canonicalSchemaObject in canonicalSchemaObjects)
                {
                    var maybeSpecificationObjects = specificationObjects
                        .FirstOrDefault(specificationObject =>
                            specificationObject.DataSetSpecificationId == dataSetSpecificationId
                            && specificationObject.SupplierObjectName == canonicalSchemaObjects.SupplierObjectName)
                        ?? new SpecificationObject
                        {
                            // NO ID PRESENT
                            DataSetSpecificationId = dataSetSpecificationId,
                            SupplierObjectName = SupplierObjectName,
                            // populate all the other properties
                        };


                    var newObjectColumn = new ObjectColumn
                    {
                        // NO ID PRESENT
                        // populate all the properties
                    };

                    maybeSpecificationObjects.ObjectColumns.Add(newObjectColumn);
                }
            });

        public async ValueTask WriteFile(List<ObjectColumn> data, string path) =>
            throw new NotImplementedException();
    }
}

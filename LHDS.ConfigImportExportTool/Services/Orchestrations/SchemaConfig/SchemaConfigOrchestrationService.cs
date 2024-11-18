// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Brokers.Loggings;
using LHDS.ConfigImportExportTool.Brokers.Storages.Sql;
using LHDS.ConfigImportExportTool.Models.Foundations.Datasets;
using LHDS.ConfigImportExportTool.Models.Foundations.DatasetSpecifications;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Services.Processings.DataSets;
using LHDS.ConfigImportExportTool.Services.Processings.ObjectColumns;
using LHDS.ConfigImportExportTool.Services.Processings.SpecificationObjects;
using Microsoft.EntityFrameworkCore;

namespace LHDS.ConfigImportExportTool.Services.Orchestrations.SchemaConfigs
{
    internal partial class SchemaConfigOrchestrationService : ISchemaConfigOrchestrationService
    {
        private readonly IObjectColumnProcessingService objectColumnProcessingService;
        private readonly ISpecificationObjectProcessingService specificationObjectProcessingService;
        private readonly IDataSetProcessingService dataSetProcessingService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IStorageBroker storageBroker;

        public SchemaConfigOrchestrationService(
            IObjectColumnProcessingService objectColumnProcessingService,
            ISpecificationObjectProcessingService specificationObjectProcessingService,
            IDataSetProcessingService dataSetProcessingService,
            ILoggingBroker loggingBroker,
            IStorageBroker storageBroker)
        {
            this.objectColumnProcessingService = objectColumnProcessingService;
            this.specificationObjectProcessingService = specificationObjectProcessingService;
            this.dataSetProcessingService = dataSetProcessingService;
            this.loggingBroker = loggingBroker;
            this.storageBroker = storageBroker;
        }

        public async ValueTask Export(List<SpecificationObject> specificationObject, string dataSetName, string version) =>
            throw new NotImplementedException();

        public ValueTask Import(
            List<SpecificationObject> specificationObjects,
            string dataSetName,
            string version) =>
                TryCatch(async () =>
                {
                    ValidateSchemaImportArguments(specificationObjects, dataSetName, version);

                    IQueryable<DataSet> storageDataSetQuery =
                        await this.dataSetProcessingService.RetrieveAllDataSetsAsync();

                    storageDataSetQuery = storageDataSetQuery
                        .Include(dataSet => dataSet.DataSetSpecifications)
                        .Where(dataSet => dataSet.DataSetName == dataSetName);

                    DataSet matchedDataSet = 
                        storageDataSetQuery.FirstOrDefault(dataSet => dataSet.DataSetName == dataSetName);

                    DataSetSpecification dataSetSpecification = matchedDataSet.DataSetSpecifications
                        .FirstOrDefault(specification => specification.SupplierSpecificationVersion == version);

                    foreach (SpecificationObject specificationObject in specificationObjects)
                    {
                        specificationObject.DataSetSpecificationId = dataSetSpecification.Id;

                        SpecificationObject storageSpecificationObject = await specificationObjectProcessingService
                            .ReadOrInsertSpecificationObjectAsync(specificationObject);

                        foreach (ObjectColumn objectColumn in specificationObject.ObjectColumns)
                        {
                            objectColumn.SpecificationObjectId = storageSpecificationObject.Id;

                            ObjectColumn storageObjectColumn = await objectColumnProcessingService
                                .ReadOrInsertObjectColumnAsync(objectColumn);
                        }
                    };
                });
    }
}

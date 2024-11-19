// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Brokers.DateTimes;
using LHDS.ConfigImportExportTool.Brokers.Identifiers;
using LHDS.ConfigImportExportTool.Brokers.Loggings;
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
        private readonly IIdentifierBroker identifierBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public SchemaConfigOrchestrationService(
            IObjectColumnProcessingService objectColumnProcessingService,
            ISpecificationObjectProcessingService specificationObjectProcessingService,
            IDataSetProcessingService dataSetProcessingService,
            ILoggingBroker loggingBroker,
            IIdentifierBroker identifierBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.objectColumnProcessingService = objectColumnProcessingService;
            this.specificationObjectProcessingService = specificationObjectProcessingService;
            this.dataSetProcessingService = dataSetProcessingService;
            this.loggingBroker = loggingBroker;
            this.identifierBroker = identifierBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public async ValueTask Export(List<SpecificationObject> specificationObjects, string dataSetName, string version) =>
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
                        DateTimeOffset currentDateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
                        specificationObject.Id = this.identifierBroker.GetIdentifier();
                        specificationObject.DataSetSpecificationId = dataSetSpecification.Id;
                        specificationObject.OurObjectName = specificationObject.SupplierObjectName;
                        specificationObject.CreatedBy = "System";
                        specificationObject.UpdatedBy = "System";
                        specificationObject.CreatedDate = currentDateTime;
                        specificationObject.UpdatedDate = currentDateTime;

                        SpecificationObject storageSpecificationObject = await specificationObjectProcessingService
                            .ReadOrInsertSpecificationObjectAsync(specificationObject);

                        foreach (ObjectColumn objectColumn in specificationObject.ObjectColumns)
                        {
                            DateTimeOffset dateTimeNow = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
                            objectColumn.Id = this.identifierBroker.GetIdentifier();
                            objectColumn.SpecificationObjectId = storageSpecificationObject.Id;
                            objectColumn.OurColumnName = objectColumn.SupplierColumnName;
                            objectColumn.CodeSystem = "System";
                            objectColumn.CreatedBy = "System";
                            objectColumn.UpdatedBy = "System";
                            objectColumn.CreatedDate = dateTimeNow;
                            objectColumn.UpdatedDate = dateTimeNow;

                            ObjectColumn storageObjectColumn = await objectColumnProcessingService
                                .ReadOrInsertObjectColumnAsync(objectColumn);
                        }
                    };
                });
    }
}

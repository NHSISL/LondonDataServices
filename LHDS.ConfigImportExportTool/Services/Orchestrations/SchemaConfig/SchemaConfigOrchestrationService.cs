// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Brokers.Loggings;
using LHDS.ConfigImportExportTool.Models.Bases.SchemaConfigs;
using LHDS.ConfigImportExportTool.Models.Foundations.Datasets;
using LHDS.ConfigImportExportTool.Models.Foundations.DatasetSpecifications;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Services.Processings.DataSets;
using LHDS.ConfigImportExportTool.Services.Processings.ObjectColumns;
using LHDS.ConfigImportExportTool.Services.Processings.SpecificationObjects;

namespace LHDS.ConfigImportExportTool.Services.Orchestrations.SchemaConfigs
{
    internal partial class SchemaConfigOrchestrationService : ISchemaConfigOrchestrationService
    {
        private readonly IObjectColumnProcessingService objectColumnProcessingService;
        private readonly ISpecificationObjectProcessingService specificationObjectProcessingService;
        private readonly IDataSetProcessingService dataSetProcessingService;
        private readonly ILoggingBroker loggingBroker;

        public SchemaConfigOrchestrationService(
            IObjectColumnProcessingService objectColumnProcessingService,
            ISpecificationObjectProcessingService specificationObjectProcessingService,
            IDataSetProcessingService dataSetProcessingService,
            ILoggingBroker loggingBroker)
        {
            this.objectColumnProcessingService = objectColumnProcessingService;
            this.specificationObjectProcessingService = specificationObjectProcessingService;
            this.dataSetProcessingService = dataSetProcessingService;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask Export(SchemaConfig schemaConfig, string dataSetName, string version) =>
            throw new NotImplementedException();

        public async ValueTask Import(SchemaConfig schemaConfig, string dataSetName, string version)
        {
            IQueryable<DataSet> storageDataSets = await this.dataSetProcessingService.RetrieveAllDataSetsAsync();

            DataSet? matchedDataSet = 
                storageDataSets.Where(dataSet => dataSet.DataSetName == dataSetName).FirstOrDefault();

            foreach (SpecificationObject specificationObject in schemaConfig.SpecificationObjects)
            {
                specificationObject.DataSetSpecificationId = matchedDataSet.Id;

                SpecificationObject insertedSpecificationObject = 
                    await this.specificationObjectProcessingService.ReadOrInsertSpecificationObjectAsync(
                        specificationObject);
            };

            foreach (ObjectColumn objectColumn in schemaConfig.ObjectColumns)
            {
                IQueryable<DataSetSpecification> retrievedDataSetSpecifications = 
                    matchedDataSet.DataSetSpecifications.AsQueryable();

                DataSetSpecification? matchedDataSetSpecification = 
                    retrievedDataSetSpecifications.Where(specfication => 
                        specfication.SupplierSpecificationVersion == version).FirstOrDefault();

                objectColumn.SpecificationObjectId = matchedDataSetSpecification.Id;

                IQueryable<SpecificationObject> storageSpecificaitonObjects = 
                    await this.specificationObjectProcessingService.RetrieveAllSpecificationObjectsAsync();

                storageSpecificaitonObjects.Where(specificationObject => specificationObject.SupplierObjectName == objectColumn.)

                this.objectColumnProcessingServiceMock.Setup(service =>
                    service.ReadOrInsertObjectColumnAsync(objectColumn))
                        .ReturnsAsync(objectColumn);
            };
        }
    }
}

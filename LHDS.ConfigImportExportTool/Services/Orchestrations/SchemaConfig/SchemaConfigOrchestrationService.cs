// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Brokers.Loggings;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Services.Processings.DataSets;
using LHDS.ConfigImportExportTool.Services.Processings.ObjectColumns;
using LHDS.ConfigImportExportTool.Services.Processings.SpecificationObjects;

namespace LHDS.ConfigImportExportTool.Services.Orchestrations.SchemaConfigs
{
    internal partial class SchemaConfigOrchestrationService : ISchemaConfigOrchestrationService
    {
        private readonly IObjectColumnProcessingService objectColumnService;
        private readonly ISpecificationObjectProcessingService specificationObjectService;
        private readonly IDataSetProcessingService dataSetProcessingService;
        private readonly ILoggingBroker loggingBroker;

        public SchemaConfigOrchestrationService(
            IObjectColumnProcessingService objectColumnService,
            ISpecificationObjectProcessingService specificationObjectService,
            IDataSetProcessingService dataSetProcessingService,
            ILoggingBroker loggingBroker)
        {
            this.objectColumnService = objectColumnService;
            this.specificationObjectService = specificationObjectService;
            this.dataSetProcessingService = dataSetProcessingService;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask Export(List<SpecificationObject> SpecificationObject, string dataSetName, string version) =>
            throw new NotImplementedException();

        public async ValueTask Import(List<SpecificationObject> SpecificationObject, string dataSetName, string version) =>
            throw new NotImplementedException();
    }
}

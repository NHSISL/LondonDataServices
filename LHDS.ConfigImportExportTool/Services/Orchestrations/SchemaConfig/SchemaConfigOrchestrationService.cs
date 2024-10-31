// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Brokers.Loggings;
using LHDS.ConfigImportExportTool.Models.Bases.SchemaConfigs;
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
            IDataSetProcessingService dataSetProcessingProcessingService,
            ILoggingBroker loggingBroker)
        {
            this.objectColumnProcessingService = objectColumnProcessingService;
            this.specificationObjectProcessingService = specificationObjectProcessingService;
            this.dataSetProcessingService = dataSetProcessingService;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask Export(List<SchemaConfig> data, string dataSetName, string version) =>
            throw new NotImplementedException();

        public async ValueTask Import(List<SchemaConfig> data, string dataSetName, string version) =>
            throw new NotImplementedException();
    }
}

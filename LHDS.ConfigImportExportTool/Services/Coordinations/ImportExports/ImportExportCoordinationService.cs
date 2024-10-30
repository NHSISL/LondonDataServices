// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Brokers.Loggings;
using LHDS.ConfigImportExportTool.Services.Orchestrations.ReadSchema;
using LHDS.ConfigImportExportTool.Services.Orchestrations.SchemaConfigs;

namespace LHDS.ConfigImportExportTool.Services.Coordinations.ImportExports
{
    internal partial class ImportExportCoordinationService : IImportExportCoordinationService
    {
        private readonly IReadSchemaOrchestrationService readSchemaOrchestrationService;
        private readonly ISchemaConfigOrchestrationService schemaConfigOrchestrationService;
        private readonly ILoggingBroker loggingBroker;

        public ImportExportCoordinationService(
            IReadSchemaOrchestrationService readSchemaOrchestrationService,
            ISchemaConfigOrchestrationService schemaConfigOrchestrationService,
            ILoggingBroker loggingBroker)
        {
            this.readSchemaOrchestrationService = readSchemaOrchestrationService;
            this.schemaConfigOrchestrationService = schemaConfigOrchestrationService;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask Export(string dataSetName, string version, string filePath) =>
            throw new NotImplementedException();

        public async ValueTask Import(string dataSetName, string version, string filePath) =>
            throw new NotImplementedException();
    }
}

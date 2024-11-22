// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Brokers.Loggings;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
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

        public ValueTask Export(string dataSetName, string version, string filePath) =>
        TryCatch(async () =>
        {
            ValidateExportFileArguments(dataSetName, version, filePath);

            List<SpecificationObject> specificationObjects =
                await this.schemaConfigOrchestrationService.Export(dataSetName, version);

            await this.readSchemaOrchestrationService.WriteFile(specificationObjects, filePath);
        });

        public ValueTask Import(string dataSetName, string version, string filePath) =>
        TryCatch(async () =>
        {
            ValidateImportFileArguments(dataSetName, version, filePath);

            List<SpecificationObject> specificationObjects =
                await this.readSchemaOrchestrationService.ReadFile(filePath);

            await this.schemaConfigOrchestrationService.Import(specificationObjects, dataSetName, version);
        });
    }
}

// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Brokers.Loggings;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Services.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Services.Foundations.SpecificationObjects;

namespace LHDS.ConfigImportExportTool.Services.Orchestrations.SchemaConfig
{
    internal partial class SchemaConfigOrchestrationService : ISchemaConfigOrchestrationService
    {
        private readonly IObjectColumnService objectColumnService;
        private readonly ISpecificationObjectService specificationObjectService;
        private readonly ILoggingBroker loggingBroker;

        public SchemaConfigOrchestrationService(
            IObjectColumnService objectColumnService,
            ISpecificationObjectService specificationObjectService,
            ILoggingBroker loggingBroker)
        {
            this.objectColumnService = objectColumnService;
            this.specificationObjectService = specificationObjectService;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask Import(List<ObjectColumn> objectColumnList) =>
            throw new NotImplementedException();
    }
}

// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Orchestrations.SchemaConfigs.Exceptions
{
    public class InvalidArgumentSchemaConfigOrchestrationException : Xeption
    {
        public InvalidArgumentSchemaConfigOrchestrationException(string message)
            : base(message)
        { }
    }
}
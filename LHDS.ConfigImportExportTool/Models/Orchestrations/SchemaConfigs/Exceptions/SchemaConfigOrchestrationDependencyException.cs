// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Orchestrations.SchemaConfigs.Exceptions
{
    public class SchemaConfigOrchestrationDependencyException : Xeption
    {
        public SchemaConfigOrchestrationDependencyException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}

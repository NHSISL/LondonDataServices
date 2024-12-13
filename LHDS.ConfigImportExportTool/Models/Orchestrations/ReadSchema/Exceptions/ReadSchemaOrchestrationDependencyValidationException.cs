// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Orchestrations.ReadSchema.Exceptions
{
    public class ReadSchemaOrchestrationDependencyValidationException : Xeption
    {
        public ReadSchemaOrchestrationDependencyValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}

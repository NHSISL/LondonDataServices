// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Orchestrations.ReadSchema.Exceptions
{
    public class ReadSchemaValidationOrchestrationException : Xeption
    {
        public ReadSchemaValidationOrchestrationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}

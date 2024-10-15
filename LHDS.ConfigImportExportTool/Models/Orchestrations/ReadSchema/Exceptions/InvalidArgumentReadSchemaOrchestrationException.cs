// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Orchestrations.ReadSchema.Exceptions
{
    public class InvalidArgumentReadSchemaOrchestrationException : Xeption
    {
        public InvalidArgumentReadSchemaOrchestrationException(string message)
            : base(message)
        { }
    }
}
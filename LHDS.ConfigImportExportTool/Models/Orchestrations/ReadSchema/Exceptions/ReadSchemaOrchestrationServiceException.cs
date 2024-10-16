// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Orchestrations.ReadSchema.Exceptions
{
    public class ReadSchemaOrchestrationServiceException : Xeption
    {
        public ReadSchemaOrchestrationServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}
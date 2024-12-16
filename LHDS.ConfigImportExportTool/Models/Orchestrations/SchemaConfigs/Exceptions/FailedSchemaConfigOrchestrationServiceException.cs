// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Orchestrations.SchemaConfigs.Exceptions
{
    public class FailedSchemaConfigOrchestrationServiceException : Xeption
    {
        public FailedSchemaConfigOrchestrationServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
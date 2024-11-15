// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Coordinations.ImportExports.Exceptions
{
    public class ImportExportCoordinationServiceException : Xeption
    {
        public ImportExportCoordinationServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}
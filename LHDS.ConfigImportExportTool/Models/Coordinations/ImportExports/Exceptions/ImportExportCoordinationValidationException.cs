// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Coordinations.ImportExports.Exceptions
{
    public class ImportExportCoordinationValidationException : Xeption
    {
        public ImportExportCoordinationValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}

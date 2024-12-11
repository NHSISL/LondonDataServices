// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Coordinations.ImportExports.Exceptions
{
    public class ImportExportCoordinationDependencyException : Xeption
    {
        public ImportExportCoordinationDependencyException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}

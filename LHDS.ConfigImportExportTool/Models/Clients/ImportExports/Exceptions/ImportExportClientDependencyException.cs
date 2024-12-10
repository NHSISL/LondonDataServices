// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Clients.ImportExports.Exceptions
{
    public class ImportExportClientDependencyException : Xeption
    {
        public ImportExportClientDependencyException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}

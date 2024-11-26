// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Clients.Exceptions
{
    internal class ImportExportClientDependencyValidationException : Xeption
    {
        public ImportExportClientDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}

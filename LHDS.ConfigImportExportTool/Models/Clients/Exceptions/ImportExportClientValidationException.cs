// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Clients.Exceptions
{
    public class ImportExportClientValidationException : Xeption
    {
        public ImportExportClientValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}

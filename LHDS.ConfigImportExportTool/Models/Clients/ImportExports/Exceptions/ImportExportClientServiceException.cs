// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Clients.ImportExports.Exceptions
{
    public class ImportExportClientServiceException : Xeption
    {
        public ImportExportClientServiceException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}

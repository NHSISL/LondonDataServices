// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Coordinations.ImportExports.Exceptions
{
    public class InvalidArgumentImportExportCoordinationException : Xeption
    {
        public InvalidArgumentImportExportCoordinationException(string message)
            : base(message)
        { }
    }
}
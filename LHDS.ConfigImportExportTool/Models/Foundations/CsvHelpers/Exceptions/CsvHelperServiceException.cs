// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Foundations.CsvHelpers.Exceptions
{
    internal class CsvHelperServiceException : Xeption
    {
        public CsvHelperServiceException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}

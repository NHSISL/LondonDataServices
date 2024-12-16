// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Foundations.CsvHelpers.Exceptions
{
    public class InvalidArgumentCsvHelperException : Xeption
    {
        public InvalidArgumentCsvHelperException(string message)
            : base(message)
        { }
    }
}

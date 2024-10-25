// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Foundations.CsvHelpers.Exceptions
{
    public class FailedCsvHelperDependencyException : Xeption
    {
        public FailedCsvHelperDependencyException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}

// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Foundations.CsvHelpers.Exceptions
{
    public class InvalidCsvHelperServiceDependencyException : Xeption
    {
        public InvalidCsvHelperServiceDependencyException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

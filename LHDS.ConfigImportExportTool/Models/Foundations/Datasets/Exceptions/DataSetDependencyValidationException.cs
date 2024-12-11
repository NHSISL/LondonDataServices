// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Foundations.Datasets.Exceptions
{
    public class DataSetDependencyValidationException : Xeption
    {
        public DataSetDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
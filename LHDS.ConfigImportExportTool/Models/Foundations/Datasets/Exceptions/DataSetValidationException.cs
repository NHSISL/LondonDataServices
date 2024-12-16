// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Foundations.Datasets.Exceptions
{
    public class DataSetValidationException : Xeption
    {
        public DataSetValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Processings.DataSets.Exceptions
{
    public class DataSetProcessingDependencyValidationException : Xeption
    {
        public DataSetProcessingDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}

// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Processings.DataSets.Exceptions
{
    public class DataSetProcessingDependencyException : Xeption
    {
        public DataSetProcessingDependencyException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}

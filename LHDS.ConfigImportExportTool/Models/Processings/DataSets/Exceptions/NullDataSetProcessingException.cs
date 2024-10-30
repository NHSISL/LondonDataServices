// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Processings.DataSets.Exceptions
{
    public class NullDataSetProcessingException : Xeption
    {
        public NullDataSetProcessingException(string message)
            : base(message)
        { }
    }
}

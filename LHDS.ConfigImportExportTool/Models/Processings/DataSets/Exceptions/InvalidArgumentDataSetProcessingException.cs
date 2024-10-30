// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Processings.DataSets.Exceptions
{
    public class InvalidArgumentDataSetProcessingException : Xeption
    {
        public InvalidArgumentDataSetProcessingException(string message)
            : base(message)
        { }
    }
}
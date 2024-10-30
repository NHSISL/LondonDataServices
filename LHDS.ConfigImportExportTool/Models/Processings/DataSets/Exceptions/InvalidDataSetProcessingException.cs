// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Processings.DataSets.Exceptions
{
    public class InvalidDataSetProcessingException : Xeption
    {
        public InvalidDataSetProcessingException(string message)
            : base(message)
        { }
    }
}
// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Processings.DataSets.Exceptions
{
    public class FailedDataSetProcessingServiceException : Xeption
    {
        public FailedDataSetProcessingServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}

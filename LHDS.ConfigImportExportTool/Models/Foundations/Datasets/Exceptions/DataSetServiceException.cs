// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Foundations.Datasets.Exceptions
{
    public class DataSetServiceException : Xeption
    {
        public DataSetServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
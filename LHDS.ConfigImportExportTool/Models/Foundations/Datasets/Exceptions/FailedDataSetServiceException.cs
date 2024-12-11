// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Foundations.Datasets.Exceptions
{
    public class FailedDataSetServiceException : Xeption
    {
        public FailedDataSetServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}
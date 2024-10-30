// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Processings.SpecificationObjects.Exceptions
{
    public class FailedSpecificationObjectProcessingServiceException : Xeption
    {
        public FailedSpecificationObjectProcessingServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}

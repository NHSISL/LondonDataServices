// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Processings.ObjectColumns.Exceptions
{
    public class ObjectColumnProcessingServiceException : Xeption
    {
        public ObjectColumnProcessingServiceException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}

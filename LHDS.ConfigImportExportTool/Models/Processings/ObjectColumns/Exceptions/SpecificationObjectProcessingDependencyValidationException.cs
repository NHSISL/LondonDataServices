// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Processings.ObjectColumns.Exceptions
{
    public class ObjectColumnProcessingDependencyValidationException : Xeption
    {
        public ObjectColumnProcessingDependencyValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}

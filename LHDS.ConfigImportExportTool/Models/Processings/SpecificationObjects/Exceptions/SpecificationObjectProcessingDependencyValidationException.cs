// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Processings.SpecificationObjects.Exceptions
{
    public class SpecificationObjectProcessingDependencyValidationException : Xeption
    {
        public SpecificationObjectProcessingDependencyValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}

// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Processings.SpecificationObjects.Exceptions
{
    public class SpecificationObjectProcessingValidationException : Xeption
    {
        public SpecificationObjectProcessingValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}

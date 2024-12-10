// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Processings.SpecificationObjects.Exceptions
{
    public class SpecificationObjectProcessingDependencyException : Xeption
    {
        public SpecificationObjectProcessingDependencyException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}

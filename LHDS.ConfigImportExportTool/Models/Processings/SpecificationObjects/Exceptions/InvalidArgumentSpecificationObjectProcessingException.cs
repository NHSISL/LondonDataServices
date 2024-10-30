// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Processings.SpecificationObjects.Exceptions
{
    public class InvalidArgumentSpecificationObjectProcessingException : Xeption
    {
        public InvalidArgumentSpecificationObjectProcessingException(string message)
            : base(message)
        { }
    }
}
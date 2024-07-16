// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Documents.Exceptions
{
    public class InvalidArgumentsDocumentProcessingException : Xeption
    {
        public InvalidArgumentsDocumentProcessingException(string message)
            : base(message)
        { }
    }
}

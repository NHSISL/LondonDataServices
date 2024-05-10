// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Documents.Exceptions
{
    public class NotFoundDocumentProcessingException : Xeption
    {
        public NotFoundDocumentProcessingException(string message)
            : base(message)
        { }
    }
}

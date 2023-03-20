// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Documents.Exceptions
{
    public class NullDocumentProcessingException : Xeption
    {
        public NullDocumentProcessingException()
            : base(message: $"Document processing is Null")
        { }
    }
}

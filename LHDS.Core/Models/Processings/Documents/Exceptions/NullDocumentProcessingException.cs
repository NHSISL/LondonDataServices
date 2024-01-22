// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Documents.Exceptions
{
    public class NullDocumentProcessingException : Xeption
    {
        public NullDocumentProcessingException(string message)
            : base(message)
        { }
    }
}

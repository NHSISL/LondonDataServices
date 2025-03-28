// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Documents.Exceptions
{
    public class DocumentProcessingValidationException : Xeption
    {
        public DocumentProcessingValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}

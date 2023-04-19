// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Documents.Exceptions
{
    public class DocumentProcessingValidationException : Xeption
    {
        public DocumentProcessingValidationException(Xeption innerException)
            : base(
                message: "Document processing validation errors occured, please try again",
                innerException)
        { }
    }
}

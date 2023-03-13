// ----------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Documents.Exceptions
{
    public class DocumentProcessingDependencyValidationException : Xeption
    {
        public DocumentProcessingDependencyValidationException(Xeption innerException)
            : base(message: "Document processing dependency validation occurred, please try again.", innerException) { }

    }
}

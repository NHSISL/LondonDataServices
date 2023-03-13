// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Documents.Exceptions
{
    public class DocumentProcessingDependencyException : Xeption
    {
        public DocumentProcessingDependencyException(Xeption innerException) :
            base(message: "Document processing dependency error occurred, contact support.", innerException)
        { }
    }
}

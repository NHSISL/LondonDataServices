// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Documents.Exceptions
{
    public class DocumentProcessingServiceException : Xeption
    {
        public DocumentProcessingServiceException(Xeption innerException)
          : base(message: "Document processing service error occurred, contact support.",
                innerException)
        { }
    }
}

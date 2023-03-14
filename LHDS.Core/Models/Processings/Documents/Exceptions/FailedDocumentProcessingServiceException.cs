// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Processings.Documents.Exceptions
{
    public class FailedDocumentProcessingServiceException : Xeption
    {
        public FailedDocumentProcessingServiceException(Exception innerException)
          : base(message: "Failed document processing service error occurred, contact support.",
                innerException)
        { }
    }
}

// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Landings.Client.Models.Foundations.Documents.Exceptions
{
    public class FailedDocumentServiceException : Xeption
    {
        public FailedDocumentServiceException(Exception innerException)
          : base(message: "Failed document service error occurred, contact support.",
                innerException)
        { }
    }
}

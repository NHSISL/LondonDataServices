// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Documents.Exceptions
{
    public class FailedDocumentRequestException : Xeption
    {
        public FailedDocumentRequestException(Exception innerException)
            : base(message: "Failed document request occurred, please contact support", innerException) { }
    }
}

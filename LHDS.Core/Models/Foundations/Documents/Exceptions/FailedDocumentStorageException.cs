// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Documents.Exceptions
{
    public class FailedDocumentStorageException : Xeption
    {
        public FailedDocumentStorageException(Exception innerException)
           : base(message: "Failed document storage error occurred, contact support.", innerException) { }
    }
}

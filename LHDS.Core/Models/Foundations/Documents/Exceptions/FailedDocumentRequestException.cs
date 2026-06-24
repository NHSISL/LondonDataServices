// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Documents.Exceptions
{
    public class FailedDocumentRequestException : Xeption
    {
        public FailedDocumentRequestException(string message, Exception? innerException, IDictionary data)
            : base(message, innerException, data)
        { }
    }
}

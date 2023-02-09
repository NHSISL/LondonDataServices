// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Documents.Exceptions
{
    public class AlreadyExistsDocumentException : Xeption
    {
        public AlreadyExistsDocumentException(Exception innerException)
            : base(message: "Document with the same Id already exists.", innerException)
        { }
    }
}

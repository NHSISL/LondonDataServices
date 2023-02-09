// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.Premises.Api.Models.Documents.Exceptions
{
    public class InvalidDocumentException : Xeption
    {
        public InvalidDocumentException()
            : base(message: "Invalid document. Please correct the errors and try again.")
        { }
    }
}

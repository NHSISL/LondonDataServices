// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Documents.Exceptions
{
    public class DocumentValidationException : Xeption
    {
        public DocumentValidationException(Xeption innerException)
            : base(message: "Document validation errors occured, please try again",
                  innerException)
        { }
    }
}

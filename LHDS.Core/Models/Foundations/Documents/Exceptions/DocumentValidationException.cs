// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Documents.Exceptions
{
    public class DocumentValidationException : Xeption
    {
        public DocumentValidationException(string message, Xeption? innerException)
            : base(message,innerException)
        { }
    }
}

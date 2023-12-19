// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Tpp.Exceptions
{
    public class TppDocumentValidationException : Xeption
    {
        public TppDocumentValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}

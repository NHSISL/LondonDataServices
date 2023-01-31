// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Landings.Client.Models.Foundations.Documents.Exceptions
{
    public class DocumentDependencyValidationException : Xeption
    {
        public DocumentDependencyValidationException(Xeption innerException)
            : base(message: "Document dependency validation occurred, please try again.", innerException)
        { }

    }
}

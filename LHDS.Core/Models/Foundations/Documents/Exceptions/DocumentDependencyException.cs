// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Documents.Exceptions
{
    public class DocumentDependencyException : Xeption
    {
        public DocumentDependencyException(string message, Xeption? innerException) :
            base(message, innerException)
        { }
    }
}

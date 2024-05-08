// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Ontologies.Exceptions
{
    public class OntologyValidationException : Xeption
    {
        public OntologyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
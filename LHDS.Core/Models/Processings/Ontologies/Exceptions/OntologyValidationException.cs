// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Ontologies.Exceptions
{
    public class OntologyProcessingValidationException : Xeption
    {
        public OntologyProcessingValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Ontologies.Exceptions
{
    public class InvalidArgumentOntologyProcessingException : Xeption
    {
        public InvalidArgumentOntologyProcessingException(string message)
            : base(message)
        { }
    }
}
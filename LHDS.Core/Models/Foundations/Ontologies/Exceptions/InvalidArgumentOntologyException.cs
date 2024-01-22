// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Ontologies.Exceptions
{
    public class InvalidArgumentOntologyException : Xeption
    {
        public InvalidArgumentOntologyException(string message)
            : base(message)
        { }
    }
}
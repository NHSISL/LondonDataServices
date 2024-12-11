// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Ontologies.Exceptions
{
    public class OntologyProcessingServiceException : Xeption
    {
        public OntologyProcessingServiceException(string message, Xeption? innerException)
          : base(message, innerException)
        { }
    }
}

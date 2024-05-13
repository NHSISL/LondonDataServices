// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Processings.Ontologies.Exceptions
{
    public class FailedOntologyProcessingServiceException : Xeption
    {
        public FailedOntologyProcessingServiceException(string message, Exception? innerException)
          : base(message, innerException)
        { }
    }
}

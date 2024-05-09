// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Ontologies.Exceptions
{
    public class FailedOntologyServiceException : Xeption
    {
        public FailedOntologyServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}
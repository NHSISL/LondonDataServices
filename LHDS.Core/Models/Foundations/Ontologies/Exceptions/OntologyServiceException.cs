// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Ontologies.Exceptions
{
    public class OntologyServiceException : Xeption
    {
        public OntologyServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}
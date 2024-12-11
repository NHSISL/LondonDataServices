// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Processings.TerminologyArtifacts.Exceptions
{
    public class FailedTerminologyArtifactProcessingServiceException : Xeption
    {
        public FailedTerminologyArtifactProcessingServiceException(string message, Exception? innerException)
          : base(message, innerException)
        { }
    }
}

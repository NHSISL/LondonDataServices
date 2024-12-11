// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions
{
    public class TerminologyArtifactServiceException : Xeption
    {
        public TerminologyArtifactServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}
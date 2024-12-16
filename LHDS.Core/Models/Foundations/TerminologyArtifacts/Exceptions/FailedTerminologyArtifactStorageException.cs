// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions
{
    public class FailedTerminologyArtifactStorageException : Xeption
    {
        public FailedTerminologyArtifactStorageException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}
// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Decryptions.Exceptions
{
    public class FailedDecryptionServiceException : Xeption
    {
        public FailedDecryptionServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
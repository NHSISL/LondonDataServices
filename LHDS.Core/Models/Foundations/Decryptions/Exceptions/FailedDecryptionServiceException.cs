// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Decryptions.Exceptions
{
    public class FailedDecryptionServiceException : Xeption
    {
        public FailedDecryptionServiceException(Exception innerException)
            : base(message: "Failed decryption service occurred, please contact support", innerException) { }
    }
}
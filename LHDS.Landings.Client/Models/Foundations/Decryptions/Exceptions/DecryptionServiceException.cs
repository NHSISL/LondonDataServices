// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Landings.Client.Models.Foundations.Decryptions.Exceptions
{
    public class DecryptionServiceException : Xeption
    {
        public DecryptionServiceException(Exception innerException)
            : base(message: "Decryption service error occurred, contact support.", innerException)
        { }
    }
}
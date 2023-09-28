// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Decryptions.Exceptions
{
    public class DecryptionServiceException : Xeption
    {
        public DecryptionServiceException(string message, Exception innerException)
            : base(message, innerException) 
        { }
    }
}
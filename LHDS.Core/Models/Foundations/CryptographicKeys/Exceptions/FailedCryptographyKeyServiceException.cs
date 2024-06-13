// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.CryptographicKeys.Exceptions
{
    public class FailedCryptographyKeyServiceException : Xeption
    {
        public FailedCryptographyKeyServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}
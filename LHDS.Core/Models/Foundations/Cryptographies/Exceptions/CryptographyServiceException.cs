// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Cryptographies.Exceptions
{
    public class CryptographyServiceException : Xeption
    {
        public CryptographyServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}
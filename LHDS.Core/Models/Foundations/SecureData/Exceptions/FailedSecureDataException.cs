// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.SecureData.Exceptions
{
    public class FailedSecureDataException : Xeption
    {
        public FailedSecureDataException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}
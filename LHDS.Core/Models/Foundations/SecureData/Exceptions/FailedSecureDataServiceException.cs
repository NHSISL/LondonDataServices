// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.SecureData.Exceptions
{
    public class FailedSecureDataServiceException : Xeption
    {
        public FailedSecureDataServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}
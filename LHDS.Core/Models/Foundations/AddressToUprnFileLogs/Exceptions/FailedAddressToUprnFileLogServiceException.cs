// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressToUprnFileLogs.Exceptions
{
    public class FailedAddressToUprnFileLogServiceException : Xeption
    {
        public FailedAddressToUprnFileLogServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}

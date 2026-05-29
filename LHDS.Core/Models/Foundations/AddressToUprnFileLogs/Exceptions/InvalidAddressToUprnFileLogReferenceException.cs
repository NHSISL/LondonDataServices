// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressToUprnFileLogs.Exceptions
{
    public class InvalidAddressToUprnFileLogReferenceException : Xeption
    {
        public InvalidAddressToUprnFileLogReferenceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}

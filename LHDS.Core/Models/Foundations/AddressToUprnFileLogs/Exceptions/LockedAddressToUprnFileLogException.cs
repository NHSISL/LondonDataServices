// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressToUprnFileLogs.Exceptions
{
    public class LockedAddressToUprnFileLogException : Xeption
    {
        public LockedAddressToUprnFileLogException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}
